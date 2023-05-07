using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public struct CollisionEventDamage: IComponentData
{
    public float Power;
}

public struct FighterStats: IComponentData
{
    public float HP;
}

public class CollisionEventDamageAuthoring : MonoBehaviour
{
    public float Power;
    public float InitialHP;

    void OnEnable() { }

    class CollisionEventDamageBaker : Baker<CollisionEventDamageAuthoring>
    {
        public override void Bake(CollisionEventDamageAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionEventDamage()
            {
                Power = authoring.Power,
            });
            AddComponent(entity, new FighterStats()
            {
                HP = authoring.InitialHP,
            });
        }
    }
}

// This system applies damage to any static body that collides with another static (only player and enemy layers).
// A Static body is defined by a PhysicsShapeAuthoring with the `Raise Collision Events` flag ticked and a
// CollisionEventDamage behaviour added.
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct CollisionEventSystem : ISystem
{
    internal ComponentDataHandles m_ComponentDataHandles;

    internal struct ComponentDataHandles
    {
        public ComponentLookup<CollisionEventDamage> CollisionEventDamageData;
        public ComponentLookup<PhysicsCollider> PhysicsColliderData;

        public ComponentDataHandles(ref SystemState systemState)
        {
            CollisionEventDamageData = systemState.GetComponentLookup<CollisionEventDamage>(true);
            PhysicsColliderData = systemState.GetComponentLookup<PhysicsCollider>(false);
        }

        public void Update(ref SystemState systemState)
        {
            CollisionEventDamageData.Update(ref systemState);
            PhysicsColliderData.Update(ref systemState);
        }
    }

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate(state.GetEntityQuery(ComponentType.ReadOnly<CollisionEventDamage>()));
        m_ComponentDataHandles = new ComponentDataHandles(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        m_ComponentDataHandles.Update(ref state);
        state.Dependency = new CollisionEventJob
        {
            CollisionEventDamageData = m_ComponentDataHandles.CollisionEventDamageData,
            PhysicsColliderData = m_ComponentDataHandles.PhysicsColliderData,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    struct CollisionEventJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<CollisionEventDamage> CollisionEventDamageData;
        public ComponentLookup<PhysicsCollider> PhysicsColliderData;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isBodyAStatic = PhysicsColliderData.HasComponent(entityA);
            bool isBodyBStatic = PhysicsColliderData.HasComponent(entityB);

            bool isBodyADamager = CollisionEventDamageData.HasComponent(entityA);
            bool isBodyBDamager = CollisionEventDamageData.HasComponent(entityB);

            if (isBodyADamager && isBodyBDamager)
            {
                var damageComponentB = CollisionEventDamageData[entityB];
                var damageComponentA = CollisionEventDamageData[entityA];

                damageComponentB.HP -= damageComponentA.Power;
                damageComponentA.HP -= damageComponentB.Power;

                CollisionEventDamageData[entityB] = damageComponentB;
                CollisionEventDamageData[entityA] = damageComponentA;

                Debug.Log("damagetaken by B: " + damageComponentA.Power + " and HP: " + damageComponentB.HP);

            }

        }
    }
}