using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial class MoveForwardSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.
        WithAll<AsteroidTag>().
        WithNone<PlayerTag>().
        ForEach((ref Translation pos, in MoveData moveData, in Rotation rotation) =>
        {
            float3 forwardDirection = math.forward(rotation.Value);
            pos.Value += forwardDirection * moveData.speed * deltaTime;
        }).ScheduleParallel();


        Entities.
        WithAll<ChaserTag>().
        WithNone<PlayerTag>().
        ForEach((ref Translation pos, in MoveData moveData, in Rotation rotation) =>
        {
            float3 forwardDirection = math.forward(rotation.Value);
            pos.Value += forwardDirection * moveData.speed * deltaTime;
        }).ScheduleParallel();
    }
}
