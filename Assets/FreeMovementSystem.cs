using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets
{
    /// <summary>
    /// Component info for top-down free character movement input
    /// </summary>
    public struct FreeMovementInput : IComponentData
    {
        bool IsInputEnabled;
        float2 Direction;
        float Magnitude;
    }

    /// <summary>
    /// Component info for movement speed
    /// </summary>
    public struct MovementSpeed : IComponentData
    {
        float MoveSpeed;
    }

    class FreeMovementSystem : ComponentSystem
    {
        EntityQuery freeMovementInputQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            freeMovementInputQuery = GetEntityQuery(typeof(FreeMovementInput));
        }

        protected override void OnUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
