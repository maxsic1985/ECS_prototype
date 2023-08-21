using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UniRx;



namespace MSuhininTestovoe.B2B
{
    public class BoxRespawnSystem : IEcsInitSystem, IEcsDestroySystem,IDisposable
    {
        private EcsFilter filter;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
        private IPoolService _poolService;
        private List<IDisposable> _disposables = new List<IDisposable>();
        private PlayerSharedData _sharedData;


      
        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;

            _poolService = Service<IPoolService>.Get();
            EcsWorld world = systems.GetWorld();
            filter = systems.GetWorld()
                .Filter<IsBoxComponent>()
                .End();
            _isMovingComponentPool = world.GetPool<IsMoveComponent>();


            
                Observable.Interval(TimeSpan.FromMilliseconds(LimitsConstants.COOLDOWN_BOX)).Where(_ => true).Subscribe(x =>
                    {
                        Respawn(_sharedData.GetPlayerCharacteristic.CurrentScore);
                    })
                    .AddTo(_disposables);
              
            }
         
           
        

        private void Respawn(int cnt)
        {
            foreach (var _ in filter)
            {
                var spawnCnt = new System.Random().Next(1, cnt+1);
                for (int i = 0; i < spawnCnt; i++)
                {
                    if (_poolService == null)
                    {
                        _poolService = Service<IPoolService>.Get();
                    }

                    var pooled = _poolService.Get(GameObjectsTypeId.Box);

                    var entity = pooled.gameObject.GetComponent<BorderActor>().Entity;
                    if (!_isMovingComponentPool.Has(entity))
                    {
                        ref IsMoveComponent isMoveComponent = ref _isMovingComponentPool.Add(entity);
                    }
              
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