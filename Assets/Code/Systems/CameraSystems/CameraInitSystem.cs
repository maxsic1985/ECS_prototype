using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{


    public class CameraInitSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabPool;
        private EcsPool<CameraStartPositionComponent> _cameraStartPositionComponentPool;
        private EcsPool<CameraStartRotationComponent> _cameraStartRotationComponentPool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsCameraComponent>().Inc<ScriptableObjectComponent>().End();
            _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
            _loadPrefabPool = _world.GetPool<LoadPrefabComponent>();
            _cameraStartPositionComponentPool = _world.GetPool<CameraStartPositionComponent>();
            _cameraStartRotationComponentPool = _world.GetPool<CameraStartRotationComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is CameraLoadData dataInit)
                {
                    ref var loadPrefabFromPool = ref _loadPrefabPool.Add(entity);
                    loadPrefabFromPool.Value = dataInit.Camera;
                    
                    ref var cameraStartPositionComponent = ref _cameraStartPositionComponentPool.Add(entity);
                    cameraStartPositionComponent.Value = dataInit.StartPosition;

                    ref var cameraStartRotationComponent = ref _cameraStartRotationComponentPool.Add(entity);
                    cameraStartRotationComponent.Value = dataInit.StartRotation;
                }
                _scriptableObjectPool.Del(entity);
            }
        }
    }
}