using UnityEngine;

public class MountainPlatformingLevelMudman : PlatformingLevelGroundMovementEnemy
{
	[SerializeField]
	private PlatformingLevelGenericExplosion splash;
	[SerializeField]
	private Transform[] explodeSpawns;
	[SerializeField]
	private PlatformingLevelGenericExplosion otherExplosion;
	[SerializeField]
	private bool isBig;
}
