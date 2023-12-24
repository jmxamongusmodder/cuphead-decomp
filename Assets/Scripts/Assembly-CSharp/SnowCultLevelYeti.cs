using UnityEngine;

public class SnowCultLevelYeti : LevelProperties.SnowCult.Entity
{
	public enum States
	{
		Intro = 0,
		Idle = 1,
		Move = 2,
		IcePillar = 3,
		Sled = 4,
		Snowball = 5,
	}

	public States state;
	[SerializeField]
	private SnowCultHandleBackground snowCultBGHandler;
	[SerializeField]
	private Transform yetiMidPoint;
	[SerializeField]
	private Transform yetiSpawnPoint;
	[SerializeField]
	private SnowCultLevelIcePillar icePillarPrefab;
	[SerializeField]
	private SnowCultLevelBat batPrefab;
	[SerializeField]
	private Effect batSpawnEffectPrefab;
	[SerializeField]
	private SnowCultLevelBurstEffect snowBurstA;
	[SerializeField]
	private SnowCultLevelBurstEffect snowFallA;
	[SerializeField]
	private SnowCultLevelSnowball smallSnowballPrefab;
	[SerializeField]
	private SnowCultLevelSnowball mediumSnowballPrefab;
	[SerializeField]
	private SnowCultLevelSnowball largeSnowballPrefab;
	[SerializeField]
	private GameObject cubeLaunchPosition;
	[SerializeField]
	private GameObject ball;
	[SerializeField]
	private Animator[] meltFXAnimator;
	[SerializeField]
	private GameObject dashGroundFX;
	[SerializeField]
	private GameObject groundMask;
	[SerializeField]
	private SpriteRenderer ballShadow;
	[SerializeField]
	private GameObject introShadow;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private Transform[] batAttackPositions;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private GameObject legs;
	[SerializeField]
	private GameObject bucket;
	public bool introRibcageClosed;
}
