using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ZigzagMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZigzagComponent>();
        state.RequireForUpdate<LocalTransform>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        new ZigzagMovementJob
        {
            DeltaTime = deltaTime
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ZigzagMovementJob : IJobEntity
{
    public float DeltaTime;

    void Execute(ref LocalTransform transform, ref ZigzagComponent zigzag)
    {
        zigzag.Time += DeltaTime;

        float forwardMovement = zigzag.MoveSpeed * zigzag.Time;
        float zigzagOffset = math.sin(zigzag.Time * zigzag.Frequency) * zigzag.Amplitude;

        float3 newPosition = zigzag.StartPosition;
        newPosition.z += forwardMovement;
        newPosition.x += zigzagOffset;

        transform.Position = newPosition;
    }
}