using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;

namespace BrokenBattleBots
{


    public class AuthorPlayer : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float MoveSpeed;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MovementInput() { Direction = new float3(0, 0, 1f), Magnitude = 0f });
            dstManager.AddComponentData(entity, new MovementSpeed() { Value = MoveSpeed });
            dstManager.AddComponentData(entity, new UsePlayerInput());
        }
    }
}