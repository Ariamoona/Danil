using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
public partial class SpawnArmySystem : SystemBase
{
    private Entity _prefabEntity;
    private bool _spawned = false;

    public int gridWidth = 250;
    public int gridHeight = 200;
    public float spacing = 2f;

    protected override void OnCreate()
    {
        RequireForUpdate<MovingCubeAuthoring>();
    }

    protected override void OnUpdate()
    {
        if (_spawned) return;

        var entityManager = EntityManager;

        var prefabQuery = SystemAPI.QueryBuilder().WithAll<MovingCubeAuthoring>().Build();
        _prefabEntity = prefabQuery.GetSingletonEntity();

        int count = 0;
        int totalCount = gridWidth * gridHeight;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var instance = entityManager.Instantiate(_prefabEntity);

                float3 position = new float3(
                    (x - gridWidth / 2f) * spacing,
                    0,
                    (y - gridHeight / 2f) * spacing
                );

                entityManager.SetComponentData(instance, new LocalTransform
                {
                    Position = position,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                entityManager.AddComponentData(instance, new CircleCenter
                {
                    Value = position
                });

                count++;
            }
        }

        Debug.Log($"Spawned {count} entities");
        _spawned = true;
        Enabled = false;
    }
}