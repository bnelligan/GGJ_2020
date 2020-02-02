using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace BrokenBattleBots
{
    /// <summary>
    /// Tag things that are dead
    /// </summary>
    struct Tag_Dead : IComponentData
    {

    }

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
        ComponentDataFromEntity<HealthDecay> decayFromEntity;
        
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
        }
        
    }

    /// <summary>
    /// Manage health repair system. Repair amount can be capped by adding a cap component
    /// </summary>
    [UpdateBefore(typeof(DamageSystem))]
    class HealthRepairSystem : ComponentSystem
    {
        ComponentDataFromEntity<RepairCap> repairCapFromEntity;
        ComponentDataFromEntity<Health> healthFromEntity;

        protected override void OnUpdate()
        {
            Entities.ForEach((ref Repair repair) =>
            {
                // Make sure target has HP
                if(healthFromEntity.HasComponent(repair.Target))
                {
                    // Check for a repair cap
                    Health health = healthFromEntity[repair.Target];
                    bool isRepairCapped = repairCapFromEntity.HasComponent(repair.Target);
                    int maxRepair = health.Max;
                    if (isRepairCapped)
                    {
                        maxRepair = repairCapFromEntity[repair.Target].MaxHealthFromRepair;
                    }
                    
                    // Perform repair
                    int newHp = health.Current + repair.Amount;
                    if(newHp > maxRepair)
                    {
                        newHp = maxRepair;
                    }
                    health.Current = newHp;
                }
            });
        }
    }


    class DamageSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }


}
