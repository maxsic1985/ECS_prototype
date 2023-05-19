using Leopotam.EcsLite;
using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSuhininTestovoe.B2B
{
    public class RestartSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<IsRestartComponent> _isRestartPool;
        private EcsFilter _filter;
        private PlayerSharedData _sharedData;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            _filter = world.Filter<IsRestartComponent>().End();
            _isRestartPool = world.GetPool<IsRestartComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var timeServise = Service<ITimeService>.Get();
                timeServise.Resume();
                _isRestartPool.Del(entity);
                _sharedData.GetPlayerCharacteristic.LoadInitValue();
                SceneManager.LoadScene(0);
            }
        }
    }
}