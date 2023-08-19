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
    public class EnemyRespawnSystem : IEcsInitSystem
    {
        private EcsFilter filterTrigger;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
        private IPoolService _poolService;
        private List<IDisposable> _disposables = new List<IDisposable>();


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();
            EcsWorld world = systems.GetWorld();
            filterTrigger = systems.GetWorld()
                .Filter<IsEnemyComponent>()
                .Inc<TransformComponent>()
                .Exc<IsMoveComponent>()
                .End();
            _isMovingComponentPool = world.GetPool<IsMoveComponent>();

            Observable.Interval(TimeSpan.FromMilliseconds(20000)).Where(_ => true).Subscribe(x => { Respawn(); })
                .AddTo(_disposables);
        }

        private void Respawn()
        {
            GameObject pooled = _poolService.Get(GameObjectsTypeId.Enemy);
            var entity = pooled.GetComponent<Actor>().Entity;
            ref IsMoveComponent isMoveComponent = ref _isMovingComponentPool.Add(entity);
        }
    }
}