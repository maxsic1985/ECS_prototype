using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UniRx;



namespace MSuhininTestovoe.B2B
{
    public class EnemyRespawnSystem : IEcsInitSystem, IEcsDestroySystem, IDisposable
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
                .End();
            _isMovingComponentPool = world.GetPool<IsMoveComponent>();

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