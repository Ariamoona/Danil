using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovingCubeAuthoring : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float radius = 5f;

    class Baker : Baker<MovingCubeAuthoring>
    {
        public override void Bake(MovingCubeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new LocalTransform
            {
                Position = float3.zero,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            AddComponent(entity, new MoveSpeed { Value = authoring.moveSpeed });
            AddComponent(entity, new Radius { Value = authoring.radius });
        }
    }
}