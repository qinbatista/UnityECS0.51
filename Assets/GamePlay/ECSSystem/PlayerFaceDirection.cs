using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial class PlayerFaceDirection : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation rotation,in Translation pos, in MoveData moveData)=>
        {
            // float normalizedDir = math.normalizes(moveData.turnSpeed);
            if(!moveData.direction.Equals(float3.zero))
            {
                quaternion targetRotation = quaternion.LookRotationSafe(moveData.direction, math.up());
                rotation.Value = math.slerp(rotation.Value, targetRotation, moveData.turnSpeed * deltaTime);
            }

            // rotation += normalizedDir * moveData.speed * deltaTime;
        }).Schedule();
    }
}
