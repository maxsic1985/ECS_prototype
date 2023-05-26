using Zenject;
using UnityEngine;

public class MazeInstaller : MonoInstaller
{
    [SerializeField] private MazeCellView _cellView;
    [SerializeField] private HintRenderer _hintRenderer;

    public override void InstallBindings()
    {
        Container.Bind<IMazeGenerator>().To<MazeGenerator>().AsSingle();
        Container.Bind<MazeCellView>().FromInstance(_cellView);
        Container.Bind<HintRenderer>().FromInstance(_hintRenderer);
    }
}
