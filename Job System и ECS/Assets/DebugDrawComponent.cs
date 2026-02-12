using Unity.Entities;
using Unity.Mathematics;

public struct DebugDrawComponent : IComponentData
{
    public bool DrawAll;
    public int MaxDrawCount;
    public float3 Color;
}