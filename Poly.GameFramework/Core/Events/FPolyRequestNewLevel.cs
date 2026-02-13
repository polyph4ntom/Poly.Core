using Poly.Common;
using Poly.Events;
using UnityEngine.SceneManagement;

namespace Poly.GameFramework.Core
{
	public class FPolyRequestNewLevel : IPolyEvent
	{
		public APolyWorldBase World { get; private set; }
		public FPolySceneReference RequestedScene { get; private set; }
		public string LevelName { get; private set; }
		public string LevelDescription { get; private set; }

		public FPolyRequestNewLevel(APolyWorldBase world, FPolySceneReference requestedScene, string levelName, string levelDescription)
		{
			World = world;
			RequestedScene = requestedScene;
			LevelName = levelName;
			LevelDescription = levelDescription;
		}
	}
}
