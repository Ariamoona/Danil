using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Profiling;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ECSPerformanceProfiler : ISystem
{
    private EntityQuery zigzagQuery;
    private float timer;
    private ProfilerRecorder systemUpdateRecorder;
    private ProfilerRecorder fpsRecorder;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        zigzagQuery = state.GetEntityQuery(ComponentType.ReadOnly<ZigzagComponent>());
        timer = 0f;

        systemUpdateRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Scripts, "UnityEngine.MonoBehaviour.Update");
        fpsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "FPS");
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        timer += SystemAPI.Time.DeltaTime;

        if (timer >= 0.5f)
        {
            int ballCount = zigzagQuery.CalculateEntityCount();
            float fps = 1f / SystemAPI.Time.DeltaTime;
            long totalMemory = GC.GetTotalMemory(false) / 1048576;

#if UNITY_EDITOR
            Debug.Log($"=== ECS PROFILER [{System.DateTime.Now:HH:mm:ss}] ===");
            Debug.Log($" Balls: {ballCount}");
            Debug.Log($" FPS: {fps:F1}");
            Debug.Log($" Memory: {totalMemory} MB");
            Debug.Log($" Systems: {GetSystemCount(state)}");
            Debug.Log($"=================================");
#endif

            timer = 0f;
        }
    }

    private int GetSystemCount(SystemState state)
    {
        var world = state.World;
        if (world != null)
        {
            return 0; 
        }
        return 0;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        systemUpdateRecorder.Dispose();
        fpsRecorder.Dispose();
    }
}
