using UnityEngine;

public class CircusPlatformingLevelDunkPlatform : AbstractCollidableObject
{
	[SerializeField]
	private Collider2D platform;
	[SerializeField]
	private float platformDown;
	[SerializeField]
	private float targetSpin;
}
