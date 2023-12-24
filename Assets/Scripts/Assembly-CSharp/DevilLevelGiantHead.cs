using UnityEngine;

public class DevilLevelGiantHead : LevelProperties.Devil.Entity
{
	public enum State
	{
		Intro = 0,
		Idle = 1,
		BombEye = 2,
		SkullEye = 3,
	}

	public State state;
	[SerializeField]
	private GameObject[] groundPieces;
	[SerializeField]
	private DevilLevelPlatform[] HandsPhaseExit;
	[SerializeField]
	private DevilLevelPlatform[] TearsPhaseExit;
	[SerializeField]
	private DevilLevelPlatform[] raisablePlatforms;
	[SerializeField]
	private Transform stage3Platforms;
	[SerializeField]
	private DevilLevelFireball fireballPrefab;
	[SerializeField]
	private DevilLevelBomb bombPrefab;
	[SerializeField]
	private DevilLevelSkull skullPrefab;
	[SerializeField]
	private Transform leftEyeRoot;
	[SerializeField]
	private Transform rightEyeRoot;
	[SerializeField]
	private Transform middleRoot;
	[SerializeField]
	private Transform leftTearRoot;
	[SerializeField]
	private Transform rightTearRoot;
	[SerializeField]
	private DevilLevelHand[] hands;
	[SerializeField]
	private DevilLevelSwooper swooperPrefab;
	[SerializeField]
	private DevilLevelTear tearPrefab;
	[SerializeField]
	private SpriteRenderer bottomSprite;
	[SerializeField]
	private DamageReceiver child;
	[SerializeField]
	private Transform[] spawnPoints;
}
