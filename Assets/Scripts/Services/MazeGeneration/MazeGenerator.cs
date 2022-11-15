using System.Collections.Generic;
using UnityEngine;

namespace Maze.Services.MazeGeneration
{
	public sealed class MazeGenerator : IMazeGenerator
	{
		public int width = 6;
		public int height = 5;

		public MazeGeneratorCell[,] GenerateMaze()
		{
			MazeGeneratorCell[,] maze = new MazeGeneratorCell[width, height];
			for (int i = 0; i < 100; i++)
			{

				for (int x = 0; x < maze.GetLength(0); x++)
				{
					for (int y = 0; y < maze.GetLength(1); y++)
					{
						maze[x, y] = new MazeGeneratorCell(x, y);
					}
				}

				/* Алгоритм для более простых лабиринтов
				 *
				 * RemoveWallsWithBinaryTree(maze);
				 * 
				 */

				RemoveWallsWithBackTracker(maze);
				CalcCellsNeighbours(maze);
			}

			return maze;
		}

		private void RemoveWallsWithBackTracker(MazeGeneratorCell[,] maze)
		{
			var currentCell = maze[0, 0];
			currentCell.isVisited = true;

			var stack = new Stack<MazeGeneratorCell>();

			do
			{
				var x = currentCell.x;
				var y = currentCell.y;

				List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

				if (x - 1 >= 0 && !maze[x - 1, y].isVisited) unvisitedNeighbours.Add(maze[x - 1, y]);
				if (x + 1 < maze.GetLength(0) && !maze[x + 1, y].isVisited) unvisitedNeighbours.Add(maze[x + 1, y]);
				if (y - 1 >= 0 && !maze[x, y - 1].isVisited) unvisitedNeighbours.Add(maze[x, y - 1]);
				if (y + 1 < maze.GetLength(1) && !maze[x, y + 1].isVisited) unvisitedNeighbours.Add(maze[x, y + 1]);

				if (unvisitedNeighbours.Count > 0)
				{
					var chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
					RemoveWall(currentCell, chosen);
					chosen.isVisited = true;
					currentCell = chosen;
					stack.Push(chosen);
				}
				else
				{
					currentCell = stack.Pop();
				}
			} while (stack.Count > 0);
		}

		private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
		{
			if (a.x == b.x)
			{
				if (a.y > b.y)
				{
					a.isDownWallActive = false;
					b.isUpWallActive = false;
				}
				else
				{
					a.isUpWallActive = false;
					b.isDownWallActive = false;
				}
			}
			else
			{
				if (a.x > b.x)
				{
					a.isLeftWallActive = false;
					b.isRightWallActive = false;
				}
				else
				{
					a.isRightWallActive = false;
					b.isLeftWallActive = false;
				}
			}
		}

		private void RemoveWallsWithBinaryTree(MazeGeneratorCell[,] maze)
		{
			foreach (var cell in maze)
			{
				if (cell.x == 0)
					cell.isLeftWallActive = true;
				if (cell.y == 0)
					cell.isDownWallActive = true;

				if (cell.x == width - 1 && cell.y == height - 1)
					continue;

				if (cell.y == height - 1)
				{
					cell.isRightWallActive = false;
					continue;
				}

				if (cell.x == width - 1)
				{
					cell.isUpWallActive = false;
					continue;
				}

				var random = Random.Range(0, 1f);
				if (random < 0.5f)
				{
					cell.isUpWallActive = false;
				}
				else
				{
					cell.isRightWallActive = false;
				}
			}
		}

		private void CalcCellsNeighbours(MazeGeneratorCell[,] maze)
		{
			for (int x = 0; x < maze.GetLength(0); x++)
			{
				for (int y = 0; y < maze.GetLength(1); y++)
				{
					maze[x, y].GetNeighbours(maze);
				}
			}
		}
	}
}
