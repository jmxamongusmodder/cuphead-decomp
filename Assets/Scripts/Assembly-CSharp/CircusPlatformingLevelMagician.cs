using UnityEngine;

public class CircusPlatformingLevelMagician : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private Transform startPos;
	[SerializeField]
	private Transform endPos;
	[SerializeField]
	private Transform spawnPointHolder;
	[SerializeField]
	private CircusPlatformingLevelMagicianBullet projectile;
}
