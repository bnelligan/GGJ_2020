using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _Speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

   

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.
        Vector3.MoveTowards(gameObject.transform.position, gameObject.transform.forward, 10);
    }
}
