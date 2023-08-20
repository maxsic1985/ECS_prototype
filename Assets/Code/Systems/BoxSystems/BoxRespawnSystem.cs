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
    public class BoxRespawnSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsFilter filterTrigger;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
        private IPoolService _poolService;
        private List<IDisposable> _disposables = new List<IDisposable>();
        private PlayerSharedData _sharedData;


        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;

            _poolService = Service<IPoolService>.Get();
            EcsWorld world = systems.GetWorld();
            filterTrigger = systems.GetWorld()
                .Filter<IsBoxComponent>()
                .Inc<TransformComponent>()
                .Exc<IsMoveComponent>()
                .End();
            _isMovingComponentPool = world.GetPool<IsMoveComponent>();

            Observable.Interval(TimeSpan.FromMilliseconds(5000)).Where(_ => true).Subscribe(x => { Respawn(_sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives); })
                .AddTo(_disposables);
        }

        private void Respawn(int cnt)
        {
            var spawnCnt = new System.Random().Next(1, cnt);
            for (int i = 0; i < spawnCnt; i++)
            {
                if (_poolService==null)
                {
                    _poolService = Service<IPoolService>.Get();

                }
                GameObject pooled = _poolService.Get(GameObjectsTypeId.Box);

                var entity = pooled.gameObject.GetComponent<BorderActor>().Entity;
                if (!_isMovingComponentPool.Has(entity))
                {
                    ref IsMoveComponent isMoveComponent = ref _isMovingComponentPool.Add(entity);
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            throw new NotImplementedException();
        }
    }
}