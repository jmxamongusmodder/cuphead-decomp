using UnityEngine;

public class HarbourPlatformingLevelLobster : PlatformingLevelShootingEnemy
{
	[SerializeField]
	private Transform main;
	[SerializeField]
	private Transform onTrigger;
	[SerializeField]
	private Transform offTrigger;
	[SerializeField]
	private Transform leftBoundary;
	[SerializeField]
	private Transform rightBoundary;
	[SerializeField]
	private LevelBossDeathExploder exploder;
	[SerializeField]
	private GameObject splashPrefab;
	[SerializeField]
	private Transform splashTransform;
}
