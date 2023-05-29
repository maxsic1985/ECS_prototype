using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<TransformComponent> _playerTransformComponentPool;
        private EcsPool<IsPlayerControlComponent> _isPlayerControlComponent;
        private EcsPool<EnemyPathfindingComponent> _enemyPathfindingComponenPool;
        private PlayerSharedData _sharedData;

        readonly EcsCustomInject<JoystickInputView> _joystick = default;
        private int _entity;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = systems.GetWorld().Filter<HitComponent>()
                .Inc<IsHitPlayerComponent>()
               // .Inc<IsPlayerComponent>()
             //   .Inc<TransformComponent>()
             //   .Inc<EnemyPathfindingComponent>()

                .End();

            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;

            _playerTransformComponentPool = _world.GetPool<TransformComponent>();
            _isPlayerControlComponent = _world.GetPool<IsPlayerControlComponent>();
            _enemyPathfindingComponenPool = _world.GetPool<EnemyPathfindingComponent>();

        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
           //     ref TransformComponent playerInputComponent = ref _playerTransformComponentPool.Get(entity);
                ref EnemyPathfindingComponent enemyPathfindingComponent = ref _enemyPathfindingComponenPool.Add(entity);
                if (_enemyPathfindingComponenPool.Has(entity))
                {
                ref EnemyPathfindingComponent enemyPathfindingComponent2 = ref _enemyPathfindingComponenPool.Get(entity);
                enemyPathfindingComponent2.AIDestinationSetter.target = _sharedData.GetPlayerCharacteristic.Transform;
                    
                }
                systems.GetWorld().DelEntity(entity);


            }
        }
    }
}