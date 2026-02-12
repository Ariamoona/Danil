using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int gridWidth = 250;
    public int gridHeight = 200;
    public float spacing = 2f;

    [Header("Movement Settings")]
    public float defaultMoveSpeed = 2f;
    public float defaultRadius = 5f;

    [Header("Debug Settings")]
    public bool drawGizmos = true;
    public bool drawAllEntities = false;
    public int maxDrawCount = 1000;
    public Color gizmoColor = Color.green;

    private World _world;
    private SpawnArmySystem _spawnSystem;

    void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;

        _spawnSystem = _world.GetOrCreateSystemManaged<SpawnArmySystem>();
        _spawnSystem.gridWidth = gridWidth;
        _spawnSystem.gridHeight = gridHeight;
        _spawnSystem.spacing = spacing;

        var entityManager = _world.EntityManager;
        var debugEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(debugEntity, new DebugDrawComponent
        {
            DrawAll = drawAllEntities,
            MaxDrawCount = maxDrawCount,
            Color = new float3(gizmoColor.r, gizmoColor.g, gizmoColor.b)
        });
    }

    void Update()
    {
        if (_world != null && _world.EntityManager.UniversalQuery.IsEmptyIgnoreFilter == false)
        {
            if (_world.EntityManager.HasComponent<DebugDrawComponent>(_world.EntityManager.UniversalQuery.GetSingletonEntity()))
            {
                var debugEntity = _world.EntityManager.UniversalQuery.GetSingletonEntity();
                _world.EntityManager.SetComponentData(debugEntity, new DebugDrawComponent
                {
                    DrawAll = drawAllEntities,
                    MaxDrawCount = maxDrawCount,
                    Color = new float3(gizmoColor.r, gizmoColor.g, gizmoColor.b)
                });
            }
        }

        if (_world?.EntityManager != null)
        {
            int count = _world.EntityManager.UniversalQuery.CalculateEntityCount();
            Debug.Log($"Active entities: {count}");
        }
    }

    void OnValidate()
    {
        int totalObjects = gridWidth * gridHeight;
        if (totalObjects > 100000)
        {
            Debug.LogWarning($"You're about to spawn {totalObjects} objects. This might impact performance!");
        }
    }
    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        var world = World.DefaultGameObjectInjectionWorld;
        if (world?.EntityManager != null &&
            world.EntityManager.HasComponent<DebugDrawComponent>(world.EntityManager.UniversalQuery.GetSingletonEntity()))
        {
            var debugSystem = world.GetOrCreateSystemManaged<DebugDrawSystem>();
            debugSystem.OnDrawGizmos();
        }
    }
}