using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NoizeObserver : INozeble
{
       private int _noize;
    [SerializeField] private Slider _noizeSlider;

    public int Noize
    {
        get
        {
            return _noize = (_noize >= 10) ? 10 : _noize;
        }
        set
        {
            _noize = (value < 0) ? 0 : value;
        }
    }

    [Inject]
    private void Init()
    {
        var noizup = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Where(_ => Input.anyKey).Subscribe(x => { Noize += 1; });
        var noizeDown = Observable.Interval(TimeSpan.FromMilliseconds(500)).Where(_ => !Input.anyKey).Subscribe(x => { Noize -= 1; });
    }
}
