using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using LeopotamGroup.Globals;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<TransformComponent> _playerTransformComponentPool;
        private EcsPool<EnemyIsFollowingComponent> _isEnemyAtackingComponentPool;
        private EcsPool<EnemyPathfindingComponent> _enemyPathfindingComponenPool;
        private PlayerSharedData _sharedData;

        readonly EcsCustomInject<JoystickInputView> _joystick = default;
        private int _entity;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<OnTriggerEnter2DEvent>()
                .Exc<EnemyPathfindingComponent>()
                .Exc<EnemyIsFollowingComponent>()
                .End();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;

            _playerTransformComponentPool = _world.GetPool<TransformComponent>();
            _isEnemyAtackingComponentPool = _world.GetPool<EnemyIsFollowingComponent>();
            _enemyPathfindingComponenPool = _world.GetPool<EnemyPathfindingComponent>();
        }

        public void Run(IEcsSystems ecsSystems)
        {
            var pool = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();

            foreach (var entity in _filter)
            {
                ref var eventData = ref pool.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
                var aiDestinationSetter = eventData.collider2D.GetComponent<AIDestinationSetter>();
                var enemyEntity = eventData.collider2D.GetComponent<EnemyActor>().Entity;
                if (!_isEnemyAtackingComponentPool.Has(enemyEntity))
                {
                    ref EnemyIsFollowingComponent enemyIsFollowingComponent =
                        ref _isEnemyAtackingComponentPool.Add(enemyEntity);
                }

                //  ref EnemyPathfindingComponent enemyPathfindingComponent =
                //     ref _enemyPathfindingComponenPool.Add(entity);
                var target = eventData.senderGameObject.transform;

                aiDestinationSetter.target = target;
            }
        }
    }
}