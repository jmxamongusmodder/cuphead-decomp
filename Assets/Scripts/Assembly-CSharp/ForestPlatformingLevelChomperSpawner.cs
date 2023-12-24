using System;
using UnityEngine;

public class ForestPlatformingLevelChomperSpawner : AbstractPausableComponent
{
	[Serializable]
	public class TriggerProperties
	{
		public TriggerProperties(Vector2 position)
		{
		}

		public Vector2 Position;
		public Vector2 Size;
		public float xVariation;
	}

	public TriggerProperties startTrigger;
	public ForestPlatformingLevelChomper[] chompers;
}
