using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class TestRotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsRotateComponent> _isRotateComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsBoxComponent>()
                .Exc<IsRotateComponent>()
                .End();

            _isRotateComponentPool = world.GetPool<IsRotateComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ref IsRotateComponent isRotate = ref _isRotateComponentPool.Add(entity);
                    Debug.Log("Rotation added");
                }
            }
        }
    }
}