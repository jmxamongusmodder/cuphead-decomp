using UnityEngine;

public class MountainPlatformingLevelWall : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private Transform groundPosY;
	[SerializeField]
	private Transform platform;
	[SerializeField]
	private SpriteRenderer foreground1;
	[SerializeField]
	private SpriteRenderer foreground2;
	[SerializeField]
	private SpriteRenderer shield;
	[SerializeField]
	private Transform head;
	[SerializeField]
	private Transform startTrigger;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private Effect projectileEffect;
	[SerializeField]
	private Effect projectilePinkEffect;
	[SerializeField]
	private MountainPlatformingLevelWallProjectile bouncyProjectile;
	[SerializeField]
	private MountainPlatformingLevelWallProjectile bouncyPinkProjectile;
}
