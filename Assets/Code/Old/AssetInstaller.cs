using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "AssetInstaller", menuName = "Installers/AssetInstaller")]
public class AssetInstaller : ScriptableObjectInstaller<AssetInstaller>
{
    [SerializeField] private MazeCellSize _cellSize;
    [SerializeField] private MazeSize _mazeSize;

    public override void InstallBindings()
    {
        Container.BindInstances(_cellSize,_mazeSize);
    }
}
