using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemyRayCastSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
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
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                RaycastHit2D hit = Physics2D.CircleCast(transformComponent.Value.position,10,Vector2.left);
                if (hit.collider == null) return;
                if (hit.collider.gameObject.GetComponent<PlayerActor>() != null)
                {
                    float distance = Vector2.Distance(hit.collider.gameObject.transform.position ,transformComponent.Value.position);
                  Debug.Log($"distance+{distance}");
                  Debug.DrawRay(transformComponent.Value.position,
                      hit.collider.gameObject.transform.position,
                      Color.red);
                }
            }
        }
    }
}