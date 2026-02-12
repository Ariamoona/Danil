using System.IO;
using System.Text;
using Unity.Entities;
using UnityEngine;

public class PerformanceLogger : MonoBehaviour
{
    private StreamWriter writer;
    private float timer;
    private int frameCount;

    void Start()
    {
        string path = Path.Combine(Application.dataPath, "../performance_log.csv");
        writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("Frame,Time,FPS,BallCount,CPU_Time_ms,Memory_MB");
    }

    void Update()
    {
        timer += Time.deltaTime;
        frameCount++;

        if (timer >= 0.5f) 
        {
            var world = World.DefaultGameObjectInjectionWorld;
            int ballCount = 0;

            if (world?.EntityManager != null)
            {
                var query = world.EntityManager.CreateEntityQuery(typeof(ZigzagComponent));
                ballCount = query.CalculateEntityCount();
            }

            writer.WriteLine($"{frameCount},{Time.time:F2},{1f / Time.deltaTime:F1}," +
                           $"{ballCount},{Time.deltaTime * 1000:F2},{System.GC.GetTotalMemory(false) / 1048576:F2}");
            writer.Flush();

            timer = 0f;
        }
    }

    void OnDestroy()
    {
        writer?.Close();
    }
}