using System;
using UnityEngine;

public class PlatformingLevelEnemySpawner : AbstractPausableComponent
{
	[Serializable]
	public class TriggerProperties
	{
		public TriggerProperties(Vector2 position)
		{
		}

		public Vector2 Position;
		public Vector2 Size;
	}

	public bool destroyEnemyAfterLeavingScreen;
	public MinMax spawnDelay;
	public MinMax initalSpawnDelay;
	public TriggerProperties startTrigger;
	public TriggerProperties stopTrigger;
}
