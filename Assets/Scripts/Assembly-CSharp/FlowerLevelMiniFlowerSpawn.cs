using UnityEngine;

public class FlowerLevelMiniFlowerSpawn : AbstractCollidableObject
{
	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private Transform explosion;
	[SerializeField]
	private GameObject petalA;
	[SerializeField]
	private GameObject petalB;
}
