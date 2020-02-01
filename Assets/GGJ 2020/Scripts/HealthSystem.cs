using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

namespace BrokenBattleBots
{
    // Tag things that are dead
    struct Tag_Dead : IComponentData
    {

    }

    struct Health : IComponentData
    {
        float Value;
        float Max;
    }

    /// <summary>
    /// Health loss per turn
    /// </summary>
    struct HealthDecay : IComponentData
    {
        float DecayAmount;
        float DecayTimer;
    }

    /// <summary>
    /// Damage taken by an entity. Removed after it is applied to Health.
    /// </summary>
    struct Damage : IComponentData { float Value; }

    //class HealthSystem : JobComponentSystem
    //{
    //    EndSimulationEntityCommandBufferSystem ecb_EndSim;

    //    protected override void OnCreate()
    //    {
    //        base.OnCreate();
    //        ecb_EndSim = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    //    }

    //    protected override JobHandle OnUpdate(JobHandle inputDeps)
    //    {

    //    }
        
    //    struct ApplyDamageJob : IJobForEach<Health, Damage>
        
    //}
}
