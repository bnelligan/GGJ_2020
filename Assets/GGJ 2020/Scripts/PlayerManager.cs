using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 10;
    public float jumpForce = 4;
    public float maxSpeed = 15;
    public float groundCheckDistance = 0.1f;

    public Mesh playerMesh;
    public float capsuleRadius = 0.5f;
    public float capsuleHeight = 2f;

    public Material playerMaterial;


    public float3 playerSpawnPos = new float3(1f, 0.5f, -1.8f);
    public float3 playerSize = new float3(0.5f, 0.5f, 0.5f);


    
}
