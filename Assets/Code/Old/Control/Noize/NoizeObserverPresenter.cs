using System;
using UniRx;
using UnityEngine;
using Zenject;

public class NoizeObserverPresenter : MonoBehaviour
{
    [Inject] private INozeble _nozeble;
    [Inject] private NoizeObserverView _noizeObserverView;
    [SerializeField] private ProtectorAIPresenter[] _protector;
    public Action<GameObject> NoizeIsCritical;
    private IDisposable _sliderUpdater;


    [Inject]
    private void Init()
    {
        NoizeIsCritical += StartProtection;

        _sliderUpdater = Observable.EveryUpdate()
              .Subscribe(_ =>
              {
                  _noizeObserverView.NoizeSlider.value = _nozeble.Noize;
                  if (_nozeble.Noize == 10) NoizeIsCritical?.Invoke(gameObject);
              }).AddTo(this);

    }

    private void StartProtection(GameObject gameObject)
    {
        for (int i = 0; i < _protector.Length; i++)
        {
            _protector[i].StartProtection(gameObject);
        }
    }
}