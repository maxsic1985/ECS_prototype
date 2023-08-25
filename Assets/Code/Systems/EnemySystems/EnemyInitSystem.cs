using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using Pathfinding;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public class EnemyInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private IPoolService _poolService;

        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<IsPoolLoadedComponent> _loadPrefabPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartPositionComponentPool;
        private EcsPool<EnemyStartRotationComponent> _enemyStartRotationComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemySecutityZoneComponent> _enemySecutityZoneComponentPool;
        private EcsPool<BoxColliderComponent> _enemyBoxColliderComponentPool;
        private EcsPool<EnemyPathfindingComponent> _enemyPathfindingComponenPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<IsEnemyComponent> _isEnemyComponentPool;
        private EcsPool<SpeedComponent> _speedComponentPool;


        public void Init(IEcsSystems systems)
        {
            _poolService = Service<IPoolService>.Get();
            _world = systems.GetWorld();
            _filter = _world
                .Filter<IsEnemyComponent>()
                .Inc<ScriptableObjectComponent>().End();
            _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
            _loadPrefabPool = _world.GetPool<IsPoolLoadedComponent>();
            _enemyStartPositionComponentPool = _world.GetPool<EnemyStartPositionComponent>();
            _enemyStartRotationComponentPool = _world.GetPool<EnemyStartRotationComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
            _enemySecutityZoneComponentPool = _world.GetPool<EnemySecutityZoneComponent>();
            _enemyBoxColliderComponentPool = _world.GetPool<BoxColliderComponent>();
            _enemyPathfindingComponenPool = _world.GetPool<EnemyPathfindingComponent>();
            _speedComponentPool = _world.GetPool<SpeedComponent>();
            _isMoveComponentPool = _world.GetPool<IsMoveComponent>();
            _isEnemyComponentPool = _world.GetPool<IsEnemyComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is EnemyData dataInit)
                {
                    ref IsPoolLoadedComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);

                    SpawnEnemy(entity, dataInit);
                }

                _scriptableObjectPool.Del(entity);
            }
        }


        private void SpawnEnemy(int entity, EnemyData dataInit)
        {
            for (int i = 0; i <= 10; i++)
            {
                var newEntity = _world.NewEntity();
                GameObject pooled = _poolService.Get(GameObjectsTypeId.Enemy);
                pooled.gameObject.GetComponent<Actor>().AddEntity(newEntity);

                ref IsEnemyComponent isBoxComponent = ref _isEnemyComponentPool.Add(newEntity);

                ref EnemyStartPositionComponent enemyStartPositionComponent =
                    ref _enemyStartPositionComponentPool.Add(newEntity);

                ref EnemyStartRotationComponent enemyStartRotationComponent =
                    ref _enemyStartRotationComponentPool.Add(newEntity);

                enemyStartPositionComponent.Value = dataInit.StartPositions;
                enemyStartRotationComponent.Value = dataInit.StartRotation;

                ref EnemySecutityZoneComponent securityZoneComponent =
                    ref _enemySecutityZoneComponentPool.Add(newEntity);
                securityZoneComponent.DistanceValue = dataInit.RayDistance;


                ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(newEntity);

                ref SpeedComponent speedComponent = ref _speedComponentPool.Add(newEntity);
                speedComponent.SpeedValue = dataInit.Speed;


                ref TransformComponent transformComponent = ref _transformComponentPool.Add(newEntity);
                transformComponent.Value = pooled.gameObject.GetComponent<TransformView>().Transform;

                ref EnemyPathfindingComponent enemyPathfindingComponent =
                    ref _enemyPathfindingComponenPool.Add(newEntity);
                enemyPathfindingComponent.AIDestinationSetter =
                    pooled.gameObject.GetComponent<AIDestinationSetter>();
                pooled.gameObject.GetComponent<IActor>().AddEntity(newEntity);

                ref BoxColliderComponent enemyBoxColliderComponent =
                    ref _enemyBoxColliderComponentPool.Add(newEntity);
                enemyBoxColliderComponent.ColliderValue = pooled.GetComponent<BoxCollider>();
                _poolService.Return(pooled);
            }

            _isEnemyComponentPool.Del(entity);
            _loadPrefabPool.Del(entity);
        }
    }
}