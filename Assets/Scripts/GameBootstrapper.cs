using System;
using Maze.Services;
using Maze.Services.InputProcessing;
using Maze.Services.MazeGeneration;
using Maze.Structure.AssetManagement;
using Maze.Structure.Factory;
using UnityEngine;

public sealed class GameBootstrapper : MonoBehaviour
{
	private IGameFactory _gameFactory;
	private void Awake()
	{
		RegisterServices();
		_gameFactory = AllServices.Container.Single<IGameFactory>();

		_gameFactory.InitBackground();
		_gameFactory.InitMaze();
		_gameFactory.InitBall().InjectInputService(AllServices.Container.Single<IInputService>());
	}

	private void RegisterServices()
	{
		AllServices.Container.RegisterSingle<IAssetProvider>(new AssetProvider());
		AllServices.Container.RegisterSingle<IMazeGenerator>(new MazeGenerator());
		AllServices.Container.RegisterSingle<IGameFactory>(new GameFactory( AllServices.Container.Single<IMazeGenerator>(), AllServices.Container.Single<IAssetProvider>()));
		RegisterInputService();
	}

	private void RegisterInputService()
	{
		var inputProcessor = new GameObject("InputProcessor");
		#if UNITY_STANDALONE || UNITY_EDITOR
			inputProcessor.AddComponent<StandaloneInput>();
		#endif
		
		#if UNITY_IOS || UNITY_ANDROID
			inputProcessor.AddComponent<MobileInput>();
		#endif
		AllServices.Container.RegisterSingle<IInputService>(inputProcessor.GetComponent<IInputService>());
	}
}
