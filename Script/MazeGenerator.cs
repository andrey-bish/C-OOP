using System.Collections.Generic;
using UnityEngine;


namespace RollingCube
{
    public class MazeGenerator
    {
        public int MazeWidth = 10;
        public int MazeHeight = 10;


        #region Methods

        public Maze GenerateMaze()
        {
            MazeGeneratorCell[,] cells = new MazeGeneratorCell[MazeWidth, MazeHeight];

            ///sumary
            ///Создание лабиринта
            ///sumary

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = new MazeGeneratorCell { X = x, Y = y };
                }
            }

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                cells[x, MazeHeight - 1].WallLeft = false;
                cells[x, MazeHeight - 1].isFloor = false;
            }

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[MazeWidth - 1, y].WallBottom = false;
                cells[MazeWidth - 1, y].isFloor = false;
            }

            RemoveWallsWithBacktracker(cells);

            Maze maze = new Maze();

            maze.cells = cells;
            maze.FinishPosition = PlaceMazeExit(cells);

            return maze;
        }

        private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze)
        {

            MazeGeneratorCell current = maze[0, 0];
            current.hasVisited = true;
            current.DistanceFromStart = 0;

            ///sumary
            ///Проход по созданному лабиринту и удаление стен
            ///sumary


            Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
            do
            {
                List<MazeGeneratorCell> unclimbedCell = new List<MazeGeneratorCell>();

                int x = current.X;
                int y = current.Y;

                ///sumary
                ///Если ячейка не посещена, то добавляется в массив
                ///sumary

                if (x > 0 && !maze[x - 1, y].hasVisited) unclimbedCell.Add(maze[x - 1, y]);
                if (y > 0 && !maze[x, y - 1].hasVisited) unclimbedCell.Add(maze[x, y - 1]);
                if (x < MazeWidth - 2 && !maze[x + 1, y].hasVisited) unclimbedCell.Add(maze[x + 1, y]);
                if (y < MazeHeight - 2 && !maze[x, y + 1].hasVisited) unclimbedCell.Add(maze[x, y + 1]);

                if (unclimbedCell.Count > 0)
                {
                    ///sumary
                    ///Если есть хоть одна соседняя ячейка, то выбираем случайную, переходим в неё и сносим стену 
                    ///sumary

                    MazeGeneratorCell chosen = unclimbedCell[Random.Range(0, unclimbedCell.Count)];
                    RemoveWall(current, chosen);

                    chosen.hasVisited = true;
                    stack.Push(chosen);
                    chosen.DistanceFromStart = current.DistanceFromStart + 1;
                    current = chosen;
                }
                else
                {
                    ///sumary
                    ///Если соседних ячеек не осталось, то идём обратно
                    ///sumary

                    current = stack.Pop();
                }
            } while (stack.Count > 0);
        }

        private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
        {
            ///sumary
            ///Ячейки могут быть соседними либо по Х, либо по Y. Проверяем их положение и удаляем нужную стену
            ///sumary

            if (a.X == b.X)
            {
                if (a.Y > b.Y)  a.WallBottom = false;
                else b.WallBottom = false;
            }
            else
            {
                if (a.X > b.X) a.WallLeft = false;
                else b.WallLeft = false;
            }
        }

        private Vector2Int PlaceMazeExit(MazeGeneratorCell[,] maze)
        {
            MazeGeneratorCell maxDistance = maze[0, 0];

            ///sumary
            ///Проход по краям лабиринта для нахождения самой дальней ячейки для выхода
            ///sumary

            for (int x = 0; x < maze.GetLength(0); x++)
            {
                if (maze[x, MazeHeight - 2].DistanceFromStart > maxDistance.DistanceFromStart) maxDistance = maze[x, MazeHeight - 2];
                if (maze[x, 0].DistanceFromStart > maxDistance.DistanceFromStart) maxDistance = maze[x, 0];
            }

            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if (maze[MazeWidth - 2, y].DistanceFromStart > maxDistance.DistanceFromStart) maxDistance = maze[MazeWidth - 2, y];
                if (maze[0, y].DistanceFromStart > maxDistance.DistanceFromStart) maxDistance = maze[0, y];
            }

            ///sumary
            ///Убираем стену для выхода
            ///sumary

            if (maxDistance.X == 0) maxDistance.WallLeft = false;
            else if (maxDistance.Y == 0) maxDistance.WallBottom = false;
            else if (maxDistance.X == MazeWidth - 2) maze[maxDistance.X + 1, maxDistance.Y].WallLeft = false;
            else if (maxDistance.Y == MazeHeight - 2) maze[maxDistance.X, maxDistance.Y + 1].WallBottom = false;

            return new Vector2Int(maxDistance.X, maxDistance.Y);
        }

        #endregion
    }
}