using Maze.Ball;
using Maze.Services;
using Maze.Services.MazeGeneration;
using UnityEngine;

namespace Maze.Structure.Factory
{
	public interface IGameFactory : IService
	{
		GameObject InitBackground();
		MazeGeneratorCell[,] InitMaze();
		BallMover InitBall();
	}
}