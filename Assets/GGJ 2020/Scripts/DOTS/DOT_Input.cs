using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace BrokenBattleBots
{
    /// <summary>
    /// Component info for top-down free character movement input
    /// </summary>
    public struct MovementInput : IComponentData
    {
        public float3 Direction;
        public float Magnitude;
    }

    /// <summary>
    /// Aim point in world space
    /// </summary>
    public struct AimInput : IComponentData
    {
        public float3 AimPoint;
    }

    /// <summary>
    /// Aim settings for the player
    /// </summary>
    public struct AimSettings : IComponentData
    {
        public float RotationFriction;
        public float RotationSpeed;
        public float RotationSmoothness;
    }


    /// <summary>
    /// Tag entities to use player input
    /// </summary>
    public struct UsePlayerInput : IComponentData
    {
        public int PlayerID;
    }



    /// <summary>
    /// Component info for movement speed
    /// </summary>
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
    

    /// <summary>
    /// Read game input and transfer it to the player entities
    /// </summary>
    class PlayerInputSystem : JobComponentSystem
    {
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float2 rawMoveInput = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var readMoveJob = new PlayerMoveInputJob()
            {
                RawMoveAxis = rawMoveInput
            };
            Vector3 aimPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            var readAimJob = new PlayerAimInputJob()
            {
                AimPoint = new float3(aimPoint.x, aimPoint.y, aimPoint.z)
            };

            return readMoveJob.Schedule(this, inputDeps);
        }

        /// <summary>
        /// Apply player move input to entities
        /// </summary>
        [RequireComponentTag(typeof(UsePlayerInput))]
        struct PlayerMoveInputJob : IJobForEach<MovementInput>
        {
            public float2 RawMoveAxis;
            public void Execute(ref MovementInput inputData)
            {
                float3 moveAxis = new float3(RawMoveAxis.x, 0, RawMoveAxis.y);
                if(math.length(moveAxis) > 0)
                {
                    moveAxis = math.normalize(moveAxis);
                }
                else
                {
                    moveAxis = new float3(0, 0, 1);
                }
                inputData.Direction = math.normalize(new float3(RawMoveAxis.x, 0, RawMoveAxis.y)); // vertical = forward, horiz = right
                inputData.Magnitude = math.max(math.abs(RawMoveAxis.x), math.abs(RawMoveAxis.y));
            }
        }

        /// <summary>
        /// Apply player aim input to entities
        /// </summary>
        [RequireComponentTag(typeof(UsePlayerInput))]
        struct PlayerAimInputJob : IJobForEach<AimInput>
        {
            public float3 AimPoint;
            public void Execute(ref AimInput aim)
            {
                aim.AimPoint = AimPoint;
            }
        }

    }

    /// <summary>
    /// Read move input and apply it to the physics system
    /// </summary>
    [UpdateAfter(typeof(PlayerInputSystem))]
    class ApplyMoveInputSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ApplyMoveInputJob job = new ApplyMoveInputJob();
            return job.Schedule(this, inputDeps);
        }

        struct ApplyMoveInputJob : IJobForEach<PhysicsVelocity, Rotation, MovementSpeed, MovementInput>
        {
            public void Execute(ref PhysicsVelocity velocity, ref Rotation rotation, ref MovementSpeed moveSpeed, ref MovementInput input)
            {
                // Velocity scales with input and move speed
                if(math.length(input.Direction) > 0)
                {
                    velocity.Linear = input.Direction * input.Magnitude * moveSpeed.Value;
                    rotation.Value = Quaternion.Euler(input.Direction);
                }
                else
                {
                    velocity.Linear = float3.zero;
                }
            }
        }
    }
   
}
