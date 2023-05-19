using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{


    public class DEBUG_TestSoundSystems : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<SoundEffectsSourceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    systems.GetWorld().GetPool<IsSwitchMusicComponent>().Add(entity);

                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    AddIsSoundFromTriggerComponent(SoundsEnumType.Coin, systems, entity);

                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    AddIsSoundFromTriggerComponent(SoundsEnumType.Tool, systems, entity);

                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    AddIsSoundFromTriggerComponent(SoundsEnumType.Jump, systems, entity);

                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    AddIsSoundFromTriggerComponent(SoundsEnumType.Lamp, systems, entity);

                }
            }
        }

        private void AddIsSoundFromTriggerComponent(SoundsEnumType soundType, IEcsSystems systems, int entity)
        {
            ref var isSoundFromTriggerComponent = ref systems.GetWorld().GetPool<IsPlaySoundComponent>().Add(entity);
            isSoundFromTriggerComponent.SoundType = soundType;
        }
    }
}