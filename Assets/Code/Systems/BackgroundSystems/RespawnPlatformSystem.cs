using System;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class RespawnPlatformSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _platformFilter;
        private EcsFilter _treadmillFilter;
        private EcsFilter _lastPlatformFilter;

        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<BackgroundComponent> _treadmillComponentPool;
        private EcsPool<IsLastPlatformComponent> _isLastPlatformComponentPool;
        private EcsPool<IsObjectSpawnComponent> _isObjectSpawnComponentPool;
        private EcsPool<IsBoxSpawnComponent> _isLampSpawnComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;

        private IPoolService _poolService;
        //   private IPatternService _patternService;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _treadmillFilter = _world.Filter<IsTreadmillComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _treadmillComponentPool = _world.GetPool<BackgroundComponent>();
            _isMoveComponentPool = _world.GetPool<IsMoveComponent>();
            _isLastPlatformComponentPool = _world.GetPool<IsLastPlatformComponent>();
            _isObjectSpawnComponentPool = _world.GetPool<IsObjectSpawnComponent>();
            _isLampSpawnComponentPool = _world.GetPool<IsBoxSpawnComponent>();
            _poolService = Service<IPoolService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            ref BackgroundComponent treadmillComponent =
                ref _treadmillComponentPool.Get(_treadmillFilter.GetRawEntities()[0]);

            ref TransformComponent treadmillTransformComponent =
                ref _transformComponentPool.Get(_treadmillFilter.GetRawEntities()[0]);

            if (treadmillTransformComponent.Value is null) return;
        }
    }
}