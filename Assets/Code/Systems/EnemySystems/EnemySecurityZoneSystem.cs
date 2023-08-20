using System;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsPool<EnemyIsFollowingComponent> _isFollowComponentPool;


        private PlayerSharedData _sharedData;

        private int _entity;
        private EcsFilter _enemyFilter;
        private EcsFilter _filterEnterToTrigger;
        private EcsFilter _filterExitFromTrigger;
        private EcsPool<IsRestartComponent> _isRestartPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _enemyFilter = _world.Filter<IsEnemyComponent>().End();

            _filterEnterToTrigger = _world.Filter<OnTriggerEnter2DEvent>()
                //  .Inc<IsPlayerComponent>()
                //  .Exc<EnemyPathfindingComponent>()
                //  .Exc<EnemyIsFollowingComponent>()
                .End();

            _filterExitFromTrigger = _world.Filter<OnTriggerExit2DEvent>()
                //.Inc<EnemyPathfindingComponent>()
                .End();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _isFollowComponentPool = _world.GetPool<EnemyIsFollowingComponent>();
            _isRestartPool = _world.GetPool<IsRestartComponent>();

        }

        public void Run(IEcsSystems ecsSystems)
        {
            var poolEnter = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();
            var poolExit = ecsSystems.GetWorld().GetPool<OnTriggerExit2DEvent>();

            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                //  if (eventData.collider2D.GetComponent<EnemyActor>() == null) return;
                if (eventData.senderGameObject.GetComponent<PlayerActor>() != null
                    && eventData.collider2D.GetComponent<EnemyActor>() != null)
                {
                    var aiDestinationSetter = eventData.collider2D.GetComponent<AIDestinationSetter>();
                    var reached = eventData.collider2D.GetComponent<AIPath>();
                    var enemyEntity = eventData.collider2D.GetComponent<EnemyActor>().Entity;
                    if (!_isFollowComponentPool.Has(enemyEntity))
                    {
                        ref EnemyIsFollowingComponent enemyIsFollowingComponent =
                            ref _isFollowComponentPool.Add(enemyEntity);
                    }

                    var target = eventData.senderGameObject.transform;
                    aiDestinationSetter.target = target;
                    reached.endReachedDistance = 0.5f;
                    Debug.Log("StartFollowing");
                    


                   
                }

                else if (eventData.senderGameObject.GetComponent<PlayerActor>() != null
                         && eventData.collider2D.GetComponent<BorderActor>() != null)
                {
                    Debug.Log("StartFollowing");

                    _isRestartPool.Add(entity);
                  // SceneManager.LoadScene(0);
               poolEnter.Del(entity);
               return;
                }

            }

            ExitFromTRigger(poolExit);
        }

        private void ExitFromTRigger(EcsPool<OnTriggerExit2DEvent> poolExit)
        {
            foreach (var entity in _filterExitFromTrigger)
            {
                ref var eventData = ref poolExit.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                if (eventData.collider2D.GetComponent<EnemyActor>() != null)
                {
                    var aiDestinationSetter = eventData.collider2D.GetComponent<AIDestinationSetter>();
                    var enemyEntity = eventData.collider2D.GetComponent<EnemyActor>().Entity;
                    _isFollowComponentPool.Del(enemyEntity);
                    var reached = eventData.collider2D.GetComponent<AIPath>();
                    aiDestinationSetter.target = eventData.collider2D.gameObject.transform;
                    reached.Teleport(eventData.collider2D.gameObject.transform.position, true);
                    reached.endReachedDistance = Single.PositiveInfinity;
                    reached.reachedEndOfPath = false;
                    reached.endReachedDistance = 0;
                }

               poolExit.Del(entity);
             //   poolExit.Del(entity);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            throw new NotImplementedException();
        }
    }
}