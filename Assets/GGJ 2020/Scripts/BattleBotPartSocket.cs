// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Collider))]
    [RequireComponent (typeof (AudioSource))]
    public class BattleBotPartSocket : MonoBehaviour
    {
        public AudioSource AudioSource;
        public AudioClip[] AudioClipsAttach;
        public AudioClip[] AudioClipsDetach;
        public BattleBotPart.BattleBotPartType CompatiblePartTypes;
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

            if (this.CompatiblePartTypes != battleBotPart.PartType)
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
            // this.battleBotPart.transform.localPosition = Vector3.zero;
            // this.battleBotPart.transform.localRotation = Quaternion.identity;

            this.PlayAttachSound ();
        }

        public void DetachPart (Vector3 force)
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

                this.PlayDetachSound ();
            }
        }

        private void PlayAttachSound ()
        {
            // Play random attach sound

            if (this.AudioSource != null && this.AudioClipsAttach != null && this.AudioClipsAttach.Length > 0)
            {
                this.AudioSource.PlayOneShot (this.AudioClipsAttach[Random.Range (0, this.AudioClipsAttach.Length)]);
            }
        }

        private void PlayDetachSound ()
        {
            // Play random detach sound

            if (this.AudioSource != null && this.AudioClipsAttach != null && this.AudioClipsAttach.Length > 0)
            {
                this.AudioSource.PlayOneShot (this.AudioClipsDetach[Random.Range (0, this.AudioClipsDetach.Length)]);
            }
        }

        private void Update ()
        {
            // Lerp the part to the socket's position

            float deltaTime = UnityEngine.Time.deltaTime;

            if (this.battleBotPart != null)
            {
                this.battleBotPart.transform.localPosition = Vector3.Lerp (this.battleBotPart.transform.localPosition, Vector3.zero, 6f * deltaTime);

                this.battleBotPart.transform.localRotation = Quaternion.Lerp (this.battleBotPart.transform.localRotation, Quaternion.identity, 6f * deltaTime);
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
            // Cache the socket's audio source

            this.AudioSource = this.GetComponent <AudioSource> ();

            UnityEngine.Assertions.Assert.IsNotNull (this.AudioSource, $"{ this } Missing audio source");
        }

        #endif
    }
}