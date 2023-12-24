using UnityEngine;

public class SnowCultLevelWizard : LevelProperties.SnowCult.Entity
{
	public enum States
	{
		Idle = 0,
		Quad = 1,
		Whale = 2,
		Slam = 3,
		Wind = 4,
		SeriesShot = 5,
	}

	public States state;
	[SerializeField]
	private BasicProjectile seriesShot;
	[SerializeField]
	private Animator whaleDropFX;
	[SerializeField]
	private SnowCultLevelTable table;
	[SerializeField]
	private Animator shootFX;
	[SerializeField]
	private SnowCultLevelQuadShot quadShotProjectile;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform outroPos;
	[SerializeField]
	private SpriteMask quadshotMask;
	[SerializeField]
	private Effect cardSparkle;
	[SerializeField]
	private SpriteRenderer introWizRend;
	public bool dead;
}
