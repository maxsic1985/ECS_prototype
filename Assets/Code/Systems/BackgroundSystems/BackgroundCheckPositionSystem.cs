using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BackgroundCheckPositionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _platformFilter;
        private EcsFilter _treadmillFilter;
        private EcsPool<LenghtComponent> _lenghtComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsFilter _lastPlatformFilter;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _platformFilter = _world
                .Filter<TransformComponent>()
                .Inc<IsMoveComponent>()
                .Inc<LenghtComponent>()
                .Inc<IsBackgroundComponent>()
                .End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _lenghtComponentPool = _world.GetPool<LenghtComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int platformEntity in _platformFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(platformEntity);
                ref LenghtComponent lenghtComponent = ref _lenghtComponentPool.Get(platformEntity);


                if (transformComponent.Value.position.x<=-lenghtComponent.Value*2)
                {
                    transformComponent.Value.position = new Vector2(lenghtComponent.Value, 0);
                }
            }
        }
    }
}