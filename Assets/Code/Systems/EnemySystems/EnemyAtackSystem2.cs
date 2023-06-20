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
    public class EnemyAtackSystem2 : IEcsInitSystem, IEcsRunSystem
    {
        private List<IDisposable> _disposables = new List<IDisposable>();

        private EcsFilter filterTrigger;
        private bool _reachedToPlayer;
        private EcsPool<IsEnemyCanAttackComponent> _isReachedComponentPool;
        private EcsPool<HealthViewComponent> _playerHealthViewComponentPool;
        private PlayerSharedData _sharedData;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            filterTrigger = systems.GetWorld().Filter<IsEnemyCanAttackComponent>()
                .End();
            _playerHealthViewComponentPool = world.GetPool<HealthViewComponent>();
            _isReachedComponentPool = world.GetPool<IsEnemyCanAttackComponent>();

                Observable.Interval(TimeSpan.FromMilliseconds(3000)).Where(_=>_reachedToPlayer).Subscribe(x => { Attack(); })
                    .AddTo(_disposables);
           
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filterTrigger)
            {
               
             _reachedToPlayer = _isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath;
                Debug.Log(_isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath);
            

                ref HealthViewComponent healthView = ref _playerHealthViewComponentPool.Get(entity);
                var currentHealh = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives;
                healthView.Value.size = new Vector2(currentHealh, 1);

          //  _isReachedComponentPool.Del(entity);
            }
            _disposables.Clear();
        
        }

        private void Attack()
        {
            _sharedData.GetPlayerCharacteristic.GetLives.UpdateLives(-1);
        }
    }
}