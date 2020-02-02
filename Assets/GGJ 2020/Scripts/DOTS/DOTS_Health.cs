using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;
using UnityEngine;

namespace BrokenBattleBots
{
    /// <summary>
    /// Tag things that are dead
    /// </summary>
    struct Tag_Dead : IComponentData { }

    /// <summary>
    /// Health data
    /// </summary>
    struct Health : IComponentData
    {
        public int Current;
        public int Max;
    }

    /// <summary>
    /// Health loss per turn
    /// </summary>
    struct HealthDecay : IComponentData
    {
        public int DecayAmount;
        public float DecayTimer;
        public float DecayInterval;
    }


    struct Damage : IComponentData { public int Amount; public Entity Target; }

    struct DamageSource : IComponentData { public Entity Source; }  // Needs implementation


    struct Repair : IComponentData { public int Amount; public Entity Target; }

    struct RepairCap : IComponentData { public int MaxHealthFromRepair; }

    /// <summary>
    /// Manage health decay system. Runs on the main thread
    /// </summary>
    [UpdateBefore(typeof(HealthRepairSystem))]
    class HealthDecaySystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            Entities.ForEach((ref Health hp, ref Tag_Generator gen) =>
            {
                switch (gen.ID)
                {
                    case 1:
                        GUI_Info.Gen1HP_Current = hp.Current;
                        GUI_Info.Gen1HP_Max = hp.Max;
                        break;
                    case 2:
                        GUI_Info.Gen2HP_Current = hp.Current;
                        GUI_Info.Gen2HP_Max = hp.Max;
                        break;
                    case 3:
                        GUI_Info.Gen3HP_Current = hp.Current;
                        GUI_Info.Gen3HP_Max = hp.Max;
                        break;
                }
            });

            Entities.ForEach((ref Health hp, ref UsePlayerInput player) =>
            {
                GUI_Info.PlayerHP_Current = hp.Current;
                GUI_Info.PlayerHP_Max = hp.Max;
            });

            //Entities.ForEach((ref Health hp, ref Tag_Generator gen) =>
            //{
            //    switch (gen.ID)
            //    {
            //        case 1:
            //            GUI_Info.Gen1HP_Current = hp.Current;
            //            GUI_Info.Gen1HP_Max = hp.Max;
            //            break;
            //        case 2:
            //            GUI_Info.Gen2HP_Current = hp.Current;
            //            GUI_Info.Gen2HP_Max = hp.Max;
            //            break;
            //        case 3:
            //            GUI_Info.Gen3HP_Current = hp.Current;
            //            GUI_Info.Gen3HP_Max = hp.Max;
            //            break;
            //    }
            //});
        }
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, ref HealthDecay decay) =>
            {
                decay.DecayTimer -= Time.DeltaTime;
                if(decay.DecayTimer < 0)
                {
                    // Decay timer elapsed, apply damage and reset the timer
                    decay.DecayTimer += decay.DecayInterval;
                    EntityArchetype dmgArchetype = EntityManager.CreateArchetype(typeof(Damage));                    
                    Entity dmgEntity = EntityManager.CreateEntity(dmgArchetype);
                    EntityManager.SetComponentData(dmgEntity, new Damage() { Target = e, Amount = decay.DecayAmount });
                }
            });

            Entities.ForEach((ref Health hp, ref Tag_Generator gen) =>
            {
                switch (gen.ID)
                {
                    case 1:
                        GUI_Info.Gen1HP_Current = hp.Current;
                        GUI_Info.Gen1HP_Max = hp.Max;
                        break;
                    case 2:
                        GUI_Info.Gen2HP_Current = hp.Current;
                        GUI_Info.Gen2HP_Max = hp.Max;
                        break;
                    case 3:
                        GUI_Info.Gen3HP_Current = hp.Current;
                        GUI_Info.Gen3HP_Max = hp.Max;
                        break;
                }
            });

            Entities.ForEach((ref Health hp, ref UsePlayerInput player) =>
            {
                GUI_Info.PlayerHP_Current = hp.Current;
                GUI_Info.PlayerHP_Max = hp.Max;
            });
        }
        
    }

    /// <summary>
    /// Manage health repair system. Repair amount can be capped by adding a cap component
    /// </summary>
    [UpdateBefore(typeof(DamageSystem))]
    class HealthRepairSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, ref Repair repair) =>
            {
                // Make sure target has HP
                if(EntityManager.HasComponent<Health>(repair.Target))
                {
                    // Check for a repair cap
                    Health health = EntityManager.GetComponentData<Health>(repair.Target);
                    bool isRepairCapped = EntityManager.HasComponent<RepairCap>(repair.Target);
                    int maxRepair = health.Max;
                    if (isRepairCapped)
                    {
                        maxRepair = EntityManager.GetComponentData<RepairCap>(repair.Target).MaxHealthFromRepair;
                    }
                    
                    // Perform repair
                    int newHp = health.Current + repair.Amount;
                    if(newHp > maxRepair)
                    {
                        newHp = maxRepair;
                    }
                    health.Current = newHp;
                }

                // Destroy repair entity
                EntityManager.DestroyEntity(e);
            });
        }
    }

    /// <summary>
    /// Apply damage to targets and destroy damage entities
    /// </summary>
    class DamageSystem : ComponentSystem
    {
        //public EntityCommandBuffer.Concurrent ecb_BeginSim;
        //public EntityCommandBuffer.Concurrent ecb_EndSim;

        //protected override void OnCreate()
        //{
        //    base.OnCreate();
        //    ecb_BeginSim = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>().CreateCommandBuffer().ToConcurrent();
        //    ecb_EndSim = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer().ToConcurrent();
           
        //}

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, ref Damage dmg) =>
            {
                Health hp = EntityManager.GetComponentData<Health>(dmg.Target);
                hp.Current -= dmg.Amount;
                if(hp.Current <= 0)
                {
                    hp.Current = 0;
                    EntityManager.AddComponentData(dmg.Target, new Tag_Dead());
                }
                EntityManager.DestroyEntity(e);
            });
        }
    }
    


}
