using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{
    public class EnemyInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<LoadPrefabComponent> _loadPrefabPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartPositionComponentPool;
        private EcsPool<EnemyStartRotationComponent> _enemyStartRotationComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemySecutityZoneComponent> _enemySecutityZoneComponentPool;
        private EcsPool<EnemyHealthComponent> _enemyHealthComponentPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsEnemyComponent>().Inc<ScriptableObjectComponent>().End();
            _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
            _loadPrefabPool = _world.GetPool<LoadPrefabComponent>();
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
                    ref LoadPrefabComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);
                    loadPrefabFromPool.Value = dataInit.EnemyPrefab;

                    ref EnemySecutityZoneComponent securityZoneComponent =
                        ref _enemySecutityZoneComponentPool.Add(entity);
                    securityZoneComponent.distanceValue = dataInit.SecurityZoneDistance;
                    
                    ref EnemyHealthComponent enemyHealth =
                        ref _enemyHealthComponentPool.Add(entity);
                    enemyHealth.HealthValue = dataInit.Lives;

                    
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
            

           var positionIndex= GetUniqeRandomArray(dataInit.CountForInstantiate,0,dataInit.StartPositions.Length);

            for (int i = 0; i < positionIndex.Length; i++)
            {
                enemyStartPositionComponent.Value=dataInit.StartPositions[positionIndex[i]];
                enemyStartRotationComponent.Value=dataInit.StartRotation[positionIndex[i]];
            }
        }
        
        public int[] GetUniqeRandomArray(int size , int Min , int Max ) {

            int [] UniqueArray = new int[size];
            var rnd = new System.Random();
            int Random;

            for (int i = 0 ; i < size ; i++) {

                Random = rnd.Next(Min, Max);

                for (int j = i; j >= 0 ; j--) {

                    if (UniqueArray[j] == Random)
                    { Random = rnd.Next(Min, Max); j = i; }

                }

                UniqueArray[i] = Random;

            }

            return UniqueArray;

        }
    }
}