using UnityEngine;
using Unity.Entities;

public class ZigzagController : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int ballCount = 1000;
    public float spawnRadius = 20f;
    public uint randomSeed = 12345;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float amplitude = 2f;
    public float frequency = 1.5f;

    [Header("Debug")]
    public bool showFPS = true;
    public bool showBallCount = true;

    private World world;
    private BallSpawnerSystem spawnerSystem;

    void Start()
    {
        world = World.DefaultGameObjectInjectionWorld;

        if (world != null)
        {
            spawnerSystem = world.GetExistingSystemManaged<BallSpawnerSystem>();

            if (spawnerSystem != null)
            {
                spawnerSystem.spawnCount = ballCount;
                spawnerSystem.spawnRadius = spawnRadius;
                spawnerSystem.baseMoveSpeed = moveSpeed;
                spawnerSystem.baseAmplitude = amplitude;
                spawnerSystem.baseFrequency = frequency;
                spawnerSystem.seed = randomSeed;
            }
        }
    }

    void OnGUI()
    {
        if (!showBallCount && !showFPS) return;

        int entityCount = 0;
        if (world?.EntityManager != null)
        {
            var query = world.EntityManager.CreateEntityQuery(typeof(ZigzagComponent));
            entityCount = query.CalculateEntityCount();
        }

        GUI.Box(new Rect(10, 10, 200, 60), "ECS Performance");

        if (showBallCount)
            GUI.Label(new Rect(20, 35, 180, 25), $"Balls: {entityCount}");

        if (showFPS)
            GUI.Label(new Rect(20, 55, 180, 25), $"FPS: {1f / Time.deltaTime:F1}");
    }
}