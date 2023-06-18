using System;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<EnemyIsFollowingComponent> _isEnemyAtackingComponentPool;
        private EcsPool<IsEnemyCanAttackComponent> _isEnemyCanAtackComponenPool;
        private EcsPool<IsPlayerCanAttackComponent> _isPlayerCanAtackComponenPool;
        private EcsPool<PlayerHealthViewComponent> _playerHealthViewComponentPool;

        private PlayerSharedData _sharedData;

        readonly EcsCustomInject<AttackInputView> _attackInput = default;
        private int _entity;
        private EcsFilter _filterEnterToTrigger;
        private EcsFilter _filterExitFromTrigger;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterEnterToTrigger = _world.Filter<OnTriggerEnter2DEvent>()
                //.Inc<PlayerHealthViewComponent>()
                .Exc<EnemyPathfindingComponent>()
                .Exc<EnemyIsFollowingComponent>()
                .End();

            _filterExitFromTrigger = _world.Filter<OnTriggerExit2DEvent>()
                //  .Inc<EnemyPathfindingComponent>()
                .End();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;

            _isEnemyAtackingComponentPool = _world.GetPool<EnemyIsFollowingComponent>();
            _isEnemyCanAtackComponenPool = _world.GetPool<IsEnemyCanAttackComponent>();
            _isPlayerCanAtackComponenPool = _world.GetPool<IsPlayerCanAttackComponent>();
            _playerHealthViewComponentPool = _world.GetPool<PlayerHealthViewComponent>();

        }

        public void Run(IEcsSystems ecsSystems)
        {
            var poolEnter = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();
            var poolExit = ecsSystems.GetWorld().GetPool<OnTriggerExit2DEvent>();

            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
                var aiDestinationSetter = eventData.collider2D.GetComponent<AIDestinationSetter>();
                var reached = eventData.collider2D.GetComponent<AIPath>();
                var enemyEntity = eventData.collider2D.GetComponent<EnemyActor>().Entity;
                if (!_isEnemyAtackingComponentPool.Has(enemyEntity))
                {
                    ref EnemyIsFollowingComponent enemyIsFollowingComponent =
                        ref _isEnemyAtackingComponentPool.Add(enemyEntity);
                }

                ref IsEnemyCanAttackComponent isReacheded =
                    ref _isEnemyCanAtackComponenPool.Add(entity);
                var target = eventData.senderGameObject.transform;
                aiDestinationSetter.target = target;
                isReacheded.IsRecheded = reached;
                reached.endReachedDistance = 0.5f;
                
                
                ref PlayerHealthViewComponent playerHealthView = ref _playerHealthViewComponentPool.Add(entity);
                playerHealthView.Value = eventData.senderGameObject.GetComponent<PlayerActor>().GetComponent<PlayerHealthView>().Value;

                
                ref IsPlayerCanAttackComponent canAtack =
                    ref _isPlayerCanAtackComponenPool.Add(entity);
                canAtack.AttackInputView = _attackInput.Value;
                canAtack.AttackInputView.ShowBtn(true);


            }

            ExitFromTRigger(poolExit);
        }

        private void ExitFromTRigger(EcsPool<OnTriggerExit2DEvent> poolExit)
        {
            foreach (var entity in _filterExitFromTrigger)
            {
                Debug.Log("exit");
                ref var eventData = ref poolExit.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
                var aiDestinationSetter = eventData.collider2D.GetComponent<AIDestinationSetter>();
                var enemyEntity = eventData.collider2D.GetComponent<EnemyActor>().Entity;
                if (_isEnemyAtackingComponentPool.Has(enemyEntity))
                {
                    _isEnemyAtackingComponentPool.Del(enemyEntity);
                    _isEnemyCanAtackComponenPool.Del(entity);
                }

                var reached = eventData.collider2D.GetComponent<AIPath>();

                aiDestinationSetter.target = eventData.collider2D.gameObject.transform;
                reached.Teleport(eventData.collider2D.gameObject.transform.position, true);
                reached.endReachedDistance = Single.PositiveInfinity;
                reached.reachedEndOfPath = false;
                reached.endReachedDistance = 0;
                _isEnemyCanAtackComponenPool.Del(entity);
                
                
                _isPlayerCanAtackComponenPool.Del(entity);
                _attackInput.Value.ShowBtn(false);
            }
        }
    }
}