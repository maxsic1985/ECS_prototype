using System;
using System.Collections.Generic;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using LeopotamGroup.Globals;
using TMPro;
using UniRx;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class EnemyAtackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private List<IDisposable> _disposables = new List<IDisposable>();
        private EcsFilter _playerFilter;
        private EcsFilter filterTrigger;
        private EcsFilter filterExitTrigger;

        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsEnemyAtackComponent> _isEnemyAtackingComponentPool;
        private EcsPool<OnTriggerStay2DEvent> _onTriggerStay2DEventPool;
        private EcsPool<OnTriggerExit2DEvent> _onTriggerExit2DEventPool;
        private ITimeService _timeService;
        private PlayerSharedData _sharedData;
        private Vector3 playerPosition;
        private bool fix = false;
        
        [EcsUguiNamed(UIConstants.LIVES_LBL)] readonly TextMeshProUGUI _liveslabel = default;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _onTriggerStay2DEventPool = world.GetPool<OnTriggerStay2DEvent>();
            _onTriggerExit2DEventPool = world.GetPool<OnTriggerExit2DEvent>();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _playerFilter = world.Filter<IsEnemyComponent>()
                .Inc<TransformComponent>()
                .Inc<EnemyIsFollowingComponent>()
                .End();
            filterTrigger = systems.GetWorld().Filter<OnTriggerStay2DEvent>().End();
            filterExitTrigger = systems.GetWorld().Filter<OnTriggerExit2DEvent>()
                .Inc<IsEnemyComponent>()
                .End();

            _isEnemyAtackingComponentPool = world.GetPool<IsEnemyAtackComponent>();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filterTrigger)
            {
                ref var eventData = ref _onTriggerStay2DEventPool.Get(entity);
                if (eventData.collider2D.GetComponent<PlayerActor>() && !fix)
                {
                    ref IsEnemyAtackComponent atavk = ref _isEnemyAtackingComponentPool.Add(entity);
                    Observable.Timer(TimeSpan.FromMilliseconds(3000)).Where(_ => fix).Subscribe(x => { Attack(); }).AddTo(_disposables);
                    fix = true;
                    _onTriggerStay2DEventPool.Del(entity);
                }
            }

            foreach (int entity in filterExitTrigger)
            {
                ref var eventData2 = ref _onTriggerExit2DEventPool.Get(entity);
                
                if (eventData2.collider2D.GetComponent<PlayerActor>())
                {
                    _onTriggerStay2DEventPool.Del(entity);
                    foreach (var disposable in _disposables)
                    {
                        disposable.Dispose();
                    }
                    
                    Debug.Log("here");
                    systems.DelHere<OnTriggerStay2DEvent>();
                    systems.DelHere<OnTriggerExit2DEvent>();
            
                    systems.DelHerePhysics();
                    _disposables.Clear();
                }
            
            
            }

            _liveslabel.text = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives.ToString();

//             foreach (int entity in _playerFilter)
//             {
//                 ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
//                 ref EnemyIsFollowingComponent followingComponent = ref _isEnemyAtackingComponentPool.Get(entity);
//
// //                Debug.Log(entity.ToString());
//             }
        }

        private void Attack()
        {
            _sharedData.GetPlayerCharacteristic.GetLives.UpdateLives(-1);
            fix = false;
        }

        private void Atack(OnTriggerStay2DEvent eventData)
        {
            var playerEntity = eventData.collider2D.gameObject.GetComponent<PlayerActor>().Entity;
            ref TransformComponent transformComponent = ref _transformComponentPool.Get(playerEntity);
            transformComponent.Value.position = transformComponent.Value.position + Vector3.left;
        }


        private void
            PlayerMoving(ref TransformComponent transformComponent,
                ref PlayerInputComponent inputComponent) //,ref DestinationComponent destinationComponent)
        {
            Vector3 direction = Vector3.up * inputComponent.Vertical + Vector3.right * inputComponent.Horizontal;
            transformComponent.Value.position = Vector3.Lerp(transformComponent.Value.position,
                transformComponent.Value.position + direction,
                _sharedData.GetPlayerCharacteristic.Speed * _timeService.DeltaTime);
        }
    }
}