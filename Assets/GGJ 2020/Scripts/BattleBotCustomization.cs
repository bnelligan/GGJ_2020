// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;
using System.Collections;

namespace BrokenBattleBots
{
    public class BattleBotCustomization : MonoBehaviour
    {
        public BattleBotPart[] armPrefabs;
        public BattleBotPart[] legPrefabs;
        public BattleBotPart[] headPrefabs;
        public BattleBotPartSocket socketHead;
        public BattleBotPartSocket socketArmLeft;
        public BattleBotPartSocket socketArmRight;
        public BattleBotPartSocket socketLegs;
        public BattleBotPart partTorso;
        public AudioClip[] AudioClipsCollisions;
        public AudioClip[] AudioClipsAttach;
        public AudioClip[] AudioClipsDetach;
        public AudioClip[] AudioClipsError;
        public Gradient WeldGradientPositive;
        public static BattleBotCustomization instance;
        public LayerMask LayerMaskSelect;
        public LayerMask LayerMaskDrag;
        public Camera Camera;
        public CameraFollow CameraFollow;
        public Transform playerSpawn;
        public bool Standing { get; private set; }
        public BattleBotPart SelectedBattleBotPart { get; private set; }

        private void Awake ()
        {
            instance = this;

            if (this.Camera == null)
            {
                this.Camera = FindObjectOfType <Camera> ();
            }

            this.CameraFollow = this.Camera.GetComponent <CameraFollow> ();
        }

        /*private void Start ()
        {
            this.SpawnRandomParts ();
        }*/

        public void SpawnRandomParts ()
        {
            this.SpawnRandomArms ();
            this.SpawnRandomHead();
            this.SpawnRandomLegs ();
        }

        public void SpawnRandomArms (int count = 2)
        {
            for (int index = 0; index < count; index += 1)
            {
                Instantiate (this.armPrefabs[Random.Range (0, this.armPrefabs.Length)], this.GetRandomPartSpawnPosition (), Quaternion.identity);
            }
        }

        public void SpawnRandomLegs ()
        {
            Instantiate (this.legPrefabs[Random.Range (0, this.legPrefabs.Length)], this.GetRandomPartSpawnPosition (), Quaternion.identity);
        }

        public void SpawnRandomHead ()
        {
            Instantiate (this.headPrefabs[Random.Range (0, this.headPrefabs.Length)], this.GetRandomPartSpawnPosition (), Quaternion.identity);
        }

        private Vector3 GetRandomPartSpawnPosition ()
        {
            Vector3 position = this.partTorso.transform.position;
            position.y += 20f;
            position.x += Random.Range (-5f, 5f);
            position.z += Random.Range (-5f, 5f);

            return position;
        }

        public struct IgnoreCollisionPair
        {
            public Collider colliderA;
            public Collider colliderB;
        }

        private System.Collections.Generic.List <IgnoreCollisionPair> ignoreCollisions = new System.Collections.Generic.List<IgnoreCollisionPair> ();

        public void IgnoreCollisionsTillNotOverlapping (Collider colliderA, Collider colliderB)
        {
            if (colliderA == colliderB)
            {
                UnityEngine.Debug.LogError ("NO");

                return;
            }

            IgnoreCollisionPair ignoreCollisionPair = new IgnoreCollisionPair ();
            ignoreCollisionPair.colliderA = colliderA;
            ignoreCollisionPair.colliderB = colliderB;

            UnityEngine.Physics.IgnoreCollision (colliderA, colliderB, true);

            this.ignoreCollisions.Add (ignoreCollisionPair);
        }

        private void UpdateIgnoreCollisionTillClearPairs ()
        {
            for (int index = this.ignoreCollisions.Count - 1; index >= 0; index -= 1)
            {
                Collider colliderA = ignoreCollisions[index].colliderA;
                Collider colliderB = ignoreCollisions[index].colliderB;

                if (UnityEngine.Physics.ComputePenetration (colliderA, colliderA.transform.position, colliderA.transform.rotation, colliderB, colliderB.transform.position, colliderB.transform.rotation, out Vector3 direction, out float distance) == false)
                {
                    UnityEngine.Physics.IgnoreCollision (colliderA, colliderB, false);

                    this.ignoreCollisions.RemoveAt (index);
                }
            }
        }

