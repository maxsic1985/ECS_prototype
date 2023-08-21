using Leopotam.EcsLite;
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
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<LenghtComponent> _lenghtComponentPool;


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
        }


        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is BoxData dataInit)
                {
                    ref IsPoolLoadedComponent loadPrefabFromPool = ref _isPoolLoadedPool.Add(entity);

                    ref BoxComponent boxComponent = ref _boxComponentPool.Add(entity);
                    boxComponent.StartBoxCount = dataInit.StartBoxCount;
                    boxComponent.Speed = dataInit.Speed;
                    boxComponent.SpawnBoxPoint = dataInit.StartBoxPoints;

                    for (int i = 0; i <= 10; i++)
                    {
                        GameObject pooled = _poolService.Get(GameObjectsTypeId.Box);
                        var newEntity = systems.GetWorld().NewEntity();
                        pooled.gameObject.GetComponent<Actor>().AddEntity(newEntity);

                        ref SpeedComponent speedComponent = ref _speedComponentPool.Add(newEntity);
                        speedComponent.SpeedValue = boxComponent.Speed;

                        ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(newEntity);
                        ref TransformComponent transformComponent = ref _transformComponentPool.Add(newEntity);
                        ref LenghtComponent lenghtComponent = ref _lenghtComponentPool.Add(newEntity);
                        ref IsBoxComponent isBoxComponent = ref _isBoxComponentPool.Add(newEntity);

                        var boxView = pooled.GetComponent<BoxView>();
                        boxView.MinSpeedPingPong = dataInit.MinSpeedBox;
                        boxView.MaxSpeedPingPong = dataInit.MaxSpeedBox;
                        boxView.PingPongUpPoint = dataInit.UpPoint;
                        boxView.PingPongDownPoint = dataInit.DownPoint;
                        
                        transformComponent.Value = boxView.transform;
                        var backgroundLenght = pooled.GetComponent<BackgroundView>().GetPlatformLenght();
                        lenghtComponent.Value = backgroundLenght;

                        _poolService.Return(pooled);
                    }

                    _transformComponentPool.Add(entity);
                }

                _scriptableObjectPool.Del(entity);
            }
        }
    }
}