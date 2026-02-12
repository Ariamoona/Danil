using Unity.Entities;
using Unity.Burst;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct MonitorSystem : ISystem
{
    private EntityQuery zigzagQuery;
    private float timer;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        zigzagQuery = state.GetEntityQuery(ComponentType.ReadOnly<ZigzagComponent>());
        timer = 0f;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        timer += SystemAPI.Time.DeltaTime;

        if (timer >= 1f)
        {
            int ballCount = zigzagQuery.CalculateEntityCount();
            float fps = 1f / SystemAPI.Time.DeltaTime;

#if UNITY_EDITOR
            Debug.Log($"=== ECS PERFORMANCE ===");
            Debug.Log($"🎯 FPS: {fps:F1}");
            Debug.Log($"⚽ Zigzag Balls: {ballCount}");
            Debug.Log($"⏱️ Delta Time: {SystemAPI.Time.DeltaTime * 1000:F2} ms");
            Debug.Log($"========================");
#endif

            timer = 0f;
        }
    }
}