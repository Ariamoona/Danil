using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ZigzagBallAuthoring : MonoBehaviour
{
    [Header("Zigzag Settings")]
    public float moveSpeed = 5f;
    public float amplitude = 2f;
    public float frequency = 2f;

    class Baker : Baker<ZigzagBallAuthoring>
    {
        public override void Bake(ZigzagBallAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ZigzagComponent
            {
                MoveSpeed = authoring.moveSpeed,
                Amplitude = authoring.amplitude,
                Frequency = authoring.frequency,
                StartPosition = authoring.transform.position,
                Time = 0
            });

            AddComponent(entity, new LocalTransform
            {
                Position = authoring.transform.position,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            AddComponent(entity, new LocalToWorld());
        }
    }
}