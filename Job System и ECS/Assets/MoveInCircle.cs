using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct MoveInCircleSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MoveSpeed>();
        state.RequireForUpdate<Radius>();
        state.RequireForUpdate<CircleCenter>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float time = (float)SystemAPI.Time.ElapsedTime;

        var moveJob = new MoveInCircleJob
        {
            DeltaTime = deltaTime,
            Time = time
        };

        moveJob.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct MoveInCircleJob : IJobEntity
{
    public float DeltaTime;
    public float Time;

    void Execute(ref LocalTransform transform, in MoveSpeed speed, in Radius radius, in CircleCenter center)
    {
        float angle = Time * speed.Value;

        float3 offset = new float3(
            math.sin(angle) * radius.Value,
            0,
            math.cos(angle) * radius.Value
        );

        transform.Position = center.Value + offset;

        float3 direction = new float3(
            math.cos(angle),
            0,
            -math.sin(angle)
        );

        if (!math.all(direction == float3.zero))
        {
            transform.Rotation = quaternion.LookRotation(direction, math.up());
        }
    }
}