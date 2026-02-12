using Unity.Entities;
using Unity.Mathematics;

public struct ZigzagComponent : IComponentData
{
    public float MoveSpeed;
    public float Amplitude;
    public float Frequency;
    public float3 StartPosition;
    public float Time;
}