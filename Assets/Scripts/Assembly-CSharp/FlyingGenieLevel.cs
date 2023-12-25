using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class FlyingGenieLevel : Level
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x000662B0 File Offset: 0x000646B0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.FlyingGenie.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00066346 File Offset: 0x00064746
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingGenie;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0006634D File Offset: 0x0006474D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_genie;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060004FA RID: 1274 RVA: 0x00066354 File Offset: 0x00064754
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingGenie.States.Main:
			case LevelProperties.FlyingGenie.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.FlyingGenie.States.Giant:
				return this._bossPortraitGiant;
			case LevelProperties.FlyingGenie.States.Marionette:
				return this._bossPortraitMarionette;
			case LevelProperties.FlyingGenie.States.Disappear:
				return (this.genie.state != FlyingGenieLevelGenie.State.Disappear) ? this._bossPortraitCoffin : this._bossPortraitDisappear;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060004FB RID: 1275 RVA: 0x000663FC File Offset: 0x000647FC
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingGenie.States.Main:
			case LevelProperties.FlyingGenie.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.FlyingGenie.States.Giant:
				if (PlayerData.Data.DjimmiActivatedBaseGame())
				{
					return this._bossQuoteGameDjimmi;
				}
				return this._bossQuoteGiant;
			case LevelProperties.FlyingGenie.States.Marionette:
				return this._bossQuoteMarionette;
			case LevelProperties.FlyingGenie.States.Disappear:
				return (this.genie.state != FlyingGenieLevelGenie.State.Disappear) ? this._bossQuoteCoffin : this._bossQuoteDisappear;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x000664B7 File Offset: 0x000648B7
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(FlyingGenieLevel.timer_cr());
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000664CB File Offset: 0x000648CB
	protected override void Start()
	{
		base.Start();
		this.genie.LevelInit(this.properties);
		this.genieTransformed.LevelInit(this.properties);
		this.goop.LevelInit(this.properties);
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00066506 File Offset: 0x00064906
	protected override void OnLevelStart()
	{
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00066508 File Offset: 0x00064908
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Marionette)
		{
			base.StartCoroutine(this.phase2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Giant)
		{
			base.StartCoroutine(this.phase3_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Disappear)
		{
			base.StartCoroutine(this.pillar_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Generic)
		{
			base.StartCoroutine(this.treasure_cr());
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000665B6 File Offset: 0x000649B6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitCoffin = null;
		this._bossPortraitDisappear = null;
		this._bossPortraitGiant = null;
		this._bossPortraitMain = null;
		this._bossPortraitMarionette = null;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000665E4 File Offset: 0x000649E4
	private IEnumerator flyinggeniePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00066600 File Offset: 0x00064A00
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.FlyingGenie.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0006661C File Offset: 0x00064A1C
	private IEnumerator phase2_cr()
	{
		this.genie.HitTrigger();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		this.genie.animator.SetTrigger("ToPhase2");
		yield break;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00066638 File Offset: 0x00064A38
	private IEnumerator phase3_cr()
	{
		if (!this.genieTransformed.skipMarionette)
		{
			this.genieTransformed.EndMarionette();
		}
		else
		{
			this.secretTriggered = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00066654 File Offset: 0x00064A54
	private IEnumerator pillar_cr()
	{
		this.genie.HitTrigger();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		this.genie.StartObelisk();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00066670 File Offset: 0x00064A70
	private IEnumerator treasure_cr()
	{
		this.genie.HitTrigger();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		this.genie.StartTreasure();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0006668C File Offset: 0x00064A8C
	private static IEnumerator timer_cr()
	{
		FlyingGenieLevel.mainTimer = 9.25f;
		for (;;)
		{
			FlyingGenieLevel.mainTimer += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000966 RID: 2406
	private LevelProperties.FlyingGenie properties;

	// Token: 0x04000967 RID: 2407
	public const float SHADE_PERIOD = 12f;

	// Token: 0x04000968 RID: 2408
	private const float SHADE_START_TIME = 9.25f;

	// Token: 0x04000969 RID: 2409
	public static float mainTimer;

	// Token: 0x0400096A RID: 2410
	[SerializeField]
	private FlyingGenieLevelGoop goop;

	// Token: 0x0400096B RID: 2411
	[SerializeField]
	private FlyingGenieLevelGenie genie;

	// Token: 0x0400096C RID: 2412
	[SerializeField]
	private FlyingGenieLevelGenieTransform genieTransformed;

	// Token: 0x0400096D RID: 2413
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	private Sprite _bossPortraitDisappear;

	// Token: 0x0400096F RID: 2415
	[SerializeField]
	private Sprite _bossPortraitCoffin;

	// Token: 0x04000970 RID: 2416
	[SerializeField]
	private Sprite _bossPortraitMarionette;

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private Sprite _bossPortraitGiant;

	// Token: 0x04000972 RID: 2418
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000973 RID: 2419
	[SerializeField]
	private string _bossQuoteDisappear;

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	private string _bossQuoteCoffin;

	// Token: 0x04000975 RID: 2421
	[SerializeField]
	private string _bossQuoteMarionette;

	// Token: 0x04000976 RID: 2422
	[SerializeField]
	private string _bossQuoteGiant;

	// Token: 0x04000977 RID: 2423
	[SerializeField]
	private string _bossQuoteGameDjimmi;
}
