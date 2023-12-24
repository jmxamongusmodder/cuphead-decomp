using UnityEngine;

public class DevilLevelDemon : AbstractCollidableObject
{
	[SerializeField]
	private Collider2D collider2d;
	[SerializeField]
	private float frontWaitTime;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private Color backgroundTint;
	[SerializeField]
	private PlatformingLevelGenericExplosion explosion;
}
