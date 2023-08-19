using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;
using Zenject.SpaceFighter;

namespace MSuhininTestovoe.B2B
{
    public class EnemyMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsWorld _world;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();

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


                if (transformComponent.Value.gameObject.TryGetComponent(out EnemyActor platform))
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