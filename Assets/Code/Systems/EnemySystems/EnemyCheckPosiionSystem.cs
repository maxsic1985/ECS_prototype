using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using TMPro;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace MSuhininTestovoe.B2B
{
    public class EnemyCheckPosiionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filterTrigger;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsMoveComponent> _isMovingComponent;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();
            EcsWorld world = systems.GetWorld();
            filterTrigger = systems.GetWorld()
                .Filter<IsEnemyComponent>()
                .Inc<IsMoveComponent>()
                .Inc<TransformComponent>()
                .End();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isMovingComponent = world.GetPool<IsMoveComponent>();

            //    Observable.Interval(TimeSpan.FromMilliseconds(3000)).Where(_=>_reachedToPlayer).Subscribe(x => { Attack(); })
            //        .AddTo(_disposables);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filterTrigger)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref IsMoveComponent isMoveComponent = ref _isMovingComponent.Get(entity);
                if (transformComponent.Value.gameObject.transform.position.x < -20)
                {
                    _poolService.Return(transformComponent.Value.gameObject);
                    transformComponent.Value.position = new Vector2(10, new System.Random().Next(-3, 3));
                   _isMovingComponent.Del(entity);
                }
                // _reachedToPlayer = _isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath;
                //    ref EnemyIsFollowingComponent followingComponent = ref _isFollowingComponentPool.Get(entity);
                //  _transformComponentPool.Del(entity);
                //       Debug.Log(_isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath);


                //     _reachedToPlayer = false;
                ///      _isReachedComponentPool.Del(entity);
            }
        }
    }
}