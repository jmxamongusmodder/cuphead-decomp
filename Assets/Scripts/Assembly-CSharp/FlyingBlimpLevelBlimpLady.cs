using UnityEngine;

public class FlyingBlimpLevelBlimpLady : LevelProperties.FlyingBlimp.Entity
{
	[SerializeField]
	private Material blimpMat;
	[SerializeField]
	private Material taurusMat;
	[SerializeField]
	private Material sagittariusMat;
	[SerializeField]
	private Material geminiMat;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform transformationPoint;
	[SerializeField]
	private Effect dashExplosionEffect;
	[SerializeField]
	private GameObject cloudEffect;
	[SerializeField]
	private GameObject bigCloud;
	[SerializeField]
	private SpriteRenderer constellationHandler;
	[SerializeField]
	private SpriteRenderer blackDim;
	[SerializeField]
	private FlyingBlimpLevelEnemy enemyPrefabA;
	[SerializeField]
	private FlyingBlimpLevelEnemy enemyPrefabB;
	[SerializeField]
	private FlyingBlimpLevelTornado tornadoPrefab;
	[SerializeField]
	private FlyingBlimpLevelShootProjectile shootProjectilePrefab;
	[SerializeField]
	private FlyingBlimpLevelArrowProjectile sagittariusStarPrefab;
	[SerializeField]
	private BasicProjectile sagittariusArrowPrefab;
	[SerializeField]
	private FlyingBlimpLevelGeminiShoot geminiObjectPrefab;
	[SerializeField]
	private SpriteRenderer geminiClone;
	[SerializeField]
	private SpriteRenderer sphere;
	[SerializeField]
	private FlyingBlimpLevelSpawnRadius objectSpawnRoot;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private Transform arrowRoot;
	[SerializeField]
	private Transform arrowEffectRoot;
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;
	[SerializeField]
	private Vector2 explosionOffset;
	[SerializeField]
	private Effect arrowEffect;
	[SerializeField]
	private float explosionRadius;
}
