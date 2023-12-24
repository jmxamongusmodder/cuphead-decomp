using System;
using UnityEngine;

public class PlanePlayerColliderManager : AbstractPlanePlayerComponent
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
		public PlanePlayerColliderManager.ColliderProperties default;
		public PlanePlayerColliderManager.ColliderProperties shrunk;
	}

	[SerializeField]
	private ColliderPropertiesGroup colliders;
}
