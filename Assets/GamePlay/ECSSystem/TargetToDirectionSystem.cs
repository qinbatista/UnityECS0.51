using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class TargetToDirectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.
        WithNone<PlayerTag>().
        WithAll<ChaserTag>().
        ForEach((ref MoveData moveData, ref Rotation rotation, in Translation pos, in TargetData targetData) =>
        {
            ComponentDataFromEntity<Translation> allTranslations = GetComponentDataFromEntity<Translation>(true);//true means read only
            if(!allTranslations.HasComponent(targetData.targetEntity))
            {
                return;
            }
            Translation targetPos = allTranslations[targetData.targetEntity];
            float3 dirToTarget = targetPos.Value - pos.Value;
            moveData.direction = dirToTarget;
            FaceDirection(ref rotation, moveData);
            // rotation += normalizedDir * moveData.speed * deltaTime;
        }).Run();
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