        public void StandUp ()
        {
            // The bot has legs, disable physics and go into the upright position

            this.partTorso.Rigidbody.isKinematic = true;

            this.partTorso.Rigidbody.useGravity = false;

            this.partTorso.transform.rotation = Quaternion.identity;

            Vector3 position = this.partTorso.transform.position;

            if (this.socketLegs.battleBotPart != null)
            {
                position = playerSpawn.position;
            }
            else
            {
                position.y = 3f;
            }

            this.partTorso.transform.position = position;

            if (this.socketLegs.battleBotPart == null)
            {
                this.FallOver ();

                return;
            }

            bool legsUnwelded = false;

            if (this.socketLegs.battleBotPart != null && this.socketLegs.battleBotPart.Welded < 9f)
            {
                legsUnwelded = true;
            }

            this.DetachUnweldedParts ();

            if (legsUnwelded == true)
            {
                this.FallOver ();

                return;
            }

            this.Standing = true;

            this.CameraFollow.IsRobotStanding = true;
        }

        public void DetachUnweldedParts (BattleBotPart ignore = null, float weldDetachThreshold = 9f)
        {
            if (this.socketArmLeft.battleBotPart != null && this.socketArmLeft.battleBotPart.Welded < weldDetachThreshold)
            {
                if (ignore == null || this.socketArmLeft.battleBotPart != ignore)
                {
                    this.DetachPart (this.socketArmLeft.battleBotPart);
                }
            }

            if (this.socketArmRight.battleBotPart != null && this.socketArmRight.battleBotPart.Welded < weldDetachThreshold)
            {
                if (ignore == null || this.socketArmRight.battleBotPart != ignore)
                {
                    this.DetachPart (this.socketArmRight.battleBotPart);
                }
            }

            if (this.socketHead.battleBotPart != null && this.socketHead.battleBotPart.Welded < weldDetachThreshold)
            {
                if (ignore == null || this.socketHead.battleBotPart != ignore)
                {
                    this.DetachPart (this.socketHead.battleBotPart);
                }
            }

            if (this.socketLegs.battleBotPart != null && this.socketLegs.battleBotPart.Welded < weldDetachThreshold)
            {
                if (ignore == null || this.socketLegs.battleBotPart != ignore)
                {
                    this.DetachPart (this.socketLegs.battleBotPart);
                }
            }
        }

        public void FallOver ()
        {
            this.CameraFollow.IsRobotStanding = false;

            this.Standing = false;

            this.partTorso.Rigidbody.isKinematic = false;

            this.partTorso.Rigidbody.useGravity = true;

            this.partTorso.Rigidbody.AddForce (-this.partTorso.transform.forward * 3f, ForceMode.Impulse);
        }

