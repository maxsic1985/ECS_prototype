using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public class TriggerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerSharedData _sharedData;
       // private EcsFilter hitCoinsFilter;
      ///  private EcsFilter hitPillarsFilter;
        private EcsFilter hitPlayerFilter;

      //  [EcsUguiNamed(UIConstants.COINS_LBL)] readonly TextMeshProUGUI _coinslabel = default;
      //  [EcsUguiNamed(UIConstants.LIVES_LBL)] readonly TextMeshProUGUI _liveslabel = default;
       // [EcsUguiNamed(UIConstants.KEY_LBL)] readonly TextMeshProUGUI _keyslabel = default;


        public void Init(IEcsSystems systems)
        {
          //  hitCoinsFilter = systems.GetWorld().Filter<HitComponent>().Inc<IsHitCoinsComponent>().End();
         //   hitPillarsFilter = systems.GetWorld().Filter<HitComponent>().Inc<IsHitPillarComponent>().End();
            hitPlayerFilter = systems.GetWorld().Filter<HitComponent>().Inc<IsHitPlayerComponent>().End();

            _sharedData = systems.GetShared<SharedData>().GetPlayerSharedData;
            SetInitValues();
        }

        
        private void SetInitValues()
        {
         //   _liveslabel.text = _sharedData.GetPlayerCharacteristic.GetLives.GetCurrrentLives.ToString();
        }


        public void Run(IEcsSystems systems)
        {
            GetTriggers(systems,
             //   hitCoinsFilter,
             //   hitPillarsFilter, 
                hitPlayerFilter);
        }

        
        private void GetTriggers(IEcsSystems systems,EcsFilter hitWrenchFilter)
        {
            foreach (var hitEntity in hitPlayerFilter)
            {
              Debug.Log("set ai path");
                _sharedData.GetPlayerCharacteristic.GetLives.AddLives(-1);
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