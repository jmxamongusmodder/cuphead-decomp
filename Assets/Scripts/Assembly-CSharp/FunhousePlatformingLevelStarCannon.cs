using UnityEngine;

public class FunhousePlatformingLevelStarCannon : PlatformingLevelPathMovementEnemy
{
	[SerializeField]
	private Effect diagFX;
	[SerializeField]
	private Effect straightFX;
	[SerializeField]
	private bool killable;
	[SerializeField]
	private Transform[] diagRootPositions;
	[SerializeField]
	private Transform[] straightRootPositions;
	[SerializeField]
	private FunhousePlatformingLevelCannonProjectile projectile;
}
