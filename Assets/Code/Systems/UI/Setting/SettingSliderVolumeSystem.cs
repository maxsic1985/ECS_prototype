using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace MSuhininTestovoe.B2B
{
    public class SettingSliderVolumeSystem : EcsUguiCallbackSystem, IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<BtnCloseSetting> _closeSettingPool;
        private Slider _slider;
        private AudioSource _audioSource;
        private TMP_Dropdown _dropDown;
        readonly EcsUguiEmitter _ugui = default;
        

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsSettingMenu>().Inc<BtnShowSettingMenu>().End();
            
        }

        private void GetCashedValues()
        {
            _audioSource = GameObject.FindObjectOfType<AudioSource>().gameObject?.GetComponent<AudioSource>();
            _slider = _ugui.GetNamedObject(UIConstants.MENU_SETTING_VOLUME)?.GetComponent<Slider>();
            _dropDown = _ugui.GetNamedObject(UIConstants.MENU_SETTING_DROP)?.GetComponent<TMP_Dropdown>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var e in _filter)
            {
                SoundVolumeHandle();
            }
        }

        private void SoundVolumeHandle()
        {
            if ((_slider is null) || (_audioSource is null) ||(_dropDown is null))
            {
                GetCashedValues();
               return;
            }
            _audioSource.volume = _slider.value;
            QualitySettings.SetQualityLevel(_dropDown.value, true);
           
        }
    }
}