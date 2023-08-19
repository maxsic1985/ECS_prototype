using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{
    public class BoxLoadSystem: IEcsInitSystem
    {
        private EcsPool<IsBoxComponent> _boxPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            
            _boxPool = world.GetPool<IsBoxComponent>();
            _boxPool.Add(entity);

          var loadDataByNameComponent = world.GetPool<LoadDataByNameComponent>();
            ref var loadFactoryDataComponent = ref loadDataByNameComponent.Add(entity);
            loadFactoryDataComponent.AddressableName = AssetsNamesConstants.BOX_DATA;
        }
    }
}