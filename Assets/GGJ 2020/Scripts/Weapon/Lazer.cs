﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Vector3 hit;
    public float speed;
    public float lazerTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        if (hit != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, hit, step);
        }
        else
        {
            Destroy(gameObject);
        }


        if (lazerTime > 0)
        {
        lazerTime -= Time.deltaTime;
        }
        else if (lazerTime <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }
}
