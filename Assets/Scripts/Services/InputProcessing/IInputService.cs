using System;

namespace Maze.Services.InputProcessing
{
	public interface IInputService : IService
	{
		event Action OnSwipeUp;
		event Action OnSwipeRight;
		event Action OnSwipeDown;
		event Action OnSwipeLeft;
	}
}