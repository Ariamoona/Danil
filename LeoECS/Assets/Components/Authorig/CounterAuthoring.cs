using Unity.Entities;
using UnityEngine;

public class CounterAuthoring : MonoBehaviour
{
    class Baker : Baker<CounterAuthoring>
    {
        public override void Bake(CounterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CounterComponent
            {
                Value = 0
            });
        }
    }
}