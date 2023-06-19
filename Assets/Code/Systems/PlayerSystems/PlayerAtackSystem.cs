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
    public class PlayerAtackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private List<IDisposable> _disposables = new List<IDisposable>();

        private EcsFilter _filter;
        private EcsFilter _enemyFilter;
        private bool _canCLickAtack;
        private EcsPool<IsPlayerCanAttackComponent> _isCanAttackComponentPool;
        private EcsPool<HealthViewComponent> _enemyHealthViewComponentPool;
        private EcsPool<EnemyHealthComponent> _enemyHealthComponentPool;
        private PlayerSharedData _sharedData;
        private int _entity;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _filter = systems.GetWorld().Filter<IsPlayerCanAttackComponent>()
                .End();
            _enemyFilter = systems.GetWorld().Filter<IsEnemyComponent>()
                .Inc<EnemyHealthComponent>()
                .Inc<HealthViewComponent>()
                .End();
            _enemyHealthViewComponentPool = world.GetPool<HealthViewComponent>();
            _enemyHealthComponentPool = world.GetPool<EnemyHealthComponent>();
            _isCanAttackComponentPool = world.GetPool<IsPlayerCanAttackComponent>();
            _canCLickAtack = true;
                Observable.Interval(TimeSpan.FromMilliseconds(3000)).Where(_=>_canCLickAtack).Subscribe(x => { Attack(); })
                    .AddTo(_disposables);
           
        }

        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
               
        //     _reachedToPlayer = _isCanAttackComponentPool.Get(entity).IsRecheded.reachedEndOfPath;
         //       Debug.Log(_isCanAttackComponentPool.Get(entity).IsRecheded.reachedEndOfPath);
         foreach (var VARIABLE in _enemyFilter)
         {



             ref HealthViewComponent healthView = ref _enemyHealthViewComponentPool.Get(VARIABLE);
             ref EnemyHealthComponent healthValue = ref _enemyHealthComponentPool.Get(VARIABLE);
             var currentHealh = healthValue.HealthValue;
             healthView.Value.size = new Vector2(currentHealh ,1);
             _entity = VARIABLE;
         }
            }
            _disposables.Clear();
        
        }

        private void Attack()
        {
            ref EnemyHealthComponent healthValue = ref _enemyHealthComponentPool.Get(_entity);
            
            healthValue.HealthValue -= 1;
                Debug.Log(healthValue.HealthValue);

        }
    }
}