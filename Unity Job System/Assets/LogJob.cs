using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

[BurstCompile]
public struct LogJob : IJob
{
    public NativeArray<float> results;
    public int count;

    public void Execute()
    {
        for (int i = 0; i < count; i++)
        {
            float random = Random.Range(1f, 100f);
            results[i] = Mathf.Log(random);
        }
    }
}
