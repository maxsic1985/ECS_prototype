using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{


    public class SoundMusicSwitchSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<SoundMusicSourceComponent> _soundMusicSourceComponentPool;
        private EcsPool<IsSwitchMusicComponent> _isSwitchMusicComponentPool;
        private EcsFilter _filter;
        public static int musicSourceEntity;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsSwitchMusicComponent>().End();
            _isSwitchMusicComponentPool = world.GetPool<IsSwitchMusicComponent>();
            _soundMusicSourceComponentPool = world.GetPool<SoundMusicSourceComponent>();
        }

        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var soundSwitching = ref _soundMusicSourceComponentPool.Get(musicSourceEntity);
                var audioSource = soundSwitching.Source;

                soundSwitching.PlayedTrack++;
                if (soundSwitching.PlayedTrack >= soundSwitching.Tracks.Length)
                {
                    soundSwitching.PlayedTrack = 0;
                }
                audioSource.clip = soundSwitching.Tracks[soundSwitching.PlayedTrack];
                audioSource.Play();

                _isSwitchMusicComponentPool.Del(entity);
            }
        }
    }
}