using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemyBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartPositionComponentPool;
        private EcsPool<EnemyStartRotationComponent> _enemyStartRotationComponentPool;
        private EcsPool<EnemyPathfindingComponent> _enemyPathfindingComponenPool;
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;

        private EcsPool<BoxColliderComponent> _enemyBoxColliderComponentPool;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _poolService = Service<IPoolService>.Get();
            _filter = world.Filter<IsEnemyComponent>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _enemyStartPositionComponentPool = world.GetPool<EnemyStartPositionComponent>();
            _enemyStartRotationComponentPool = world.GetPool<EnemyStartRotationComponent>();
            _enemyBoxColliderComponentPool = world.GetPool<BoxColliderComponent>();
            _enemyPathfindingComponenPool = world.GetPool<EnemyPathfindingComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
            _isMoveComponentPool = world.GetPool<IsMoveComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            foreach (int entity in _filter)
            {
                ref PrefabComponent prefabComponent = ref _prefabPool.Get(entity);
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref EnemyStartPositionComponent enemyPosition = ref _enemyStartPositionComponentPool.Get(entity);
                ref EnemyStartRotationComponent enemyRotation = ref _enemyStartRotationComponentPool.Get(entity);
                ref BoxColliderComponent enemyBoxColliderComponent = ref _enemyBoxColliderComponentPool.Add(entity);
                ref EnemyPathfindingComponent enemyPathfindingComponent = ref _enemyPathfindingComponenPool.Add(entity);
                ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(entity);
                ref SpeedComponent speedComponent = ref _speedComponentPool.Add(entity);
                speedComponent.SpeedValue = 1;
              
                    GameObject pooled = _poolService.Get(GameObjectsTypeId.Enemy);
                    transformComponent.Value = pooled.gameObject.GetComponent<TransformView>().Transform;
                    enemyPathfindingComponent.AIDestinationSetter =
                        pooled.gameObject.GetComponent<AIDestinationSetter>();
                    pooled.gameObject.transform.position = enemyPosition.Value;
                    pooled.gameObject.transform.rotation = Quaternion.EulerAngles(enemyRotation.Value);
                //    pooled.gameObject.transform.SetParent(GameObject.FindObjectOfType<BackgroundView>().gameObject.transform);
                 //   pooled.gameObject.GetComponent<CollisionCheckerView>().EcsWorld = ecsWorld;
                    pooled.gameObject.GetComponent<IActor>().AddEntity(entity);
                    enemyBoxColliderComponent.ColliderValue = pooled.GetComponent<BoxCollider>();
                    
                   
                    
                

                _prefabPool.Del(entity);
            }
        }
    }
}