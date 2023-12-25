using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class ClownLevel : Level
{
	// Token: 0x06000261 RID: 609 RVA: 0x0005BDF0 File Offset: 0x0005A1F0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Clown.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000262 RID: 610 RVA: 0x0005BE86 File Offset: 0x0005A286
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Clown;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000263 RID: 611 RVA: 0x0005BE8D File Offset: 0x0005A28D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_clown;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000264 RID: 612 RVA: 0x0005BE94 File Offset: 0x0005A294
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Clown.States.Main:
			case LevelProperties.Clown.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Clown.States.HeliumTank:
				return this._bossPortraitHeliumTank;
			case LevelProperties.Clown.States.CarouselHorse:
				return this._bossPortraitCarouselHorse;
			case LevelProperties.Clown.States.Swing:
				return this._bossPortraitSwing;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000265 RID: 613 RVA: 0x0005BF20 File Offset: 0x0005A320
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Clown.States.Main:
			case LevelProperties.Clown.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Clown.States.HeliumTank:
				return this._bossQuoteHeliumTank;
			case LevelProperties.Clown.States.CarouselHorse:
				return this._bossQuoteCarouselHorse;
			case LevelProperties.Clown.States.Swing:
				return this._bossQuoteSwing;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0005BFAC File Offset: 0x0005A3AC
	protected override void Start()
	{
		base.Start();
		this.coasterHandler.LevelInit(this.properties);
		this.clown.LevelInit(this.properties);
		this.clownHelium.LevelInit(this.properties);
		this.clownHorse.LevelInit(this.properties);
		this.clownSwing.LevelInit(this.properties);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0005C014 File Offset: 0x0005A414
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.clownPattern_cr());
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0005C024 File Offset: 0x0005A424
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Clown.States.HeliumTank)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.helium_tank_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Clown.States.CarouselHorse)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.carousel_horse_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Clown.States.Swing)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.swing_cr());
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0005C0BC File Offset: 0x0005A4BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitCarouselHorse = null;
		this._bossPortraitHeliumTank = null;
		this._bossPortraitMain = null;
		this._bossPortraitSwing = null;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0005C0E0 File Offset: 0x0005A4E0
	private IEnumerator clownPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0005C0FC File Offset: 0x0005A4FC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Clown.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Clown.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0005C118 File Offset: 0x0005A518
	private IEnumerator bumper_car_cr()
	{
		this.clown.StartBumperCar();
		yield return null;
		yield break;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0005C134 File Offset: 0x0005A534
	private IEnumerator helium_tank_cr()
	{
		this.clown.EndBumperCar();
		if (this.coasterHandler.isRunning)
		{
			this.coasterHandler.finalRun = true;
		}
		if (this.properties.CurrentState.heliumClown.coasterOn)
		{
			while (this.coasterHandler.finalRun)
			{
				yield return null;
			}
			this.coasterHandler.StartCoaster();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0005C150 File Offset: 0x0005A550
	private IEnumerator carousel_horse_cr()
	{
		this.clownHelium.StartDeath();
		if (this.coasterHandler.isRunning)
		{
			this.coasterHandler.finalRun = true;
		}
		if (this.properties.CurrentState.horse.coasterOn)
		{
			while (this.coasterHandler.finalRun)
			{
				yield return null;
			}
			this.coasterHandler.StartCoaster();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0005C16C File Offset: 0x0005A56C
	private IEnumerator swing_cr()
	{
		this.clownHorse.StartDeath();
		if (this.coasterHandler.isRunning)
		{
			this.coasterHandler.finalRun = true;
		}
		while (this.coasterHandler.finalRun)
		{
			yield return null;
		}
		this.coasterHandler.StartCoaster();
		yield return null;
		yield break;
	}

	// Token: 0x0400044C RID: 1100
	private LevelProperties.Clown properties;

	// Token: 0x0400044D RID: 1101
	[SerializeField]
	private ClownLevelClown clown;

	// Token: 0x0400044E RID: 1102
	[SerializeField]
	private ClownLevelClownHelium clownHelium;

	// Token: 0x0400044F RID: 1103
	[SerializeField]
	private ClownLevelClownHorse clownHorse;

	// Token: 0x04000450 RID: 1104
	[SerializeField]
	private ClownLevelClownSwing clownSwing;

	// Token: 0x04000451 RID: 1105
	[SerializeField]
	private ClownLevelCoasterHandler coasterHandler;

	// Token: 0x04000452 RID: 1106
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000453 RID: 1107
	[SerializeField]
	private Sprite _bossPortraitHeliumTank;

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	private Sprite _bossPortraitCarouselHorse;

	// Token: 0x04000455 RID: 1109
	[SerializeField]
	private Sprite _bossPortraitSwing;

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private string _bossQuoteHeliumTank;

	// Token: 0x04000458 RID: 1112
	[SerializeField]
	private string _bossQuoteCarouselHorse;

	// Token: 0x04000459 RID: 1113
	[SerializeField]
	private string _bossQuoteSwing;
}
