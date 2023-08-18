using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BackGroundInitSystem : IEcsInitSystem, IEcsRunSystem
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
                if (_scriptableObjectPool.Get(entity).Value is BackgroundData dataInit)
                {
                  //  ref LoadPrefabComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);
                  //  loadPrefabFromPool.Value = dataInit.Prefab;
                    


                    _transformComponentPool.Add(entity);
                }

                _scriptableObjectPool.Del(entity);
            }
        }

       
    }
}