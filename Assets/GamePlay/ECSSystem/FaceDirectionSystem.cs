using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial class FaceDirectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.
        ForEach((ref Rotation rotation, in Translation pos, in MoveData moveData) =>
        {
            FaceDirection(ref rotation, moveData);
        }).Schedule();

    }
    private static void FaceDirection(ref Rotation rotation, MoveData moveData)
    {
        if (!moveData.direction.Equals(float3.zero))
        {
            quaternion targetRotation = quaternion.LookRotationSafe(moveData.direction, math.up());
            rotation.Value = math.slerp(rotation.Value, targetRotation, moveData.turnSpeed);
        }
    }
}