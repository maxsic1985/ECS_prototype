using Leopotam.EcsLite;
using LeopotamGroup.Globals;



namespace MSuhininTestovoe.B2B
{
    public class BoxCheckPositionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _boxFilter;
        private EcsPool<LenghtComponent> _lenghtComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsMoveComponent> _isMovingComponentPool;
        private EcsWorld _world;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();
            _world = systems.GetWorld();
            _boxFilter = _world
                .Filter<TransformComponent>()
                .Inc<IsMoveComponent>()
                .Inc<LenghtComponent>()
                .Inc<IsBoxComponent>()
                .End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _lenghtComponentPool = _world.GetPool<LenghtComponent>();
            _isMovingComponentPool = _world.GetPool<IsMoveComponent>();
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int platformEntity in _boxFilter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(platformEntity);
                ref LenghtComponent lenghtComponent = ref _lenghtComponentPool.Get(platformEntity);
                
                if (transformComponent.Value.position.x <= -lenghtComponent.Value * 2)
                {
                    _poolService.Return(transformComponent.Value.gameObject);
                    _isMovingComponentPool.Del(platformEntity);
                }
            }
        }
    }
}