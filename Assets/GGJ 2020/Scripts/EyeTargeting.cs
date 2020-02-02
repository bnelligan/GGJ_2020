using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class EyeTargeting : MonoBehaviour
{
    GameObject _LastEyeTarget;
    public Rigidbody _projectile;
    float _projectileSpeed;
    public Transform _prjSpawn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TobiiAPI.GetFocusedObject() != null)
        {
            Debug.Log(TobiiAPI.GetFocusedObject().name);
            TobiiAPI.GetDisplayInfo();
            _LastEyeTarget = TobiiAPI.GetFocusedObject();

            Debug.Log(TobiiAPI.GetGazePoint().Screen);

        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit, raycastHit;
            Ray ray = Camera.main.ViewportPointToRay(TobiiAPI.GetGazePoint().Screen);
            //Ray ray = new Ray(Camera.main.transform.position, TobiiAPI.GetGazePoint().Screen);
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawLine(ray.origin, hit.point);
                Fire(_prjSpawn, _LastEyeTarget.transform.position);
            }

        }
    }

    void Fire(Transform spawn, Vector3 target)
    { 
        Rigidbody projectileClone = (Rigidbody)Instantiate(_projectile, spawn.transform.position, spawn.transform.rotation);
        projectileClone.velocity = transform.forward * _projectileSpeed;
    }
}
