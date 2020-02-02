using System;
using System.Net;
using Unity.Entities;
using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Camera))]
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance;
        public Vector3 TargetPosition;
        public bool IsRobotStanding;
        public Camera Camera;

        public bool useECS;

        private EntityManager dotManager;

        #if UNITY_EDITOR

        private void OnValidate()
        {
            // Cache the camera

            this.Camera = this.GetComponent <Camera> ();
        }

        #endif

        private void Awake()
        {
            Instance = this;
        }

        private void Start ()
        {
            dotManager = World.Active.EntityManager;
        }

        public void UpdateTargetPosition (Vector3 targetPosition)
        { 
            if (this.IsRobotStanding == true)
            {
                this.TargetPosition = targetPosition;
            }
        }

        private void LateUpdate ()
        {
            if (this.useECS == false)
            {
                float cameraHeight = 19.43f;

                float cameraFieldOfView = 32f;

                if (this.IsRobotStanding == true)
                {
                    cameraHeight = 19.43f;

                    cameraFieldOfView = 64f;
                }

                Vector3 cameraPosition = this.TargetPosition + new Vector3 (-5.67f, cameraHeight, -10.83f);

                this.transform.position = Vector3.Lerp (this.transform.position, cameraPosition, Time.deltaTime * 3f);

                this.Camera.fieldOfView = Mathf.Lerp (this.Camera.fieldOfView, cameraFieldOfView, Time.deltaTime * 3f);
            }
        }
    }
}