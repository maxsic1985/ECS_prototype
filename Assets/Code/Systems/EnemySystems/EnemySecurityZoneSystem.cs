using System;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsPool<EnemyIsFollowingComponent> _isEnemyAtackingComponentPool;
        private EcsPool<IsEnemyCanAttackComponent> _isEnemyCanAtackComponenPool;
        private EcsPool<IsPlayerCanAttackComponent> _isPlayerCanAtackComponenPool;
        private EcsPool<HealthViewComponent> _playerHealthViewComponentPool;
        private EcsPool<EnemyHealthComponent> _enemyHealthComponentPool;


        private PlayerSharedData _sharedData;

        readonly EcsCustomInject<AttackInputView> _attackInput = default;
        private int _entity;
        private EcsFilter _enemyFilter;
        private EcsFilter _filterEnterToTrigger;
        private EcsFilter _filterExitFromTrigger;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _enemyFilter = _world.Filter<IsEnemyComponent>().End();

            _filterEnterToTrigger = _world.Filter<OnTriggerEnter2DEvent>()
                // .Inc<IsPlayerComponent>()
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
            _playerHealthViewComponentPool = _world.GetPool<HealthViewComponent>();
            _enemyHealthComponentPool = _world.GetPool<EnemyHealthComponent>();
        }

        public void Run(IEcsSystems ecsSystems)
        {
            var poolEnter = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();
            var poolExit = ecsSystems.GetWorld().GetPool<OnTriggerExit2DEvent>();

            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);

                // if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                // if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
                if (eventData.senderGameObject.GetComponent<PlayerActor>() != null)
                {
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


                    ref HealthViewComponent playerHealthView = ref _playerHealthViewComponentPool.Add(entity);
                    playerHealthView.Value = eventData.senderGameObject.GetComponent<PlayerActor>()
                        .GetComponent<HealthView>().Value;

                    Extensions.AddPool<HealthViewComponent>(ecsSystems, enemyEntity);
                    ref HealthViewComponent enemyHealthView = ref _playerHealthViewComponentPool.Get(enemyEntity);
                    enemyHealthView.Value =
                        eventData.collider2D.GetComponent<EnemyActor>().GetComponent<HealthView>().Value;
                    ref IsPlayerCanAttackComponent canAtack =
                        ref _isPlayerCanAtackComponenPool.Add(entity);
                    canAtack.AttackInputView = _attackInput.Value;
                    canAtack.AttackInputView.ShowBtn(true);

                    foreach (var enemynativeEntity in _enemyFilter)
                    {
                        ref EnemyHealthComponent enemyHealth =
                            ref _enemyHealthComponentPool.Get(enemynativeEntity);
                        _enemyHealthComponentPool.Copy(enemynativeEntity, entity);
                    }
                    poolEnter.Del(entity);
                }
            }

            ExitFromTRigger(poolExit);
        }

        private void ExitFromTRigger(EcsPool<OnTriggerExit2DEvent> poolExit)
        {
            foreach (var entity in _filterExitFromTrigger)
            {
                Debug.Log("exit");
                ref var eventData = ref poolExit.Get(entity);

                // if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                // if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
                if (eventData.senderGameObject.GetComponent<PlayerActor>() != null)
                {


                    var aiDestinationSetter = eventData.collider2D.GetComponent<AIDestinationSetter>();
                    var enemyEntity = eventData.collider2D.GetComponent<EnemyActor>().Entity;
                    //    if (_isEnemyAtackingComponentPool.Has(enemyEntity))
                    //   {
                    _isEnemyAtackingComponentPool.Del(enemyEntity);
                    _isEnemyCanAtackComponenPool.Del(enemyEntity);
                    _playerHealthViewComponentPool.Del(entity);
                    _playerHealthViewComponentPool.Del(enemyEntity);
                    //   }

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
                poolExit.Del(entity);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            throw new NotImplementedException();
        }
    }
}