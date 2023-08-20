using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BoxInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<ScriptableObjectComponent> _scriptableObjectPool;
        private EcsPool<IsPoolLoadedComponent> _loadPrefabPool;
        private EcsPool<BoxComponent> _boxComponentPool;
        private EcsPool<TransformComponent> _transformComponentPool;

        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsBoxComponent>()
               .Inc<ScriptableObjectComponent>()
                .End();
           _scriptableObjectPool = _world.GetPool<ScriptableObjectComponent>();
           _loadPrefabPool = _world.GetPool<IsPoolLoadedComponent>();
            _boxComponentPool = _world.GetPool<BoxComponent>();
            _transformComponentPool = _world.GetPool<TransformComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_scriptableObjectPool.Get(entity).Value is BoxData dataInit)
                {
                   ref IsPoolLoadedComponent loadPrefabFromPool = ref _loadPrefabPool.Add(entity);

                    ref BoxComponent boxComponent = ref _boxComponentPool.Add(entity);
                    boxComponent.StartBoxCount = dataInit.StartBoxCount;
                    boxComponent.Speed = dataInit.Speed;
                    boxComponent.SpawnBoxPoint = dataInit.StartBoxPoints;
                    


                    _transformComponentPool.Add(entity);
                }

                _scriptableObjectPool.Del(entity);
            }
        }

       
    }
}