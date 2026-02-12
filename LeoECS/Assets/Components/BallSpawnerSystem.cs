using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class BallSpawnerSystem : SystemBase
{
    public int spawnCount = 1000;
    public float spawnRadius = 20f;
    public float baseMoveSpeed = 3f;
    public float baseAmplitude = 2f;
    public float baseFrequency = 1.5f;
    public uint seed = 12345;

    private bool spawned = false;
    private Entity ballPrefab;

    protected override void OnCreate()
    {
        RequireForUpdate<ZigzagBallAuthoring>();
    }

    protected override void OnUpdate()
    {
        if (spawned) return;

        var prefabQuery = SystemAPI.QueryBuilder().WithAll<ZigzagBallAuthoring>().Build();
        ballPrefab = prefabQuery.GetSingletonEntity();

        var entityManager = EntityManager;
        var random = new Random(seed);

        for (int i = 0; i < spawnCount; i++)
        {
            var ball = entityManager.Instantiate(ballPrefab);

            float angle = random.NextFloat(0, 2 * math.PI);
            float radius = random.NextFloat(0, spawnRadius);
            float3 position = new float3(
                math.cos(angle) * radius,
                0.5f,
                math.sin(angle) * radius
            );

            float moveSpeed = baseMoveSpeed + random.NextFloat(-1f, 1f);
            float amplitude = baseAmplitude + random.NextFloat(-0.5f, 0.5f);
            float frequency = baseFrequency + random.NextFloat(-0.3f, 0.3f);

            entityManager.SetComponentData(ball, new LocalTransform
            {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 0.3f
            });

            entityManager.SetComponentData(ball, new ZigzagComponent
            {
                MoveSpeed = moveSpeed,
                Amplitude = amplitude,
                Frequency = frequency,
                StartPosition = position,
                Time = random.NextFloat(0, 10f)
            });
        }

        Debug.Log($" Spawned {spawnCount} balls with zigzag movement");
        spawned = true;
        Enabled = false;
    }
}
