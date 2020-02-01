using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;


struct EntityPosition : IComponentData
{
    public float3 pos;
}

struct TimingSettings : IComponentData
{
    public float Timer;
    public float TimeVariation;
}

struct Timing : IComponentData
{
    public float TimeToZero;
}

struct EntityInstantiation
{
    public Prefab EntityPrefab;
}

public struct EnemySpawnTag : IComponentData
{
    // Nothing, just a tag
}


public class SpawnSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float timeSetting = 2.5f;
        float timeVarSetting = .5f;
        var applyInputJob = new CalculateTiming()
        {
            ts = new TimingSettings { Timer = timeSetting, TimeVariation = timeVarSetting }
        };
        return applyInputJob.Schedule(this, inputDeps);
    }

    [RequireComponentTag(typeof(EnemySpawnTag))]
    struct CalculateTiming : IJobForEach<Timing>
    {
        public TimingSettings ts;
        public void Execute(ref Timing inputData)
        {
            inputData.TimeToZero = ts.Timer + UnityEngine.Random.Range(ts.Timer - ts.TimeVariation, ts.Timer + ts.TimeVariation);
        }
    }
}
