using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace SpaceBattle
{
    public partial struct EnemyMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new EnemyMoveJob
            {
                Movement = new float3(0, SystemAPI.Time.DeltaTime * -20, 0),
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }
    [WithAll(typeof(Enemy))]
    [BurstCompile]
    public partial struct EnemyMoveJob : IJobEntity
    {
        public float3 Movement;
        public EntityCommandBuffer.ParallelWriter ECB;

        void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, ref LocalTransform cubeTransform)
        {
            cubeTransform.Position += Movement;
            if (cubeTransform.Position.y < 0)
            {
                ECB.DestroyEntity(chunkIndex, entity);
            }
        }
    }
}