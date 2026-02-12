using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct CounterSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CounterComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        double elapsedTime = SystemAPI.Time.ElapsedTime;

        new CounterJob
        {
            ElapsedTime = (float)elapsedTime
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct CounterJob : IJobEntity
{
    public float ElapsedTime;

    void Execute(ref CounterComponent counter)
    {
        counter.Value++;

        if (counter.Value % 600 == 0)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"[ECS Counter] Frame: {counter.Value}, Time: {ElapsedTime:F2}");
#endif
        }
    }
}