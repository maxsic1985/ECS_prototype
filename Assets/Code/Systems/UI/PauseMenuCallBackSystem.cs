using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using MSuhininTestovoe.B2B;
using UnityEngine.Scripting;

namespace MSuhininTestovoe
{
    sealed class PauseMenuCallBackSystem : EcsUguiCallbackSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsFilter _filtersetting;
        private EcsWorld _world;

        private EcsPool<BtnShowMenu> _showBtnCommandPool;
        private EcsPool<BtnHideMenu> _hideBtnCommandPool;
        private EcsPool<BtnRestart> _restartBtnCommandPool;
        private EcsPool<BtnShowSettingMenu> _settingBtnCommandPool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsPauseMenu>().End();
            _filtersetting = _world.Filter<IsSettingMenu>().End();
            _showBtnCommandPool = _world.GetPool<BtnShowMenu>();
            _hideBtnCommandPool = _world.GetPool<BtnHideMenu>();
            _restartBtnCommandPool = _world.GetPool<BtnRestart>();
            _settingBtnCommandPool = _world.GetPool<BtnShowSettingMenu>();
        }

        [Preserve]
        [EcsUguiClickEvent(UIConstants.MENU_BTSHOW, WorldsNamesConstants.EVENTS)]
        void OnClickForward(in EcsUguiClickEvent e)
        {
            foreach (var entity in _filter)
            {
                _showBtnCommandPool.Add(entity);
            }
        }
        
        [Preserve]
        [EcsUguiClickEvent(UIConstants.MENU_BTCLOSE, WorldsNamesConstants.EVENTS)]
        void OnClickBack(in EcsUguiClickEvent e)
        {
            foreach (var entity in _filter)
            {
                _hideBtnCommandPool.Add(entity);
            }
        }
        
        
        [Preserve]
        [EcsUguiClickEvent(UIConstants.MENU_RESTART, WorldsNamesConstants.EVENTS)]
        void OnClickRestart(in EcsUguiClickEvent e)
        {
            foreach (var entity in _filter)
            {
                _restartBtnCommandPool.Add(entity);
            }
        }
        
        [Preserve]
        [EcsUguiClickEvent(UIConstants.MENU_SETTING, WorldsNamesConstants.EVENTS)]
        void OnClickSetting(in EcsUguiClickEvent e)
        {
            foreach (var entity in _filtersetting)
            {
                _settingBtnCommandPool.Add(entity);
            }
        }
    }
}   