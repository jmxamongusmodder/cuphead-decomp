using UnityEngine;

public class OldManLevelGnomeLeader : LevelProperties.OldMan.Entity
{
	[SerializeField]
	private OldManLevelParryThermometer parryThermometer;
	public OldManLevelSplashHandler splashHandler;
	[SerializeField]
	private GameObject pit;
	[SerializeField]
	private Transform[] spitRoots;
	[SerializeField]
	private Animator spitVFXAnimator;
	[SerializeField]
	private OldManLevelGnomeProjectile projectilePrefab;
	[SerializeField]
	private OldManLevelStomachPlatform stomachPlatformPrefab;
	[SerializeField]
	public Transform[] platformPositions;
	[SerializeField]
	private float baseHeight;
	[SerializeField]
	private float topAnimSpeed;
	[SerializeField]
	private float heightRange;
	[SerializeField]
	private Collider2D coll;
}
