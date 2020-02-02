﻿// Created: 2020/02/01
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

        private void Start ()
        {
            this.SpawnRandomParts ();
        }

        public void SpawnRandomParts ()
        {
            float range = 10f;

            Instantiate (this.armPrefabs[Random.Range (0, this.armPrefabs.Length)], (this.transform.position + Vector3.up * range) + Random.onUnitSphere * range, Quaternion.identity);
            Instantiate (this.armPrefabs[Random.Range (0, this.armPrefabs.Length)], (this.transform.position + Vector3.up * range) + Random.onUnitSphere * range, Quaternion.identity);
            Instantiate (this.headPrefabs[Random.Range (0, this.headPrefabs.Length)], (this.transform.position + Vector3.up * range) + Random.onUnitSphere * range, Quaternion.identity);
            Instantiate (this.legPrefabs[Random.Range (0, this.legPrefabs.Length)], (this.transform.position + Vector3.up * range) + Random.onUnitSphere * range, Quaternion.identity);
        }

        public IEnumerator IgnoreCollisionsTillNotOverlapping (Collider colliderA, Collider colliderB)
        {
            UnityEngine.Physics.IgnoreCollision (colliderA, colliderB, true);

            while (UnityEngine.Physics.ComputePenetration (colliderA, colliderA.transform.position, colliderA.transform.rotation, colliderB, colliderB.transform.position, colliderB.transform.rotation, out Vector3 direction, out float distance))
            {
                yield return null;
            }

            UnityEngine.Physics.IgnoreCollision (colliderA, colliderB, false);
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
                position.y = this.socketLegs.battleBotPart.standHeightOffset;
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

            if (this.socketArmLeft.battleBotPart != null && this.socketArmLeft.battleBotPart.Welded < 9f)
            {
                this.DetachPart (this.socketArmLeft.battleBotPart);
            }

            if (this.socketArmRight.battleBotPart != null && this.socketArmRight.battleBotPart.Welded < 9f)
            {
                this.DetachPart (this.socketArmRight.battleBotPart);
            }

            if (this.socketHead.battleBotPart != null && this.socketHead.battleBotPart.Welded < 9f)
            {
                this.DetachPart (this.socketHead.battleBotPart);
            }

            if (this.socketLegs.battleBotPart != null && this.socketLegs.battleBotPart.Welded < 9f)
            {
                this.FallOver ();

                this.DetachPart (this.socketLegs.battleBotPart);

                return;
            }

            // Update camera follow targets to torso

            this.CameraFollow.FollowTargets = new Transform[1];

            this.CameraFollow.FollowTargets[0] = this.partTorso.transform;

            this.Standing = true;
        }

        public void FallOver ()
        {
            // Update camera follow targets to parts

            /*BattleBotPart[] parts = FindObjectsOfType <BattleBotPart> ();

            this.CameraFollow.FollowTargets = new Transform[parts.Length];

            for (int index = 0; index < parts.Length; index += 1)
            {
                this.CameraFollow.FollowTargets[index] = parts[index].transform;
            }*/

            this.Standing = false;

            this.partTorso.Rigidbody.isKinematic = false;

            this.partTorso.Rigidbody.useGravity = true;

            this.partTorso.Rigidbody.AddForce (-this.partTorso.transform.forward * 3f, ForceMode.Impulse);
        }

        private void Update ()
        {
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
                                // this.SelectedBattleBotPart = battleBotPart;

                                // this.SelectedBattleBotPart.BeingDragged = true;

                                this.DetachPart (battleBotPart);
                                
                            }
                            else
                            {
                                // The part is not in a socket

                                this.SelectedBattleBotPart = battleBotPart;

                                UnityEngine.Debug.Log ($"{ this } selected { this.SelectedBattleBotPart }");
                            }
                        }
                    }
                }
            }
            else
            {
                this.partTorso.Rigidbody.velocity = Vector3.zero;

                this.partTorso.Rigidbody.angularVelocity = Vector3.zero;

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
            this.StartCoroutine (this.IgnoreCollisionsTillNotOverlapping (battleBotPart.Socket.GetComponent <Collider> (), battleBotPart.Collider));

            battleBotPart.Socket.DetachPart (UnityEngine.Random.onUnitSphere * 10f);
        }
    }
}
