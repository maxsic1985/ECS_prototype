using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using Pathfinding;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BackgroundBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<BackgroundComponent> _backGroundComponentPool;
        private EcsPool<BoxColliderComponent> _enemyBoxColliderComponentPool;
        private IPoolService _poolService;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _poolService = Service<IPoolService>.Get();
            _filter = world.Filter<IsBackgroundComponent>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _backGroundComponentPool = world.GetPool<BackgroundComponent>();
            _enemyBoxColliderComponentPool = world.GetPool<BoxColliderComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            foreach (int entity in _filter)
            {
                ref PrefabComponent prefabComponent = ref _prefabPool.Get(entity);
                ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                ref BackgroundComponent backgroundComponent = ref _backGroundComponentPool.Get(entity);
//                ref BoxColliderComponent enemyBoxColliderComponent = ref _enemyBoxColliderComponentPool.Add(entity);

                for (int j = 0; j < backgroundComponent.StartPlatformCount; j++)
                {
                    GameObject pooled = _poolService.Get(GameObjectsTypeId.BackGround);
                    transformComponent.Value = pooled.gameObject.GetComponent<TransformView>().Transform;
                    pooled.gameObject.transform.position = backgroundComponent.SpawnPlatformPoint;
                //    pooled.gameObject.transform.rotation = Quaternion.EulerAngles(enemyRotation.Value[j]);
                 //   pooled.gameObject.GetComponent<CollisionCheckerView>().EcsWorld = ecsWorld;
                    pooled.gameObject.GetComponent<IActor>().AddEntity(entity);
                 //   enemyBoxColliderComponent.ColliderValue = pooled.GetComponent<BoxCollider>();
                }

                _prefabPool.Del(entity);
            }
        }
    }
}