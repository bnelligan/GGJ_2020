using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;

namespace BrokenBattleBots
{
    public class AuthorEnemySpawner : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameObject SpawnPrefab;
        public float SpawnFrequency;
        public Vector3 SpawnOffset;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            var enemyEntity = conversionSystem.GetPrimaryEntity(SpawnPrefab);


            EntityArchetype spawnerArchetype = dstManager.CreateArchetype(new ComponentType[] {
                typeof(SpawnPoint),
                typeof(SpawnInterval),
                typeof(Countdown) 
            });

            dstManager.AddComponentData(entity, new SpawnPoint() {
                Location = transform.position + SpawnOffset,
                Spawn = enemyEntity
            });
            dstManager.AddComponentData(entity, new SpawnInterval()
            {
                Value = SpawnFrequency
            });
            dstManager.AddComponentData(entity, new Countdown()
            {
                TimeLeft = SpawnFrequency
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> prefabs)
        {
            prefabs.Add(SpawnPrefab);
        }

    }
}