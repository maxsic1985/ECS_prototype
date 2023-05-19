using System;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class PlayerJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int UP = 1;

        private EcsWorld _world;
        private EcsFilter _playerFilter;
        private EcsPool<PlayerInputComponent> _playerInputComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<DestinationComponent> _destinationComponentPool;
        private EcsPool<IsPlayerMoveComponent> _isPlayerMoveComponentPool;
        private EcsPool<IsPlayerJumpComponent> _isPlayerJumpComponentPool;
        private EcsPool<IsOnGroundComponent> _isOnGroundComponentPool;
        private EcsPool<SpeedVectorComponent> _speedVectorComponentPool;
        private ITimeService _timeService;

        private float _distance;
        private float _startTime;
        private Vector3 _newPosition;
        private Vector3 _startPosition;
        private bool _jump;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerFilter = _world.Filter<IsPlayerComponent>().Inc<TransformComponent>().End();
            _playerInputComponentPool = _world.GetPool<PlayerInputComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _destinationComponentPool = _world.GetPool<DestinationComponent>();
            _isPlayerMoveComponentPool = _world.GetPool<IsPlayerMoveComponent>();
            _isPlayerJumpComponentPool = _world.GetPool<IsPlayerJumpComponent>();
            _speedVectorComponentPool = _world.GetPool<SpeedVectorComponent>();
            _isOnGroundComponentPool = _world.GetPool<IsOnGroundComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                if (_isPlayerMoveComponentPool.Has(entity)) return;

                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref PlayerInputComponent playerInputComponent = ref _playerInputComponentPool.Get(entity);
                ref DestinationComponent destinationComponent = ref _destinationComponentPool.Get(entity);
                ref SpeedVectorComponent speedVectorComponent = ref _speedVectorComponentPool.Get(entity);

                if (Convert.ToInt32(playerInputComponent.Vertical) == UP 
                    && !_isPlayerJumpComponentPool.Has(entity) 
                    && _isOnGroundComponentPool.Has(entity))
                {
                    //Add jump Sound
                    ref var isPlaySoundComponent = ref systems.GetWorld().
                        GetPool<IsPlaySoundComponent>().Add(entity);
                    isPlaySoundComponent.SoundType = SoundsEnumType.Jump;
                    //

                    _isPlayerJumpComponentPool.Add(entity);
                    InitializeStartMoving(ref transformComponent, ref playerInputComponent, ref destinationComponent,
                        entity);
                    return;
                }

                if (_isPlayerJumpComponentPool.Has(entity))
                    SmoothJump(entity, ref transformComponent, ref speedVectorComponent);
            }
        }

        private void InitializeStartMoving(ref TransformComponent transformComponent,
            ref PlayerInputComponent inputComponent, ref DestinationComponent destinationComponent, int entity)
        {
            _startPosition = transformComponent.Value.position;
            _newPosition = new Vector3(_startPosition.x,
                _startPosition.y + destinationComponent.Value.y * inputComponent.Vertical, 0);
            _startTime = _timeService.InGameTime;

            _distance = Vector3.Distance(_startPosition, _newPosition);
            _isOnGroundComponentPool.Del(entity);
        }

        private void SmoothJump(int entity, ref TransformComponent transformComponent,
            ref SpeedVectorComponent speedVectorComponent)
        {
            float distCovered = (_timeService.InGameTime - _startTime) * speedVectorComponent.Value.y;

            float fractionOfJourney = distCovered / _distance;

            Vector3 position = Vector3.Lerp(_startPosition, _newPosition, fractionOfJourney);

            transformComponent.Value.position = position;

            if (_newPosition == transformComponent.Value.position)
            {
                _isPlayerJumpComponentPool.Del(entity);
            }
        }
    }
}