﻿using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotPartSocket : MonoBehaviour
    {
        public BattleBotPart.Type CompatiblePartTypes;
        private BattleBotPart battleBotPart;

        private void AttachPart (BattleBotPart battleBotPart)
        {
            // Check if the part is already attached to another socket

            if (battleBotPart.Socket != null)
            {
                return;
            }

            // Check if a part is already attached to this socket

            if (this.battleBotPart != null)
            {
                return;
            }

            // Check if the part is compatible with this socket

            if (this.CompatiblePartTypes.HasFlag (battleBotPart.PartType) == false)
            {
                UnityEngine.Debug.LogWarning ($"{ battleBotPart } is not compatible with { this }");
            }

            // Attach the part to this socked

            UnityEngine.Debug.Log ($"{ this } attached { battleBotPart }");

            this.battleBotPart = battleBotPart;
            this.battleBotPart.Socket = this;
            this.battleBotPart.Rigidbody.isKinematic = true;
            this.battleBotPart.Rigidbody.useGravity = false;
            this.battleBotPart.transform.SetParent (this.transform);
            this.battleBotPart.transform.localPosition = Vector3.zero;
            this.battleBotPart.transform.localRotation = Quaternion.identity;
        }

        private void DetachPart (Vector3 force)
        {
            // Check if a part is attached to this socket

            if (this.battleBotPart != null)
            {
                UnityEngine.Debug.Log ($"{ this } detached { this.battleBotPart }");

                // Detach the part in the socket

                this.battleBotPart.transform.SetParent (null);
                this.battleBotPart.Rigidbody.isKinematic = false;
                this.battleBotPart.Rigidbody.useGravity = true;
                this.battleBotPart.Rigidbody.AddForce (force, UnityEngine.ForceMode.Impulse);
                this.battleBotPart.Socket = null;
                this.battleBotPart = null;
            }
        }

        private void OnTriggerEnter (Collider collider)
        {
            // Check if the collider is a battle bot part

            BattleBotPart battleBotPart = collider.GetComponent <BattleBotPart> ();

            if (battleBotPart != null)
            {
                // Try attaching the battle bot part to the socket

                this.AttachPart (battleBotPart);
            }
        }

        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = this.battleBotPart == null ? Color.white : Color.blue;

            Gizmos.DrawSphere (this.transform.position, 0.25f);
        }

        #endif
    }
}