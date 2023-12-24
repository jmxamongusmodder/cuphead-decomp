using UnityEngine;

public class CircusPlatformingLevelArcade : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private Transform bulletSpawnA;
	[SerializeField]
	private Transform bulletSpawnB;
	[SerializeField]
	private Effect effect;
	[SerializeField]
	private Transform arcadeRoot;
	[SerializeField]
	private Transform introBullet;
	[SerializeField]
	private BasicProjectile bullet;
	[SerializeField]
	private LevelBossDeathExploder exploder;
}
