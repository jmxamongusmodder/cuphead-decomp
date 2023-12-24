using UnityEngine;

public class HarbourPlatformingLevelFishSpawner : PlatformingLevelEnemySpawner
{
	[SerializeField]
	private HarbourPlatformingLevelFish fishPrefab;
	[SerializeField]
	private string spawnDelayString;
	[SerializeField]
	private string spawnPositionString;
	[SerializeField]
	private string spawnSideString;
	[SerializeField]
	private string typeString;
	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	private float sineSpeed;
	[SerializeField]
	private float sineSize;
}
