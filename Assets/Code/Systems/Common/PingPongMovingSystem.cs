using Leopotam.EcsLite;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public sealed class PingPongMovingSystem :  IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _poolPosition;
        private EcsPool<PingPongPositionComponent> _pingPongPosition;
        private EcsPool<PingPongSpeedComponent> _poolSpeedVector;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world
                .Filter<SpeedComponent>()
                .Inc<TransformComponent>()
                .Inc<PingPongPositionComponent>()
                .Inc<PingPongSpeedComponent>()
                .End();
            _poolPosition = world.GetPool<TransformComponent>();
            _pingPongPosition = world.GetPool<PingPongPositionComponent>();
            _poolSpeedVector = world.GetPool<PingPongSpeedComponent>();
        }


        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var position = ref _poolPosition.Get(entity);
                ref var speed = ref _poolSpeedVector.Get(entity);
                ref var pingPongPosition = ref _pingPongPosition.Get(entity);

                float pingPong = Mathf.PingPong(Time.time * speed.CurrentValue, 1);
                var downposition = new Vector2(0, pingPongPosition.DownValue);
                var upPosition = new Vector2(0, pingPongPosition.UpValue);
                var direction = Vector2.Lerp(downposition, upPosition, pingPong);
                 position.Value.position = new Vector2(  position.Value.position.x, direction.y);
            }
        }
    }
}