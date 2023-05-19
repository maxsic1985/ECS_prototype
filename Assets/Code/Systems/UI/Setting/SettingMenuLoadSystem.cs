using Leopotam.EcsLite;


namespace MSuhininTestovoe.B2B
{
    public class SettingMenuLoadSystem : IEcsInitSystem
    {
        private EcsPool<IsSettingMenu> _isSettingMenuPool;

        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            
            _isSettingMenuPool = world.GetPool<IsSettingMenu>();
            _isSettingMenuPool.Add(entity);

            var loadDataByNameComponent = world.GetPool<LoadDataByNameComponent>();
            ref var loadFactoryDataComponent = ref loadDataByNameComponent.Add(entity);
            loadFactoryDataComponent.AddressableName = AssetsNamesConstants.MENU_SETTING;
        }
    }
}