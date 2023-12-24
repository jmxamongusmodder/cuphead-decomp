using UnityEngine;

public class FlowerLevelFlower : LevelProperties.Flower.Entity
{
	public GameObject attackPoint;
	[SerializeField]
	private GameObject vineHandPrefab;
	[SerializeField]
	private GameObject boomerangPrefab;
	[SerializeField]
	private GameObject bulletSeedPrefab;
	[SerializeField]
	private GameObject cloudBombPrefab;
	[SerializeField]
	private GameObject enemySeedPrefab;
	[SerializeField]
	private GameObject pollenProjectile;
	[SerializeField]
	private Transform topProjectileSpawnPoint;
	[SerializeField]
	private Transform bottomProjectileSpawnPoint;
	[SerializeField]
	private GameObject mainVine;
	[SerializeField]
	private GameObject gattlingFX;
}
