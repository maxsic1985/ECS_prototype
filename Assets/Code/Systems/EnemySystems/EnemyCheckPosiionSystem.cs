using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;



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
        }

        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filterTrigger)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref IsMoveComponent isMoveComponent = ref _isMovingComponent.Get(entity);
                if (transformComponent.Value.gameObject.transform.position.x < -20)
                {
                    transformComponent.Value.position = new Vector2(10, new System.Random().Next(-3, 3));
                    _poolService.Return(transformComponent.Value.gameObject);
                   _isMovingComponent.Del(entity);
                }
            }
        }
    }
}