using UnityEngine;


namespace RollingCube
{
    public class MazeSpawner : MonoBehaviour
    {
        public Maze maze;
        public HintRenderer HintRenderer;

        public Vector3 CellSize = new Vector3(1, 1, 0);        

        [SerializeField] private Cell _cellPrefab;      

        private void Start()
        {
            MazeGenerator generator = new MazeGenerator();
            maze = generator.GenerateMaze();

            for (int x = 0; x < maze.cells.GetLength(0); x++)
            {
                for (int y = 0; y < maze.cells.GetLength(1); y++)
                {
                    Cell c = Instantiate(_cellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);

                    ///sumary
                    ///Убираем стены и пол по краю
                    ///sumary

                    c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                    c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                    c.Floor.SetActive(maze.cells[x, y].isFloor);
                }
            }

            HintRenderer.DrawPath();
        }
    }
}