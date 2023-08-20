using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class PlayerForceUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsPool<ForceComponent> _forceComponentPool;
        private EcsPool<PlayerRigidBodyComponent> _playerRigidBodyComponentPool;
        private Vector3 playerPosition;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<IsPlayerComponent>()
                .Inc<TransformComponent>()
                .Inc<IsPlayerControlComponent>()
                .Inc<ForceComponent>()
                .Inc<PlayerRigidBodyComponent>()
                .End();
            _forceComponentPool = world.GetPool<ForceComponent>();
            _playerRigidBodyComponentPool = world.GetPool<PlayerRigidBodyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _playerFilter)
            {
                ref PlayerRigidBodyComponent rigitBodyComponent = ref _playerRigidBodyComponentPool.Get(entity);
                ref ForceComponent forceComponent = ref _forceComponentPool.Get(entity);
                if (rigitBodyComponent.PlayerRigidbody == null)
                    return;

                rigitBodyComponent.PlayerRigidbody.AddForce(Vector2.up * forceComponent.ForceValue
                    , ForceMode2D.Force);
            }
        }
    }
}