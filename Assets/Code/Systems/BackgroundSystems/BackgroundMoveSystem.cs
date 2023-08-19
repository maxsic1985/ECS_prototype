using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BackgroundMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _platformFilter;
        private EcsFilter _treadmillFilter;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsFilter _lastPlatformFilter;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _platformFilter = _world
                .Filter<TransformComponent>()
                .Inc<IsMoveComponent>()
                .Inc<SpeedComponent>()
                .End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _speedComponentPool = _world.GetPool<SpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int platformEntity in _platformFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(platformEntity);
                ref SpeedComponent speedComponent = ref _speedComponentPool.Get(platformEntity);


                if (transformComponent.Value.gameObject.TryGetComponent(out BackgroundView platform))
                {
                    Vector2 position = transformComponent.Value.position;
                    position -= new Vector2(speedComponent.SpeedValue * Time.deltaTime, 0);
                    transformComponent.Value.localPosition = position;
                    platform.Position = position;
                }
            }
        }
    }
}