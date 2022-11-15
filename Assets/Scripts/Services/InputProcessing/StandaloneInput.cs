using System;
using UnityEngine;

namespace Maze.Services.InputProcessing
{
	public sealed class StandaloneInput : MonoBehaviour, IInputService
	{
		public event Action OnSwipeUp;
		public event Action OnSwipeRight;
		public event Action OnSwipeDown;
		public event Action OnSwipeLeft;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				OnSwipeLeft?.Invoke();
			}

			if (Input.GetKeyDown(KeyCode.D))
			{
				OnSwipeRight?.Invoke();
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				OnSwipeDown?.Invoke();
			}

			if (Input.GetKeyDown(KeyCode.W))
			{
				OnSwipeUp?.Invoke();
			}
		}
	}
}