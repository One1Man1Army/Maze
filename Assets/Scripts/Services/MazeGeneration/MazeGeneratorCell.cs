using System.Collections.Generic;

namespace Maze.Services.MazeGeneration
{
	public sealed class MazeGeneratorCell
	{
		public int x;
		public int y;

		public bool isUpWallActive = true;
		public bool isRightWallActive = true;
		public bool isDownWallActive = true;
		public bool isLeftWallActive = true;

		public bool isVisited;

		public int gCost;
		public int hCost;
		public int fCost;
		
		public MazeGeneratorCell cameFromCell;
		public List<MazeGeneratorCell> neighbours;

		public MazeGeneratorCell(int xIndex, int yIndex)
		{
			x = xIndex;
			y = yIndex;
		}

		public void GetNeighbours(MazeGeneratorCell[,] maze)
		{
			 neighbours = new List<MazeGeneratorCell>();
			
			if (y + 1 < maze.GetLength(1) && !isUpWallActive)
				neighbours.Add(maze[x, y+1]);
			
			if (x + 1 < maze.GetLength(0) && !isRightWallActive)
				neighbours.Add(maze[x+1, y]);
			
			if (y - 1 >= 0 && !isDownWallActive)
				neighbours.Add(maze[x, y-1]);
			
			if (x - 1 >= 0 && !isLeftWallActive)
				neighbours.Add(maze[x-1, y]);
		}
		
		public void CalculateFCost() => 
			fCost = gCost + hCost;
	}
}