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
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<BackgroundComponent> _backGroundComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<IsBackgroundComponent>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
            _backGroundComponentPool = world.GetPool<BackgroundComponent>();
            _isMoveComponentPool = world.GetPool<IsMoveComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            foreach (int entity in _filter)
            {
                ref PrefabComponent prefabComponent = ref _prefabPool.Get(entity);
                ref BackgroundComponent backgroundComponent = ref _backGroundComponentPool.Get(entity);

                for (int j = 0; j < backgroundComponent.StartPlatformCount; j++)
                {
                    var moveEntity = ecsWorld.NewEntity();
                    ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(moveEntity);
                    ref TransformComponent transformComponent = ref _transformComponentPool.Add(moveEntity);
                    ref SpeedComponent speedComponent = ref _speedComponentPool.Add(moveEntity);
                    speedComponent.SpeedValue = backgroundComponent.Speed;


                    var gameObject = Object.Instantiate(prefabComponent.Value);
                    transformComponent.Value = gameObject.GetComponent<TransformView>().Transform;
                    //  var backgroundLenght = gameObject.GetComponent<BackgroundView>().GetPlatformLenght();
                    gameObject.transform.position = backgroundComponent.SpawnPlatformPoint[j];
                    if (GameObject.FindObjectOfType<PathfinderScan>().gameObject
                        .TryGetComponent(out PathfinderScan scan))
                        gameObject.transform.SetParent(scan.gameObject.transform);
                    gameObject.GetComponent<IActor>().AddEntity(moveEntity);
                }

                _prefabPool.Del(entity);
            }
        }
    }
}