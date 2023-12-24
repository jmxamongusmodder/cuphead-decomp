using UnityEngine;

public class TreePlatformingLevelLog : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private TreePlatformingLevelLogProjectile projectile;
	[SerializeField]
	private Transform root;
	[SerializeField]
	private float shootDelay;
	[SerializeField]
	private SpriteDeathParts[] parts;
	[SerializeField]
	private bool canShoot;
	[SerializeField]
	private string pinkString;
	[SerializeField]
	private Effect projectilePuff;
	public bool isDying;
	public bool isSliding;
	public float start;
}
