using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class AssignPlayerToTargetSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityQuery playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());
        // Entity playerEntity = playerQuery.ToEntityArray(Allocator.Temp)[0];
        Entity playerEntity = playerQuery.GetSingletonEntity();
        Entities.
        WithAll<ChaserTag>().
        ForEach((ref TargetData targetData) =>
        {
            if(playerEntity!=Entity.Null)
            {
                targetData.targetEntity = playerEntity;
            }
        }).Schedule();
    }

}
