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
            countdown.TimeLeft -= Time.DeltaTime;
            if(countdown.TimeLeft <= 0)
            {
                EntityManager.AddComponent(e, typeof(Tag_CountdownElapsed));
            }
        });
    }
}

[UpdateAfter(typeof(CountdownSystem))]
public class SpawnSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref Tag_CountdownElapsed countdownElapsed, ref SpawnPoint spawnInfo, ref SpawnInterval spawnInterval, ref Countdown countdown) =>
        {
            EntityManager.Instantiate(spawnInfo.Spawn);
            spawnInterval.Value = spawnInterval.Value * 0.95f;
        });

        Entities.ForEach((Entity e, ref Tag_CountdownElapsed countdownElapsed, ref SpawnPoint spawnInfo, ref SpawnInterval interval, ref Countdown countdown) =>
        {
            EntityManager.RemoveComponent(e, typeof(Tag_CountdownElapsed));
            countdown.TimeLeft += interval.Value;
            if (countdown.TimeLeft < 0)
            {
                // just in case 
                countdown.TimeLeft = interval.Value;
            }
        });

        //spawnAfterCountdownJob.Schedule(this, inputDeps).Complete();
        //ResetCountdownJob resetCountdownJob = new ResetCountdownJob()
        //{
        //    ecb = ecb_EndSim.CreateCommandBuffer()
        //};
        //resetCountdownJob.Schedule(this, inputDeps).Complete();
        
        
        //return inputDeps;
    }

    //[RequireComponentTag(typeof(Tag_CountdownElapsed))]
    //struct SpawnAfterCountdown : IJobForEachWithEntity<SpawnPoint, SpawnInterval, Countdown>
    //{
    //    [ReadOnly] public EntityCommandBuffer ecb;
    //    public void Execute(Entity entity, int index, [ReadOnly] ref SpawnPoint spawnInfo, [ReadOnly] ref SpawnInterval interval, ref Countdown countdown)
    //    {
            
    //        // Create spawn entity
    //        ecb.Instantiate(spawnInfo.Spawn);
    //        // Reduce spawn interval by 5% each time
    //        interval.Value = interval.Value * 0.95f;
            
    //    }
    //}

    //[RequireComponentTag(typeof(Tag_CountdownElapsed))]
    //struct ResetCountdownJob : IJobForEachWithEntity<SpawnPoint, SpawnInterval, Countdown>
    //{
    //    [ReadOnly] public EntityCommandBuffer ecb;
    //    public void Execute(Entity entity, int index, [ReadOnly] ref SpawnPoint spawnInfo, [ReadOnly] ref SpawnInterval interval, ref Countdown countdown)
    //    {
    //        // Remove countdown tag and reset timer
    //        ecb.RemoveComponent(entity, typeof(Tag_CountdownElapsed));
    //        countdown.TimeLeft += interval.Value;
    //        if (countdown.TimeLeft < 0)
    //        {
    //            // just in case 
    //            countdown.TimeLeft = interval.Value;
    //        }
            
    //    }
    //}

}
 


