using UnityEngine;

[CreateAssetMenu(fileName = nameof(MazeSize), menuName = "Maze/" + nameof(MazeSize), order = 0)]
public class MazeSize : ScriptableObject
{
    [SerializeField] private int[] _currentSize;
    public int[] CurrentSize => _currentSize;
}