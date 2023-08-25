using Leopotam.EcsLite;


namespace MSuhininTestovoe.B2B
{
    public class EnemyInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<IsPoolLoadedComponent> _loadPrefabPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartPositionComponentPool;
        private EcsPool<EnemyStartRotationComponent> _enemyStartRotationComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemySecutityZoneComponent> _enemySecutityZoneComponentPool;
        private EcsPool<EnemyHealthComponent> _enemyHealthComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsEnemyComponent>()
                .Inc<ScriptableObjectComponent>().End();
            _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
            _loadPrefabPool = _world.GetPool<IsPoolLoadedComponent>();
            _enemyStartPositionComponentPool = _world.GetPool<EnemyStartPositionComponent>();
            _enemyStartRotationComponentPool = _world.GetPool<EnemyStartRotationComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _enemySecutityZoneComponentPool = _world.GetPool<EnemySecutityZoneComponent>();
            _enemyHealthComponentPool = _world.GetPool<EnemyHealthComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is EnemyData dataInit)
                {
                    ref IsPoolLoadedComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);

                    SpawnEnemy(entity, dataInit);
                    _transformComponentPool.Add(entity);
                }

                _scriptableObjectPool.Del(entity);
            }
        }


        private void SpawnEnemy(int entity, EnemyData dataInit)
        {
            ref EnemyStartPositionComponent enemyStartPositionComponent =
                ref _enemyStartPositionComponentPool.Add(entity);


            ref EnemyStartRotationComponent enemyStartRotationComponent =
                ref _enemyStartRotationComponentPool.Add(entity);


            enemyStartPositionComponent.Value = dataInit.StartPositions;
            enemyStartRotationComponent.Value = dataInit.StartRotation;

            ref EnemySecutityZoneComponent securityZoneComponent =
                ref _enemySecutityZoneComponentPool.Add(entity);
            securityZoneComponent.DistanceValue = dataInit.SecurityZoneDistance;
        }
    }
}