using UnityEngine;

public class SallyStagePlayLevelSally : LevelProperties.SallyStagePlay.Entity
{
	[SerializeField]
	public CollisionChild collisionChild;
	[SerializeField]
	private Transform husband;
	[SerializeField]
	private SallyStagePlayLevelAngel angel;
	[SerializeField]
	private SallyStagePlayLevelShurikenBomb shurikenPrefab;
	[SerializeField]
	private SallyStagePlayLevelProjectile projectilePrefab;
	[SerializeField]
	private SallyStagePlayLevelUmbrellaProjectile umbrellaProjectilePrefab;
	[SerializeField]
	private SallyStagePlayLevelHeart heartPrefab;
	[SerializeField]
	private SallyStagePlayLevelHouse house;
	[SerializeField]
	private GameObject shadowPrefab;
	[SerializeField]
	private Transform centerPoint;
	[SerializeField]
	private Transform[] spawnPoints;
}
