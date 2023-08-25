using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using Pathfinding;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public class EnemyBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<IsPoolLoadedComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartPositionComponentPool;
        private EcsPool<EnemyStartRotationComponent> _enemyStartRotationComponentPool;
        private EcsPool<EnemyPathfindingComponent> _enemyPathfindingComponenPool;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<IsEnemyComponent> _isEnemyComponentPool;
        private EcsPool<BoxColliderComponent> _enemyBoxColliderComponentPool;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _poolService = Service<IPoolService>.Get();
            _filter = world.Filter<IsEnemyComponent>().Inc<IsPoolLoadedComponent>().End();
            _prefabPool = world.GetPool<IsPoolLoadedComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _enemyStartPositionComponentPool = world.GetPool<EnemyStartPositionComponent>();
            _enemyStartRotationComponentPool = world.GetPool<EnemyStartRotationComponent>();
            _enemyBoxColliderComponentPool = world.GetPool<BoxColliderComponent>();
            _enemyPathfindingComponenPool = world.GetPool<EnemyPathfindingComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
            _isMoveComponentPool = world.GetPool<IsMoveComponent>();
            _isEnemyComponentPool = world.GetPool<IsEnemyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            foreach (int entity in _filter)
            {
                for (int i = 0; i <= 10; i++)
                {
                    var newEntity = systems.GetWorld().NewEntity();
                    GameObject pooled = _poolService.Get(GameObjectsTypeId.Enemy);
                    pooled.gameObject.GetComponent<Actor>().AddEntity(newEntity);

                    ref TransformComponent transformComponent = ref _transformComponentPool.Add(newEntity);
                    ref EnemyStartPositionComponent enemyPosition = ref _enemyStartPositionComponentPool.Add(newEntity);
                    ref EnemyStartRotationComponent enemyRotation = ref _enemyStartRotationComponentPool.Add(newEntity);
                    ref BoxColliderComponent enemyBoxColliderComponent =
                        ref _enemyBoxColliderComponentPool.Add(newEntity);
                    ref EnemyPathfindingComponent enemyPathfindingComponent =
                        ref _enemyPathfindingComponenPool.Add(newEntity);
                    ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(newEntity);
                    ref SpeedComponent speedComponent = ref _speedComponentPool.Add(newEntity);

                    speedComponent.SpeedValue = 3;
                    ref IsEnemyComponent isBoxComponent = ref _isEnemyComponentPool.Add(newEntity);

                    transformComponent.Value = pooled.gameObject.GetComponent<TransformView>().Transform;
                    enemyPathfindingComponent.AIDestinationSetter =
                        pooled.gameObject.GetComponent<AIDestinationSetter>();
                  //  pooled.gameObject.transform.position = enemyPosition.Value[0];
//                    pooled.gameObject.transform.rotation = Quaternion.EulerAngles(enemyRotation.Value[0]);
                    pooled.gameObject.GetComponent<IActor>().AddEntity(newEntity);
                    enemyBoxColliderComponent.ColliderValue = pooled.GetComponent<BoxCollider>();
                    _poolService.Return(pooled);
                }

                _isEnemyComponentPool.Del(entity);
                _prefabPool.Del(entity);
            }
        }
    }
}