using Maze.Services;
using UnityEngine;

namespace Maze.Structure.AssetManagement
{
	public interface IAssetProvider : IService
	{
		GameObject Instantiate(string path, Vector3 at);
		GameObject Instantiate(string path);
	}
}