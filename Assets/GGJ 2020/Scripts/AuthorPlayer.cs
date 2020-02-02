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
    public class AuthorPlayer : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float MoveSpeed;
        public int Health;
        public int DecayAmount;
        public int DecayInterval;




        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            EntityArchetype playerArchetype = dstManager.CreateArchetype(new ComponentType[] {
                typeof(MovementSpeed),
                typeof(MovementInput),
                typeof(UsePlayerInput),
                typeof(Health),
                typeof(HealthDecay),
                typeof(PlayerScore),
                typeof(RepairCap)
            });

            //dstManager.SetArchetype(entity, playerArchetype);
            dstManager.AddComponentData(entity, new MovementSpeed() {
                Value = MoveSpeed
            });
            dstManager.AddComponentData(entity, new MovementInput() {
                Direction = new float3(0, 0, 1f),
                Magnitude = 0f
            });
            dstManager.AddComponentData(entity, new UsePlayerInput() {
                PlayerID = 1
            });
            dstManager.AddComponentData(entity, new Health(){
                Current = Health
            });
            dstManager.AddComponentData(entity, new HealthDecay() {
                DecayAmount = DecayAmount,
                DecayInterval = DecayInterval,
                DecayTimer = DecayInterval
            });
            dstManager.AddComponentData(entity, new RepairCap() {
                MaxHealthFromRepair = Health
            });

        }
    }
}