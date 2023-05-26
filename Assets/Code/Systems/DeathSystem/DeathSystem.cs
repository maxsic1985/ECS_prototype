using Leopotam.EcsLite;
using LeopotamGroup.Globals;


namespace MSuhininTestovoe.B2B
{
    public class DeathSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsPlayerDeathComponent> _isPlayerDeathPool;
        private EcsPool<ShowDeathMenu> _isBtnShowMenu;
        private PlayerSharedData _sharedData;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;

            _filter = world.Filter<IsPlayerComponent>().Exc<IsPlayerDeathComponent>().End();
            _isPlayerDeathPool = world.GetPool<IsPlayerDeathComponent>();
            _isBtnShowMenu = world.GetPool<ShowDeathMenu>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                if (_sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives <= 0)
                {
                    ref var deathComponent = ref _isPlayerDeathPool.Add(entity);
                    ref var showMenu = ref _isBtnShowMenu.Add(entity);
                    var timeServise = Service<ITimeService>.Get();
                    timeServise.Pause();
                }
            }
        }
    }
}