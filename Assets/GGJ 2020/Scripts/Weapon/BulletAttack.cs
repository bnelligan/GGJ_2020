﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public TrailRenderer trail;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
            {
                // Swap the position of the cylinder.
                target.transform.position *= -1.0f;
            }
        } else {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        } else if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }
}
