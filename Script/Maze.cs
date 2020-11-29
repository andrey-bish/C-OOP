using UnityEngine;


namespace RollingCube
{
    public class Maze
    {
        public MazeGeneratorCell[,] cells;
        public Vector2Int FinishPosition;
    }

    public class MazeGeneratorCell
    {
        public int X;
        public int Y;

        public bool WallLeft = true;
        public bool WallBottom = true;
        public bool isFloor = true;

        public bool hasVisited = false;
        public int DistanceFromStart;
    }
}
