using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSuhininTestovoe.B2B
{
    public class BoxSecurityZoneSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerSharedData _sharedData;
        private EcsFilter _filterEnterToTrigger;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterEnterToTrigger = _world.Filter<OnTriggerExit2DEvent>()
                //  .Inc<IsBoxComponent>()
                .End();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
        }

        public void Run(IEcsSystems ecsSystems)
        {
            var poolEnter = ecsSystems.GetWorld().GetPool<OnTriggerExit2DEvent>();

            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);
                Debug.Log(eventData.senderGameObject);
                Debug.Log(eventData.collider2D);

                // if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;
                //  if (eventData.collider2D.GetComponent<BorderActor>() == null) return;

                SceneManager.LoadScene(0);


                poolEnter.Del(entity);
            }
        }
    }
}