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
    public class EnemyRespawnSystem : IEcsInitSystem,IEcsDestroySystem,IDisposable
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
              //  .Inc<TransformComponent>()
              //  .Exc<IsMoveComponent>()
                .End();
            _isMovingComponentPool = world.GetPool<IsMoveComponent>();

            Observable.Interval(TimeSpan.FromMilliseconds(2000)).Where(_ => true).Subscribe(x => { Respawn(); })
                .AddTo(_disposables);
        }

        private void Respawn()
        {
            
            foreach (var VARIABLE in filterTrigger)
            {
                if(_poolService == null)
                    {
                        _poolService = Service<IPoolService>.Get();
                        Dispose();
                    }

                    var pooled = _poolService.Get(GameObjectsTypeId.Enemy);

                    var entity = pooled.gameObject.GetComponent<EnemyActor>().Entity;
                    if (!_isMovingComponentPool.Has(entity))
                    {
                        ref IsMoveComponent isMoveComponent = ref _isMovingComponentPool.Add(entity);
                    }
                return;
            }
            // GameObject pooled = _poolService.Get(GameObjectsTypeId.Enemy);
            // var entity = pooled.GetComponent<EnemyActor>().Entity;
            // if (_isMovingComponentPool.Has(entity)) return;
            // ref IsMoveComponent isMoveComponent = ref _isMovingComponentPool.Add(entity);
        }

        public void Destroy(IEcsSystems systems)
        {
            Dispose();
            Debug.Log("here");
            systems.DelHere<OnTriggerStay2DEvent>();
            systems.DelHere<OnTriggerExit2DEvent>();

            systems.DelHerePhysics();
            _disposables.Clear();
        }

        public void Dispose()
        {
            _disposables.Clear();
        }
    }
}