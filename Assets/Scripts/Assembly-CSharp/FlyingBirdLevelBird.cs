using UnityEngine;

public class FlyingBirdLevelBird : LevelProperties.FlyingBird.Entity
{
	[SerializeField]
	private GameObject bootPrefab;
	[SerializeField]
	private GameObject bootPinkPrefab;
	[SerializeField]
	private GameObject fishPrefab;
	[SerializeField]
	private GameObject applePrefab;
	[SerializeField]
	private FlyingBirdLevelSmallBird smallBird;
	[SerializeField]
	private FlyingBirdLevelBirdFeather featherPrefab;
	[SerializeField]
	private Transform eggRoot;
	[SerializeField]
	private FlyingBirdLevelBirdEgg eggPrefab;
	[SerializeField]
	private GameObject deathParts;
	[SerializeField]
	private Transform nurse1Root;
	[SerializeField]
	private Transform nurse2Root;
	[SerializeField]
	private Transform garbageRoot;
	[SerializeField]
	private FlyingBirdLevelHeart heart;
	[SerializeField]
	private GameObject heartSpitFX;
	[SerializeField]
	private FlyingBirdLevelNurses nurses;
	[SerializeField]
	private GameObject head;
	[SerializeField]
	private Transform[] laserRoots;
	[SerializeField]
	private BasicProjectile laserPrefab;
	[SerializeField]
	private Effect laserEffect;
	[SerializeField]
	private Transform deathEffectsRoot;
	[SerializeField]
	private Effect deathEffectFront;
	[SerializeField]
	private Effect deathEffectBack;
}
