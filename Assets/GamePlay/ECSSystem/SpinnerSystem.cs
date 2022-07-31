using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial class SpinnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation rotation, in MoveData moveData) =>
        {
            quaternion normalizedRot = math.normalize(rotation.Value);
            quaternion angleToRotate = quaternion.AxisAngle(math.up(), moveData.turnSpeed * deltaTime);
            rotation.Value = math.mul(normalizedRot, angleToRotate);
        }).ScheduleParallel();
    }
}
