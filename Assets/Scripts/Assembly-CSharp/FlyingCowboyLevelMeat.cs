using UnityEngine;

public class FlyingCowboyLevelMeat : LevelProperties.FlyingCowboy.Entity
{
	[SerializeField]
	private Transform sausageSpawnPosition;
	[SerializeField]
	private FlyingCowboyLevelBeans beansPrefab;
	[SerializeField]
	private FlyingCowboyLevelSpinningBullet sausageRunSpitBullet;
	[SerializeField]
	private Effect sausageRunSpitBulletEffect;
	[SerializeField]
	private Transform runTopSpitBulletSpawn;
	[SerializeField]
	private Transform runTopSpitBulletEffectSpawn;
	[SerializeField]
	private Transform runBottomSpitBulletSpawn;
	[SerializeField]
	private Transform runBottomSpitBulletEffectSpawn;
	[SerializeField]
	private Vector2 sausageWobbleRadius;
	[SerializeField]
	private Vector2 sausageWobbleDuration;
	[SerializeField]
	private Transform canTransform;
	[SerializeField]
	private GameObject sausageTransforms;
	[SerializeField]
	private BasicProjectile canBullet;
	[SerializeField]
	private Effect canBulletMuzzleFX;
	[SerializeField]
	private Transform bulletRoot;
	[SerializeField]
	private Transform shadowTransform;
	[SerializeField]
	private BasicProjectile sausage;
	[SerializeField]
	private Transform sausageLinkSqueezePoint;
	[SerializeField]
	private Transform sausageHolderA;
	[SerializeField]
	private Transform sausageHolderB;
	[SerializeField]
	private Transform nextBulletSpawnPointA;
	[SerializeField]
	private Transform nextBulletSpawnPointB;
	[SerializeField]
	private FlyingCowboyFloatingSausages floatingSausage;
	[SerializeField]
	private Transform floatingSausageSpawnPointLeft;
	[SerializeField]
	private Transform floatingSausageSpawnPointRight;
	[SerializeField]
	private BasicProjectile sausageString;
	[SerializeField]
	private TriggerZone[] beanCanTriggerZones;
	[SerializeField]
	private Effect sausageDeathEffect;
	[SerializeField]
	private Effect sausageStringDeathEffect;
}
