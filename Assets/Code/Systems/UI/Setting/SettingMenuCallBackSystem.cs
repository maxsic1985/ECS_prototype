using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using MSuhininTestovoe.B2B;
using UnityEngine;
using UnityEngine.Scripting;


namespace MSuhininTestovoe
{
    sealed class SettingMenuCallBackSystem : EcsUguiCallbackSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<BtnCloseSetting> _closeSettingPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsSettingMenu>().Exc<BtnCloseSetting>().End();
            _closeSettingPool = _world.GetPool<BtnCloseSetting>();
        }

        [Preserve]
        [EcsUguiClickEvent(UIConstants.MENU_SETTING_CLOSE, WorldsNamesConstants.EVENTS)]
        void OnClickCloseSetting(in EcsUguiClickEvent e)
        {
            foreach (var entity in _filter)
            {
                _closeSettingPool.Add(entity);
            }
        }
    }
}