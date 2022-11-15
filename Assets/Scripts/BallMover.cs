using DG.Tweening;
using Maze.Services.InputProcessing;
using Maze.Services.MazeGeneration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maze.Ball
{
	public sealed class BallMover : MonoBehaviour
	{
		private IInputService _inputService;
		
		public MazeGeneratorCell[,] maze;
		public MazeGeneratorCell _currentCell;
		public MazeGeneratorCell _targetCell;
		private MazeGeneratorCell _nextCell;
		private SwipeDirection _swipeDirection;
		private Ease _ease;
		private float _time;
		private bool _isRotating;
		private float _rotationAnglePerFrame = 0.01f;
		private bool _isMoving;

		public void InjectInputService(IInputService inputService)
		{
			_inputService = inputService;
			
			_inputService.OnSwipeUp += OnSwipeUp;
			_inputService.OnSwipeRight += OnSwipeRight;
			_inputService.OnSwipeDown += OnSwipeDown;
			_inputService.OnSwipeLeft += OnSwipeLeft;
		}
		
		private void Update()
		{
			if (_isRotating)
			{
				transform.RotateAround(transform.forward, _rotationAnglePerFrame);
			}
		}

		#region Swipes Processing

		private void OnSwipeUp()
		{
			if (_isMoving) return;
			_swipeDirection = SwipeDirection.Up;
			MoveBall();
		}

		private void OnSwipeRight()
		{
			if (_isMoving) return;
			_swipeDirection = SwipeDirection.Right;
			MoveBall();
		}

		private void OnSwipeDown()
		{
			if (_isMoving) return;
			_swipeDirection = SwipeDirection.Down;
			MoveBall();
		}

		private void OnSwipeLeft()
		{
			if (_isMoving) return;
			_swipeDirection = SwipeDirection.Left;
			MoveBall();
		}

		#endregion

		private void MoveBall()
		{
			if (!CanMove()) return;
			_isMoving = true;
			CalcNextCell();
			var nextCellPos = new Vector3(_nextCell.x * 2, 0, _nextCell.y * 2);
			CalcEase(nextCellPos);
			CalcTime(nextCellPos);
			_isRotating = true;
			transform.DOMove(nextCellPos, _time).SetEase(_ease).OnComplete(() =>
			{
				_currentCell = _nextCell;
				_isRotating = false;
				_isMoving = false;
				if (_currentCell == _targetCell)
					SceneManager.LoadScene(0);
			});

		}

		#region Movement Calculations
		
		private void CalcEase(Vector3 nextPos)
		{
			if (IsWallBounce() && Vector3.Distance(transform.position, nextPos) > 4f)
			{
				_ease = Ease.OutBounce;
			}
			else
			{
				_ease = Ease.InSine;
			}
		}

		private void CalcTime(Vector3 nextCellPos)
		{
			_time = Vector3.Distance(transform.position, nextCellPos) / 6f;
		}

		private bool IsWallBounce()
		{
			switch (_swipeDirection)
			{
				case SwipeDirection.Up:
					if (_nextCell.isUpWallActive) return true;
					break;
				case SwipeDirection.Right:
					if (_nextCell.isRightWallActive) return true;
					break;
				case SwipeDirection.Down:
					if (_nextCell.isDownWallActive) return true;
					break;
				case SwipeDirection.Left:
					if (_nextCell.isLeftWallActive) return true;
					break;
			}

			return false;
		}

		private bool CanMove()
		{
			switch (_swipeDirection)
			{
				case SwipeDirection.Up:
					if (_currentCell.isUpWallActive) return false;
					break;
				case SwipeDirection.Right:
					if (_currentCell.isRightWallActive) return false;
					break;
				case SwipeDirection.Down:
					if (_currentCell.isDownWallActive) return false;
					break;
				case SwipeDirection.Left:
					if (_currentCell.isLeftWallActive) return false;
					break;
			}

			return true;
		}

		private void CalcNextCell()
		{
			if (_swipeDirection == SwipeDirection.Up)
			{
				for (int i = _currentCell.y + 1; i < maze.GetLength(1); i++)
				{
					if (_currentCell.x - 1 >= 0)
						if (maze[_currentCell.x, i].neighbours.Contains(maze[_currentCell.x - 1, i]))
						{
							_nextCell = maze[_currentCell.x, i];
							return;
						}

					if (_currentCell.x + 1 < maze.GetLength(0))
						if (maze[_currentCell.x, i].neighbours.Contains(maze[_currentCell.x + 1, i]))
						{
							_nextCell = maze[_currentCell.x, i];
							return;
						}

					if (maze[_currentCell.x, i].isUpWallActive)
						break;
				}

				for (int i = _currentCell.y + 1; i < maze.GetLength(1); i++)
				{
					if (maze[_currentCell.x, i].isUpWallActive)
					{
						_nextCell = maze[_currentCell.x, i];
						return;
					}
				}
			}

			if (_swipeDirection == SwipeDirection.Right)
			{
				for (int i = _currentCell.x + 1; i < maze.GetLength(0); i++)
				{
					if (_currentCell.y + 1 < maze.GetLength(1))
						if (maze[i, _currentCell.y].neighbours.Contains(maze[i, _currentCell.y + 1]))
						{
							_nextCell = maze[i, _currentCell.y];
							return;
						}

					if (_currentCell.y - 1 >= 0)
						if (maze[i, _currentCell.y].neighbours.Contains(maze[i, _currentCell.y - 1]))
						{
							_nextCell = maze[i, _currentCell.y];
							return;
						}

					if (maze[i, _currentCell.y].isRightWallActive)
						break;
				}

				for (int i = _currentCell.x + 1; i < maze.GetLength(0); i++)
				{
					if (maze[i, _currentCell.y].isRightWallActive)
					{
						_nextCell = maze[i, _currentCell.y];
						return;
					}
				}
			}

			if (_swipeDirection == SwipeDirection.Down)
			{
				for (int i = _currentCell.y - 1; i >= 0; i--)
				{
					if (_currentCell.x - 1 >= 0)
						if (maze[_currentCell.x, i].neighbours.Contains(maze[_currentCell.x - 1, i]))
						{
							_nextCell = maze[_currentCell.x, i];
							return;
						}

					if (_currentCell.x + 1 < maze.GetLength(0))
						if (maze[_currentCell.x, i].neighbours.Contains(maze[_currentCell.x + 1, i]))
						{
							_nextCell = maze[_currentCell.x, i];
							return;
						}

					if (maze[_currentCell.x, i].isDownWallActive)
						break;
				}

				for (int i = _currentCell.y - 1; i >= 0; i--)
				{
					if (maze[_currentCell.x, i].isDownWallActive)
					{
						_nextCell = maze[_currentCell.x, i];
						return;
					}
				}
			}

			if (_swipeDirection == SwipeDirection.Left)
			{
				for (int i = _currentCell.x - 1; i >= 0; i--)
				{
					if (_currentCell.y + 1 < maze.GetLength(1))
						if (maze[i, _currentCell.y].neighbours.Contains(maze[i, _currentCell.y + 1]))
						{
							_nextCell = maze[i, _currentCell.y];
							return;
						}

					if (_currentCell.y - 1 >= 0)
						if (maze[i, _currentCell.y].neighbours.Contains(maze[i, _currentCell.y - 1]))
						{
							_nextCell = maze[i, _currentCell.y];
							return;
						}

					if (maze[i, _currentCell.y].isLeftWallActive)
						break;
				}

				for (int i = _currentCell.x - 1; i >= 0; i--)
				{
					if (maze[i, _currentCell.y].isLeftWallActive)
					{
						_nextCell = maze[i, _currentCell.y];
						return;
					}
				}
			}
		}
		#endregion
	}
}
