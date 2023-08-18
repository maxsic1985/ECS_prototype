using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BackgroundMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _platformFilter;
        private EcsFilter _treadmillFilter;
        private EcsPool<BackgroundComponent> _treadmillComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsFilter _lastPlatformFilter;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _platformFilter = _world.Filter<IsPlatformComponent>()
                                    .Inc<TransformComponent>()
                                    .Inc<IsMoveComponent>()
                                   // .Inc<IsRuntimeGame>()
                                    .End();
            _treadmillFilter = _world.Filter<IsTreadmillComponent>().Inc<BackgroundComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _treadmillComponentPool = _world.GetPool<BackgroundComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            ref BackgroundComponent backgroundComponent =
                ref _treadmillComponentPool.Get(_treadmillFilter.GetRawEntities()[0]);

            foreach (int platformEntity in _platformFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(platformEntity);

                if (transformComponent.Value.gameObject.TryGetComponent(out BackgroundView platform))
                {
                    Vector2 position = transformComponent.Value.position;
                    position -= new Vector2(backgroundComponent.Speed * Time.deltaTime, 0 );
                    transformComponent.Value.localPosition = position;
                    platform.Position = position;
                }
            }

            _lastPlatformFilter = _world.Filter<IsPlatformComponent>().Inc<IsLastPlatformComponent>().End();

            ref TransformComponent lastTransform =
                ref _transformComponentPool.Get(_lastPlatformFilter.GetRawEntities()[0]);

            if (lastTransform.Value is null) return;

          //  backgroundComponent.SpawnPlatformPoint = lastTransform.Value.position;
        }
    }
}