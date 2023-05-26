using Leopotam.EcsLite;
using LeopotamGroup.Globals;


namespace MSuhininTestovoe.B2B
{
    public class SettingMenuSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _filterPause;
        private EcsFilter _filterCloseSetting;
        private EcsWorld _world;

        private EcsPool<BtnCloseSetting> _closeSettingPool;
        private EcsPool<IsSettingMenu> _isSettingMenuPool;
        private EcsPool<BtnCloseSetting> _isBtnClosePool;
        private EcsPool<BtnShowSettingMenu> _isBtnShowSettingPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsSettingMenu>().Inc<BtnShowSettingMenu>().End();
            _filterCloseSetting = _world.Filter<IsSettingMenu>().Inc<BtnCloseSetting>().End();
            _filterPause = _world.Filter<IsPauseMenu>().End();
            _isSettingMenuPool = _world.GetPool<IsSettingMenu>();
            _isBtnClosePool = _world.GetPool<BtnCloseSetting>();
            _isBtnShowSettingPool = _world.GetPool<BtnShowSettingMenu>();
        }


        public void Run(IEcsSystems systems)
        {
            ShowSettingMenu();

            HideSettingMenu();
        }

        private void HideSettingMenu()
        {
            foreach (var entity in _filterCloseSetting)
            {
                ref var menu = ref _isSettingMenuPool.Get(entity);
                if (_isSettingMenuPool.Has(entity))
                {
                    menu.MenuValue.SetActive(false);
                }

                _isBtnClosePool.Del(entity);
                _isBtnShowSettingPool.Del(entity);
            }
        }

        private void ShowSettingMenu()
        {
            foreach (var e in _filterPause)
            {
                foreach (var entity in _filter)
                {
                    ref var menu = ref _isSettingMenuPool.Get(entity);
                    if (_isSettingMenuPool.Has(entity))
                    {
                        menu.MenuValue.SetActive(true);
                    }
                }
            }
        }
    }
}