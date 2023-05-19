using UnityEngine;
using Zenject;

public class MazeSpawnerPersenter : MonoBehaviour
{
    [Inject] MazeCellView CellPrefab;
    [Inject] HintRenderer HintRenderer;
    [Inject] IMazeGenerator generator;
    [Inject] MazeCellSize _cellSize;
    [Inject] MazeSize _mazeSize;

    [SerializeField] private Transform _mazeTransformParent;

    public Maze maze;
  
    private void Awake()
    {
        generator = new MazeGenerator(_mazeSize);
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                MazeCellView c = Instantiate(CellPrefab, new Vector3(x * _cellSize.CurrentSize.x, y * _cellSize.CurrentSize.y, y * _cellSize.CurrentSize.z), Quaternion.identity,_mazeTransformParent);

                c.WallLeft.gameObject.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.gameObject.SetActive(maze.cells[x, y].WallBottom);
            }
        }
      //  HintRenderer.DrawPath();
    }
}
