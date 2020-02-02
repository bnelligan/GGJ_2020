using UnityEngine;

namespace BrokenBattleBots
{
    [RequireComponent (typeof (Camera))]
    public class CameraFollow : MonoBehaviour
    {
        public Camera Camera;
        public Transform[] FollowTargets;

        private void Awake()
        {
            BattleBotPart[] parts = FindObjectsOfType <BattleBotPart> ();

            if (this.FollowTargets == null || this.FollowTargets.Length == 0)
            {
                this.FollowTargets = new Transform[parts.Length];

                for (int index = 0; index < parts.Length; index += 1)
                {
                    this.FollowTargets[index] = parts[index].transform;
                }
            }
        }

        private void LateUpdate ()
        {
            float furthestPartFromCameraDistance = float.MinValue;

            Vector3 averagePosition = Vector3.zero;

            foreach (Transform followTarget in this.FollowTargets)
            {
                averagePosition += followTarget.transform.position;

                Vector3 a = followTarget.transform.position;
                a.y = 0f;
                Vector3 b = this.Camera.transform.position;
                a.z = 0f;

                float distance = Vector3.Distance (a, b);

                if (distance > furthestPartFromCameraDistance)
                {
                    furthestPartFromCameraDistance = distance;
                }
            }

            // Scale camera field of view based on the furthest part (from the camera)

            this.Camera.fieldOfView = UnityEngine.Mathf.Lerp (this.Camera.fieldOfView, furthestPartFromCameraDistance * 1.4f, UnityEngine.Time.deltaTime * 3f);

            averagePosition /= (float) this.FollowTargets.Length;

            // Move the camera based on the center of all the parts

            Vector3 cameraPosition = averagePosition + new Vector3 (-5.67f, 19.43f, -10.83f);

            // cameraPosition.y = 19.43f;

            this.Camera.transform.position = Vector3.Lerp (this.Camera.transform.position, cameraPosition, UnityEngine.Time.deltaTime * 3f);
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