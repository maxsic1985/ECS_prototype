using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class PlayerMoveForwardSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<IsPlayerControlComponent> _isPlayerMoveComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsPlayerComponent> _isPlayerComponent;
        private ITimeService _timeService;
        private PlayerSharedData _sharedData;
        private Vector3 playerPosition;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _playerFilter = world.Filter<IsPlayerComponent>()
                .Inc<TransformComponent>()
                 .Inc<SpeedComponent>()
                .End();
            _isPlayerComponent = world.GetPool<IsPlayerComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isPlayerMoveComponentPool = world.GetPool<IsPlayerControlComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                  ref SpeedComponent speedComponent = ref _speedComponentPool.Get(entity);
                if (transformComponent.Value == null)
                    return;

                if (_isPlayerComponent.Has(entity))
                    PlayerMoving(ref transformComponent,ref speedComponent);
            }
        }


        private void PlayerMoving(ref TransformComponent transformComponent,ref SpeedComponent speedComponent) 
        {
            Vector2 position = transformComponent.Value.position;
            position += new Vector2(Vector2.right.x*speedComponent.SpeedValue * _timeService.DeltaTime, 0);

            transformComponent.Value.position = position;

            
        }
       
        
        
        
        
    }
}