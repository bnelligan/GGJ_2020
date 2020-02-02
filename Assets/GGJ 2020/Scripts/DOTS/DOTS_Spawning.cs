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

struct SpawnPoint : IComponentData
{
    public float3 Location;
    public Entity Spawn;
}
struct SpawnInterval : IComponentData
{
    public float Value;
}

struct Countdown : IComponentData
{
    public float TimeLeft;
}

struct Tag_CountdownElapsed : IComponentData { }

public class CountdownSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref Countdown countdown) =>
        {
            if(countdown.TimeLeft <= 0)
            {
                EntityManager.AddComponent(e, typeof(Tag_CountdownElapsed));
            }
        });
    }
}

[UpdateAfter(typeof(CountdownSystem))]
public class SpawnSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem ecb_EndSim;

    protected override void OnCreate()
    {
        base.OnCreate();
        ecb_EndSim = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        SpawnAfterCountdown spawnAfterCountdownJob = new SpawnAfterCountdown()
        {
            ecb = ecb_EndSim.CreateCommandBuffer().ToConcurrent()
        };
        inputDeps = spawnAfterCountdownJob.Schedule(this, inputDeps);
        return inputDeps;
    }

    [RequireComponentTag(typeof(Tag_CountdownElapsed))]
    struct SpawnAfterCountdown : IJobForEachWithEntity<SpawnPoint, SpawnInterval, Countdown>
    {
        public EntityCommandBuffer.Concurrent ecb;
        public void Execute(Entity entity, int index, [ReadOnly] ref SpawnPoint spawnInfo, [ReadOnly] ref SpawnInterval interval, ref Countdown countdown)
        {
            // Remove countdown tag and reset timer
            ecb.RemoveComponent(index, entity, typeof(Tag_CountdownElapsed));
            countdown.TimeLeft += interval.Value;
            if(countdown.TimeLeft < 0)
            {
                // just in case 
                countdown.TimeLeft = interval.Value;
            }
            // Create spawn entity
            ecb.Instantiate(index, entity);
            // Reduce spawn interval by 5% each time
            interval.Value = interval.Value * 0.95f;
        }
    }

}
 


