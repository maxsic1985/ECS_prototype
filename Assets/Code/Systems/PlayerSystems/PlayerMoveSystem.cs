using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsPool<PlayerInputComponent> _playerInputComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsPlayerControlComponent> _isPlayerMoveComponentPool;
        private EcsPool<PlayerRigidBodyComponent> _playerRBPool;
        private PlatformSide _platformSide;
        private ITimeService _timeService;
        private Vector3 _newPosition;
        private Vector3 _startPosition;
        private EcsPool<SpeedVectorComponent> _speedVectorComponentPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<IsPlayerComponent>()
                .Inc<TransformComponent>()
                .Inc<PlayerRigidBodyComponent>()
                .Inc<IsPlayerControlComponent>()
                .End();
            _playerInputComponentPool = world.GetPool<PlayerInputComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isPlayerMoveComponentPool = world.GetPool<IsPlayerControlComponent>();
            _playerRBPool = world.GetPool<PlayerRigidBodyComponent>();
            _speedVectorComponentPool = world.GetPool<SpeedVectorComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref PlayerInputComponent playerInputComponent = ref _playerInputComponentPool.Get(entity);
                ref PlayerRigidBodyComponent rb = ref _playerRBPool.Get(entity);

                if (!_isPlayerMoveComponentPool.Has(entity))
                {
                    
                    ref var isSoundFromTriggerComponent = ref systems.GetWorld().
                        GetPool<IsPlaySoundComponent>().Add(entity);
                    isSoundFromTriggerComponent.SoundType = SoundsEnumType.Jump;
                    //

                    _isPlayerMoveComponentPool.Add(entity);
                }

                if (_isPlayerMoveComponentPool.Has(entity))
                    SmoothMoving(entity, ref rb, ref transformComponent,
                        ref playerInputComponent); //, ref destinationComponent);
            }
        }

      
        private void SmoothMoving(int entity, ref PlayerRigidBodyComponent rb,
            ref TransformComponent transformComponent, ref PlayerInputComponent inputComponent)//,ref DestinationComponent destinationComponent)
        {
           
            Vector3 direction = Vector3.up * inputComponent.Vertical + Vector3.right * inputComponent.Horizontal;
         //   rb.PlayerRigidbody.AddForce(direction * 10 * _timeService.DeltaTime,ForceMode2D.Force);
         transformComponent.Value.position = Vector3.Lerp( transformComponent.Value.position, direction,
             1 * _timeService.DeltaTime);
        }

       
    }
}