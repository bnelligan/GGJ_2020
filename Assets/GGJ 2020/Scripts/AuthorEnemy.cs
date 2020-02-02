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
        public float MoveSpeed = 5;
        public int Health = 100;
        public int RoamRadius = 25;
        public int MaxChaseDist = 20;
        public int AggroDist = 10;
        public float AttackDelay = 2;
        public int AttackDamage = 15;
        public int AttackRange = 5;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            EntityArchetype enemyArchetype = dstManager.CreateArchetype(new ComponentType[]
            {
                typeof(MovementSpeed),
                typeof(Health),
                typeof(EnemyAI),
                typeof(MoveDestination),
                typeof(RoamRadius),
                typeof(AggroInfo),
                typeof(Rotation),
                typeof(AimInput),
                typeof(Countdown)
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
            dstManager.AddComponentData(entity, new AggroInfo()
            {
                AggroDistance = AggroDist,
                MaxDistance = MaxChaseDist,
                Target = Entity.Null,
                AttackDamage = AttackDamage,
                AttackFrequency = AttackDelay,
                AttackRange = AttackRange
            });
            dstManager.AddComponentData(entity, new AimInput()
            {
                AimPoint = float3.zero
            });
            dstManager.AddComponentData(entity, new Countdown()
            {
                TimeLeft = AttackDelay
            });
        }
        
    }
}