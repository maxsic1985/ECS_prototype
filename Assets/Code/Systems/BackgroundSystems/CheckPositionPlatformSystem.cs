using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class CheckPositionPlatformSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _platformFilter;
        private EcsFilter _treadmillFilter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<BackgroundComponent> _backGroundComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<IsPlatformComponent> _isPlatformComponentPool;
        private IPoolService _poolService;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _platformFilter = _world.Filter<IsPlatformComponent>().End();
            _treadmillFilter = _world.Filter<IsTreadmillComponent>().Inc<BackgroundComponent>().End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _backGroundComponentPool = _world.GetPool<BackgroundComponent>();
            _isMoveComponentPool = _world.GetPool<IsMoveComponent>();
            _isPlatformComponentPool = _world.GetPool<IsPlatformComponent>();
            _poolService = Service<IPoolService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            _platformFilter = _world.Filter<IsPlatformComponent>().End();

            ref BackgroundComponent treadmillComponent =
                ref _backGroundComponentPool.Get(_treadmillFilter.GetRawEntities()[0]);

            foreach (int platformEntity in _platformFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(platformEntity);
                ref IsPlatformComponent isPlatformComponent = ref _isPlatformComponentPool.Get(platformEntity);
                
                if (transformComponent.Value.position.x < treadmillComponent.ReturnToPoolPoint.x)
                {
                    _isMoveComponentPool.Del(platformEntity);
                    
                    ReturnToPoolAllObjects(ref isPlatformComponent);
                    
                    _poolService.Return(transformComponent.Value.gameObject);
                 //   treadmillComponent.Platforms.Dequeue();
                }
            }
        }

        private void ReturnToPoolAllObjects(ref IsPlatformComponent isPlatformComponent)
        {
            foreach (GameObject pillarLamp in isPlatformComponent.PillarLamps)
            {
                _poolService.Return(pillarLamp);
            }

            foreach (GameObject wallLamp in isPlatformComponent.WallLamps)
            {
                _poolService.Return(wallLamp);
            }

            foreach (GameObject pickableObject in isPlatformComponent.PickableObjects)
            {
                _poolService.Return(pickableObject);
            }
            
            isPlatformComponent.Clear();
        }
    }
}