using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class GravitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsOnGroundComponent> _isOnGroundComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<GravityComponent> _gravityComponentPool;
        private EcsPool<IsPlayerMoveComponent> _isPlayerMoveComponentPool;
        private ITimeService _timeService;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<GravityComponent>().Inc<TransformComponent>().End();
            _isOnGroundComponentPool = world.GetPool<IsOnGroundComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _gravityComponentPool = world.GetPool<GravityComponent>();
            _isPlayerMoveComponentPool = world.GetPool<IsPlayerMoveComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_isOnGroundComponentPool.Has(entity) || _isPlayerMoveComponentPool.Has(entity))
                {
                    //_gravityComponentPool.Del(entity);
                    ref GravityComponent gravityComponent = ref _gravityComponentPool.Get(entity);
                    gravityComponent.Velocity = 0;
                    return;
                }

                ref GravityComponent gComponent = ref _gravityComponentPool.Get(entity);
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);

                if (transformComponent.Value is null) return;

                gComponent.Velocity += Physics.gravity.y * _timeService.DeltaTime;
                transformComponent.Value.position += new Vector3(0, gComponent.Velocity * _timeService.DeltaTime, 0);
            }
        }
    }
}