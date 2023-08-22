using Leopotam.EcsLite;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public class CameraBuildSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<CameraStartPositionComponent> _cameraStartPositionComponentPool;
        private EcsPool<CameraStartRotationComponent> _cameraStartRotationComponentPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsCameraComponent>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _cameraStartPositionComponentPool = world.GetPool<CameraStartPositionComponent>();
            _cameraStartRotationComponentPool = world.GetPool<CameraStartRotationComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var prefabComponent = ref _prefabPool.Get(entity);
                ref var transformComponent = ref _transformComponentPool.Add(entity);
                ref var cameraPosition = ref _cameraStartPositionComponentPool.Get(entity);
                ref var cameraRotation = ref _cameraStartRotationComponentPool.Get(entity);

                var gameObject = Object.Instantiate(prefabComponent.Value);
                transformComponent.Value =  gameObject.GetComponent<TransformView>().Transform;
                gameObject.transform.position = cameraPosition.Value;
                gameObject.transform.eulerAngles = cameraRotation.Value;
               _prefabPool.Del(entity);
            }
        }
    }
}