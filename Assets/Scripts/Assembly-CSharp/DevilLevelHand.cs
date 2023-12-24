using UnityEngine;

public class DevilLevelHand : AbstractCollidableObject
{
	public enum State
	{
		Uninitialized = 0,
		Idle = 1,
	}

	public State state;
	public bool despawned;
	public bool isDead;
	[SerializeField]
	private bool onLeft;
	[SerializeField]
	private float shootAngle;
	[SerializeField]
	private Transform bulletRoot;
	[SerializeField]
	private BasicProjectile bulletPrefab;
	[SerializeField]
	private BasicProjectile bulletPinkPrefab;
	[SerializeField]
	private SpriteRenderer demonSprite;
	[SerializeField]
	private SpriteRenderer handSprite;
}
