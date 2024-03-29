﻿using Leopotam.EcsLite;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public class EnemyMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsWorld _world;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<TransformComponent>()
                .Inc<IsMoveComponent>()
                .Inc<SpeedComponent>()
                .Exc<EnemyIsFollowingComponent>()
                .End();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _speedComponentPool = _world.GetPool<SpeedComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (int platformEntity in _filter)
            {
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(platformEntity);
                ref SpeedComponent speedComponent = ref _speedComponentPool.Get(platformEntity);


                if (transformComponent.Value.gameObject.TryGetComponent(out EnemyActor platform)
                    && transformComponent.Value.gameObject.activeSelf)
                {
                    Vector2 position = transformComponent.Value.position;
                    position -= new Vector2(speedComponent.SpeedValue * Time.deltaTime, 0);
                    transformComponent.Value.localPosition = position;
                    platform.gameObject.transform.position = position;
                }
            }
        }
    }
}