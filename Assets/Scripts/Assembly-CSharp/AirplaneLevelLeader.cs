using UnityEngine;
using System.Collections.Generic;

public class AirplaneLevelLeader : LevelProperties.Airplane.Entity
{
	[SerializeField]
	private Transform[] laserPositions;
	[SerializeField]
	private Transform rocketSpawnLeft;
	[SerializeField]
	private Transform rocketSpawnRight;
	[SerializeField]
	private Transform yellowPosSideways;
	[SerializeField]
	private Transform redPosSideways;
	[SerializeField]
	private Transform flashRootLeft;
	[SerializeField]
	private Transform flashRootRight;
	[SerializeField]
	private Transform leftDogBowlSpawn;
	[SerializeField]
	private Transform rightDogBowlSpawn;
	[SerializeField]
	private AnimationClip buildLaserAni;
	[SerializeField]
	private AirplaneLevelRocket rocketPrefab;
	[SerializeField]
	private AirplaneLevelDropBullet yellowBullet;
	[SerializeField]
	private AirplaneLevelDropBullet redBullet;
	[SerializeField]
	private Animator[] laserAnimator;
	[SerializeField]
	private Effect flashEffect;
	[SerializeField]
	private LevelBossDeathExploder rotatedExploder;
	[SerializeField]
	private LevelBossDeathExploder pawRightExploder;
	[SerializeField]
	private LevelBossDeathExploder pawLeftExploder;
	[SerializeField]
	private List<Animator> deathPuffs;
}
