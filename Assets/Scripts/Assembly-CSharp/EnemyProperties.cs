using System;
using UnityEngine;

// Token: 0x02000431 RID: 1073
[Serializable]
public class EnemyProperties
{
	// Token: 0x06000FA8 RID: 4008 RVA: 0x0009C6AC File Offset: 0x0009AAAC
	public EnemyProperties()
	{
		this._id = TimeUtils.GetCurrentSecond();
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0009C812 File Offset: 0x0009AC12
	public string DisplayName
	{
		get
		{
			return this._displayName;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0009C81A File Offset: 0x0009AC1A
	public string EnumName
	{
		get
		{
			return this._enumName;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0009C822 File Offset: 0x0009AC22
	public int ID
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0009C82A File Offset: 0x0009AC2A
	public float Health
	{
		get
		{
			return this._health;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0009C832 File Offset: 0x0009AC32
	public bool CanParry
	{
		get
		{
			return this._parryable;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0009C83A File Offset: 0x0009AC3A
	public EnemyProperties.LoopMode MoveLoopMode
	{
		get
		{
			return this._moveLoopMode;
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0009C842 File Offset: 0x0009AC42
	public float MoveSpeed
	{
		get
		{
			return this._moveSpeed;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0009C84A File Offset: 0x0009AC4A
	public bool canJump
	{
		get
		{
			return this._canJump;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0009C852 File Offset: 0x0009AC52
	public float gravity
	{
		get
		{
			return this._gravity;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0009C85A File Offset: 0x0009AC5A
	public float floatSpeed
	{
		get
		{
			return this._floatSpeed;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0009C862 File Offset: 0x0009AC62
	public float jumpHeight
	{
		get
		{
			return this._jumpHeight;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0009C86A File Offset: 0x0009AC6A
	public float jumpLength
	{
		get
		{
			return this._jumpLength;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0009C872 File Offset: 0x0009AC72
	public EnemyProperties.AimMode ProjectileAimMode
	{
		get
		{
			return this._projectileAimMode;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0009C87A File Offset: 0x0009AC7A
	public bool ProjectileParryable
	{
		get
		{
			return this._projectileParryable;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0009C882 File Offset: 0x0009AC82
	public float ProjectileSpeed
	{
		get
		{
			return this._projectileSpeed;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0009C88A File Offset: 0x0009AC8A
	public float ArcProjectileMinSpeed
	{
		get
		{
			return this._arcProjectileMinSpeed;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0009C892 File Offset: 0x0009AC92
	public float ProjectileAngle
	{
		get
		{
			return this._projectileAngle;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0009C89A File Offset: 0x0009AC9A
	public float ArcProjectileMinAngle
	{
		get
		{
			return this._arcProjectileMinAngle;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0009C8A2 File Offset: 0x0009ACA2
	public float ProjectileGravity
	{
		get
		{
			return this._projectileGravity;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0009C8AA File Offset: 0x0009ACAA
	public float ProjectileStoneTime
	{
		get
		{
			return this._projectileStoneTime;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0009C8B2 File Offset: 0x0009ACB2
	public MinMax ProjectileDelay
	{
		get
		{
			return this._projectileDelay;
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0009C8BA File Offset: 0x0009ACBA
	public MinMax MushroomPinkNumber
	{
		get
		{
			return this._mushroomPinkNumber;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0009C8C2 File Offset: 0x0009ACC2
	public float AcornFlySpeed
	{
		get
		{
			return this._acornFlySpeed;
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0009C8CA File Offset: 0x0009ACCA
	public float AcornDropSpeed
	{
		get
		{
			return this._acornDropSpeed;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0009C8D2 File Offset: 0x0009ACD2
	public float AcornPropellerSpeed
	{
		get
		{
			return this._acornPropellerSpeed;
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0009C8DA File Offset: 0x0009ACDA
	public MinMax BlobRunnerMeltDelay
	{
		get
		{
			return this._blobRunnerMeltDelay;
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0009C8E2 File Offset: 0x0009ACE2
	public float BlobRunnerUnmeltLoopTime
	{
		get
		{
			return this._blobRunnerUnnmeltLoopTime;
		}
	}

	// Token: 0x040018DA RID: 6362
	[SerializeField]
	private string _displayName = string.Empty;

	// Token: 0x040018DB RID: 6363
	[SerializeField]
	private string _enumName = string.Empty;

	// Token: 0x040018DC RID: 6364
	[SerializeField]
	private int _id;

	// Token: 0x040018DD RID: 6365
	[SerializeField]
	private float _health = 1f;

	// Token: 0x040018DE RID: 6366
	[SerializeField]
	private bool _parryable;

	// Token: 0x040018DF RID: 6367
	[Header("Movement")]
	[SerializeField]
	private EnemyProperties.LoopMode _moveLoopMode;

	// Token: 0x040018E0 RID: 6368
	[SerializeField]
	private float _moveSpeed = 500f;

	// Token: 0x040018E1 RID: 6369
	[SerializeField]
	private bool _canJump;

	// Token: 0x040018E2 RID: 6370
	[SerializeField]
	private float _gravity = 1200f;

	// Token: 0x040018E3 RID: 6371
	[SerializeField]
	private float _floatSpeed = 400f;

	// Token: 0x040018E4 RID: 6372
	[SerializeField]
	private float _jumpHeight = 200f;

	// Token: 0x040018E5 RID: 6373
	[SerializeField]
	private float _jumpLength = 500f;

	// Token: 0x040018E6 RID: 6374
	[Header("Projectiles")]
	[SerializeField]
	private EnemyProperties.AimMode _projectileAimMode = EnemyProperties.AimMode.Straight;

	// Token: 0x040018E7 RID: 6375
	[SerializeField]
	private bool _projectileParryable;

	// Token: 0x040018E8 RID: 6376
	[SerializeField]
	private float _projectileSpeed = 500f;

	// Token: 0x040018E9 RID: 6377
	[SerializeField]
	private float _arcProjectileMinSpeed = 250f;

	// Token: 0x040018EA RID: 6378
	[SerializeField]
	private float _projectileAngle;

	// Token: 0x040018EB RID: 6379
	[SerializeField]
	private float _arcProjectileMinAngle;

	// Token: 0x040018EC RID: 6380
	[SerializeField]
	private float _projectileGravity = 15f;

	// Token: 0x040018ED RID: 6381
	[SerializeField]
	private float _projectileStoneTime;

	// Token: 0x040018EE RID: 6382
	[SerializeField]
	private MinMax _projectileDelay = new MinMax(1f, 1f);

	// Token: 0x040018EF RID: 6383
	[SerializeField]
	private MinMax _mushroomPinkNumber = new MinMax(3f, 5f);

	// Token: 0x040018F0 RID: 6384
	[SerializeField]
	private float _acornFlySpeed = 500f;

	// Token: 0x040018F1 RID: 6385
	[SerializeField]
	private float _acornDropSpeed = 500f;

	// Token: 0x040018F2 RID: 6386
	[SerializeField]
	private float _acornPropellerSpeed = 300f;

	// Token: 0x040018F3 RID: 6387
	[SerializeField]
	private MinMax _blobRunnerMeltDelay = new MinMax(2f, 3f);

	// Token: 0x040018F4 RID: 6388
	[SerializeField]
	private float _blobRunnerUnnmeltLoopTime = 0.5f;

	// Token: 0x040018F5 RID: 6389
	public float ClamTimeSpeedUp = 0.8f;

	// Token: 0x040018F6 RID: 6390
	public float ClamTimeSpeedDown = 1f;

	// Token: 0x040018F7 RID: 6391
	public MinMax ClamMaxPointRange = new MinMax(600f, 700f);

	// Token: 0x040018F8 RID: 6392
	public int ClamShotCount = 4;

	// Token: 0x040018F9 RID: 6393
	public MinMax ClamDespawnDelayRange = new MinMax(3.5f, 5f);

	// Token: 0x040018FA RID: 6394
	public float fastMovement = 400f;

	// Token: 0x040018FB RID: 6395
	public float slowMovement = 200f;

	// Token: 0x040018FC RID: 6396
	public string dragonFlyAimString;

	// Token: 0x040018FD RID: 6397
	public string dragonFlyAtkDelayString;

	// Token: 0x040018FE RID: 6398
	public float dragonFlyWarningDuration;

	// Token: 0x040018FF RID: 6399
	public float dragonFlyAttackDuration;

	// Token: 0x04001900 RID: 6400
	public float dragonFlyProjectileSpeed;

	// Token: 0x04001901 RID: 6401
	public float dragonFlyProjectileDelay;

	// Token: 0x04001902 RID: 6402
	public float dragonFlyLockDistOffset;

	// Token: 0x04001903 RID: 6403
	public float dragonFlyInitRiseTime;

	// Token: 0x04001904 RID: 6404
	public float WoodpeckerWarningDuration;

	// Token: 0x04001905 RID: 6405
	public float WoodpeckerAttackDuration;

	// Token: 0x04001906 RID: 6406
	public float WoodpeckermoveDownTime;

	// Token: 0x04001907 RID: 6407
	public float WoodpeckermoveUpTime;

	// Token: 0x04001908 RID: 6408
	public float flyingFishVelocity;

	// Token: 0x04001909 RID: 6409
	public float flyingFishSinVelocity;

	// Token: 0x0400190A RID: 6410
	public float flyingFishSinSize;

	// Token: 0x0400190B RID: 6411
	public float lobsterTuckTime;

	// Token: 0x0400190C RID: 6412
	public float lobsterOffscreenTime;

	// Token: 0x0400190D RID: 6413
	public float lobsterSpeed;

	// Token: 0x0400190E RID: 6414
	public float lobsterWarningTime;

	// Token: 0x0400190F RID: 6415
	public float lobsterY;

	// Token: 0x04001910 RID: 6416
	public MinMax krillVelocityX;

	// Token: 0x04001911 RID: 6417
	public MinMax krillVelocityY;

	// Token: 0x04001912 RID: 6418
	public float krillLaunchDelay;

	// Token: 0x04001913 RID: 6419
	public float krillGravity;

	// Token: 0x04001914 RID: 6420
	public float dragonTimeIn;

	// Token: 0x04001915 RID: 6421
	public float dragonTimeOut;

	// Token: 0x04001916 RID: 6422
	public float dragonLeaveDelay;

	// Token: 0x04001917 RID: 6423
	public float minerShootSpeed;

	// Token: 0x04001918 RID: 6424
	public float minerDescendTime;

	// Token: 0x04001919 RID: 6425
	public float minerRopeAscendTime;

	// Token: 0x0400191A RID: 6426
	public MinMax minerShotDelay;

	// Token: 0x0400191B RID: 6427
	public float minerDistance;

	// Token: 0x0400191C RID: 6428
	public float wallFaceTravelTime;

	// Token: 0x0400191D RID: 6429
	public MinMax wallAttackDelay;

	// Token: 0x0400191E RID: 6430
	public float wallProjectileXSpeed;

	// Token: 0x0400191F RID: 6431
	public float wallProjectileYSpeed;

	// Token: 0x04001920 RID: 6432
	public float wallProjectileGravity;

	// Token: 0x04001921 RID: 6433
	public float flamerCirSpeed;

	// Token: 0x04001922 RID: 6434
	public MinMax flamerXSpeed;

	// Token: 0x04001923 RID: 6435
	public float flamerLoopSize;

	// Token: 0x04001924 RID: 6436
	public float fanVelocity;

	// Token: 0x04001925 RID: 6437
	public MinMax fanWaitTime;

	// Token: 0x04001926 RID: 6438
	public MinMax funWallTopDelayRange;

	// Token: 0x04001927 RID: 6439
	public MinMax funWallBottomDelayRange;

	// Token: 0x04001928 RID: 6440
	public float funWallProjectileSpeed;

	// Token: 0x04001929 RID: 6441
	public float funWallMouthOpenTime;

	// Token: 0x0400192A RID: 6442
	public MinMax funWallCarDelayRange;

	// Token: 0x0400192B RID: 6443
	public float funWallCarSpeed;

	// Token: 0x0400192C RID: 6444
	public MinMax funWallTongueDelayRange;

	// Token: 0x0400192D RID: 6445
	public float funWallTongueLoopTime;

	// Token: 0x0400192E RID: 6446
	public float jackLaunchVelocity;

	// Token: 0x0400192F RID: 6447
	public float jackHomingMoveSpeed;

	// Token: 0x04001930 RID: 6448
	public float jackRotationSpeed;

	// Token: 0x04001931 RID: 6449
	public float jacktimeBeforeDeath;

	// Token: 0x04001932 RID: 6450
	public float jacktimeBeforeHoming;

	// Token: 0x04001933 RID: 6451
	public float jackEaseTime;

	// Token: 0x04001934 RID: 6452
	public string jackinDirectionString;

	// Token: 0x04001935 RID: 6453
	public MinMax jackinAppearDelay;

	// Token: 0x04001936 RID: 6454
	public MinMax jackinDeathAppearDelay;

	// Token: 0x04001937 RID: 6455
	public float jackinWarningDuration;

	// Token: 0x04001938 RID: 6456
	public float jackinShootDelay;

	// Token: 0x04001939 RID: 6457
	public int tubaACount;

	// Token: 0x0400193A RID: 6458
	public float tubaInitialDelay;

	// Token: 0x0400193B RID: 6459
	public MinMax tubaMainDelayRange;

	// Token: 0x0400193C RID: 6460
	public float cannonSpeed;

	// Token: 0x0400193D RID: 6461
	public float cannonShotDelay;

	// Token: 0x0400193E RID: 6462
	public float bulletDeathTime;

	// Token: 0x0400193F RID: 6463
	public MinMax pretzelXSpeedRange;

	// Token: 0x04001940 RID: 6464
	public float pretzelYSpeed;

	// Token: 0x04001941 RID: 6465
	public float pretzelGroundDelay;

	// Token: 0x04001942 RID: 6466
	public MinMax arcadeAttackDelayInit;

	// Token: 0x04001943 RID: 6467
	public MinMax arcadeAttackDelay;

	// Token: 0x04001944 RID: 6468
	public float arcadeBulletSpeed;

	// Token: 0x04001945 RID: 6469
	public MinMax arcadeBulletReturnDelay;

	// Token: 0x04001946 RID: 6470
	public int arcadeBulletCount;

	// Token: 0x04001947 RID: 6471
	public float arcadeBulletIndividualDelay;

	// Token: 0x04001948 RID: 6472
	public MinMax magicianAppearDelayRange;

	// Token: 0x04001949 RID: 6473
	public MinMax magicianDeathDelayRange;

	// Token: 0x0400194A RID: 6474
	public float magicianDurationAppear;

	// Token: 0x0400194B RID: 6475
	public float poleSpeedMovement;

	// Token: 0x02000432 RID: 1074
	public enum AimMode
	{
		// Token: 0x0400194D RID: 6477
		AimedAtPlayer,
		// Token: 0x0400194E RID: 6478
		ArcAimedAtPlayer,
		// Token: 0x0400194F RID: 6479
		Straight,
		// Token: 0x04001950 RID: 6480
		Spread,
		// Token: 0x04001951 RID: 6481
		Arc
	}

	// Token: 0x02000433 RID: 1075
	public enum LoopMode
	{
		// Token: 0x04001953 RID: 6483
		PingPong,
		// Token: 0x04001954 RID: 6484
		Repeat,
		// Token: 0x04001955 RID: 6485
		Once,
		// Token: 0x04001956 RID: 6486
		DelayAtPoint
	}
}
