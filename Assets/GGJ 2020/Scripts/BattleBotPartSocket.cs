// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Collider))]
    public class BattleBotPartSocket : MonoBehaviour
    {
        public BattleBotPart parentBattleBotPart;
        public BattleBotPart.BattleBotPartType CompatiblePartTypes;
        public BattleBotPart battleBotPart { get; private set; }
        public Vector3 attachPosition;
        public Vector3 attachRotationEulerAngles;

        private void AttachPart (BattleBotPart battleBotPart)
        {
            if (this.parentBattleBotPart != null && this.parentBattleBotPart == battleBotPart)
            {
                return;
            }

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

            if (this.CompatiblePartTypes != battleBotPart.PartType)
            {
                UnityEngine.Debug.LogWarning ($"{ battleBotPart } is not compatible with { this }");

                if (battleBotPart.BeingDragged == true)
                {
                    battleBotPart.PlayErrorSound ();
                }

                return;
            }

            // Attach the part to this socked

            UnityEngine.Debug.Log ($"{ this } attached { battleBotPart }");

            this.battleBotPart = battleBotPart;
            this.battleBotPart.Socket = this;
            this.battleBotPart.Rigidbody.isKinematic = true;
            this.battleBotPart.Rigidbody.useGravity = false;
            this.battleBotPart.transform.SetParent (this.transform);
            /*this.battleBotPart.transform.localPosition = this.attachPosition;
            this.battleBotPart.transform.localRotation = Quaternion.Euler (this.attachRotationEulerAngles);*/

            if (this.parentBattleBotPart != null)
            {
                // NEED TO REMOVE RIGIDBODY BECAUSE UNITY COO

                Destroy (this.battleBotPart.Rigidbody);
            }

            this.battleBotPart.PlayAttachSound ();

            if (this.parentBattleBotPart != null)
            {
                Vector3 direction = this.battleBotPart.transform.position - this.parentBattleBotPart.transform.position;

                this.parentBattleBotPart.Rigidbody.AddForce (-direction * 100f, ForceMode.Force);
            }
        }

        public void DetachPart (Vector3 force)
        {
            // Check if a part is attached to this socket

            if (this.battleBotPart != null)
            {
                UnityEngine.Debug.Log ($"{ this } detached { this.battleBotPart }");

                // Detach the part in the socket

                if (this.parentBattleBotPart != null)
                {
                    // UnityEngine.Physics.IgnoreCollision (this.battleBotPart.Collider, this.parentBattleBotPart.Collider, false);
                }

                this.battleBotPart.PlayCollisionSound (1f);
                this.battleBotPart.PlayDetachSound ();
                this.battleBotPart.transform.SetParent (null);
                
                if (this.battleBotPart.Rigidbody == null)
                {
                    this.battleBotPart.Rigidbody = this.battleBotPart.gameObject.AddComponent<Rigidbody>();
                }
                
                this.battleBotPart.Rigidbody.isKinematic = false;
                this.battleBotPart.Rigidbody.useGravity = true;
                this.battleBotPart.Rigidbody.AddForce (force, UnityEngine.ForceMode.Impulse);
                this.battleBotPart.Socket = null;
                this.battleBotPart = null;
            }
        }

        private void Update ()
        {
            // Lerp the part to the socket's position

            float deltaTime = UnityEngine.Time.deltaTime;

            if (this.battleBotPart != null)
            {
                this.battleBotPart.transform.localPosition = Vector3.Lerp (this.battleBotPart.transform.localPosition, this.attachPosition, 6f * deltaTime);

                this.battleBotPart.transform.localRotation = Quaternion.Lerp (this.battleBotPart.transform.localRotation, Quaternion.Euler (this.attachRotationEulerAngles), 6f * deltaTime);
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

        private void OnValidate ()
        {
            // Cache the socket's parent bot part

            this.parentBattleBotPart = this.GetComponentInParent <BattleBotPart> ();
        }

        #endif
    }
}