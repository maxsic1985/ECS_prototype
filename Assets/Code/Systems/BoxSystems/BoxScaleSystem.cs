using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class BoxScaleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<IsBoxComponent> _isBoxComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<Scale2DComponent> _scale2DComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<IsBoxComponent>()
                .Inc<TransformComponent>()
                .Inc<Scale2DComponent>()
                .Inc<IsChangeScaleComponent>()
                .End();
         
            _isBoxComponentPool = _world.GetPool<IsBoxComponent>();
            _isMoveComponentPool = _world.GetPool<IsMoveComponent>();
            _scale2DComponentPool = _world.GetPool<Scale2DComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref Scale2DComponent scale2D = ref _scale2DComponentPool.Get(entity);
                var scale = scale2D.ScaleValue;
                transformComponent.Value.localScale = scale;
            }
        }
    }
}