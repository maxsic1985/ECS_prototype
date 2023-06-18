using System;
using System.Collections.Generic;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
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

        private EcsPool<IsReachedDestanationComponent> _isReachedComponentPool;
        private EcsPool<PlayerHealthViewComponent> _playerHealthViewComponentPool;

        private ITimeService _timeService;
        private PlayerSharedData _sharedData;

        [EcsUguiNamed(UIConstants.LIVES_LBL)] readonly TextMeshProUGUI _liveslabel = default;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            filterTrigger = systems.GetWorld().Filter<IsReachedDestanationComponent>()
             //  .Inc<PlayerHealthViewComponent>()
                .End();
            _playerHealthViewComponentPool = world.GetPool<PlayerHealthViewComponent>();

            _isReachedComponentPool = world.GetPool<IsReachedDestanationComponent>();
            _timeService = Service<ITimeService>.Get();

                Observable.Interval(TimeSpan.FromMilliseconds(3000)).Where(_=>_reachedToPlayer).Subscribe(x => { Attack(); })
                    .AddTo(_disposables);
           
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filterTrigger)
            {
               
             _reachedToPlayer = _isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath;
                Debug.Log(_isReachedComponentPool.Get(entity).IsRecheded.reachedEndOfPath);
            

                ref PlayerHealthViewComponent playerHealthView = ref _playerHealthViewComponentPool.Get(entity);
                var currentHealh = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives;
                playerHealthView.Value.size = new Vector2(currentHealh, 1);

            }
            _disposables.Clear();
        
        }

        private void Attack()
        {
            _sharedData.GetPlayerCharacteristic.GetLives.UpdateLives(-1);
        }
    }
}