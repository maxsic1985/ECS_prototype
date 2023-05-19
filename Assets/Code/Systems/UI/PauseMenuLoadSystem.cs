using Leopotam.EcsLite;


namespace MSuhininTestovoe.B2B
{
    public class PauseMenuLoadSystem : IEcsInitSystem
    {
        private EcsPool<IsPauseMenu> _isMenu;

        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            
            _isMenu = world.GetPool<IsPauseMenu>();
            _isMenu.Add(entity);

            var loadDataByNameComponent = world.GetPool<LoadDataByNameComponent>();
            ref var loadFactoryDataComponent = ref loadDataByNameComponent.Add(entity);
            loadFactoryDataComponent.AddressableName = AssetsNamesConstants.MENU_PAUSE;
        }
    }
}