// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;
using System.Collections;

namespace BrokenBattleBots
{
    public class BattleBotCustomization : MonoBehaviour
    {
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
        public BattleBotPart SelectedBattleBotPart { get; private set; }

        private void Awake ()
        {
            instance = this;

            if (this.Camera == null)
            {
                this.Camera = FindObjectOfType <Camera> ();
            }
        }

        private IEnumerator IgnoreCollisionsTillNotOverlapping (Collider colliderA, Collider colliderB)
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
            if (this.socketLegs.battleBotPart == null)
            {
                UnityEngine.Debug.LogWarning ("No legs can't stand up");

                // TORSO IS GARUNTEED NOT NULL

                this.partTorso.PlayErrorSound ();

                this.partTorso.Rigidbody.AddForce (Vector3.up * 5f, ForceMode.Impulse);

                this.partTorso.Rigidbody.angularVelocity = Random.onUnitSphere * 3f;

                return;
            }
        }

        public void FallOver ()
        {

        }

        private void Update ()
        {
            if (UnityEngine.Input.GetKeyDown (UnityEngine.KeyCode.Space))
            {
                this.StandUp ();
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
