using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<PlayerInputComponent> _playerInputComponentPool;
        private EcsPool<IsPlayerControlComponent> _isPlayerControlComponent;

        private int _entity;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPlayerComponent>()
                .Inc<TransformComponent>()
                .End();
            _isPlayerControlComponent = _world.GetPool<IsPlayerControlComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (Input.GetKey(KeyCode.Space) && !_isPlayerControlComponent.Has(entity))
                {
                    ref IsPlayerControlComponent playerIsControllComponent =
                        ref _isPlayerControlComponent.Add(entity);
                }
                else
                {
                    _isPlayerControlComponent.Del(entity);
                }
            }
        }
    }
}