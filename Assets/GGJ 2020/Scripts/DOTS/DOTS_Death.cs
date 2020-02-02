using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;
using UnityEngine;


namespace BrokenBattleBots
{
    struct Tag_Generator : IComponentData{ public int ID; }

    
    class DeathSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, ref Tag_Dead dead, ref Tag_Generator deadGenerator) =>
            {
                Debug.Log("Dead Gen");
                // EntityManager.DestroyEntity(e);
            
            });
        }
    }
    
}

