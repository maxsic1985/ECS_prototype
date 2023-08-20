using System;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine.SceneManagement;

namespace MSuhininTestovoe.B2B
{
    public class BoxSecurityZoneSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private PlayerSharedData _sharedData;
        private EcsFilter _filterEnterToTrigger;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterEnterToTrigger = _world.Filter<OnTriggerEnter2DEvent>()
                .End();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
        }

        public void Run(IEcsSystems ecsSystems)
        {
            var poolEnter = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();

            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
              //  if (eventData.collider2D.GetComponent<BorderActor>() == null) return;

                SceneManager.LoadScene(0);


                poolEnter.Del(entity);
            }

        }



        public void Destroy(IEcsSystems systems)
        {
            throw new NotImplementedException();
        }
    }
}