// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Rigidbody))]
    public class BattleBotPart : MonoBehaviour
    {
        public float Welded;
        public bool BeingDragged;
        public BattleBotPartSocket Socket;
        public Rigidbody Rigidbody;
        public Collider Collider;
        public BattleBotPartType PartType;
        public AudioSource AudioSource;
        public AudioClip[] AudioClipsCollisions;
        public AudioClip[] AudioClipsAttach;
        public AudioClip[] AudioClipsDetach;
        public AudioClip[] AudioClipsError;

        public enum BattleBotPartType
        {
            Head = 0,
            ArmLeft = 1,
            ArmRight = 2,
            ShoulderLeft = 3,
            ShoulderRight = 4,
            Chest = 5,
            Bottom = 6,
        }

        private void OnCollisionEnter (Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 2f)
            {
                this.PlayCollisionSound (collision.relativeVelocity.magnitude / 10f);
            }
        }

        public void PlayCollisionSound (float volume)
        {
            // Play random collision sound

            if (this.AudioSource != null && this.AudioClipsCollisions != null && this.AudioClipsCollisions.Length > 0)
            {
                this.AudioSource.PlayOneShot (this.AudioClipsCollisions[Random.Range (0, this.AudioClipsCollisions.Length)], volume);
            }
        }

        public void PlayAttachSound ()
        {
            // Play random attach sound

            if (this.AudioSource != null && this.AudioClipsAttach != null && this.AudioClipsAttach.Length > 0)
            {
                this.AudioSource.PlayOneShot (this.AudioClipsAttach[Random.Range (0, this.AudioClipsAttach.Length)]);
            }
        }

        public void PlayDetachSound ()
        {
            // Play random detach sound

            if (this.AudioSource != null && this.AudioClipsAttach != null && this.AudioClipsAttach.Length > 0)
            {
                this.AudioSource.PlayOneShot (this.AudioClipsDetach[Random.Range (0, this.AudioClipsDetach.Length)]);
            }
        }

        public void PlayErrorSound ()
        {
            // Play random error sound

            if (this.AudioSource != null && this.AudioClipsError != null && this.AudioClipsError.Length > 0)
            {
                this.AudioSource.PlayOneShot (this.AudioClipsError[Random.Range(0, this.AudioClipsError.Length)]);
            }
        }

        #if UNITY_EDITOR

        private void OnValidate ()
        {
            // Cache the part's rigidbody

            this.Rigidbody = this.GetComponent <Rigidbody> ();

            UnityEngine.Assertions.Assert.IsNotNull (this.Rigidbody, "Missing rigidbody");

            // Cache the part's collider

            this.Collider = this.GetComponent <Collider> ();

            UnityEngine.Assertions.Assert.IsNotNull (this.Collider, "Missing collider");

            // Cache the socket's audio source

            this.AudioSource = this.GetComponent<AudioSource>();

            UnityEngine.Assertions.Assert.IsNotNull(this.AudioSource, $"{ this } Missing audio source");
        }

        #endif
    }
}