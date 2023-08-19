using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using TMPro;
using UniRx;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemyRichSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filterTrigger;
        private bool _reachedToPlayer;
        private EcsPool<IsEnemyCanAttackComponent> _isReachedComponentPool;
        private EcsPool<EnemyIsFollowingComponent> _isFollowingComponentPool;
        private PlayerSharedData _sharedData;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            filterTrigger = systems.GetWorld().Filter<IsEnemyCanAttackComponent>()
                .End();
            _isReachedComponentPool = world.GetPool<IsEnemyCanAttackComponent>();
            _isFollowingComponentPool = world.GetPool<EnemyIsFollowingComponent>();

            //    Observable.Interval(TimeSpan.FromMilliseconds(3000)).Where(_=>_reachedToPlayer).Subscribe(x => { Attack(); })
            //        .AddTo(_disposables);
           
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filterTrigger)
            {
               
             _reachedToPlayer = _isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath;
         //    ref EnemyIsFollowingComponent followingComponent = ref _isFollowingComponentPool.Get(entity);
         _isFollowingComponentPool.Del(entity);
                Debug.Log(_isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath);
                
             
                    _reachedToPlayer = false;
                    _isReachedComponentPool.Del(entity);
             
            }

         
        }

    }
}