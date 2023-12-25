using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class RetroArcadeLevel : Level
{
	// Token: 0x06000698 RID: 1688 RVA: 0x000708F4 File Offset: 0x0006ECF4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.RetroArcade.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000699 RID: 1689 RVA: 0x0007098A File Offset: 0x0006ED8A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.RetroArcade;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x0600069A RID: 1690 RVA: 0x00070991 File Offset: 0x0006ED91
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_retro_arcade;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x0600069B RID: 1691 RVA: 0x00070995 File Offset: 0x0006ED95
	// (set) Token: 0x0600069C RID: 1692 RVA: 0x0007099C File Offset: 0x0006ED9C
	public static float ACCURACY_BONUS { get; private set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600069D RID: 1693 RVA: 0x000709A4 File Offset: 0x0006EDA4
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x000709AC File Offset: 0x0006EDAC
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000709B4 File Offset: 0x0006EDB4
	protected override void Start()
	{
		base.Start();
		this.alienManager.LevelInit(this.properties);
		this.caterpillarManager.LevelInit(this.properties);
		this.robotManager.LevelInit(this.properties);
		this.paddleShip.LevelInit(this.properties);
		this.qShip.LevelInit(this.properties);
		this.ufo.LevelInit(this.properties);
		this.toadManager.LevelInit(this.properties);
		this.worm.LevelInit(this.properties);
		this.bouncyManager.LevelInit(this.properties);
		this.missileMan.LevelInit(this.properties);
		this.chaserManager.LevelInit(this.properties);
		this.sheriffManager.LevelInit(this.properties);
		this.snakeManager.LevelInit(this.properties);
		this.tentacleManager.LevelInit(this.properties);
		this.trafficManager.LevelInit(this.properties);
		RetroArcadeLevel.ACCURACY_BONUS = this.properties.CurrentState.general.accuracyBonus;
		this.bigCuphead.Init(PlayerManager.GetPlayer(PlayerId.PlayerOne) as ArcadePlayerController);
		this.bigMugman.Init(PlayerManager.GetPlayer(PlayerId.PlayerTwo) as ArcadePlayerController);
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00070B0C File Offset: 0x0006EF0C
	protected override void CreatePlayers()
	{
		base.CreatePlayers();
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00070B14 File Offset: 0x0006EF14
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00070B1C File Offset: 0x0006EF1C
	protected override void OnLevelStart()
	{
		this.bigCuphead.LevelStart();
		this.bigMugman.LevelStart();
		this.StartStateCoroutine();
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00070B3A File Offset: 0x0006EF3A
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.bigCuphead.OnVictory();
		this.bigMugman.OnVictory();
		this.StartStateCoroutine();
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00070B60 File Offset: 0x0006EF60
	private void StartStateCoroutine()
	{
		switch (this.properties.CurrentState.stateName)
		{
		case LevelProperties.RetroArcade.States.Main:
		case LevelProperties.RetroArcade.States.MissileMan:
			base.StartCoroutine(this.startMissile_cr());
			break;
		case LevelProperties.RetroArcade.States.Caterpillar:
			base.StartCoroutine(this.startCaterpillars_cr());
			break;
		case LevelProperties.RetroArcade.States.Robots:
			base.StartCoroutine(this.startRobots_cr());
			break;
		case LevelProperties.RetroArcade.States.PaddleShip:
			base.StartCoroutine(this.startPaddleShip_cr());
			break;
		case LevelProperties.RetroArcade.States.QShip:
			base.StartCoroutine(this.startQShip_cr());
			break;
		case LevelProperties.RetroArcade.States.UFO:
			base.StartCoroutine(this.startUFO_cr());
			break;
		case LevelProperties.RetroArcade.States.Toad:
			base.StartCoroutine(this.startToad_cr());
			break;
		case LevelProperties.RetroArcade.States.Worm:
			base.StartCoroutine(this.startWorm_cr());
			break;
		case LevelProperties.RetroArcade.States.Aliens:
			base.StartCoroutine(this.startAliens_cr());
			break;
		case LevelProperties.RetroArcade.States.Bouncy:
			base.StartCoroutine(this.startBouncy_cr());
			break;
		case LevelProperties.RetroArcade.States.Chaser:
			base.StartCoroutine(this.startChaser_cr());
			break;
		case LevelProperties.RetroArcade.States.Sheriff:
			base.StartCoroutine(this.startSheriff_cr());
			break;
		case LevelProperties.RetroArcade.States.Snake:
			base.StartCoroutine(this.startSnake_cr());
			break;
		case LevelProperties.RetroArcade.States.Tentacle:
			base.StartCoroutine(this.startTentacle_cr());
			break;
		case LevelProperties.RetroArcade.States.Traffic:
			base.StartCoroutine(this.startTrafficUFO_cr());
			break;
		case LevelProperties.RetroArcade.States.JetpackTest:
			base.StartCoroutine(this.switchToJetpack_cr());
			break;
		}
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00070CF1 File Offset: 0x0006F0F1
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.bigCuphead.OnVictory();
		this.bigMugman.OnVictory();
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00070D10 File Offset: 0x0006F110
	private IEnumerator startAliens_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.alienManager.StartAliens();
		yield break;
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00070D2C File Offset: 0x0006F12C
	private IEnumerator startCaterpillars_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.caterpillarManager.StartCaterpillar();
		yield break;
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00070D48 File Offset: 0x0006F148
	private IEnumerator startRobots_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.robotManager.StartRobots();
		yield break;
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00070D64 File Offset: 0x0006F164
	private IEnumerator startPaddleShip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.paddleShip.StartPaddleShip();
		yield break;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00070D80 File Offset: 0x0006F180
	private IEnumerator startQShip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.qShip.StartQShip();
		yield break;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00070D9C File Offset: 0x0006F19C
	private IEnumerator startUFO_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.ufo.StartUFO();
		yield break;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00070DB8 File Offset: 0x0006F1B8
	private IEnumerator startToad_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.toadManager.StartToad();
		yield break;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00070DD4 File Offset: 0x0006F1D4
	private IEnumerator startWorm_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.worm.StartWorm();
		yield break;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00070DF0 File Offset: 0x0006F1F0
	private IEnumerator startBouncy_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.bouncyManager.StartBouncy();
		yield break;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00070E0C File Offset: 0x0006F20C
	private IEnumerator startMissile_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.missileMan.StartMissile();
		yield break;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00070E28 File Offset: 0x0006F228
	private IEnumerator switchToRocket_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		player.ChangeToRocket();
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			ArcadePlayerController player2 = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerTwo);
			player2.ChangeToRocket();
		}
		yield break;
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00070E44 File Offset: 0x0006F244
	private IEnumerator startChaser_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.chaserManager.StartChasers();
		yield break;
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00070E60 File Offset: 0x0006F260
	private IEnumerator startSheriff_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.sheriffManager.StartSheriff();
		yield break;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00070E7C File Offset: 0x0006F27C
	private IEnumerator startSnake_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.snakeManager.StartSnake();
		yield break;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00070E98 File Offset: 0x0006F298
	private IEnumerator startTentacle_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.tentacleManager.StartTentacle();
		yield break;
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00070EB4 File Offset: 0x0006F2B4
	private IEnumerator startTrafficUFO_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.trafficManager.StartTraffic();
		yield break;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00070ED0 File Offset: 0x0006F2D0
	private IEnumerator switchToJetpack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		player.ChangeToJetpack();
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			ArcadePlayerController player2 = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerTwo);
			player2.ChangeToJetpack();
		}
		yield return null;
		yield break;
	}

	// Token: 0x04000CED RID: 3309
	private LevelProperties.RetroArcade properties;

	// Token: 0x04000CEE RID: 3310
	public static float TOTAL_POINTS;

	// Token: 0x04000CF0 RID: 3312
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000CF1 RID: 3313
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x04000CF2 RID: 3314
	[SerializeField]
	private RetroArcadeTrafficManager trafficManager;

	// Token: 0x04000CF3 RID: 3315
	[SerializeField]
	private RetroArcadeTentacleManager tentacleManager;

	// Token: 0x04000CF4 RID: 3316
	[SerializeField]
	private RetroArcadeSnakeManager snakeManager;

	// Token: 0x04000CF5 RID: 3317
	[SerializeField]
	private RetroArcadeSheriffManager sheriffManager;

	// Token: 0x04000CF6 RID: 3318
	[SerializeField]
	private RetroArcadeChaserManager chaserManager;

	// Token: 0x04000CF7 RID: 3319
	[SerializeField]
	private RetroArcadeBouncyManager bouncyManager;

	// Token: 0x04000CF8 RID: 3320
	[SerializeField]
	private RetroArcadeAlienManager alienManager;

	// Token: 0x04000CF9 RID: 3321
	[SerializeField]
	private RetroArcadeCaterpillarManager caterpillarManager;

	// Token: 0x04000CFA RID: 3322
	[SerializeField]
	private RetroArcadeRobotManager robotManager;

	// Token: 0x04000CFB RID: 3323
	[SerializeField]
	private RetroArcadePaddleShip paddleShip;

	// Token: 0x04000CFC RID: 3324
	[SerializeField]
	private RetroArcadeQShip qShip;

	// Token: 0x04000CFD RID: 3325
	[SerializeField]
	private RetroArcadeUFO ufo;

	// Token: 0x04000CFE RID: 3326
	[SerializeField]
	private RetroArcadeToadManager toadManager;

	// Token: 0x04000CFF RID: 3327
	[SerializeField]
	private RetroArcadeMissileMan missileMan;

	// Token: 0x04000D00 RID: 3328
	[SerializeField]
	private RetroArcadeWorm worm;

	// Token: 0x04000D01 RID: 3329
	[SerializeField]
	private RetroArcadeBigPlayer bigCuphead;

	// Token: 0x04000D02 RID: 3330
	[SerializeField]
	private RetroArcadeBigPlayer bigMugman;

	// Token: 0x04000D03 RID: 3331
	public ArcadePlayerController playerPrefab;
}
