using System;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem2 : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
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
        private EcsPool<OnTriggerExit2DEvent> poolExit;
        private EcsPool<OnTriggerEnter2DEvent> poolEnter;

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
            poolExit = systems.GetWorld().GetPool<OnTriggerExit2DEvent>();
            poolEnter = systems.GetWorld().GetPool<OnTriggerEnter2DEvent>();
        }

        public void Run(IEcsSystems ecsSystems)
        {
            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() != null)
                {
                    Debug.Log("enter");
                }

                poolEnter.Del(entity);

                //   if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
            }

            ExitFromTRigger(poolExit);
        }

        private void ExitFromTRigger(EcsPool<OnTriggerExit2DEvent> poolExit)
        {
            foreach (var entity in _filterExitFromTrigger)
            {
                ref var eventData = ref poolExit.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() != null) 
                Debug.Log("exit");
                poolExit.Del(entity);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
        }
    }
}