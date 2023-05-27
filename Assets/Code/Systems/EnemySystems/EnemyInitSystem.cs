using Leopotam.EcsLite;
using UnityEngine;

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
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is EnemyData dataInit)
                {
                    ref LoadPrefabComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);
                    loadPrefabFromPool.Value = dataInit.EnemyPrefab;

                    ref EnemyStartPositionComponent enemyStartPositionComponent =
                        ref _enemyStartPositionComponentPool.Add(entity);
                    enemyStartPositionComponent.Value = dataInit.StartPositions[0]; 
                    
                    ref EnemyStartRotationComponent enemyStartRotationComponent =
                        ref _enemyStartRotationComponentPool.Add(entity);
                    enemyStartRotationComponent.Value = dataInit.StartRotation[0];

                    ref EnemySecutityZoneComponent securityZoneComponent = ref _enemySecutityZoneComponentPool.Add(entity);
                    securityZoneComponent.distanceValue = dataInit.SecurityZoneDistance;

                    _transformComponentPool.Add(entity);
                }

                _scriptableObjectPool.Del(entity);
            }
        }
    }
}