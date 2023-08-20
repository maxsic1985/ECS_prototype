using System;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MSuhininTestovoe.B2B
{
    public class BoxBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsPoolLoadedComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<BoxComponent> _boxComponentPool;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<LenghtComponent> _lenghtComponentPool;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _poolService = Service<IPoolService>.Get();

            _filter = world.Filter<IsBoxComponent>()
                .Inc<BoxComponent>()
                .Inc<IsPoolLoadedComponent>()
                .End();
            _prefabPool = world.GetPool<IsPoolLoadedComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _boxComponentPool = world.GetPool<BoxComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
            _isMoveComponentPool = world.GetPool<IsMoveComponent>();
            _lenghtComponentPool = world.GetPool<LenghtComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref BoxComponent boxComponent = ref _boxComponentPool.Get(entity);
                ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(entity);
                ref SpeedComponent speedComponent = ref _speedComponentPool.Add(entity);
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref LenghtComponent lenghtComponent = ref _lenghtComponentPool.Add(entity);
              
                   speedComponent.SpeedValue = boxComponent.Speed;
                for (int i = 0; i <= boxComponent.StartBoxCount; i++)
                {
                    
                    GameObject pooled = _poolService.Get(GameObjectsTypeId.Box);
                
                    transformComponent.Value = pooled.GetComponent<BoxView>().transform;
                   
                    var backgroundLenght = pooled.GetComponent<BackgroundView>().GetPlatformLenght();
                    lenghtComponent.Value = backgroundLenght;
                    
                    
                    //   transformComponent.Value = pooled.gameObject.GetComponent<BackgroundView>().transform;
                 //   speedComponent.SpeedValue = 2;
                }

                _prefabPool.Del(entity);
            }
        }
    }
}