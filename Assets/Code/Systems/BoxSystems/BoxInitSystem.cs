﻿using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class BoxInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private IPoolService _poolService;
        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<IsPoolLoadedComponent> _isPoolLoadedPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<BoxComponent> _boxComponentPool;
        private EcsPool<IsBoxComponent> _isBoxComponentPool;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<PingPongPositionComponent> _pingPongComponentPool;
        private EcsPool<PingPongSpeedComponent> _pingPongSpeedComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<LenghtComponent> _lenghtComponentPool;
        private EcsPool<Scale2DComponent> _scale2DComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _poolService = Service<IPoolService>.Get();

            _filter = _world.Filter<IsBoxComponent>()
                .Inc<ScriptableObjectComponent>()
                .End();
            _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
            _isPoolLoadedPool = _world.GetPool<IsPoolLoadedComponent>();
            _boxComponentPool = _world.GetPool<BoxComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _isBoxComponentPool = _world.GetPool<IsBoxComponent>();
            _speedComponentPool = _world.GetPool<SpeedComponent>();
            _isMoveComponentPool = _world.GetPool<IsMoveComponent>();
            _lenghtComponentPool = _world.GetPool<LenghtComponent>();
            _pingPongComponentPool = _world.GetPool<PingPongPositionComponent>();
            _pingPongSpeedComponentPool = _world.GetPool<PingPongSpeedComponent>();
            _scale2DComponentPool = _world.GetPool<Scale2DComponent>();

        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is BoxData dataInit)
                {
                    ref IsPoolLoadedComponent loadPrefabFromPool = ref _isPoolLoadedPool.Add(entity);


                    for (int i = 0; i <= _poolService.Capacity; i++)
                    {
                        GameObject pooled = _poolService.Get(GameObjectsTypeId.Box,_world);
                        var newEntity = systems.GetWorld().NewEntity();

                        ref BoxComponent boxComponent = ref _boxComponentPool.Add(newEntity);
                        boxComponent.Speed = dataInit.Speed;
                        boxComponent.SpawnHorisontalPoint = dataInit.SpawnHorisontalPoint;
                        pooled.gameObject.GetComponent<Actor>().AddEntity(newEntity);

                        ref SpeedComponent speedComponent = ref _speedComponentPool.Add(newEntity);
                        speedComponent.SpeedValue = boxComponent.Speed;

                        ref Scale2DComponent scale2D = ref _scale2DComponentPool.Add(newEntity);
                        scale2D.ScaleValue = dataInit.Scale2D;
                        
                        ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(newEntity);
                        ref TransformComponent transformComponent = ref _transformComponentPool.Add(newEntity);
                        ref LenghtComponent lenghtComponent = ref _lenghtComponentPool.Add(newEntity);
                        ref IsBoxComponent isBoxComponent = ref _isBoxComponentPool.Add(newEntity);
                        ref PingPongPositionComponent pingPongComponent = ref _pingPongComponentPool.Add(newEntity);
                        pingPongComponent.UpValue = dataInit.UpperPoint;
                        pingPongComponent.DownValue = dataInit.DownerPoint;

                        ref PingPongSpeedComponent pingPongSpeedComponent =
                            ref _pingPongSpeedComponentPool.Add(newEntity);
                        pingPongSpeedComponent.MinValue = dataInit.MinSpeedBox;
                        pingPongSpeedComponent.MaxValue = dataInit.MaxSpeedBox;
                        pingPongSpeedComponent.CurrentValue = pingPongSpeedComponent.GetRandomSpeed;

                        var boxView = pooled.GetComponent<BoxView>();
                        transformComponent.Value = boxView.transform;

                        var backgroundLenght = pooled.GetComponent<BackgroundView>().GetPlatformLenght();
                        lenghtComponent.Value = backgroundLenght;

                        _poolService.Return(pooled);
                    }
                }

                _scriptableObjectPool.Del(entity);
            }
        }
    }
}