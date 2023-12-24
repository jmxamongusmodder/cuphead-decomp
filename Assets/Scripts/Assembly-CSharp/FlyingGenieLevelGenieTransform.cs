using UnityEngine;

public class FlyingGenieLevelGenieTransform : LevelProperties.FlyingGenie.Entity
{
	[SerializeField]
	private Effect deathPuffEffect;
	[SerializeField]
	private SpriteRenderer bottomLayer;
	[SerializeField]
	private FlyingGenieLevelSpawner spawnerPrefab;
	[SerializeField]
	private Transform marionetteShootRoot;
	[SerializeField]
	private BasicProjectile shotBullet;
	[SerializeField]
	private BasicProjectile pinkBullet;
	[SerializeField]
	private BasicProjectile shootBullet;
	[SerializeField]
	private BasicProjectile spreadProjectile;
	[SerializeField]
	private FlyingGenieLevelRing ringPrefab;
	[SerializeField]
	private FlyingGenieLevelRing pinkRingPrefab;
	[SerializeField]
	private FlyingGenieLevelPyramid pyramidPrefab;
	[SerializeField]
	private FlyingGenieLevelTinyMarionette tinyMarionette;
	[SerializeField]
	private Transform pyramidPivotPoint;
	[SerializeField]
	private Transform gemStone;
	[SerializeField]
	private Transform pipe;
	[SerializeField]
	private Transform giantRoot;
	[SerializeField]
	private Transform handFront;
	[SerializeField]
	private Transform handBack;
	[SerializeField]
	private Transform deathPuffRoot;
	[SerializeField]
	private Transform morphRoot;
	[SerializeField]
	private Transform marionetteRoot;
	[SerializeField]
	private GameObject spark;
}
