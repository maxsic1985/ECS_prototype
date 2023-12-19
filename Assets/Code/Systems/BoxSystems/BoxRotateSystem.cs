using System;
using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{
    public class BoxRotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transportComponentPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<IsBoxComponent>()
                .Inc<TransformComponent>()
                .End();
            _transportComponentPool = world.GetPool<TransformComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref TransformComponent transformComponent = ref _transportComponentPool.Get(entity);
                transformComponent.Value.Rotate(1,1,0);
            }
        }
    }
}