using UnityEngine;

public class MountainPlatformingLevelDragonSpawner : AbstractPausableComponent
{
	[SerializeField]
	private bool isElevator;
	[SerializeField]
	private Transform[] spawnPoints;
	[SerializeField]
	private MountainPlatformingLevelDragon dragonMiddlePrefab;
	[SerializeField]
	private MountainPlatformingLevelDragon dragonSidePrefab;
	[SerializeField]
	private string spawnString;
	[SerializeField]
	private float spawnDelay;
}
