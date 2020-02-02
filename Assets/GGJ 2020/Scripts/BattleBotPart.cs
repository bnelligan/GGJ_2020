// Created: 2020/02/01
// Creator: Chris Nobrega

using UnityEngine;

namespace BrokenBattleBots
{
    public class BattleBotPart : MonoBehaviour
    {
        public float Welded { get; private set; }
        public BattleBotPartSocket Socket;
        public Rigidbody Rigidbody;
        public Collider Collider;
        public BattleBotPartType PartType;
        public AudioSource AudioSource;
        public float weldSpeedMultiplier = 1f;
        public Vector3 attachPosition;
        public Vector3 attachRotationEulerAngles;
        [Tooltip ("Applies to legs only")]
        public float standHeightOffset = 0f;

        public enum BattleBotPartType
        {
            Head = 0,
            Arm = 1,
            // ArmLeft = 1,
            // ArmRight = 2,
            ShoulderLeft = 3,
            ShoulderRight = 4,
            Chest = 5,
            Bottom = 6,
        }

        public void Weld (float value, bool reset = false)
        {
            this.Welded += value * this.weldSpeedMultiplier;

            if (this.Rigidbody != null)
            {
                this.Rigidbody.angularVelocity = Random.onUnitSphere;
            }
            else
            {
                // Shake the torso

                this.transform.root.GetComponent <Rigidbody> ().angularVelocity = Random.onUnitSphere * Random.Range (0f, 3f);
            }

            BattleBotCustomization.instance.DetachUnweldedParts (this);

            // Clamp weld value

            UnityEngine.Mathf.Clamp01 (this.Welded);

            if (reset == true)
            {
                this.Welded = 0f;
            }

            MeshRenderer[] meshRenderers = this.GetComponentsInChildren <MeshRenderer> ();

            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                // Color color = Color.Lerp (Color.black, Color.red, this.Welded / 10f);

                Color color = BattleBotCustomization.instance.WeldGradientPositive.Evaluate (this.Welded / 10f);

                meshRenderer.material.color = color;
            }
        }

        private void OnCollisionEnter (Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 1f)
            {
                this.PlayCollisionSound (collision.relativeVelocity.magnitude / 10f);
            }
        }

        public void PlayCollisionSound (float volume)
        {
            // Play random collision sound

            if (this.AudioSource != null && BattleBotCustomization.instance.AudioClipsCollisions != null && BattleBotCustomization.instance.AudioClipsCollisions.Length > 0)
            {
                this.AudioSource.PlayOneShot (BattleBotCustomization.instance.AudioClipsCollisions[Random.Range (0, BattleBotCustomization.instance.AudioClipsCollisions.Length)], volume);
            }
        }

        public void PlayAttachSound ()
        {
            // Play random attach sound

            if (this.AudioSource != null && BattleBotCustomization.instance.AudioClipsAttach != null && BattleBotCustomization.instance.AudioClipsAttach.Length > 0)
            {
                this.AudioSource.PlayOneShot (BattleBotCustomization.instance.AudioClipsAttach[Random.Range (0, BattleBotCustomization.instance.AudioClipsAttach.Length)]);
            }
        }

        public void PlayDetachSound ()
        {
            // Play random detach sound

            if (this.AudioSource != null && BattleBotCustomization.instance.AudioClipsAttach != null && BattleBotCustomization.instance.AudioClipsAttach.Length > 0)
            {
                this.AudioSource.PlayOneShot (BattleBotCustomization.instance.AudioClipsDetach[Random.Range (0, BattleBotCustomization.instance.AudioClipsDetach.Length)]);
            }
        }

        public void PlayErrorSound ()
        {
            // Play random error sound

            if (this.AudioSource != null && BattleBotCustomization.instance.AudioClipsError != null && BattleBotCustomization.instance.AudioClipsError.Length > 0)
            {
                this.AudioSource.PlayOneShot (BattleBotCustomization.instance.AudioClipsError[Random.Range(0, BattleBotCustomization.instance.AudioClipsError.Length)]);
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