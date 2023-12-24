using UnityEngine;

public class ClownLevelClownSwing : LevelProperties.Clown.Entity
{
	[SerializeField]
	private ClownLevelCoasterHandler coasterHandler;
	[SerializeField]
	private GameObject umbrella;
	[SerializeField]
	private GameObject topper;
	[SerializeField]
	private ClownLevelEnemy enemy;
	[SerializeField]
	private ClownLevelSwings swingFrontPrefab;
	[SerializeField]
	private ClownLevelSwings swingBackPrefab;
	[SerializeField]
	private BasicProjectile clownBullet;
	[SerializeField]
	private Transform swingStopPosition;
}
