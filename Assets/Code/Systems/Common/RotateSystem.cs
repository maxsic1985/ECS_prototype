using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    internal class RotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<IsRotateComponent>()
                .Inc<TransformComponent>()
                .End();
            _transformPool = world.GetPool<TransformComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref TransformComponent trransformComponent = ref _transformPool.Get(entity);
                trransformComponent.Value.Rotate(1,1,0);
                Debug.Log("I'm rotating!");
            }
        }
    }
}