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
    public class AuthorEnemy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float MoveSpeed;
        public int Health;
        public int RoamRadius = 25;
        public int MaxChaseDist = 20;
        public int AggroDist = 10;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            EntityArchetype enemyArchetype = dstManager.CreateArchetype(new ComponentType[]
            {
                typeof(MovementSpeed),
                typeof(Health),
                typeof(EnemyAI),
                typeof(MoveDestination),
                typeof(RoamRadius),
                typeof(ChaseTarget),
            });


            dstManager.AddComponentData(entity, new MovementSpeed()
            {
                Value = MoveSpeed
            });
            dstManager.AddComponentData(entity, new MoveDestination()
            {
                CloseEnoughThreshold = 0.25f,
                IsReached = true,
                Position = float3.zero
            });
            dstManager.AddComponentData(entity, new Health()
            {
                Current = Health,
                Max = Health
            });
            dstManager.AddComponentData(entity, new EnemyAI()
            {
                Ai_State = 1,
                Next_State = 1
            });
            dstManager.AddComponentData(entity, new RoamRadius()
            {
                Value = RoamRadius
            });
            dstManager.AddComponentData(entity, new ChaseTarget()
            {
                AggroDistance = AggroDist,
                MaxDistance = MaxChaseDist,
                Target = Entity.Null
            });
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}