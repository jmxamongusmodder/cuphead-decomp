using UnityEngine;

public class ForestPlatformingLevelAcornMaker : PlatformingLevelShootingEnemy
{
	[SerializeField]
	private Effect explosion;
	[SerializeField]
	private Transform gruntRoot;
	[SerializeField]
	private SpriteRenderer gruntSprite;
	[SerializeField]
	private ForestPlatformingLevelAcorn acornPrefab;
	[SerializeField]
	private Transform spawnRoot;
}
