using UnityEngine;

public class CircusPlatformingLevelTrampoline : AbstractCollidableObject
{
	[SerializeField]
	private float bounds;
	[SerializeField]
	private float AwakeningZone;
	public float maxSpeed;
	public float acceleration;
	public float knockUpHeight;
}
