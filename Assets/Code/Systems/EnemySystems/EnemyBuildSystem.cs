﻿using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.UIElements;

namespace MSuhininTestovoe.B2B
{
    public class EnemyBuildSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<EnemyStartPositionComponent> _enemyStartPositionComponentPool;
        private EcsPool<EnemyStartRotationComponent> _enemyStartRotationComponentPool;
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

                //GameObject gameObject = Object.Instantiate(prefabComponent.Value);
                GameObject pooled = _poolService.Get(GameObjectsTypeId.Enemy);
                transformComponent.Value = pooled.gameObject.GetComponent<TransformView>().Transform;
                pooled.gameObject.transform.position = enemyPosition.Value;
                pooled.gameObject.transform.rotation = Quaternion.EulerAngles(enemyRotation.Value); 
                pooled.gameObject.GetComponentInChildren<CollisionCheckerView>().EcsWorld = ecsWorld;
                pooled.gameObject.GetComponent<IActor>().AddEntity(entity);
                enemyBoxColliderComponent.ColliderValue = pooled.GetComponent<BoxCollider>();
               _prefabPool.Del(entity);
            }
        }
    }
}