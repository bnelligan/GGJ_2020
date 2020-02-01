// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotCustomization : MonoBehaviour
    {
        public LayerMask LayerMaskSelect;
        public LayerMask LayerMaskDrag;
        public Camera Camera;
        public SpringJoint SpringJoint;
        private BattleBotPart selectedBattleBotPart;

        private void Awake ()
        {
            if (this.Camera == null)
            {
                this.Camera = FindObjectOfType <Camera> ();
            }
        }

        private void Update ()
        {
            Ray ray = this.Camera.ScreenPointToRay (UnityEngine.Input.mousePosition);

            if (this.selectedBattleBotPart == null)
            { 
                if (UnityEngine.Input.GetKeyDown (UnityEngine.KeyCode.Mouse0) == true)
                {
                    if (UnityEngine.Physics.SphereCast (ray, 1f, out RaycastHit raycastHit, float.MaxValue, this.LayerMaskSelect))
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
                                battleBotPart.Socket.DetachPart (UnityEngine.Random.onUnitSphere * Random.Range (2f, 8f));
                            }
                            else
                            {
                                // The part is not in a socket

                                this.selectedBattleBotPart = battleBotPart;

                                // this.SpringJoint.connectedBody = this.selectedBattleBotPart.Rigidbody;

                                UnityEngine.Debug.Log ($"{ this } selected { this.selectedBattleBotPart }");
                            }
                        }
                    }
                }
            }
            else
            {
                // Check if the part has been attached to a socket or released by the user

                if (UnityEngine.Input.GetKeyUp (UnityEngine.KeyCode.Mouse0) == true || this.selectedBattleBotPart.Socket != null)
                {
                    UnityEngine.Debug.Log ($"{ this } released { this.selectedBattleBotPart }");

                    this.SpringJoint.connectedBody = null;

                    this.selectedBattleBotPart = null;

                    return;
                }

                if (UnityEngine.Physics.SphereCast (ray, 1f, out RaycastHit raycastHit, float.MaxValue, this.LayerMaskDrag))
                {
                    #if UNITY_EDITOR

                    UnityEngine.Debug.DrawLine (ray.origin, raycastHit.point, Color.blue);

                    #endif

                    this.selectedBattleBotPart.transform.position = raycastHit.point;
                }
            }
        }
    }
}
