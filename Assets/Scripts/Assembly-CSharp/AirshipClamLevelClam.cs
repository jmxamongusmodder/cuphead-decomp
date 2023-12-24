using UnityEngine;

public class AirshipClamLevelClam : LevelProperties.AirshipClam.Entity
{
	[SerializeField]
	private BasicProjectile pearlPrefab;
	[SerializeField]
	private AirshipClamLevelBarnacle barnaclePrefab;
	[SerializeField]
	private AirshipClamLevelBarnacleParryable barnacleParryablePrefab;
	[SerializeField]
	private Transform[] spawnPoints;
}
