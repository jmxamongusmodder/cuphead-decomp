using UnityEngine;

public class FlyingBirdLevelSmallBird : LevelProperties.FlyingBird.Entity
{
	[SerializeField]
	private FlyingBirdLevelSmallBirdSprite sprite;
	[SerializeField]
	private FlyingBirdLevelSmallBirdEgg eggPrefab;
	[SerializeField]
	private BasicProjectile bulletPrefab;
	[SerializeField]
	private Transform bulletRoot;
}
