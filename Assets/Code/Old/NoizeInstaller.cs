using Zenject;
using UnityEngine;

public class NoizeInstaller : MonoInstaller
{
    [SerializeField] private NoizeObserverView _noizeObserverView;
    public override void InstallBindings()
    {
        Container.Bind<INozeble>().To<NoizeObserver>().AsSingle();
        Container.Bind<NoizeObserverView>().FromInstance(_noizeObserverView);
    }
}