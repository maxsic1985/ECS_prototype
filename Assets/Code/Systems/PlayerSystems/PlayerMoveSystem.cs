using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int Right = 1;
        private const int Left = -1;

        private EcsFilter _playerFilter;
        private EcsPool<PlayerInputComponent> _playerInputComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<DestinationComponent> _destinationComponentPool;
        private EcsPool<IsPlayerMoveComponent> _isPlayerMoveComponentPool;
        private EcsPool<IsOnGroundComponent> _isOnGroundComponentPool;
        private EcsPool<PlatformSideComponent> _platformSideComponentPool;
        private PlatformSide _platformSide;
        private ITimeService _timeService;
        private float _distance;
        private int _direction;
        private float _startTime;
        private Vector3 _newPosition;
        private Vector3 _startPosition;
        private EcsPool<SpeedVectorComponent> _speedVectorComponentPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<IsPlayerComponent>().Inc<TransformComponent>().End();
            _playerInputComponentPool = world.GetPool<PlayerInputComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _destinationComponentPool = world.GetPool<DestinationComponent>();
            _isPlayerMoveComponentPool = world.GetPool<IsPlayerMoveComponent>();
            _platformSideComponentPool = world.GetPool<PlatformSideComponent>();
            _speedVectorComponentPool = world.GetPool<SpeedVectorComponent>();
            _isOnGroundComponentPool = world.GetPool<IsOnGroundComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                //if (!_isOnGroundComponentPool.Has(entity)) return;
                //if(_isPlayerJumpComponentPool.Has(entity)) return;

                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref PlayerInputComponent playerInputComponent = ref _playerInputComponentPool.Get(entity);
                ref DestinationComponent destinationComponent = ref _destinationComponentPool.Get(entity);
                ref SpeedVectorComponent speedVectorComponent = ref _speedVectorComponentPool.Get(entity);
                ref PlatformSideComponent platformSideComponent = ref _platformSideComponentPool.Get(entity);

                if (playerInputComponent.Horizontal != 0 && !_isPlayerMoveComponentPool.Has(entity) &&
                    _isOnGroundComponentPool.Has(entity))
                {
                    //Add jump Sound
                    ref var isSoundFromTriggerComponent = ref systems.GetWorld().
                        GetPool<IsPlaySoundComponent>().Add(entity);
                    isSoundFromTriggerComponent.SoundType = SoundsEnumType.Jump;
                    //

                    _isPlayerMoveComponentPool.Add(entity);
                    InitializeStartMoving(ref transformComponent, ref playerInputComponent, ref destinationComponent);
                }

                if (_isPlayerMoveComponentPool.Has(entity))
                    SmoothMoving(entity, ref speedVectorComponent, ref transformComponent,
                        ref platformSideComponent, ref destinationComponent);
            }
        }

        private void InitializeStartMoving(ref TransformComponent transformComponent,
            ref PlayerInputComponent inputComponent, ref DestinationComponent destinationComponent)
        {
            _direction = (int) inputComponent.Horizontal;
            _startPosition = transformComponent.Value.position;
            _newPosition =
                new Vector3(
                    _startPosition.x + destinationComponent.Value.x * _direction, _startPosition.y, 0);
            _startTime = _timeService.InGameTime;

            _distance = Vector3.Distance(_startPosition, _newPosition);
        }

        private void SmoothMoving(int entity, ref SpeedVectorComponent speedVectorComponent,
            ref TransformComponent transformComponent, ref PlatformSideComponent platformSideComponent,
            ref DestinationComponent destinationComponent)
        {
            switch (_direction)
            {
                case 0:
                    return;
                case Right when platformSideComponent.PlatformSide == PlatformSide.Right:
                case Left when platformSideComponent.PlatformSide == PlatformSide.Left:
                    _isPlayerMoveComponentPool.Del(entity);
                    return;
            }

            float distCovered = (_timeService.InGameTime - _startTime) * speedVectorComponent.Value.x;

            float fractionOfJourney = distCovered / _distance;

            Vector3 position = GetPoint(_startPosition, new Vector3(_startPosition.x, destinationComponent.Value.y, 0),
                new Vector3(_newPosition.x, destinationComponent.Value.y, 0), _newPosition, fractionOfJourney);

            transformComponent.Value.position = position;

            if (_newPosition == transformComponent.Value.position)
            {
                _isPlayerMoveComponentPool.Del(entity);

                platformSideComponent.PlatformSide = _direction == Right ? PlatformSide.Right : PlatformSide.Left;

                if (_newPosition.x == 0)
                    platformSideComponent.PlatformSide = PlatformSide.Center;

                _direction = 0;
            }
        }

        private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1 - t;

            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }
    }
}