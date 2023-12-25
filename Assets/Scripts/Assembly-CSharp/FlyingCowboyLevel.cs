using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class FlyingCowboyLevel : Level
{
	// Token: 0x060004D0 RID: 1232 RVA: 0x00065840 File Offset: 0x00063C40
	protected override void PartialInit()
	{
		this.properties = LevelProperties.FlyingCowboy.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060004D1 RID: 1233 RVA: 0x000658D6 File Offset: 0x00063CD6
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingCowboy;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060004D2 RID: 1234 RVA: 0x000658DD File Offset: 0x00063CDD
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_cowboy;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060004D3 RID: 1235 RVA: 0x000658E4 File Offset: 0x00063CE4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingCowboy.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.FlyingCowboy.States.Vacuum:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.FlyingCowboy.States.Sausage:
				return this._bossPortraitPhaseFour;
			case LevelProperties.FlyingCowboy.States.Meatball:
				return this._bossPortraitPhaseThree;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00065970 File Offset: 0x00063D70
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingCowboy.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.FlyingCowboy.States.Vacuum:
				return this._bossQuotePhaseTwo;
			case LevelProperties.FlyingCowboy.States.Sausage:
				return this._bossQuotePhaseFour;
			case LevelProperties.FlyingCowboy.States.Meatball:
				return this._bossQuotePhaseThree;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000659F9 File Offset: 0x00063DF9
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseFour = null;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00065A20 File Offset: 0x00063E20
	protected override void Start()
	{
		base.Start();
		this.cowboy.LevelInit(this.properties);
		this.meat.LevelInit(this.properties);
		this.playerDusts[0] = UnityEngine.Object.Instantiate<PlanePlayerDust>(this.playerDust);
		this.playerDusts[1] = UnityEngine.Object.Instantiate<PlanePlayerDust>(this.playerDust);
		this.playerDusts[0].Initialize(this.players[0], this.playerDustSmallTrigger, this.playerDustLargeTrigger);
		this.playerDusts[1].Initialize(this.players[1], this.playerDustSmallTrigger, this.playerDustLargeTrigger);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00065ABD File Offset: 0x00063EBD
	protected override void CreatePlayerTwoOnJoin()
	{
		base.CreatePlayerTwoOnJoin();
		this.playerDusts[1].Initialize(this.players[1], this.playerDustSmallTrigger, this.playerDustLargeTrigger);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00065AE8 File Offset: 0x00063EE8
	protected override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(new Vector3(-1000f, this.playerDustSmallTrigger), Vector3.right * 2000f);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(new Vector3(-1000f, this.playerDustLargeTrigger), Vector3.right * 2000f);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00065B54 File Offset: 0x00063F54
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingCowboy.States.Vacuum)
		{
			base.StartCoroutine(this.toPhase2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingCowboy.States.Meatball)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.toPhase3_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingCowboy.States.Sausage)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.toPhase4_cr());
		}
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00065BE8 File Offset: 0x00063FE8
	private IEnumerator toPhase2_cr()
	{
		LevelProperties.FlyingCowboy.Pattern pattern = this.properties.CurrentState.PeekNextPattern;
		this.cowboy.OnPhase2(pattern);
		yield return this.cowboy.animator.WaitForAnimationToStart(this, "Ph1_To_Ph2", false);
		while (this.cowboy.state == FlyingCowboyLevelCowboy.State.PhaseTrans)
		{
			yield return null;
		}
		base.StartCoroutine(this.phase2Loop_cr());
		yield break;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00065C04 File Offset: 0x00064004
	private IEnumerator phase2Loop_cr()
	{
		bool initial = true;
		for (;;)
		{
			LevelProperties.FlyingCowboy.Pattern p = this.properties.CurrentState.NextPattern;
			if (p == LevelProperties.FlyingCowboy.Pattern.Vacuum)
			{
				yield return base.StartCoroutine(this.vacuum_cr(initial));
			}
			else
			{
				yield return base.StartCoroutine(this.ricochet_cr(initial));
			}
			initial = false;
		}
		yield break;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00065C20 File Offset: 0x00064020
	private IEnumerator vacuum_cr(bool initial)
	{
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		this.cowboy.Vacuum(initial, LevelProperties.FlyingCowboy.Pattern.Default);
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00065C44 File Offset: 0x00064044
	private IEnumerator ricochet_cr(bool initial)
	{
		if (initial && this.properties.CurrentState.ricochet.useRicochet)
		{
			this.cowboy.animator.SetBool("OnRicochet", true);
		}
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		if (this.properties.CurrentState.ricochet.useRicochet)
		{
			this.cowboy.Ricochet();
		}
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00065C68 File Offset: 0x00064068
	private IEnumerator toPhase3_cr()
	{
		this.background.BeginTransition();
		if (this.cowboy != null)
		{
			this.cowboy.Death();
		}
		while (!this.cowboy.IsDead)
		{
			yield return null;
		}
		this.meat.SelectPhase(FlyingCowboyLevelMeat.MeatPhase.Sausage);
		yield break;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00065C84 File Offset: 0x00064084
	private IEnumerator toPhase4_cr()
	{
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		this.meat.SelectPhase(FlyingCowboyLevelMeat.MeatPhase.Can);
		yield break;
	}

	// Token: 0x040008CB RID: 2251
	private LevelProperties.FlyingCowboy properties;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private FlyingCowboyLevelCowboy cowboy;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private FlyingCowboyLevelMeat meat;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private FlyingCowboyLevelBackground background;

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private PlanePlayerDust playerDust;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private float playerDustSmallTrigger;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private float playerDustLargeTrigger;

	// Token: 0x040008D2 RID: 2258
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x040008D4 RID: 2260
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x040008D5 RID: 2261
	[SerializeField]
	private Sprite _bossPortraitPhaseFour;

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x040008D7 RID: 2263
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private string _bossQuotePhaseThree;

	// Token: 0x040008D9 RID: 2265
	[SerializeField]
	private string _bossQuotePhaseFour;

	// Token: 0x040008DA RID: 2266
	private PlanePlayerDust[] playerDusts = new PlanePlayerDust[2];
}
