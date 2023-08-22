using Leopotam.EcsLite;



namespace MSuhininTestovoe.B2B
{
    public class BackGroundInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabPool;
        private EcsPool<BackgroundComponent> _backGroundComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;

        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsBackgroundComponent>()
                .Inc<ScriptableObjectComponent>().End();
            _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
            _loadPrefabPool = _world.GetPool<LoadPrefabComponent>();
            _backGroundComponentPool = _world.GetPool<BackgroundComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is BackgroundData dataInit)
                {
                    ref LoadPrefabComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);
                    loadPrefabFromPool.Value = dataInit.Value;

                    ref BackgroundComponent backgroundComponent = ref _backGroundComponentPool.Add(entity);
                    backgroundComponent.StartPlatformCount = dataInit.StartPlatformCount;
                    backgroundComponent.Speed = dataInit.Speed;
                    backgroundComponent.SpawnPlatformPoint = dataInit.StartPlatformPosition;
                    
                    _transformComponentPool.Add(entity);
                }
                _scriptableObjectPool.Del(entity);
            }
        }
    }
}