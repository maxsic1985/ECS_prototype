using System;
using Leopotam.EcsLite;
using Pathfinding;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public class EnemyRayCastSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemyIsFollowingComponent> _isFollowComponentPool;
        private EcsPool<EnemyPathfindingComponent> _enemyPathFindingComponentPool;
        private EcsPool<EnemySecutityZoneComponent> _enemySecurityZoneComponentPool;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            filter = systems.GetWorld()
                .Filter<IsEnemyComponent>()
                .Inc<TransformComponent>()
                .Inc<EnemySecutityZoneComponent>()
                .Inc<IsMoveComponent>()
                .End();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isFollowComponentPool = world.GetPool<EnemyIsFollowingComponent>();
            _enemySecurityZoneComponentPool = world.GetPool<EnemySecutityZoneComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref EnemySecutityZoneComponent securityZoneSystem = ref _enemySecurityZoneComponentPool.Get(entity);

                RaycastHit2D hit = Physics2D.CircleCast(
                    transformComponent.Value.position,
                    securityZoneSystem.DistanceValue,
                    Vector2.left);
                if (hit.collider == null) return;
                if (hit.collider.gameObject.GetComponent<PlayerActor>() != null
                    && transformComponent.Value.gameObject.activeSelf)
                {
                    float distance = Vector2.Distance(
                        hit.collider.gameObject.transform.position,
                        transformComponent.Value.position);


                    var reached = transformComponent.Value.GetComponent<AIPath>();
                    var aiDestinationSetter = transformComponent.Value.GetComponent<AIDestinationSetter>();
                    Debug.DrawRay(transformComponent.Value.position,
                        hit.collider.gameObject.transform.position,
                        Color.red);

                    if (distance < securityZoneSystem.DistanceValue &&
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

                        var target = hit.collider.gameObject.transform.GetChild(1).transform;
                        aiDestinationSetter.target = target;
                        reached.endReachedDistance = LimitsConstants.ENEMY_END_REACH_DISTANCE;
                    }
                    else if (reached.reachedEndOfPath)
                    {
                        {
                            var enemyEntity = transformComponent.Value.GetComponent<EnemyActor>().Entity;
                            _isFollowComponentPool.Del(enemyEntity);
                            aiDestinationSetter.target = transformComponent.Value.transform;
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