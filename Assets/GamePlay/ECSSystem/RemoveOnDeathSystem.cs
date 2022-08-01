using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial class RemoveOnDeathSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        Entities.
        WithAll<ChaserTag>().
        ForEach((Entity entity, in HealthData healthData) =>
        {
            if(entity==Entity.Null|| entity.Index == 0)
            {
                return;
            }
            if (healthData.isDead)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }

        }).Schedule();

        Entities.
        WithAll<PlayerTag>().
        ForEach((Entity entity, in HealthData healthData) =>
        {
            if(entity==Entity.Null|| entity.Index == 0)
            {
                return;
            }
            if (healthData.isDead)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }

        }).Schedule();

        commandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}