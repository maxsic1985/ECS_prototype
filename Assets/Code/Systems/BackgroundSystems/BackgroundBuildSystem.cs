using System;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MSuhininTestovoe.B2B
{
    public class BackgroundBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<BackgroundComponent> _backGroundComponentPool;
        private EcsPool<BoxColliderComponent> _enemyBoxColliderComponentPool;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
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

                for (int j = 0; j < backgroundComponent.StartPlatformCount; j++)
                {
                    var gameObject = Object.Instantiate(prefabComponent.Value);
                    transformComponent.Value = gameObject.GetComponent<TransformView>().Transform;
                    gameObject.transform.position = backgroundComponent.SpawnPlatformPoint[j];
                    if (GameObject.FindObjectOfType<PathfinderScan>().gameObject
                        .TryGetComponent(out PathfinderScan scan))
                        gameObject.transform.SetParent(scan.gameObject.transform);
                    gameObject.GetComponent<IActor>().AddEntity(entity);
                }

                _prefabPool.Del(entity);
            }
        }
    }
}