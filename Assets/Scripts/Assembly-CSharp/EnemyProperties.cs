using System;
using UnityEngine;

[Serializable]
public class EnemyProperties
{
	public enum LoopMode
	{
		PingPong = 0,
		Repeat = 1,
		Once = 2,
		DelayAtPoint = 3,
	}

	public enum AimMode
	{
		AimedAtPlayer = 0,
		ArcAimedAtPlayer = 1,
		Straight = 2,
		Spread = 3,
		Arc = 4,
	}

	[SerializeField]
	private string _displayName;
	[SerializeField]
	private string _enumName;
	[SerializeField]
	private int _id;
	[SerializeField]
	private float _health;
	[SerializeField]
	private bool _parryable;
	[SerializeField]
	private LoopMode _moveLoopMode;
	[SerializeField]
	private float _moveSpeed;
	[SerializeField]
	private bool _canJump;
	[SerializeField]
	private float _gravity;
	[SerializeField]
	private float _floatSpeed;
	[SerializeField]
	private float _jumpHeight;
	[SerializeField]
	private float _jumpLength;
	[SerializeField]
	private AimMode _projectileAimMode;
	[SerializeField]
	private bool _projectileParryable;
	[SerializeField]
	private float _projectileSpeed;
	[SerializeField]
	private float _arcProjectileMinSpeed;
	[SerializeField]
	private float _projectileAngle;
	[SerializeField]
	private float _arcProjectileMinAngle;
	[SerializeField]
	private float _projectileGravity;
	[SerializeField]
	private float _projectileStoneTime;
	[SerializeField]
	private MinMax _projectileDelay;
	[SerializeField]
	private MinMax _mushroomPinkNumber;
	[SerializeField]
	private float _acornFlySpeed;
	[SerializeField]
	private float _acornDropSpeed;
	[SerializeField]
	private float _acornPropellerSpeed;
	[SerializeField]
	private MinMax _blobRunnerMeltDelay;
	[SerializeField]
	private float _blobRunnerUnnmeltLoopTime;
	public float ClamTimeSpeedUp;
	public float ClamTimeSpeedDown;
	public MinMax ClamMaxPointRange;
	public int ClamShotCount;
	public MinMax ClamDespawnDelayRange;
	public float fastMovement;
	public float slowMovement;
	public string dragonFlyAimString;
	public string dragonFlyAtkDelayString;
	public float dragonFlyWarningDuration;
	public float dragonFlyAttackDuration;
	public float dragonFlyProjectileSpeed;
	public float dragonFlyProjectileDelay;
	public float dragonFlyLockDistOffset;
	public float dragonFlyInitRiseTime;
	public float WoodpeckerWarningDuration;
	public float WoodpeckerAttackDuration;
	public float WoodpeckermoveDownTime;
	public float WoodpeckermoveUpTime;
	public float flyingFishVelocity;
	public float flyingFishSinVelocity;
	public float flyingFishSinSize;
	public float lobsterTuckTime;
	public float lobsterOffscreenTime;
	public float lobsterSpeed;
	public float lobsterWarningTime;
	public float lobsterY;
	public MinMax krillVelocityX;
	public MinMax krillVelocityY;
	public float krillLaunchDelay;
	public float krillGravity;
	public float dragonTimeIn;
	public float dragonTimeOut;
	public float dragonLeaveDelay;
	public float minerShootSpeed;
	public float minerDescendTime;
	public float minerRopeAscendTime;
	public MinMax minerShotDelay;
	public float minerDistance;
	public float wallFaceTravelTime;
	public MinMax wallAttackDelay;
	public float wallProjectileXSpeed;
	public float wallProjectileYSpeed;
	public float wallProjectileGravity;
	public float flamerCirSpeed;
	public MinMax flamerXSpeed;
	public float flamerLoopSize;
	public float fanVelocity;
	public MinMax fanWaitTime;
	public MinMax funWallTopDelayRange;
	public MinMax funWallBottomDelayRange;
	public float funWallProjectileSpeed;
	public float funWallMouthOpenTime;
	public MinMax funWallCarDelayRange;
	public float funWallCarSpeed;
	public MinMax funWallTongueDelayRange;
	public float funWallTongueLoopTime;
	public float jackLaunchVelocity;
	public float jackHomingMoveSpeed;
	public float jackRotationSpeed;
	public float jacktimeBeforeDeath;
	public float jacktimeBeforeHoming;
	public float jackEaseTime;
	public string jackinDirectionString;
	public MinMax jackinAppearDelay;
	public MinMax jackinDeathAppearDelay;
	public float jackinWarningDuration;
	public float jackinShootDelay;
	public int tubaACount;
	public float tubaInitialDelay;
	public MinMax tubaMainDelayRange;
	public float cannonSpeed;
	public float cannonShotDelay;
	public float bulletDeathTime;
	public MinMax pretzelXSpeedRange;
	public float pretzelYSpeed;
	public float pretzelGroundDelay;
	public MinMax arcadeAttackDelayInit;
	public MinMax arcadeAttackDelay;
	public float arcadeBulletSpeed;
	public MinMax arcadeBulletReturnDelay;
	public int arcadeBulletCount;
	public float arcadeBulletIndividualDelay;
	public MinMax magicianAppearDelayRange;
	public MinMax magicianDeathDelayRange;
	public float magicianDurationAppear;
	public float poleSpeedMovement;
}
