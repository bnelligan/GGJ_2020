using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class WeldDraw : MonoBehaviour
{
    public GameObject weldPrefab;
    public TrailRenderer weldLine;
    public Camera screenSpace;
    public ParticleSystem sparks;
    public Transform sparksLocaiton;
    
    private GameObject currentWeld;
    private Vector3 mouseLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(mouseLocation);

        if (Input.GetMouseButtonDown(0))
        {
            mouseLocation =
                screenSpace.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
           SpawnTrail();
           sparksLocaiton.position = mouseLocation;
           sparks.Play();
        }
        else if (Input.GetMouseButton(0))
        {
            mouseLocation =
                screenSpace.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            sparksLocaiton.position = mouseLocation;
            if (currentWeld)
            {
                currentWeld.transform.position = mouseLocation;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            sparks.Stop();
            RemoveTrail();
        }
    }

    void SpawnTrail()
    {
        GameObject weldObj = Instantiate(weldPrefab, mouseLocation, Quaternion.identity);
        currentWeld = weldObj;
        weldLine = currentWeld.GetComponent<TrailRenderer>();
    }

    void RemoveTrail()
    {
        
    }
}
