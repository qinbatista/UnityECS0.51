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
public partial class DeathOnCollisionSystem : SystemBase
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
    struct DeathOnCollisionSystemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<DeathColliderTag> deathColliderGroup;
        [ReadOnly] public ComponentDataFromEntity<ChaserTag> chaseGroup;
        [ReadOnly] public ComponentDataFromEntity<HealthData> healthGroup;
        public EntityCommandBuffer entityCommandBuffer;


        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            bool entityAIsChaser = chaseGroup.HasComponent(entityA);
            bool entityAIsDeathCollider = deathColliderGroup.HasComponent(entityA);
            bool entityBIsChaser = chaseGroup.HasComponent(entityB);
            bool entityBIsDeathCollider = deathColliderGroup.HasComponent(entityB);
            UnityEngine.Debug.Log("This:" + entityA + " Other:" + entityB);
            if(entityAIsDeathCollider && entityBIsChaser)
            {
                HealthData modifiedHealth = healthGroup[entityB];
                modifiedHealth.isDead = true;
                healthGroup[entityB] = modifiedHealth;
            }
            else if(entityBIsDeathCollider && entityAIsChaser)
            {
                HealthData modifiedHealth = healthGroup[entityA];
                modifiedHealth.isDead = true;
                healthGroup[entityA] = modifiedHealth;
            }
        }
    }
    protected override void OnUpdate()
    {
        DeathOnCollisionSystemJob triggerJob = new DeathOnCollisionSystemJob()
        {
            deathColliderGroup = GetComponentDataFromEntity<DeathColliderTag>(true),
            chaseGroup = GetComponentDataFromEntity<ChaserTag>(true),
            healthGroup = GetComponentDataFromEntity<HealthData>(true),
            entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
        };
        Dependency = triggerJob.Schedule(stepPhysicsWorld.Simulation, Dependency);
        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
