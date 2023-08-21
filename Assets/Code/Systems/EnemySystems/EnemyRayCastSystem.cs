using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemyRayCastSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
        private EcsPool<EnemyIsFollowingComponent> _isFollowComponentPool;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();
            EcsWorld world = systems.GetWorld();
            filter = systems.GetWorld()
                .Filter<IsEnemyComponent>()
                .Inc<TransformComponent>()
                .Inc<IsMoveComponent>()
                .End();
            _isMovingComponentPool = world.GetPool<IsMoveComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isFollowComponentPool = world.GetPool<EnemyIsFollowingComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                RaycastHit2D hit = Physics2D.CircleCast(transformComponent.Value.position,
                    3, 
                    Vector2.left);
                if (hit.collider == null) return;
                if (hit.collider.gameObject.activeSelf==false) return;
                if (hit.collider.gameObject.GetComponent<PlayerActor>() != null)
                {
                    float distance = Vector2.Distance(hit.collider.gameObject.transform.position,
                        transformComponent.Value.position);
                    Debug.Log($"distance+{distance}");


                    var reached = transformComponent.Value.GetComponent<AIPath>();
                    var aiDestinationSetter = transformComponent.Value.GetComponent<AIDestinationSetter>();
                    Debug.DrawRay(transformComponent.Value.position,
                        hit.collider.gameObject.transform.position,
                        Color.red);

                    if (distance < 3 &&
                        !reached.reachedEndOfPath &&
                        !_isFollowComponentPool.Has(entity) &&
                       Math.Abs(transformComponent.Value.position.x)
                       < Math.Abs(hit.collider.gameObject.transform.position.x))

                    {
                        var enemyEntity = transformComponent.Value.GetComponent<EnemyActor>().Entity;
                        if (!_isFollowComponentPool.Has(enemyEntity))
                        {
                            ref EnemyIsFollowingComponent enemyIsFollowingComponent =
                                ref _isFollowComponentPool.Add(enemyEntity);
                        }

                        var target = hit.collider.gameObject.transform;
                        aiDestinationSetter.target = target;
                        reached.endReachedDistance = 0.5f;
                        Debug.Log("StartFollowing");
                    }
                    else if (reached.reachedEndOfPath)
                    {
                        {
                            var enemyEntity = transformComponent.Value.GetComponent<EnemyActor>().Entity;
                            _isFollowComponentPool.Del(enemyEntity);
                            aiDestinationSetter.target = transformComponent.Value.transform;
                            reached.Teleport(transformComponent.Value.gameObject.transform.position+Vector3.left, true);
                          //  reached.Teleport(transformComponent.Value.gameObject.transform.position, true);
                            reached.endReachedDistance = Single.PositiveInfinity;
                            reached.reachedEndOfPath = false;
                            reached.endReachedDistance = 0;
                        }
                    }
                }
            }
        }
    }
}