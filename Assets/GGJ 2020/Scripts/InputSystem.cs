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
    /// Tag entities to use player input
    /// </summary>
    public struct UsePlayerInput : IComponentData
    {
        // Nothing, just a tag
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
    class ReadPlayerInputSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float2 rawMoveInput = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var applyInputJob = new ReadMoveInputJob()
            {
                rawMoveAxis = rawMoveInput
            };
            return applyInputJob.Schedule(this, inputDeps);
        }

        
        [RequireComponentTag(typeof(UsePlayerInput))]
        struct ReadMoveInputJob : IJobForEach<MovementInput>
        {
            public float2 rawMoveAxis;
            public void Execute(ref MovementInput inputData)
            {
                inputData.Direction = math.normalize(new float3(rawMoveAxis.x, 0, rawMoveAxis.y)); // vertical = forward, horiz = right
                inputData.Magnitude = math.max(math.abs(rawMoveAxis.x), math.abs(rawMoveAxis.y));
            }
        }
    }

    /// <summary>
    /// Read move input and apply it to the physics system
    /// </summary>
    [UpdateAfter(typeof(ReadPlayerInputSystem))]
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
