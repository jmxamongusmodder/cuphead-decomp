using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200022E RID: 558
public class OldManLevel : Level
{
	// Token: 0x06000638 RID: 1592 RVA: 0x0006D76C File Offset: 0x0006BB6C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.OldMan.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x0006D802 File Offset: 0x0006BC02
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.OldMan;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600063A RID: 1594 RVA: 0x0006D809 File Offset: 0x0006BC09
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_old_man;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600063B RID: 1595 RVA: 0x0006D810 File Offset: 0x0006BC10
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.OldMan.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.OldMan.States.SockPuppet:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.OldMan.States.GnomeLeader:
				return this._bossPortraitPhaseThree;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600063C RID: 1596 RVA: 0x0006D890 File Offset: 0x0006BC90
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.OldMan.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.OldMan.States.SockPuppet:
				return this._bossQuotePhaseTwo;
			case LevelProperties.OldMan.States.GnomeLeader:
				return this._bossQuotePhaseThree;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0006D90E File Offset: 0x0006BD0E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0006D934 File Offset: 0x0006BD34
	protected override void Start()
	{
		base.Start();
		this.firstAttack = true;
		this.platformManager.LevelInit(this.properties);
		this.oldMan.LevelInit(this.properties);
		this.sockPuppet.LevelInit(this.properties);
		this.gnomeLeader.LevelInit(this.properties);
		this.climberPosString = new PatternString(this.properties.CurrentState.climberGnomes.gnomePositionStrings, true, true);
		for (int i = 0; i < this.spikes.Length; i++)
		{
			this.spikes[i].SetProperties(this.properties);
			this.spikes[i].SetID(i);
		}
		this.gnomeLeader.gameObject.SetActive(false);
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 0f, 0f);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0006DA14 File Offset: 0x0006BE14
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.OldMan.States.SockPuppet)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_2_trans_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.OldMan.States.GnomeLeader)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_3_trans_cr());
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0006DA7E File Offset: 0x0006BE7E
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.oldmanPattern_cr());
		base.StartCoroutine(this.gnome_turrets_cr());
		base.StartCoroutine(this.climbers_cr());
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0006DAA7 File Offset: 0x0006BEA7
	protected override void OnPreWin()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.sockPuppet.OnPhase3();
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0006DAC4 File Offset: 0x0006BEC4
	public void CreateFX(Vector3 pos, bool isSparkle, bool isPink)
	{
		Effect effect = null;
		List<Effect> list = (!isSparkle) ? this.smokeFXPool : this.sparkleFXPool;
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].inUse)
			{
				effect = list[i];
				break;
			}
		}
		if (effect == null)
		{
			effect = ((!isSparkle) ? this.smokePrefab.Create(pos) : this.sparklePrefab.Create(pos));
			list.Add(effect);
		}
		effect.Initialize(pos);
		effect.animator.Play((!isPink) ? this.EffectReset : this.EffectResetPink);
		effect.inUse = true;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0006DB88 File Offset: 0x0006BF88
	private void ClearFX(List<Effect> pool)
	{
		for (int i = 0; i < pool.Count; i++)
		{
			if (pool[i].inUse)
			{
				pool[i].removeOnEnd = true;
			}
			else
			{
				UnityEngine.Object.Destroy(pool[i].gameObject);
			}
		}
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0006DBE0 File Offset: 0x0006BFE0
	private IEnumerator oldmanPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0006DBFC File Offset: 0x0006BFFC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.OldMan.Pattern p = this.properties.CurrentState.NextPattern;
		while (p == LevelProperties.OldMan.Pattern.Camel && this.firstAttack)
		{
			p = this.properties.CurrentState.NextPattern;
		}
		this.firstAttack = false;
		switch (p)
		{
		case LevelProperties.OldMan.Pattern.Spit:
			yield return base.StartCoroutine(this.spit_cr());
			break;
		case LevelProperties.OldMan.Pattern.Duck:
			yield return base.StartCoroutine(this.duck_cr());
			break;
		case LevelProperties.OldMan.Pattern.Camel:
			yield return base.StartCoroutine(this.camel_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0006DC18 File Offset: 0x0006C018
	private IEnumerator spit_cr()
	{
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		this.oldMan.Spit();
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0006DC34 File Offset: 0x0006C034
	private IEnumerator duck_cr()
	{
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		this.oldMan.Goose();
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0006DC50 File Offset: 0x0006C050
	private IEnumerator camel_cr()
	{
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		this.oldMan.Bear();
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0006DC6C File Offset: 0x0006C06C
	private IEnumerator gnome_turrets_cr()
	{
		LevelProperties.OldMan.Turret p = this.properties.CurrentState.turret;
		this.gnomesSpawned = new List<OldManLevelSpikeFloor>();
		PatternString appearString = new PatternString(p.appearOrder, true, true);
		for (;;)
		{
			while (!p.gnomesOn)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, p.appearDelayRange.RandomFloat());
			this.gnomesSpawned.RemoveAll((OldManLevelSpikeFloor g) => g.spikeState != OldManLevelSpikeFloor.SpikeState.Gnomed);
			if (this.gnomesSpawned.Count < p.maxCount)
			{
				int appearOrder = 0;
				do
				{
					appearOrder = appearString.PopInt();
					yield return null;
				}
				while (this.spikes[appearOrder].spikeState != OldManLevelSpikeFloor.SpikeState.Idle);
				this.spikes[appearOrder].SpawnGnome();
				this.gnomesSpawned.Add(this.spikes[appearOrder]);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0006DC88 File Offset: 0x0006C088
	private IEnumerator climbers_cr()
	{
		LevelProperties.OldMan.ClimberGnomes p = this.properties.CurrentState.climberGnomes;
		for (;;)
		{
			yield return null;
			int pos = this.climberPosString.PopInt();
			int platform = 4 - pos / 2;
			if (!this.platformManager.PlatformRemoved(platform))
			{
				OldManLevelGnomeClimber oldManLevelGnomeClimber = this.gnomeClimberPrefab.Spawn<OldManLevelGnomeClimber>();
				float facing = (float)((pos % 2 != 0) ? 1 : -1);
				Transform platform2 = this.platformManager.GetPlatform(platform);
				oldManLevelGnomeClimber.Init(this.climberXPosition[pos], facing, platform2, p);
				this.platformManager.AttachGnome(platform, oldManLevelGnomeClimber);
			}
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
		}
		yield break;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0006DCA4 File Offset: 0x0006C0A4
	public void ActivatePhase2Beard()
	{
		foreach (GameObject gameObject in this.hairObjects)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0006DCD8 File Offset: 0x0006C0D8
	private IEnumerator phase_2_trans_cr()
	{
		this.oldMan.EndPhase1();
		this.ClearFX(this.sparkleFXPool);
		this.ClearFX(this.smokeFXPool);
		yield return this.oldMan.animator.WaitForAnimationToStart(this, "Phase_Trans", false);
		this.oldMan.StopAllCoroutines();
		yield return null;
		foreach (OldManLevelSpikeFloor oldManLevelSpikeFloor in this.spikes)
		{
			oldManLevelSpikeFloor.Exit();
		}
		this.platformManager.EndPhase();
		while (this.oldMan.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.68421054f)
		{
			yield return null;
		}
		Level.Current.SetBounds(new int?(1249), new int?(331), null, null);
		CupheadLevelCamera.Current.ChangeHorizontalBounds(1002, 85);
		Vector3 cameraEndPos = new Vector3(-460f, 0f, 0f);
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEndPos, 3f));
		base.StartCoroutine(this.move_clouds_cr(3f));
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.oldMan.OnPhase2();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.bleachers.SetActive(true);
		yield return null;
		yield break;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0006DCF4 File Offset: 0x0006C0F4
	private IEnumerator move_clouds_cr(float time)
	{
		float leftStartPos = this.cloudLeft.transform.localPosition.x;
		float rightStartPos = this.cloudRight.transform.localPosition.x;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.cloudLeft.transform.localPosition = new Vector3(EaseUtils.EaseInOutSine(leftStartPos, -720f, t / time), this.cloudLeft.transform.localPosition.y);
			this.cloudRight.transform.localPosition = new Vector3(EaseUtils.EaseInOutSine(rightStartPos, 420f, t / time), this.cloudRight.transform.localPosition.y);
			yield return null;
		}
		this.cloudLeft.transform.localPosition = new Vector3(-720f, this.cloudLeft.transform.localPosition.y);
		this.cloudRight.transform.localPosition = new Vector3(420f, this.cloudRight.transform.localPosition.y);
		yield break;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0006DD16 File Offset: 0x0006C116
	public bool InPhase2()
	{
		return this.properties.CurrentState.stateName == LevelProperties.OldMan.States.SockPuppet;
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0006DD2C File Offset: 0x0006C12C
	private IEnumerator phase_3_trans_cr()
	{
		UnityEngine.Object.Destroy(this.platformManager.gameObject);
		this.sockPuppet.OnPhase3();
		AbstractPlayerController p = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		LevelPlayerWeaponManager weaponManagerP = p.GetComponent<LevelPlayerWeaponManager>();
		LevelPlayerMotor motorP = p.GetComponent<LevelPlayerMotor>();
		weaponManagerP.InterruptSuper();
		LevelPlayerWeaponManager weaponManagerP2 = null;
		LevelPlayerMotor motorP2 = null;
		AbstractPlayerController p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (p2 != null)
		{
			motorP2 = p2.GetComponent<LevelPlayerMotor>();
			weaponManagerP2 = p2.GetComponent<LevelPlayerWeaponManager>();
			weaponManagerP2.InterruptSuper();
		}
		yield return new WaitForEndOfFrame();
		while (this.sockPuppet.transState != OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed)
		{
			yield return null;
		}
		this.mainPlatform.SetActive(false);
		this.phaseTransitionTrigger.gameObject.SetActive(true);
		this.mainPit.SetActive(false);
		if (motorP)
		{
			motorP.OnTrampolineKnockUp(-2.3f);
		}
		if (motorP2)
		{
			motorP2.OnTrampolineKnockUp(-2.3f);
		}
		bool readyToGo = false;
		while (!readyToGo)
		{
			if ((p.IsDead || this.phaseTransitionTrigger.bounds.Contains(p.transform.position + Vector3.down * 10f)) && (PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null || PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead || this.phaseTransitionTrigger.bounds.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.down * 10f)))
			{
				readyToGo = true;
				this.sockPuppet.SwallowedPlayers();
			}
			if (p.IsDead || this.phaseTransitionTrigger.bounds.Contains(p.transform.position + Vector3.down * 10f))
			{
				p.gameObject.SetActive(false);
			}
			if ((PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null || PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead || this.phaseTransitionTrigger.bounds.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.down * 10f)) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				PlayerManager.GetPlayer(PlayerId.PlayerTwo).gameObject.SetActive(false);
			}
			yield return null;
		}
		PlayerDeathEffect[] ghosts = UnityEngine.Object.FindObjectsOfType(typeof(PlayerDeathEffect)) as PlayerDeathEffect[];
		foreach (PlayerDeathEffect playerDeathEffect in ghosts)
		{
			playerDeathEffect.transform.position += Vector3.up * 5000f;
		}
		PlayerSuperGhost[] superGhosts = UnityEngine.Object.FindObjectsOfType(typeof(PlayerSuperGhost)) as PlayerSuperGhost[];
		foreach (PlayerSuperGhost playerSuperGhost in superGhosts)
		{
			UnityEngine.Object.Destroy(playerSuperGhost.gameObject);
		}
		PlayerSuperGhostHeart[] superGhostHearts = UnityEngine.Object.FindObjectsOfType(typeof(PlayerSuperGhostHeart)) as PlayerSuperGhostHeart[];
		foreach (PlayerSuperGhostHeart obj in superGhostHearts)
		{
			UnityEngine.Object.Destroy(obj);
		}
		while (this.sockPuppet.transState != OldManLevelSockPuppetHandler.TransitionState.InStomach)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.iris_cr());
		if (!p.IsDead)
		{
			motorP.EnableInput();
		}
		p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (p2 != null && !p2.IsDead)
		{
			motorP2.EnableInput();
		}
		base.StartCoroutine(this.scuba_gnomes_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0006DD48 File Offset: 0x0006C148
	private IEnumerator iris_cr()
	{
		LevelPauseGUI pauseGUI = GameObject.Find("Level_UI").GetComponentInChildren<LevelPauseGUI>();
		pauseGUI.ForceDisablePause(true);
		Animator faderAni = this.fader.GetComponent<Animator>();
		Color c = this.fader.color;
		c.a = 1f;
		this.fader.color = c;
		faderAni.SetTrigger("Iris_In");
		yield return faderAni.WaitForAnimationToEnd(this, "Iris_In", false, true);
		yield return new WaitForSeconds(0.9f);
		this.SetupStomach();
		faderAni.SetTrigger("Iris_Out");
		this.gnomeLeader.StartGnomeLeader();
		yield return faderAni.WaitForAnimationToEnd(this, "Iris_Out", false, true);
		pauseGUI.ForceDisablePause(false);
		c = this.fader.color;
		c.a = 0f;
		this.fader.color = c;
		yield break;
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0006DD64 File Offset: 0x0006C164
	private void SetupStomach()
	{
		Level.Current.SetBounds(new int?(1249), new int?(331), null, null);
		this.gnomeLeader.gameObject.SetActive(true);
		this.phaseTransitionTrigger.gameObject.SetActive(false);
		this.mountainBG.SetActive(false);
		this.stomachBG.SetActive(true);
		this.sockPuppet.FinishPuppet();
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (!player.IsDead)
		{
			player.gameObject.SetActive(true);
			LevelPlayerMotor component = player.GetComponent<LevelPlayerMotor>();
			component.ClearBufferedInput();
			component.ForceLooking(new Trilean2(1, 1));
			player.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
			component.OnRevive(this.gnomeLeader.platformPositions[1].position + Vector3.up * 1000f);
			component.CancelReviveBounce();
			component.EnableInput();
		}
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null && !player2.IsDead)
		{
			player2.gameObject.SetActive(true);
			LevelPlayerMotor component2 = player2.GetComponent<LevelPlayerMotor>();
			component2 = player2.GetComponent<LevelPlayerMotor>();
			component2.ClearBufferedInput();
			component2.ForceLooking(new Trilean2(1, 1));
			player2.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
			component2.OnRevive(this.gnomeLeader.platformPositions[3].position + Vector3.up * 1000f);
			component2.CancelReviveBounce();
			component2.EnableInput();
		}
		this.SFX_StomachLoop();
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0006DEFC File Offset: 0x0006C2FC
	private IEnumerator scuba_gnomes_cr()
	{
		bool onLeft = Rand.Bool();
		LevelProperties.OldMan.ScubaGnomes p = this.properties.CurrentState.scubaGnomes;
		PatternString scubaTypeString = new PatternString(p.scubaTypeString, true, true);
		PatternString spawnDelayString = new PatternString(p.spawnDelayString, true, true);
		PatternString dartParryableString = new PatternString(p.dartParryableString, true);
		float offset = 50f;
		for (;;)
		{
			float xPos = (!onLeft) ? (CupheadLevelCamera.Current.Bounds.xMax - offset) : (CupheadLevelCamera.Current.Bounds.xMin + offset);
			OldManLevelScubaGnome scubaGnome = this.scubaGnomePrefab.Spawn<OldManLevelScubaGnome>();
			scubaGnome.Init(new Vector3(xPos, CupheadLevelCamera.Current.Bounds.yMin), PlayerManager.GetNext(), scubaTypeString.PopLetter() == 'A', onLeft, dartParryableString.PopLetter() == 'P', p, this.gnomeLeader);
			yield return CupheadTime.WaitForSeconds(this, spawnDelayString.PopFloat());
			onLeft = !onLeft;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0006DF18 File Offset: 0x0006C318
	private void SFX_StomachLoop()
	{
		base.transform.position = this.stomachBG.transform.position;
		AudioManager.PlayLoop("sfx_dlc_omm_p3_stomachacid_amb_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_stomachacid_amb_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 1f, 1f);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0006DF70 File Offset: 0x0006C370
	private void WORKAROUND_NullifyFields()
	{
		this.platformManager = null;
		this.fader = null;
		this.hairObjects = null;
		this.scubaGnomePrefab = null;
		this.mainPlatform = null;
		this.oldMan = null;
		this.sockPuppet = null;
		this.gnomeLeader = null;
		this.gnomeClimberPrefab = null;
		this.spikes = null;
		this.mountainBG = null;
		this.cloudLeft = null;
		this.cloudRight = null;
		this.stomachBG = null;
		this.phaseTransitionTrigger = null;
		this.mainPit = null;
		this.bleachers = null;
		this.gnomesSpawned = null;
		this.climberPosString = null;
		this.climberXPosition = null;
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossQuoteMain = null;
		this._bossQuotePhaseTwo = null;
		this._bossQuotePhaseThree = null;
	}

	// Token: 0x04000BB2 RID: 2994
	private LevelProperties.OldMan properties;

	// Token: 0x04000BB3 RID: 2995
	private int EffectReset = Animator.StringToHash("Reset");

	// Token: 0x04000BB4 RID: 2996
	private int EffectResetPink = Animator.StringToHash("ResetPink");

	// Token: 0x04000BB5 RID: 2997
	private const float CAM_END_POS_X = -460f;

	// Token: 0x04000BB6 RID: 2998
	private const float CAM_MOVE_TIME = 3f;

	// Token: 0x04000BB7 RID: 2999
	private const int CAM_PHASE2_BOUNDS_LEFT = 1002;

	// Token: 0x04000BB8 RID: 3000
	private const int CAM_PHASE2_BOUNDS_RIGHT = 85;

	// Token: 0x04000BB9 RID: 3001
	private const int PHASE2_BOUNDS_LEFT = 1249;

	// Token: 0x04000BBA RID: 3002
	private const int PHASE2_BOUNDS_RIGHT = 331;

	// Token: 0x04000BBB RID: 3003
	private const float IRIS_TIME = 0.9f;

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	private Image fader;

	// Token: 0x04000BBD RID: 3005
	[SerializeField]
	private GameObject[] hairObjects;

	// Token: 0x04000BBE RID: 3006
	[SerializeField]
	private OldManLevelScubaGnome scubaGnomePrefab;

	// Token: 0x04000BBF RID: 3007
	[SerializeField]
	private GameObject mainPlatform;

	// Token: 0x04000BC0 RID: 3008
	public OldManLevelPlatformManager platformManager;

	// Token: 0x04000BC1 RID: 3009
	[SerializeField]
	private OldManLevelOldMan oldMan;

	// Token: 0x04000BC2 RID: 3010
	[SerializeField]
	private OldManLevelSockPuppetHandler sockPuppet;

	// Token: 0x04000BC3 RID: 3011
	[SerializeField]
	private OldManLevelGnomeLeader gnomeLeader;

	// Token: 0x04000BC4 RID: 3012
	[SerializeField]
	private OldManLevelGnomeClimber gnomeClimberPrefab;

	// Token: 0x04000BC5 RID: 3013
	[SerializeField]
	private OldManLevelSpikeFloor[] spikes;

	// Token: 0x04000BC6 RID: 3014
	[SerializeField]
	private GameObject mountainBG;

	// Token: 0x04000BC7 RID: 3015
	[SerializeField]
	private GameObject cloudLeft;

	// Token: 0x04000BC8 RID: 3016
	[SerializeField]
	private GameObject cloudRight;

	// Token: 0x04000BC9 RID: 3017
	[SerializeField]
	private GameObject stomachBG;

	// Token: 0x04000BCA RID: 3018
	[SerializeField]
	private Collider2D phaseTransitionTrigger;

	// Token: 0x04000BCB RID: 3019
	[SerializeField]
	private GameObject mainPit;

	// Token: 0x04000BCC RID: 3020
	[SerializeField]
	private GameObject bleachers;

	// Token: 0x04000BCD RID: 3021
	private List<OldManLevelSpikeFloor> gnomesSpawned;

	// Token: 0x04000BCE RID: 3022
	private PatternString climberPosString;

	// Token: 0x04000BCF RID: 3023
	public bool playedFirstSpikeSound;

	// Token: 0x04000BD0 RID: 3024
	private bool firstAttack;

	// Token: 0x04000BD1 RID: 3025
	private float[] climberXPosition = new float[]
	{
		-1006f,
		-766f,
		-792f,
		-562.2f,
		-590.8f,
		-357f,
		-377f,
		-147.7f,
		-163.2f,
		63.2f
	};

	// Token: 0x04000BD2 RID: 3026
	private List<Effect> smokeFXPool = new List<Effect>();

	// Token: 0x04000BD3 RID: 3027
	[SerializeField]
	private Effect smokePrefab;

	// Token: 0x04000BD4 RID: 3028
	private List<Effect> sparkleFXPool = new List<Effect>();

	// Token: 0x04000BD5 RID: 3029
	[SerializeField]
	private Effect sparklePrefab;

	// Token: 0x04000BD6 RID: 3030
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000BD7 RID: 3031
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x04000BD8 RID: 3032
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x04000BD9 RID: 3033
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000BDA RID: 3034
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x04000BDB RID: 3035
	[SerializeField]
	private string _bossQuotePhaseThree;
}
