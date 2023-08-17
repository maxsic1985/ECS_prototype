using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class PlayerForceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsPool<IsPlayerControlComponent> _isPlayerControlComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsPlayerComponent> _isPlayerComponent;
        private EcsPool<PlayerRigidBodyComponent> _playerRigidBodyComponentPool;

        private ITimeService _timeService;
        private PlayerSharedData _sharedData;
        private Vector3 playerPosition;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _playerFilter = world.Filter<IsPlayerComponent>()
                .Inc<TransformComponent>()
                .Inc<IsPlayerControlComponent>()
                .Inc<PlayerRigidBodyComponent>()
                .End();
            _isPlayerComponent = world.GetPool<IsPlayerComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isPlayerControlComponentPool = world.GetPool<IsPlayerControlComponent>();
            _playerRigidBodyComponentPool = world.GetPool<PlayerRigidBodyComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref PlayerRigidBodyComponent rigitBodyComponent = ref _playerRigidBodyComponentPool.Get(entity);
                ref IsPlayerControlComponent playerControlComponentPool = ref _isPlayerControlComponentPool.Get(entity);
                if (rigitBodyComponent.PlayerRigidbody == null)
                    return;

                rigitBodyComponent.PlayerRigidbody.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
            }
        }
    }
}