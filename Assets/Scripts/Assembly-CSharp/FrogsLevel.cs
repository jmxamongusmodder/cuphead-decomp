using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001ED RID: 493
public class FrogsLevel : Level
{
	// Token: 0x0600055E RID: 1374 RVA: 0x00067E04 File Offset: 0x00066204
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Frogs.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600055F RID: 1375 RVA: 0x00067E9A File Offset: 0x0006629A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Frogs;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x00067E9D File Offset: 0x0006629D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_frogs;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000561 RID: 1377 RVA: 0x00067EA1 File Offset: 0x000662A1
	// (set) Token: 0x06000562 RID: 1378 RVA: 0x00067EA8 File Offset: 0x000662A8
	public static bool FINAL_FORM { get; private set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000563 RID: 1379 RVA: 0x00067EB0 File Offset: 0x000662B0
	// (set) Token: 0x06000564 RID: 1380 RVA: 0x00067EB7 File Offset: 0x000662B7
	public static bool DEMON_TRIGGERED { get; private set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000565 RID: 1381 RVA: 0x00067EC0 File Offset: 0x000662C0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Frogs.States.Main:
			case LevelProperties.Frogs.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Frogs.States.Roll:
				return this._bossPortraitRoll;
			case LevelProperties.Frogs.States.Morph:
				return this._bossPortraitMorph;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000566 RID: 1382 RVA: 0x00067F40 File Offset: 0x00066340
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Frogs.States.Main:
			case LevelProperties.Frogs.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Frogs.States.Roll:
				return this._bossQuoteRoll;
			case LevelProperties.Frogs.States.Morph:
				return this._bossQuoteMorph;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00067FBE File Offset: 0x000663BE
	protected override void Start()
	{
		base.Start();
		this.tall.LevelInit(this.properties);
		this.small.LevelInit(this.properties);
		this.morphed.LevelInit(this.properties);
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00067FF9 File Offset: 0x000663F9
	protected override void OnLevelStart()
	{
		FrogsLevel.FINAL_FORM = false;
		this.StartState(LevelProperties.Frogs.States.Main);
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00068008 File Offset: 0x00066408
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		LevelProperties.Frogs.States stateName = this.properties.CurrentState.stateName;
		if (stateName == LevelProperties.Frogs.States.Morph)
		{
			FrogsLevel.FINAL_FORM = true;
		}
		this.StartState(stateName);
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00068040 File Offset: 0x00066440
	protected override void CreatePlayers()
	{
		base.CreatePlayers();
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			this.tall.AddFanForce(this.players[0]);
			this.tall.AddFanForce(this.players[1]);
		}
		else
		{
			this.tall.AddFanForce(this.players[0]);
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x000680A6 File Offset: 0x000664A6
	protected override void CreatePlayerTwoOnJoin()
	{
		base.CreatePlayerTwoOnJoin();
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			this.tall.AddFanForce(this.players[1]);
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x000680D6 File Offset: 0x000664D6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitMorph = null;
		this._bossPortraitRoll = null;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000680F4 File Offset: 0x000664F4
	private void StartState(LevelProperties.Frogs.States state)
	{
		if (state != LevelProperties.Frogs.States.Generic)
		{
			if (this.checkCoroutine != null)
			{
				base.StopCoroutine(this.checkCoroutine);
			}
			this.checkCoroutine = null;
			if (this.stateCoroutine != null)
			{
				base.StopCoroutine(this.stateCoroutine);
			}
			this.stateCoroutine = null;
			if (this.fanCoroutine != null)
			{
				base.StopCoroutine(this.fanCoroutine);
			}
			this.fanCoroutine = null;
		}
		this.checkCoroutine = base.StartCoroutine(this.startState_cr(state));
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00068178 File Offset: 0x00066578
	private IEnumerator startState_cr(LevelProperties.Frogs.States state)
	{
		switch (state)
		{
		default:
			this.stateCoroutine = base.StartCoroutine(this.mainState_cr());
			break;
		case LevelProperties.Frogs.States.Roll:
			this.wantsToRoll = true;
			yield return base.StartCoroutine(this.waitForFrogs_cr());
			this.small.StartRoll();
			yield return base.StartCoroutine(this.waitForShort_cr());
			this.stateCoroutine = base.StartCoroutine(this.rollState_cr());
			break;
		case LevelProperties.Frogs.States.Morph:
			yield return base.StartCoroutine(this.waitForFrogs_cr());
			FrogsLevel.DEMON_TRIGGERED = this.demonTrigger.getTrigger();
			this.tall.StartMorph();
			this.small.StartMorph();
			break;
		}
		yield break;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0006819C File Offset: 0x0006659C
	private IEnumerator mainState_cr()
	{
		for (;;)
		{
			switch (this.properties.CurrentState.NextPattern)
			{
			case LevelProperties.Frogs.Pattern.TallFan:
				yield return base.StartCoroutine(this.tallFan_cr());
				break;
			case LevelProperties.Frogs.Pattern.ShortRage:
				yield return base.StartCoroutine(this.shortRage_cr());
				break;
			case LevelProperties.Frogs.Pattern.TallFireflies:
				yield return base.StartCoroutine(this.tallFireflies_cr());
				break;
			case LevelProperties.Frogs.Pattern.ShortClap:
				yield return base.StartCoroutine(this.shortClap_cr());
				break;
			case LevelProperties.Frogs.Pattern.Morph:
				goto IL_181;
			case LevelProperties.Frogs.Pattern.RagePlusFireflies:
				yield return base.StartCoroutine(this.ragePlusFireflies_cr());
				break;
			default:
				goto IL_181;
			}
			continue;
			IL_181:
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000681B8 File Offset: 0x000665B8
	private IEnumerator rollState_cr()
	{
		if (this.fanCoroutine != null)
		{
			base.StopCoroutine(this.fanCoroutine);
		}
		this.fanCoroutine = base.StartCoroutine(this.rollFan_cr());
		for (;;)
		{
			LevelProperties.Frogs.Pattern p = this.properties.CurrentState.NextPattern;
			if (p != LevelProperties.Frogs.Pattern.ShortClap)
			{
				if (p != LevelProperties.Frogs.Pattern.ShortRage)
				{
					yield return new WaitForSeconds(1f);
				}
				else
				{
					yield return base.StartCoroutine(this.shortRage_cr());
				}
			}
			else
			{
				yield return base.StartCoroutine(this.shortClap_cr());
			}
		}
		yield break;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x000681D4 File Offset: 0x000665D4
	private IEnumerator rollFan_cr()
	{
		float hesitate = (float)this.properties.CurrentState.tallFan.hesitate;
		yield return base.StartCoroutine(this.waitForShort_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			this.tall.StartFan();
			yield return base.StartCoroutine(this.waitForTall_cr());
			yield return CupheadTime.WaitForSeconds(this, hesitate);
		}
		yield break;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000681F0 File Offset: 0x000665F0
	private IEnumerator waitForFrogs_cr()
	{
		while ((this.tall.state != FrogsLevelTall.State.Complete && this.tall.state != FrogsLevelTall.State.Morphed && this.tall.state != FrogsLevelTall.State.Idle) || (this.small.state != FrogsLevelShort.State.Complete && this.small.state != FrogsLevelShort.State.Morphed && this.small.state != FrogsLevelShort.State.Idle))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0006820C File Offset: 0x0006660C
	private IEnumerator waitForTall_cr()
	{
		while (this.tall.state != FrogsLevelTall.State.Complete && this.tall.state != FrogsLevelTall.State.Morphed)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00068228 File Offset: 0x00066628
	private IEnumerator waitForShort_cr()
	{
		while (this.small.state != FrogsLevelShort.State.Complete && this.small.state != FrogsLevelShort.State.Morphed)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00068244 File Offset: 0x00066644
	private IEnumerator tallFan_cr()
	{
		this.tall.StartFan();
		yield return base.StartCoroutine(this.waitForTall_cr());
		yield break;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00068260 File Offset: 0x00066660
	private IEnumerator tallFireflies_cr()
	{
		this.tall.StartFireflies();
		yield return base.StartCoroutine(this.waitForTall_cr());
		yield break;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0006827C File Offset: 0x0006667C
	private IEnumerator shortRage_cr()
	{
		this.small.StartRage();
		yield return base.StartCoroutine(this.waitForShort_cr());
		yield break;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00068298 File Offset: 0x00066698
	private IEnumerator ragePlusFireflies_cr()
	{
		this.tall.StartFireflies();
		this.small.StartRage();
		while (!this.wantsToRoll)
		{
			if (this.tall.state == FrogsLevelTall.State.Complete)
			{
				this.tall.StartFireflies();
			}
			if (this.small.state == FrogsLevelShort.State.Complete)
			{
				this.small.StartRage();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x000682B4 File Offset: 0x000666B4
	private IEnumerator shortClap_cr()
	{
		this.small.StartClap();
		yield return base.StartCoroutine(this.waitForShort_cr());
		yield break;
	}

	// Token: 0x04000A43 RID: 2627
	private LevelProperties.Frogs properties;

	// Token: 0x04000A46 RID: 2630
	[SerializeField]
	private FrogsLevelTall tall;

	// Token: 0x04000A47 RID: 2631
	[SerializeField]
	private FrogsLevelShort small;

	// Token: 0x04000A48 RID: 2632
	[SerializeField]
	private FrogsLevelMorphed morphed;

	// Token: 0x04000A49 RID: 2633
	[SerializeField]
	private FrogsLevelDemonTrigger demonTrigger;

	// Token: 0x04000A4A RID: 2634
	private bool wantsToRoll;

	// Token: 0x04000A4B RID: 2635
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000A4C RID: 2636
	[SerializeField]
	private Sprite _bossPortraitRoll;

	// Token: 0x04000A4D RID: 2637
	[SerializeField]
	private Sprite _bossPortraitMorph;

	// Token: 0x04000A4E RID: 2638
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000A4F RID: 2639
	[SerializeField]
	private string _bossQuoteRoll;

	// Token: 0x04000A50 RID: 2640
	[SerializeField]
	private string _bossQuoteMorph;

	// Token: 0x04000A51 RID: 2641
	private Coroutine checkCoroutine;

	// Token: 0x04000A52 RID: 2642
	private Coroutine stateCoroutine;

	// Token: 0x04000A53 RID: 2643
	private Coroutine fanCoroutine;

	// Token: 0x020006C2 RID: 1730
	[Serializable]
	public class Prefabs
	{
	}
}
