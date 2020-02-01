// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotCustomization : MonoBehaviour
    {
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

        private void Update ()
        {
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

                            /*if (battleBotPart.Socket != null)
                            {
                                battleBotPart.Socket.DetachPart (UnityEngine.Random.onUnitSphere * Random.Range (2f, 8f));

                                this.selectedBattleBotPart = battleBotPart;

                                if (battleBotPart.Socket.parentBattleBotPart != null)
                                {

                                }
                            }
                            else*/

                            if (battleBotPart.Socket == null)
                            {
                                // The part is not in a socket

                                this.SelectedBattleBotPart = battleBotPart;

                                this.SelectedBattleBotPart.BeingDragged = true;

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

                    this.SelectedBattleBotPart.BeingDragged = false;

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
    }
}
