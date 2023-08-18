using Leopotam.EcsLite;
using LeopotamGroup.Globals;

namespace MSuhininTestovoe.B2B
{
    public class AccelerationPlatformSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _treadmillFilter;
        private EcsPool<BackgroundComponent> _backgroundComponentPool;
        private float _duration;
        private ITimeService _timeService;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _treadmillFilter = world.Filter<IsTreadmillComponent>().Inc<BackgroundComponent>().End();
            _backgroundComponentPool = world.GetPool<BackgroundComponent>();
            _timeService = Service<ITimeService>.Get();
        }

        public void Run(IEcsSystems systems)
        {
            ref BackgroundComponent treadmillComponent =
                ref _backgroundComponentPool.Get(_treadmillFilter.GetRawEntities()[0]);

            if (_duration < treadmillComponent.AccelerationInterval)
            {
                _duration += _timeService.DeltaTime;
            }
            else
            {
                treadmillComponent.Speed += treadmillComponent.AccelerationValue;
                _duration = 0;
            }
        }
    }
}