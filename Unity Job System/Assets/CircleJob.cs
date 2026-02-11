using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

[BurstCompile]
public struct CircleJob : IJobParallelForTransform
{
    public float deltaTime;
    public float speed;
    public float radius;

    public NativeArray<float> angles;

    public void Execute(int index, TransformAccess transform)
    {
        float angle = angles[index];
        angle += speed * deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, 0, z);

        angles[index] = angle;
    }
}
