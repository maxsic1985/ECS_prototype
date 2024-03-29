using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public sealed class TransformMovingSystem : EcsUguiCallbackSystem, IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PositionComponent> _poolPosition;
        private EcsPool<SpeedComponent> _poolSpeedVector;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<SpeedComponent>().Inc<PositionComponent>().End();
            _poolPosition = world.GetPool<PositionComponent>();
            _poolSpeedVector = world.GetPool<SpeedComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var position = ref _poolPosition.Get(entity);
                ref var speed = ref _poolSpeedVector.Get(entity);

                position.Value += new Vector2(speed.SpeedValue * Service<ITimeService>.Get().DeltaTime, 0);
            }
        }
    }
}