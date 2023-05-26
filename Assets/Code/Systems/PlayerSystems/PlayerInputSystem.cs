﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LeopotamGroup.Globals;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<PlayerInputComponent> _playerInputComponentPool;
      //  private readonly JoystickService _joystick;
        readonly EcsCustomInject<JoystickInputView> _joystick = default;
        private int _entity;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPlayerComponent>().Inc<TransformComponent>().End();
            _playerInputComponentPool = _world.GetPool<PlayerInputComponent>();
         //   _inputService = Service<IInputService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
           

            foreach (int entity in _filter)
            {
                ref PlayerInputComponent playerInputComponent = ref _playerInputComponentPool.Get(entity);
                playerInputComponent.Horizontal = _joystick.Value.Horizontal;
                playerInputComponent.Vertical = _joystick.Value.Vertical;
            }
        }
    }
}