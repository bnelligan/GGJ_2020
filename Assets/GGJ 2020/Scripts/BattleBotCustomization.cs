﻿// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotCustomization : MonoBehaviour
    {
        public LayerMask LayerMaskSelect;
        public LayerMask LayerMaskDrag;
        public Camera Camera;
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
                            {
                                // The part is not in a socket

                                this.selectedBattleBotPart = battleBotPart;

                                this.selectedBattleBotPart.BeingDragged = true;

                                UnityEngine.Debug.Log ($"{ this } selected { this.selectedBattleBotPart }");
                            }
                        }
                    }
                }
            }
            else
            {
                // Check if the part has been attached to a socket or released by the user

                if (UnityEngine.Input.GetKey (UnityEngine.KeyCode.Mouse0) == false || this.selectedBattleBotPart.Socket != null)
                {
                    UnityEngine.Debug.Log ($"{ this } released { this.selectedBattleBotPart }");

                    this.selectedBattleBotPart.BeingDragged = false;

                    this.selectedBattleBotPart = null;

                    return;
                }

                if (UnityEngine.Physics.Raycast (ray, out RaycastHit raycastHit, float.MaxValue, this.LayerMaskDrag))
                {
                    #if UNITY_EDITOR

                    UnityEngine.Debug.DrawLine (ray.origin, raycastHit.point, Color.blue);

                    #endif

                    this.selectedBattleBotPart.transform.position = Vector3.Lerp (this.selectedBattleBotPart.transform.position, raycastHit.point + Vector3.up * 0.5f, 3f * UnityEngine.Time.deltaTime);
                }
            }
        }
    }
}
