using System;
using UnityEngine;

public class ArcadePlayerColliderManager : AbstractArcadePlayerComponent
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
		public ArcadePlayerColliderManager.ColliderProperties default;
		public ArcadePlayerColliderManager.ColliderProperties air;
		public ArcadePlayerColliderManager.ColliderProperties dashing;
		public ArcadePlayerColliderManager.ColliderProperties rocket;
	}

	[SerializeField]
	private ColliderPropertiesGroup colliders;
}
