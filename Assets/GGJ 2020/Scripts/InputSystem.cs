using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Component info for top-down free character movement input
    /// </summary>
    public struct MovementInput : IComponentData
    {
        public float2 Direction;
        public float Magnitude;
    }

    /// <summary>
    /// Tag entities to use player input
    /// </summary>
    public struct UsePlayerMovement : IComponentData
    {
        // Nothing, just a tag
    }



    /// <summary>
    /// Component info for movement speed
    /// </summary>
    public struct MovementSpeed : IComponentData
    {
        public float Max;
        public float Value;
    }
    

    /// <summary>
    /// Read game input and transfer it to the player entities
    /// </summary>
    class PlayerMoveInputSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float2 rawMoveInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var applyInputJob = new ApplyMoveInput()
            {
                rawMoveAxis = rawMoveInput
            };
            return applyInputJob.Schedule(this, inputDeps);
        }

        
        [RequireComponentTag(typeof(UsePlayerMovement))]
        struct ApplyMoveInput : IJobForEach<MovementInput>
        {
            public float2 rawMoveAxis;
            public void Execute(ref MovementInput inputData)
            {
                inputData.Direction = math.normalize(new float2(rawMoveAxis));
                inputData.Magnitude = math.max(rawMoveAxis.x, rawMoveAxis.y);

            }
        }
    }

    [UpdateAfter(typeof(PlayerMoveInputSystem))]
    class ApplyFreeMovementSystem : ComponentSystem
    {
        EntityQuery freeMovementInputQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
        }
    }
   
}
