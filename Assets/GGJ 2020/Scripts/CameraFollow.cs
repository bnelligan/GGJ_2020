using System;
using System.Net;
using Unity.Entities;
using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Camera))]
    public class CameraFollow : MonoBehaviour
    {
        public Camera Camera;
        public Transform FollowTarget;
        public float CameraHeight = 19.43f;
        public float CameraFieldOfView = 32f;

        public bool useECS;

        private EntityManager dotManager;

        private void Start ()
        {
            dotManager = World.Active.EntityManager;
        }

        private void LateUpdate ()
        {
            if (!useECS)
            {
                Vector3 cameraPosition = this.transform.position;

                if (this.FollowTarget != null)
                {
                    cameraPosition = this.FollowTarget.position + new Vector3 (-5.67f, this.CameraHeight, -10.83f);
                }

                this.transform.position = Vector3.Lerp (this.transform.position, cameraPosition, Time.deltaTime * 3f);

                this.Camera.fieldOfView = Mathf.Lerp (this.Camera.fieldOfView, this.CameraFieldOfView, Time.deltaTime * 3f);
            }
        }

        #if UNITY_EDITOR

        private void OnValidate()
        {
            // Cache the camera

            this.Camera = this.GetComponent <Camera> ();
        }

        #endif
    }
}