using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceBattle
{
    public class EnemyAuthoring : MonoBehaviour { }

    public class EnemyBaker: Baker<EnemyAuthoring> {

        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Enemy>(entity);
            AddComponent<NewSpawn>(entity);
        }
    }

    public class Enemy : IComponentData
    {
        public float HP;
        public float Speed;
    }

    public struct NewSpawn : IComponentData { }

}