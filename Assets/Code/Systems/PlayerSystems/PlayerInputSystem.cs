using Leopotam.EcsLite;
using LeopotamGroup.Globals;

namespace MSuhininTestovoe.B2B
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private IInputService _inputService;
        private EcsPool<PlayerInputComponent> _playerInputComponentPool;
        private int _entity;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPlayerComponent>().Inc<TransformComponent>().End();
            _playerInputComponentPool = _world.GetPool<PlayerInputComponent>();
            _inputService = Service<IInputService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            _inputService.Update();

            foreach (int entity in _filter)
            {
                ref PlayerInputComponent playerInputComponent = ref _playerInputComponentPool.Get(entity);
                playerInputComponent.Horizontal = _inputService.Horizontal;
                playerInputComponent.Vertical = _inputService.Vertical;
            }
        }
    }
}