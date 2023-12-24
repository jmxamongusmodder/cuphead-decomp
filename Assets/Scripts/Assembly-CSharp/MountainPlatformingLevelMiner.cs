using UnityEngine;

public class MountainPlatformingLevelMiner : PlatformingLevelGroundMovementEnemy
{
	[SerializeField]
	private MountainPlatformingLevelPickaxeProjectile pickaxe;
	[SerializeField]
	private SpriteRenderer straight;
	[SerializeField]
	private SpriteRenderer up;
	[SerializeField]
	private SpriteRenderer down;
	[SerializeField]
	private Transform lookAt;
	[SerializeField]
	private MountainPlatformingLevelMinerRope rope;
	[SerializeField]
	private Transform root;
	[SerializeField]
	private Transform catchRoot;
}
