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
        private EcsPool<SpeedComponent> _speedComponentPool;
        private EcsPool<IsMoveComponent> _isMoveComponentPool;
        private EcsPool<IsBackgroundComponent> _isbackGroundComponentPool;
        private EcsPool<LenghtComponent> _lenghtComponentPool;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<IsBackgroundComponent>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _backGroundComponentPool = world.GetPool<BackgroundComponent>();
            _isbackGroundComponentPool = world.GetPool<IsBackgroundComponent>();
            _speedComponentPool = world.GetPool<SpeedComponent>();
            _isMoveComponentPool = world.GetPool<IsMoveComponent>();
            _lenghtComponentPool = world.GetPool<LenghtComponent>();
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
                    var gameObject = Object.Instantiate(prefabComponent.Value);


                    var moveEntity = ecsWorld.NewEntity();
                    ref IsMoveComponent isMoveComponent = ref _isMoveComponentPool.Add(moveEntity);
                    ref TransformComponent transformComponent = ref _transformComponentPool.Add(moveEntity);
                    ref SpeedComponent speedComponent = ref _speedComponentPool.Add(moveEntity);
                    ref LenghtComponent lenghtComponent = ref _lenghtComponentPool.Add(moveEntity);
                    ref IsBackgroundComponent isBackgroundComponent = ref _isbackGroundComponentPool.Add(moveEntity);
                  
                    speedComponent.SpeedValue = backgroundComponent.Speed;
                    transformComponent.Value = gameObject.GetComponent<TransformView>().Transform;
                   
                    var backgroundLenght = gameObject.GetComponent<BackgroundView>().GetPlatformLenght();
                     lenghtComponent.Value = backgroundLenght;
                  
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