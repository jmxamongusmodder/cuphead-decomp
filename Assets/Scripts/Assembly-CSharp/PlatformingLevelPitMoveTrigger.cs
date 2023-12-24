using System;
using UnityEngine;

public class PlatformingLevelPitMoveTrigger : AbstractPausableComponent
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

	[SerializeField]
	private float pitOffset;
	public TriggerProperties trigger;
}
