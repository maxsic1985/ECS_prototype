using Leopotam.EcsLite;



namespace MSuhininTestovoe.B2B
{
    public class BackgroundLoadSystem: IEcsInitSystem
    {
        private EcsPool<IsBackgroundComponent> _isEnemyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            
            _isEnemyPool = world.GetPool<IsBackgroundComponent>();
            _isEnemyPool.Add(entity);

            var loadDataByNameComponent = world.GetPool<LoadDataByNameComponent>();
            ref var loadFactoryDataComponent = ref loadDataByNameComponent.Add(entity);
            loadFactoryDataComponent.AddressableName = AssetsNamesConstants.BACKGROUND_DATA;
        }
    }
}