        private void Update ()
        {
            if (this.Standing == false)
            {
                // Update camera follow targets to torso

                Vector3 averagePosition = Vector3.zero;
                averagePosition += this.socketHead.transform.position;
                averagePosition += this.socketLegs.transform.position;
                averagePosition += this.socketArmLeft.transform.position;
                averagePosition += this.socketArmRight.transform.position;
                averagePosition /= 4f;

                this.CameraFollow.TargetPosition = averagePosition;

                // Spawn any parts not in vicinity

                int arms = 0;
                int legs = 0;
                int heads = 0;

                Collider[] overlapShereResults = UnityEngine.Physics.OverlapSphere (this.partTorso.transform.position, 20f, this.LayerMaskSelect);

                foreach (Collider collider in overlapShereResults)
                {
                    BattleBotPart battleBotPart = collider.GetComponent <BattleBotPart> ();

                    if (battleBotPart != null)
                    {
                        // if (battleBotPart.Socket == null)
                        {
                            switch (battleBotPart.PartType)
                            {
                                case BattleBotPart.BattleBotPartType.Arm:
                                {
                                    arms += 1;

                                    break;
                                }

                                case BattleBotPart.BattleBotPartType.Bottom:
                                {
                                    legs += 1;

                                    break;
                                }

                                case BattleBotPart.BattleBotPartType.Head:
                                {
                                    heads += 1;

                                    break;
                                }
                            }
                        }
                    }
                }

                if (this.partTorso.Rigidbody.velocity.y >= 0f)
                {
                    if (arms == 0)
                    {
                        this.SpawnRandomArms (2);
                    }
                    else if (arms == 1)
                    {
                        this.SpawnRandomArms (1);
                    }
                    else if (legs == 0)
                    {
                        this.SpawnRandomLegs ();
                    }
                    else if (heads == 0)
                    {
                        this.SpawnRandomHead ();
                    }
                }
            }

            if (UnityEngine.Input.GetKeyDown (UnityEngine.KeyCode.R))
            {
                this.SpawnRandomParts ();
            }

            this.UpdateIgnoreCollisionTillClearPairs ();

            if (UnityEngine.Input.GetKeyDown (UnityEngine.KeyCode.Space))
            {
                if (this.Standing == true)
                {
                    this.FallOver();
                }
                else
                {
                    this.StandUp ();
                } 
            }

            if (this.Standing == true)
            {
                return;
            }

            Ray ray = this.Camera.ScreenPointToRay (UnityEngine.Input.mousePosition);

            if (this.SelectedBattleBotPart == null)
            { 
                if (UnityEngine.Input.GetKeyDown (UnityEngine.KeyCode.Mouse0) == true)
                {
                    if (UnityEngine.Physics.Raycast (ray, out RaycastHit raycastHit, float.MaxValue, this.LayerMaskSelect))
                    {
                        #if UNITY_EDITOR

                        UnityEngine.Debug.DrawLine (ray.origin, raycastHit.point, Color.blue);

                        #endif

                        // Check if the hit collider is a battle bot part

                        BattleBotPart battleBotPart = raycastHit.collider.GetComponent <BattleBotPart>();

                        if (battleBotPart != null)
                        {
                            // If the part is in a socket - detach it

                            if (battleBotPart.Socket != null)
                            {
                                // this.SelectedBattleBotPart = battleBotPart;FFAB2D

                                // this.SelectedBattleBotPart.BeingDragged = true;

                                this.DetachPart (battleBotPart);
                                
                            }
                            else
                            {
                                // The part is not in a socket

                                this.SelectedBattleBotPart = battleBotPart;

                                this.partTorso.Rigidbody.velocity = Vector3.zero;

                                this.partTorso.Rigidbody.angularVelocity = Vector3.zero;

                                UnityEngine.Debug.Log ($"{ this } selected { this.SelectedBattleBotPart }");
                            }
                        }
                    }
                }
            }
            else
            {
                // Check if the part has been attached to a socket or released by the user

                if (UnityEngine.Input.GetKey (UnityEngine.KeyCode.Mouse0) == false || this.SelectedBattleBotPart.Socket != null)
                {
                    UnityEngine.Debug.Log ($"{ this } released { this.SelectedBattleBotPart }");

                    this.SelectedBattleBotPart = null;

                    return;
                }

                if (UnityEngine.Physics.Raycast (ray, out RaycastHit raycastHit, float.MaxValue, this.LayerMaskDrag))
                {
                    #if UNITY_EDITOR

                    UnityEngine.Debug.DrawLine (ray.origin, raycastHit.point, Color.blue);

                    #endif

                    this.SelectedBattleBotPart.transform.position = Vector3.Lerp (this.SelectedBattleBotPart.transform.position, raycastHit.point + Vector3.up * 0.5f, 3f * UnityEngine.Time.deltaTime);
                }
            }
        }

        private void DetachPart (BattleBotPart battleBotPart)
        {
            this.IgnoreCollisionsTillNotOverlapping (battleBotPart.Socket.Collider, battleBotPart.Collider);

            battleBotPart.Socket.DetachPart (UnityEngine.Random.onUnitSphere * 10f);
        }
    }
}
