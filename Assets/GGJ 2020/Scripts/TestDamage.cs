using System;
using System.Collections;
using System.Collections.Generic;
using BrokenBattleBots;
using Unity.Entities;
using UnityEngine;

public class TestDamage : MonoBehaviour, IConvertGameObjectToEntity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Generator"))
        {
            Debug.Log("Hit Generator");
        }
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
      
    }
}
