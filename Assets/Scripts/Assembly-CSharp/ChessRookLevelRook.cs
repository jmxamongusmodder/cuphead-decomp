using UnityEngine;

public class ChessRookLevelRook : LevelProperties.ChessRook.Entity
{
	[SerializeField]
	private SpriteRenderer wheelRenderer;
	[SerializeField]
	private Transform cannonballSpawnRoot;
	[SerializeField]
	private ChessRookLevelPinkCannonBall cannonballPink;
	[SerializeField]
	private ChessRookLevelRegularCannonball cannonballRegular;
	[SerializeField]
	private BasicProjectile straightShot;
	[SerializeField]
	private Transform[] straightShotSpawnPoints;
	[SerializeField]
	private Effect hitSparkEffect;
	[SerializeField]
	private Effect straightShotSparkEffect;
	[SerializeField]
	private Effect smokeEffect;
	[SerializeField]
	private Animator spawnEffect;
	[SerializeField]
	private HitFlash hitFlash;
}
