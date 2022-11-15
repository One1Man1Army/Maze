using Maze.Ball;
using Maze.Services.MazeGeneration;
using Maze.Structure.AssetManagement;
using UnityEngine;

namespace Maze.Structure.Factory
{
	public sealed class GameFactory : IGameFactory
	{
		private readonly IMazeGenerator _mazeGenerator;
		private readonly IAssetProvider _assetProvider;

		private MazeGeneratorCell[,] _maze;

		public GameFactory(IMazeGenerator mazeGenerator, IAssetProvider assetProvider)
		{
			_mazeGenerator = mazeGenerator;
			_assetProvider = assetProvider;
		}

		public GameObject InitBackground() => 
			_assetProvider.Instantiate(AssetPath.backgroundPath);

		public MazeGeneratorCell[,] InitMaze()
		{
			var maze = _mazeGenerator.GenerateMaze();
			
			var mazeFolder = new GameObject("Maze");
			for (int x = 0; x < maze.GetLength(0); x++)
			{
				for (int y = 0; y < maze.GetLength(1); y++)
				{
					var cell = _assetProvider.Instantiate(AssetPath.cellPath, new Vector3(x * 2, 0, y * 2)).GetComponent<Cell>();
					cell.transform.SetParent(mazeFolder.transform);
					
					cell.upWall.SetActive(maze[x,y].isUpWallActive);
					cell.rightWall.SetActive(maze[x,y].isRightWallActive);
					cell.downWall.SetActive(maze[x,y].isDownWallActive);
					cell.leftWall.SetActive(maze[x,y].isLeftWallActive);
				}
			}

			_maze = maze;
			return maze;
		}

		public BallMover InitBall()
		{
			var ball = _assetProvider.Instantiate(AssetPath.ballPath).GetComponent<BallMover>();
			ball.maze = _maze;
			ball._currentCell = _maze[0, 0];
			ball._targetCell = _maze[_maze.GetLength(0)-1, _maze.GetLength(1)-1];
			return ball;
		}
	}
}