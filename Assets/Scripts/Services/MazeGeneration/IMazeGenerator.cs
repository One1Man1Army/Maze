using Maze.Services;

namespace Maze.Services.MazeGeneration
{
	public interface IMazeGenerator : IService
	{
		MazeGeneratorCell[,] GenerateMaze();
	}
}