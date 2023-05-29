using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LeopotamGroup.Globals;
using Pathfinding;
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

        public void Run(IEcsSystems ecsSystems)
        {
            var filter = ecsSystems.GetWorld().Filter<OnTriggerEnter2DEvent>()
                .Exc<EnemyPathfindingComponent>()
                .End();
            var pool = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();

            foreach (var entity in filter)
            {
                ref var eventData = ref pool.Get(entity);

                Debug.Log(eventData.senderGameObject.gameObject);
                if (eventData.senderGameObject.GetComponent<PlayerActor>() ==null) return;
                if (eventData.collider2D.GetComponent<EnemyActor>() ==null) return;
                var pf = eventData.collider2D.GetComponent<AIDestinationSetter>();
               var ent= eventData.collider2D.GetComponent<EnemyActor>().Entity; 
               ref EnemyPathfindingComponent enemyPathfindingComponent =
                           ref _enemyPathfindingComponenPool.Add(entity);
                var target = eventData.senderGameObject.transform;

                pf.target = target;
                //  systems.GetWorld().DelEntity(entity);
                // ref TransformComponent playerInputComponent = ref _playerTransformComponentPool.Get(entity);
                // if (_enemyPathfindingComponenPool.Has(entity))
                // {
                //     ref EnemyPathfindingComponent enemyPathfindingComponent =
                //         ref _enemyPathfindingComponenPool.Get(entity);
                //     enemyPathfindingComponent.AIDestinationSetter.target =
                //         _sharedData.GetPlayerCharacteristic.Transform;
                // }
            }
        }

        // public void Run(IEcsSystems systems)
        // {
        //     foreach (int entity in _filter)
        //     {
        //    //     ref TransformComponent playerInputComponent = ref _playerTransformComponentPool.Get(entity);
        //         ref EnemyPathfindingComponent enemyPathfindingComponent = ref _enemyPathfindingComponenPool.Add(entity);
        //         if (_enemyPathfindingComponenPool.Has(entity))
        //         {
        //         ref EnemyPathfindingComponent enemyPathfindingComponent2 = ref _enemyPathfindingComponenPool.Get(entity);
        //         enemyPathfindingComponent2.AIDestinationSetter.target = _sharedData.GetPlayerCharacteristic.Transform;
        //             
        //         }
        //         systems.GetWorld().DelEntity(entity);
        //
        //
        //     }
        // }
    }
}