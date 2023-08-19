using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class CameraFollowSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _cameraFilter;
        private EcsFilter _playerFilter;
        private EcsPool<IsCameraComponent> _isCameraComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _cameraFilter = world.Filter<IsCameraComponent>().Inc<TransformComponent>().End();
            _playerFilter = world.Filter<IsPlayerComponent>().End();
            _isCameraComponentPool = world.GetPool<IsCameraComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int cameraEntity in _cameraFilter)
            {
                ref IsCameraComponent isCameraComponent = ref _isCameraComponentPool.Get(cameraEntity);
                ref TransformComponent cameraTransformComponent = ref _transformComponentPool.Get(cameraEntity);
                ref TransformComponent playerTransformComponent =
                    ref _transformComponentPool.Get(_playerFilter.GetRawEntities()[0]);
                Vector3 currentPosition = cameraTransformComponent.Value.position;
                Vector3 targetPoint = new Vector3(playerTransformComponent.Value.position.x, 0,-10) + isCameraComponent.Offset;

                cameraTransformComponent.Value.position =
                    Vector3.SmoothDamp(
                    currentPosition,
                    targetPoint,
                    ref isCameraComponent.CurrentVelocity,
                    isCameraComponent.CameraSmoothness);
            }
        }
    }
}