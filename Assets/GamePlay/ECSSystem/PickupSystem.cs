using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public partial class PickOnTriggerSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem commandBufferSystem;
    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    // [BurstCompile]
    struct PickupOnTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PickupTag> allPickups;
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> allPlayers;
        public EntityCommandBuffer entityCommandBuffer;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
            if (allPickups.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityA);
                UnityEngine.Debug.Log("This:" + entityA + " Other:" + entityB);
                return;
            }
            if (allPickups.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                UnityEngine.Debug.Log("Pickup Entity A:" + entityA + " collided with player Entity B:" + entityB);
            }
            else if (allPickups.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                UnityEngine.Debug.Log("player Entity A:" + entityA + " collided with Pickup Entity B:" + entityB);
            }
        }
    }
    protected override void OnUpdate()
    {
        PickupOnTriggerSystemJob triggerJob = new PickupOnTriggerSystemJob()
        {
            allPickups = GetComponentDataFromEntity<PickupTag>(true),
            allPlayers = GetComponentDataFromEntity<PlayerTag>(true),
            entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
        };
        Dependency = triggerJob.Schedule(stepPhysicsWorld.Simulation, Dependency);
        commandBufferSystem.AddJobHandleForProducer(Dependency);

        // Entities.
        // WithAll<PickupTag>().
        // ForEach((ref Rotation rotation, in MoveData moveData) =>
        // {
        //     quaternion normalizedRot = math.normalize(rotation.Value);
        //     quaternion angleToRotate = quaternion.AxisAngle(math.up(), moveData.turnSpeed);
        //     rotation.Value = math.mul(normalizedRot, angleToRotate);
        // }).ScheduleParallel();
    }
}
