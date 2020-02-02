﻿using System.Collections;
using System.Collections.Generic;
using BrokenBattleBots;
using Unity.Entities;
using UnityEngine;

public class AuthorGenerator : MonoBehaviour, IConvertGameObjectToEntity
{
    public int Health = 100;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        EntityArchetype generatorArchetype = dstManager.CreateArchetype(new ComponentType[]
        {
            typeof(Health)
        });
        
        dstManager.AddComponentData(entity, new Health(){
            Current = Health,
            Max = Health
        });

        dstManager.AddComponentData(entity, new Tag_Generator());
    }
}