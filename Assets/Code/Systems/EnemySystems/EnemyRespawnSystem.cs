using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UniRx;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class EnemyRespawnSystem : IEcsInitSystem, IEcsDestroySystem, IDisposable
    {
        private EcsFilter filterTrigger;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
        private IPoolService _poolService;
        private List<IDisposable> _disposables = new List<IDisposable>();
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartComponentPool;


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();
            EcsWorld _world = systems.GetWorld();
            filterTrigger = systems.GetWorld()
                .Filter<IsEnemyComponent>()
                .Inc<EnemyStartPositionComponent>()
                .End();

            _isMovingComponentPool = _world.GetPool<IsMoveComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _enemyStartComponentPool = _world.GetPool<EnemyStartPositionComponent>();

            Observable.Interval(TimeSpan.FromMilliseconds(LimitsConstants.COOLDOWN_ENEMY)).Where(_ => true)
                .Subscribe(x => { Respawn(); })
                .AddTo(_disposables);
        }


        private void Respawn()
        {
            foreach (var _ in filterTrigger)
            {
                if (_poolService == null)
                {
                    _poolService = Service<IPoolService>.Get();
                    Dispose();
                }

                var pooled = _poolService.Get(GameObjectsTypeId.Enemy);
                var entity = pooled.gameObject.GetComponent<EnemyActor>().Entity;

                ref EnemyStartPositionComponent enemyStartPositionComponent = ref _enemyStartComponentPool.Get(entity);
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                transformComponent.Value.position = enemyStartPositionComponent.Value;
             


                if (!_isMovingComponentPool.Has(entity))
                {
                    ref IsMoveComponent isMoveComponent = ref _isMovingComponentPool.Add(entity);
                }

                return;
            }
        }


        public void Destroy(IEcsSystems systems)
        {
            Dispose();
        }


        public void Dispose()
        {
            _disposables.Clear();
        }
    }
}