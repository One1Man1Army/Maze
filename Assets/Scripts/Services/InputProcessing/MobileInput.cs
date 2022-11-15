using System;
using UnityEngine;

namespace Maze.Services.InputProcessing
{
	public sealed class MobileInput : MonoBehaviour, IInputService
	{
		private Vector2 fingerDown;
		private Vector2 fingerUp;
		
		public bool detectSwipeOnlyAfterRelease = false;

		public float SWIPE_THRESHOLD = 20f;

		public bool isInputEnabled = true;
		
		public event Action OnSwipeUp;
		public event Action OnSwipeRight;
		public event Action OnSwipeDown;
		public event Action OnSwipeLeft;
		
		private void Update()
		{
			if (!isInputEnabled)
				return;

			foreach (Touch touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					fingerUp = touch.position;
					fingerDown = touch.position;
				}
				
				if (touch.phase == TouchPhase.Moved)
				{
					if (!detectSwipeOnlyAfterRelease)
					{
						fingerDown = touch.position;
						CheckSwipe();
					}
				}
				
				if (touch.phase == TouchPhase.Ended)
				{
					fingerDown = touch.position;
					CheckSwipe();
				}
			}
		}

		private void CheckSwipe()
		{
			if (VerticalMove() > SWIPE_THRESHOLD && VerticalMove() > HorizontalValMove())
			{
				if (fingerDown.y - fingerUp.y > 0) 
				{
					OnSwipeUp?.Invoke();
				}
				else if (fingerDown.y - fingerUp.y < 0)
				{
					OnSwipeDown?.Invoke();
				}

				fingerUp = fingerDown;
			}
			
			else if (HorizontalValMove() > SWIPE_THRESHOLD && HorizontalValMove() > VerticalMove())
			{
				if (fingerDown.x - fingerUp.x > 0)
				{
					OnSwipeRight?.Invoke();
				}
				else if (fingerDown.x - fingerUp.x < 0)
				{
					OnSwipeLeft?.Invoke();
				}

				fingerUp = fingerDown;
			}
		}

		private float VerticalMove()
		{
			return Mathf.Abs(fingerDown.y - fingerUp.y);
		}

		private float HorizontalValMove()
		{
			return Mathf.Abs(fingerDown.x - fingerUp.x);
		}
	}
}