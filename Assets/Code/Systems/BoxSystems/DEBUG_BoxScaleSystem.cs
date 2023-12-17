using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class DEBUG_BoxScaleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<IsChangeScaleComponent> _isChsngeScaleComponent;
        private EcsPool<IsBoxComponent> _isBoxComponentPool;
        private EcsPool<Scale2DComponent> _scale2DComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsFilter _filter;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world
                .Filter<IsBoxComponent>()
                .Inc<Scale2DComponent>()
                .Exc<IsChangeScaleComponent>()
                .End();
            _scale2DComponentPool = world.GetPool<Scale2DComponent>();
            _isChsngeScaleComponent = world.GetPool<IsChangeScaleComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Scale2DComponent scale2D = ref _scale2DComponentPool.Get(entity);
                
                if (Input.GetKeyDown(KeyCode.M))
                {
                    ref IsChangeScaleComponent changeScale = ref _isChsngeScaleComponent.Add(entity);
                }
            }
        }
    }
}