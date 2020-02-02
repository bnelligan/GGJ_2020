using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;
using Unity.Burst;

namespace BrokenBattleBots
{
    public struct EnemyAI : IComponentData
    {
        // 0 - Invalid
        // 1 - Roam
        // 2 - Chase
        public uint Ai_State;
        public uint Next_State;
    }

    public struct MoveDestination : IComponentData
    {
        public float3 Position;
        public float CloseEnoughThreshold;
        public bool IsReached;
    }

    public struct RoamRadius : IComponentData
    {
        public float Value;
    }

    public struct ChaseTarget : IComponentData
    {
        public Entity Target;
        public float AggroDistance;
        public float MaxDistance;
    }

    public struct EnemyTarget : IComponentData
    {
        public bool IsPriority;
    }
    

    public class EnemyAiSystem : JobComponentSystem
    {
        Unity.Mathematics.Random rand;
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            rand = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(0, 100000));
            EntityQuery possibleTargetsQuery = GetEntityQuery(typeof(EnemyTarget), typeof(Translation));
            AiStateUpdate updateStatesJob = new AiStateUpdate()
            {
                potentialTargets = possibleTargetsQuery.ToEntityArray(Allocator.Persistent, out inputDeps),
                translationFromEntity = GetComponentDataFromEntity<Translation>(true),
                tagDeadFromEntity = GetComponentDataFromEntity<Tag_Dead>(true),
                enemyTargetFromEntity = GetComponentDataFromEntity<EnemyTarget>(true),
                rng = rand
            };
            // possibleTargetsQuery.Dispose();
            updateStatesJob.Schedule(this, inputDeps).Complete();
            updateStatesJob.potentialTargets.Dispose();
            return inputDeps;
        }

        
        struct AiStateUpdate : IJobForEachWithEntity<EnemyAI, Translation, RoamRadius, ChaseTarget, MoveDestination>
        {
            [ReadOnly] public ComponentDataFromEntity<Translation> translationFromEntity;
            [ReadOnly] public ComponentDataFromEntity<Tag_Dead> tagDeadFromEntity;
            [ReadOnly] public ComponentDataFromEntity<EnemyTarget> enemyTargetFromEntity;
            [ReadOnly] public NativeArray<Entity> potentialTargets;
            public Unity.Mathematics.Random rng;

            public void Execute(Entity entity, int index, ref EnemyAI ai, [ReadOnly] ref Translation position, 
                [ReadOnly] ref RoamRadius roamBounds, ref ChaseTarget chase, ref MoveDestination moveDest)
            {
                // Handle state change
                if (ai.Next_State != 0)
                {
                    ai.Ai_State = ai.Next_State;
                    ai.Next_State = 0;
                }

                // Check for a valid chase target
                switch(ai.Ai_State)
                {
                    // Roam State
                    case 1:
                        // Check if reached destination
                        if(moveDest.IsReached)
                        {
                            // Choose next destination
                            moveDest.Position = GetNewRoamPoint(roamBounds.Value);
                            moveDest.IsReached = false;
                        }

                        // Aggro check
                        Entity aggroTarget = Entity.Null;
                        float bestSqDist = chase.MaxDistance * chase.MaxDistance;
                        bool hasAggro = false;
                        foreach (Entity e in potentialTargets)
                        {
                            if (translationFromEntity.HasComponent(e) && !tagDeadFromEntity.HasComponent(e))
                            {
                                float sqDist = math.length(position.Value - translationFromEntity[e].Value);
                                if (sqDist <= chase.AggroDistance * chase.AggroDistance)
                                {
                                    hasAggro = true;
                                    if (!hasAggro)
                                    {
                                        aggroTarget = e;
                                        bestSqDist = sqDist;
                                    }
                                    else if(sqDist < bestSqDist || enemyTargetFromEntity[e].IsPriority)
                                    {
                                        aggroTarget = e;
                                    }
                                }
                            }
                        }
                        if (hasAggro)
                        {
                            ai.Next_State = 2;
                            chase.Target = aggroTarget;
                        }
                        break;

                    // Chase State
                    case 2:
                        // Check if target is dead
                        if (tagDeadFromEntity.HasComponent(chase.Target) || !translationFromEntity.HasComponent(chase.Target))
                        {
                            // Target dead, Switch to roaming
                            ai.Next_State = 1;
                            chase.Target = Entity.Null;
                        }
                        else if(sqDist(translationFromEntity[chase.Target].Value, position.Value) > chase.MaxDistance * chase.MaxDistance)
                        {
                            // Target excaped, switch to roaming
                            ai.Next_State = 1;
                            chase.Target = Entity.Null;
                        }
                        else
                        {
                            moveDest.Position = translationFromEntity[chase.Target].Value;
                        }
                        break;
                }
            }
            
            private float sqDist(float3 v1, float3 v2)
            {
                return math.lengthsq(v1 - v2);
            }
            
            private float3 GetNewRoamPoint(float maxDist)
            {
                float distFromCenter = rng.NextFloat(0, maxDist);
                float degree = rng.NextFloat(0f, 2f * math.PI);
                return new float3(math.cos(degree) * distFromCenter, 0, math.sin(degree) * distFromCenter);
            }
        }
        
    }

    //public class EnemyAttackSystem : ComponentSystem
    //{

    //}
}