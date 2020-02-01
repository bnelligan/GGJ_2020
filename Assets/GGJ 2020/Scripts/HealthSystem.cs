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

    struct HealthDecay : IComponentData
    {
        float DecayAmount;
        float DecayTimer;
    }

    class HealthSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

        }
    }
}
