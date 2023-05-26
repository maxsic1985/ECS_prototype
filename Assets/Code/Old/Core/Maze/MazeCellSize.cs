using UnityEngine;

[CreateAssetMenu(fileName = nameof(MazeCellSize), menuName = "Maze/" + nameof(MazeCellSize), order = 0)]
public class MazeCellSize : ScriptableObject
{
    [SerializeField] private Vector3 _currentSize;
    public Vector3 CurrentSize => _currentSize;
}
