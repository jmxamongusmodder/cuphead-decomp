using System;
using UnityEngine;

public class LevelPlayerColliderManager : AbstractLevelPlayerComponent
{
	[Serializable]
	public class ColliderProperties
	{
		public ColliderProperties(Vector2 center, Vector2 size)
		{
		}

		public Vector2 center;
		public Vector2 size;
	}

	[Serializable]
	public class ColliderPropertiesGroup
	{
		public LevelPlayerColliderManager.ColliderProperties default;
		public LevelPlayerColliderManager.ColliderProperties air;
		public LevelPlayerColliderManager.ColliderProperties ducking;
		public LevelPlayerColliderManager.ColliderProperties dashing;
		public LevelPlayerColliderManager.ColliderProperties chaliceFirstJump;
	}

	[SerializeField]
	private ColliderPropertiesGroup colliders;
}
