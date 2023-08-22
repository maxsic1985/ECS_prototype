using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using TMPro;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class EnemySecurityZoneSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<EnemyIsFollowingComponent> _isFollowComponentPool;
        [EcsUguiNamed(UIConstants.COINS_LBL)] readonly TextMeshProUGUI _coinslabel = default;
        private PlayerSharedData _sharedData;
        private int _entity;
        private IPoolService _poolService;
        private EcsFilter _filterEnterToTrigger;
        private EcsPool<IsRestartComponent> _isRestartPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _poolService = Service<IPoolService>.Get();
            _filterEnterToTrigger = _world.Filter<OnTriggerEnter2DEvent>()
                .End();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _isFollowComponentPool = _world.GetPool<EnemyIsFollowingComponent>();
            _isRestartPool = _world.GetPool<IsRestartComponent>();
        }

        public void Run(IEcsSystems ecsSystems)
        {
          


            var poolEnter = ecsSystems.GetWorld().GetPool<OnTriggerEnter2DEvent>();
            foreach (var entity in _filterEnterToTrigger)
            {
                ref var eventData = ref poolEnter.Get(entity);
                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;

                if (eventData.collider2D.GetComponent<BorderActor>() != null)
                {
                    _isRestartPool.Add(entity);
                    _isFollowComponentPool.Del(entity);
                    poolEnter.Del(entity);
                    return;
                }

                if (eventData.senderGameObject.GetComponent<PlayerActor>() == null) return;

                if (eventData.collider2D.GetComponent<EnemyActor>() != null)
                {
                    var score = _sharedData.GetPlayerCharacteristic.AddScore(1);
                    _coinslabel.text = score.ToString();
                    _poolService.Return(eventData.collider2D.gameObject);
                }

                poolEnter.Del(entity);
            }
        }
    }
}