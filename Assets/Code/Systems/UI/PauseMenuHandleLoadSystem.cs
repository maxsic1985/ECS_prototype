using Leopotam.EcsLite;


namespace MSuhininTestovoe.B2B
{
    public class PauseMenuHandleLoadSystem : IEcsInitSystem
    {
        private EcsPool<CloseBtnCommand> _isCloseBtn;

        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            
            _isCloseBtn = world.GetPool<CloseBtnCommand>();
            _isCloseBtn.Add(entity);

            var loadDataByNameComponent = world.GetPool<LoadDataByNameComponent>();
            ref var loadFactoryDataComponent = ref loadDataByNameComponent.Add(entity);
            loadFactoryDataComponent.AddressableName = AssetsNamesConstants.BTN_SHOW_MENU;
        }
    }
}