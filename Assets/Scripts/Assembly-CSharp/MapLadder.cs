using System;
using UnityEngine;

public class MapLadder : AbstractMonoBehaviour
{
	[Serializable]
	public class PointProperties
	{
		public Vector2 interactionPoint;
		public float interactionDistance;
		public Vector2 dialogueOffset;
		public Vector2 exit;
	}

	public float height;
	[SerializeField]
	private PointProperties top;
	[SerializeField]
	private PointProperties bottom;
}
