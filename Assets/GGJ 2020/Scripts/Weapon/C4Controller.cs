﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4Controller : MonoBehaviour
{
    public GameObject c4;
    public int c4Limit;
    public List<GameObject> c4List = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && c4List.Count < c4Limit)
        {
            GameObject OBJc4 = Instantiate(c4, transform.position, Quaternion.identity);
            c4List.Add(OBJc4);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (var item in c4List)
            {
                item.GetComponent<C4>().TimeToExplode = true;
            }
            for (int i = c4List.Count-1; i >= 0; i--)
            {
                c4List.RemoveAt(i);
            }
        }

    }
}