using UnityEngine;

public class SnowCultLevelJackFrost : LevelProperties.SnowCult.Entity
{
	public enum States
	{
		Intro = 0,
		Idle = 1,
		Switch = 2,
		Eye = 3,
		Beam = 4,
		Hazard = 5,
		Shard = 6,
		SplitShot = 7,
		Arc = 8,
	}

	[SerializeField]
	private BoxCollider2D boxCollider;
	[SerializeField]
	private SnowCultLevelSplitShotBullet mouthPrefab;
	[SerializeField]
	private SnowCultLevelSplitShotBullet mouthPinkPrefab;
	[SerializeField]
	private SnowCultLevelShard shardPrefab;
	[SerializeField]
	private Effect iceCreamSparkle;
	[SerializeField]
	private SnowCultLevelEyeProjectile eyeProjectile;
	public Transform eyeProjectileGuide;
	[SerializeField]
	private Transform eyeRoot;
	[SerializeField]
	private Transform mouthRoot;
	[SerializeField]
	private Transform splitShotRoot;
	[SerializeField]
	private Transform platformPivotPoint;
	[SerializeField]
	private GameObject platformPrefab;
	[SerializeField]
	private Transform[] platformsPresetPositions;
	[SerializeField]
	private GameObject wizardDeath;
	[SerializeField]
	private GameObject bucket;
	[SerializeField]
	private SpriteRenderer iceCreamGhostRenderer;
	public States state;
	public bool dead;
}
