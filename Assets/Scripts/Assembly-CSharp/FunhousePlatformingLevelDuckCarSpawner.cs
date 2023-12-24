using UnityEngine;

public class FunhousePlatformingLevelDuckCarSpawner : PlatformingLevelEnemySpawner
{
	[SerializeField]
	private Effect honkEffect;
	[SerializeField]
	private Transform topSpawnRoot;
	[SerializeField]
	private Transform bottomSpawnRoot;
	[SerializeField]
	private FunhousePlatformingLevelCar carPrefabNormal;
	[SerializeField]
	private float carSpeed;
	[SerializeField]
	private float carDelay;
	[SerializeField]
	private float carSpacing;
	[SerializeField]
	private int carCount;
	[SerializeField]
	private FunhousePlatformingLevelDuck bigDuckPrefab;
	[SerializeField]
	private FunhousePlatformingLevelDuck smallDuckPrefab;
	[SerializeField]
	private FunhousePlatformingLevelDuck smallDuckPinkPrefab;
	[SerializeField]
	private float duckDelay;
	[SerializeField]
	private float duckCount;
	[SerializeField]
	private float duckSpacing;
	[SerializeField]
	private string duckPinkString;
}
