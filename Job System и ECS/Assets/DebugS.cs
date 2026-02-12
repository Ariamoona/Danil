using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class DebugDrawSystem : SystemBase
{
    private DebugDrawComponent _debugSettings;
    private int _drawnCount;

    protected override void OnCreate()
    {
        RequireForUpdate<DebugDrawComponent>();
    }

    protected override void OnStartRunning()
    {
        _debugSettings = SystemAPI.GetSingleton<DebugDrawComponent>();
    }

    protected override void OnUpdate()
    {
        _debugSettings = SystemAPI.GetSingleton<DebugDrawComponent>();
        _drawnCount = 0;

        Enabled = false;
    }

    public void OnDrawGizmos()
    {
        if (!SystemAPI.HasSingleton<DebugDrawComponent>())
            return;

        var settings = SystemAPI.GetSingleton<DebugDrawComponent>();
        int maxDraw = settings.DrawAll ? int.MaxValue : settings.MaxDrawCount;
        int drawn = 0;

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (transform, entity) in SystemAPI.Query<LocalTransform>().WithEntityAccess())
        {
            if (drawn >= maxDraw)
                break;

            Gizmos.color = new Color(settings.Color.x, settings.Color.y, settings.Color.z, 0.5f);
            Gizmos.DrawWireSphere(transform.Position, 0.3f);

            if (SystemAPI.HasComponent<CircleCenter>(entity))
            {
                var center = SystemAPI.GetComponent<CircleCenter>(entity);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(center.Value, SystemAPI.GetComponent<Radius>(entity).Value);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(center.Value, transform.Position);
            }

            drawn++;
        }

        _drawnCount = drawn;
        ecb.Dispose();
    }
}