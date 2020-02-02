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

        public int RotationFriction;
        public int RotationSmoothness;
        public int RotationSpeed;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            EntityArchetype playerArchetype = dstManager.CreateArchetype(new ComponentType[] {
                typeof(MovementSpeed),
                typeof(MovementInput),
                typeof(UsePlayerInput),
                typeof(Health),
                typeof(HealthDecay),
                typeof(PlayerScore),
                // typeof(RepairCap),
                typeof(AimInput),
                // typeof(AimSettings),
                typeof(EnemyTarget)
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
                Current = Health,
                Max = 100
            });
            dstManager.AddComponentData(entity, new HealthDecay() {
                DecayAmount = DecayAmount,
                DecayInterval = DecayInterval,
                DecayTimer = DecayInterval
            });
            dstManager.AddComponentData(entity, new RepairCap() {
                MaxHealthFromRepair = Health
            });
            dstManager.AddComponentData(entity, new AimInput()
            {
                AimPoint = Vector3.zero
            });
            //dstManager.AddComponentData(entity, new AimSettings()
            //{
            //    RotationFriction = RotationFriction,
            //    RotationSmoothness = RotationSmoothness,
            //    RotationSpeed = RotationSpeed
            //});
            dstManager.AddComponentData(entity, new EnemyTarget()
            {
                IsPriority = true
            });
            

        }
    }
}