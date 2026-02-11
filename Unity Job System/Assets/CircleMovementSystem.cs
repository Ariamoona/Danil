using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;

public class CircleMovementSystem : MonoBehaviour
{
    public GameObject prefab;
    public int objectCount = 1000;
    public float radius = 10f;
    public float speed = 1f;

    private TransformAccessArray transformArray;
    private NativeArray<float> angles;
    public float calculationInterval = 2f;
    private float timer;
    private NativeArray<float> results;
    void Start()
    {
        results = new NativeArray<float>(objectCount, Allocator.Persistent);

        transformArray = new TransformAccessArray(objectCount);
        angles = new NativeArray<float>(objectCount, Allocator.Persistent);

        for (int i = 0; i < objectCount; i++)
        {
            float angle = (i / (float)objectCount) * Mathf.PI * 2f;
            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            transformArray.Add(obj.transform);
            angles[i] = angle;
        }
    }

    void Update()
    {
        var job = new CircleJob
        {
            deltaTime = Time.deltaTime,
            speed = speed,
            radius = radius,
            angles = angles
        };

        JobHandle handle = job.Schedule(transformArray);
        handle.Complete();
    }

    void OnDestroy()
    {
        transformArray.Dispose();
        angles.Dispose();
        results.Dispose();

    }
}
