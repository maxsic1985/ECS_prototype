using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class TriggerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerSharedData _sharedData;
        private EcsFilter hitCoinsFilter;
        private EcsFilter hitPillarsFilter;
        private EcsFilter hitWrenchFilter;

        [EcsUguiNamed(UIConstants.COINS_LBL)] readonly TextMeshProUGUI _coinslabel = default;

        [EcsUguiNamed(UIConstants.LIVES_LBL)] readonly TextMeshProUGUI _liveslabel = default;
        
        [EcsUguiNamed(UIConstants.KEY_LBL)] readonly TextMeshProUGUI _keyslabel = default;


        public void Init(IEcsSystems systems)
        {
            hitCoinsFilter = systems.GetWorld().Filter<HitComponent>().Inc<IsHitCoinsComponent>().End();
            hitPillarsFilter = systems.GetWorld().Filter<HitComponent>().Inc<IsHitPillarComponent>().End();
            hitWrenchFilter = systems.GetWorld().Filter<HitComponent>().Inc<IsHitWrenchComponent>().End();

            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            SetInitValues();
        }

        
        private void SetInitValues()
        {
            _coinslabel.text = _sharedData.GetPlayerCharacteristic.GetCurrentCoins.ToString();
            _liveslabel.text = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives.ToString();
        }


        public void Run(IEcsSystems systems)
        {
            GetTriggers(systems,
                hitCoinsFilter,
                hitPillarsFilter, 
                hitWrenchFilter);
        }

        
        private void GetTriggers(IEcsSystems systems, EcsFilter hitCoinsFilter, EcsFilter hitPillarsFilter,EcsFilter hitWrenchFilter)
        {
            foreach (var hitEntity in hitCoinsFilter)
            {
                // Add HitSound
                AddHitSoundComponent(systems, SoundsEnumType.Coin);
                //

                _sharedData.GetPlayerCharacteristic.AddCoins(1);
                _coinslabel.text = _sharedData.GetPlayerCharacteristic.GetCurrentCoins.ToString();
                systems.GetWorld().DelEntity(hitEntity);
            }

            foreach (var hitEntity in hitPillarsFilter)
            {
                // Add HitSound
                AddHitSoundComponent(systems, SoundsEnumType.Lamp);
                //

                // Change Music
                systems.GetWorld()
                .GetPool<IsSwitchMusicComponent>()
                .Add(SoundMusicSwitchSystem.musicSourceEntity);
                //

                _sharedData.GetPlayerCharacteristic.GetLives.AddLives(-1);
                _liveslabel.text = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives.ToString();
                systems.GetWorld().DelEntity(hitEntity);
            }

            foreach (var hitEntity in hitWrenchFilter)
            {
                // Add HitSound
                AddHitSoundComponent(systems, SoundsEnumType.Tool);
                //

                //TODO Wrench logic
                _sharedData.GetPlayerCharacteristic.GetWrench.UpdateWrench(1,
                     ref _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives,
                    _sharedData.GetPlayerCharacteristic.GetLives.GetMaxLives);
                
                _keyslabel.text = _sharedData.GetPlayerCharacteristic.GetWrench.GetCurrentWrench.ToString();
                _liveslabel.text = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives.ToString();
                systems.GetWorld().DelEntity(hitEntity);
            }
        }

        private void AddHitSoundComponent(IEcsSystems systems, SoundsEnumType type)
        {
            ref var isHitSoundComponent = ref systems.GetWorld()
                .GetPool<IsPlaySoundComponent>()
                .Add(SoundCatchSystem.sounEffectsSourceEntity);
            isHitSoundComponent.SoundType = type;
        }
    }
}