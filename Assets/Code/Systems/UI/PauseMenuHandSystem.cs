using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MSuhininTestovoe.B2B
{
    public class PauseMenuHandSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filterShowMenu;
        private EcsFilter _filterHieMenu;
        private EcsFilter _filterRestart;
        private EcsWorld _world;

        private EcsPool<BtnShowMenu> _menuPool;
        private EcsPool<BtnHideMenu> _menuHidePool;
        private EcsPool<BtnRestart> _menuRestartpool;
        private EcsPool<IsRestartComponent> _isRestartPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterShowMenu = _world.Filter<BtnShowMenu>().Inc<IsPauseMenu>().End();
            _filterHieMenu = _world.Filter<BtnHideMenu>().Inc<IsPauseMenu>().End();
            _filterRestart = _world.Filter<BtnRestart>().Inc<IsPauseMenu>().End();
           
            _menuPool = _world.GetPool<BtnShowMenu>();
            _menuHidePool = _world.GetPool<BtnHideMenu>();
            _menuRestartpool = _world.GetPool<BtnRestart>();
            _isRestartPool = _world.GetPool<IsRestartComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            ShowMenu();

            HideMenu();

            Restart();
        }

        private void Restart()
        {
            foreach (var entity in _filterRestart)
            {
                _isRestartPool.Add(entity);

                var menuPool = _world.GetPool<IsPauseMenu>();
                ref var menu = ref menuPool.Get(entity);
                if (_menuHidePool.Has(entity))
                {
                    menu.MenuValue.SetActive(false);
                }
                var timeServise = Service<ITimeService>.Get();
                timeServise.Resume();
                _menuHidePool.Del(entity);
                _menuRestartpool.Del(entity);
            }
        }

        private void HideMenu()
        {
            foreach (var entity in _filterHieMenu)
            {
                var menuPool = _world.GetPool<IsPauseMenu>();
                ref var menu = ref menuPool.Get(entity);

                if (_menuHidePool.Has(entity))
                {
                    menu.MenuValue.SetActive(false);
                }
                var timeServise = Service<ITimeService>.Get();
                timeServise.Resume();
                _menuHidePool.Del(entity);
            }
        }

        private void ShowMenu()
        {
            foreach (var entity in _filterShowMenu)
            {
                var timeServise = Service<ITimeService>.Get();
                timeServise.Pause();
                
                var menuPool = _world.GetPool<IsPauseMenu>();
                ref var menu = ref menuPool.Get(entity);

                if (_menuPool.Has(entity))
                {
                    menu.MenuValue.SetActive(true);
                }

                _menuPool.Del(entity);
            }
        }
    }
}