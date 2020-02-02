using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Collections;
using Unity.Burst;
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
    //public struct AimSettings : IComponentData
    //{
    //    public float RotationFriction;
    //    public float RotationSpeed;
    //    public float RotationSmoothness;
    //}


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
            // Move input
            float2 rawMoveInput = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var readMoveJob = new PlayerMoveInputJob()
            {
                RawMoveAxis = rawMoveInput
            };
            inputDeps = readMoveJob.Schedule(this, inputDeps);

            // Aim input
            Vector3 aimPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            // Debug.Log("AimPoint: " + aimPoint);
            var readAimJob = new PlayerAimInputJob()
            {
                AimPoint = new float3(aimPoint.x, 0, aimPoint.z)
            };
            inputDeps = readAimJob.Schedule(this, inputDeps);
            return inputDeps;
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

    [UpdateAfter (typeof (PlayerInputSystem))]
    [UpdateAfter (typeof (EnemyAiSystem))]
    [UpdateAfter (typeof (ApplyMoveInputSystem))]
    [UpdateAfter (typeof (ApplyAimInputSystem))]
    class ApplyCameraFollowSystem : ComponentSystem
    {
        public ApplyCameraFollowSystem ()
        {

        }

        protected override void OnUpdate ()
        {
            Entities.ForEach ((ref Translation translate, ref UsePlayerInput player) =>
            {
                CameraFollow.Instance.UpdateTargetPosition (translate.Value);
            });
        }
    }

    /// <summary>
    /// Read move input and apply it to the physics system
    /// </summary>
    [UpdateAfter(typeof(PlayerInputSystem))]
    [UpdateAfter(typeof(EnemyAiSystem))]
    class ApplyMoveInputSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ApplyMoveInputJob moveInputJob = new ApplyMoveInputJob();
            ApplyMovePositionJob movePositionJob = new ApplyMovePositionJob();

            inputDeps = movePositionJob.Schedule(this, inputDeps);
            return moveInputJob.Schedule(this, inputDeps);
        }

        [ExcludeComponent(typeof(MoveDestination))]
        struct ApplyMoveInputJob : IJobForEach<PhysicsVelocity, MovementSpeed, MovementInput>
        {
            public void Execute(ref PhysicsVelocity velocity, ref MovementSpeed moveSpeed, ref MovementInput input)
            {
                // Velocity scales with input and move speed
                if(math.length(input.Direction) > 0)
                {
                    velocity.Linear = input.Direction * input.Magnitude * moveSpeed.Value;
                }
                else
                {
                    velocity.Linear = float3.zero;
                }
            }
        }

        [ExcludeComponent(typeof(MovementInput))]
        struct ApplyMovePositionJob : IJobForEach<Translation, PhysicsVelocity, MovementSpeed, MoveDestination>
        {
            public void Execute([ReadOnly] ref Translation translation, ref PhysicsVelocity velocity, ref MovementSpeed speed, ref MoveDestination dest)
            {
                float3 toDestVec = new float3(dest.Position.x, 0, dest.Position.z) - new float3(translation.Value.x, 0f, translation.Value.z);

                float mag = math.length(toDestVec);
                if (mag <= dest.CloseEnoughThreshold)
                {
                    dest.IsReached = true;
                    toDestVec = float3.zero;
                    velocity.Linear = float3.zero;
                }
                else
                {
                    dest.IsReached = false;
                    velocity.Linear = math.normalize(toDestVec) * speed.Value;
                }
            }
        }

    }
   
    /// <summary>
    /// Read aim input and apply to the aim input sus
    /// </summary>
    [UpdateAfter(typeof(ApplyMoveInputSystem))]
    class ApplyAimInputSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ApplyAimInputJob aimJob = new ApplyAimInputJob();
            return aimJob.Schedule(this, inputDeps);
        }

        struct ApplyAimInputJob : IJobForEach<Rotation, Translation, AimInput>
        {
            public void Execute(ref Rotation rotation, [ReadOnly] ref Translation translation, [ReadOnly] ref AimInput input)
            {
                float3 look = input.AimPoint - new float3(translation.Value.x, 0f, translation.Value.z);
                quaternion rot = Quaternion.LookRotation(look);
                
                rotation.Value = math.normalize(new quaternion(0, rot.value.y, 0, rot.value.w));
            }
        }

    }
}
