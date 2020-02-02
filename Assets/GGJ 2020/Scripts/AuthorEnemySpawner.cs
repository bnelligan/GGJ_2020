using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;

public class AuthorEnemySpawner : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject SpawnPrefab;
    public GameObject SpawnFrequency;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

    }
    
}
