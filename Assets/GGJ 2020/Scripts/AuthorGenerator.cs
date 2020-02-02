using System.Collections;
using System.Collections.Generic;
using BrokenBattleBots;
using Unity.Entities;
using UnityEngine;

public class AuthorGenerator : MonoBehaviour, IConvertGameObjectToEntity
{
    public int Health;
    public int GenID = 1;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        EntityArchetype generatorArchetype = dstManager.CreateArchetype(new ComponentType[]
        {
            typeof(Health),
            typeof(Tag_Generator)
        });
        
        dstManager.AddComponentData(entity, new Health(){
            Current = Health,
            Max = 100
        });

        dstManager.AddComponentData(entity, new Tag_Generator() { ID = GenID});
        GenID++;
    }
}
