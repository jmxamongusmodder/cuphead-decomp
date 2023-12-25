using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class ChessQueenLevel : ChessLevel
{
	// Token: 0x06000228 RID: 552 RVA: 0x0005B49C File Offset: 0x0005989C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessQueen.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000229 RID: 553 RVA: 0x0005B532 File Offset: 0x00059932
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessQueen;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600022A RID: 554 RVA: 0x0005B539 File Offset: 0x00059939
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_queen;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600022B RID: 555 RVA: 0x0005B53D File Offset: 0x0005993D
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600022C RID: 556 RVA: 0x0005B545 File Offset: 0x00059945
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0005B54D File Offset: 0x0005994D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this.queen = null;
		this.mouseAnimator = null;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0005B56A File Offset: 0x0005996A
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.queen.LevelInit(this.properties);
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0005B58C File Offset: 0x0005998C
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		float num = UnityEngine.Random.Range(0f, 1f);
		this.mouseAnimator[0].Play("Win", 0, num);
		this.mouseAnimator[1].Play("Win", 0, num + 0.33f);
		this.mouseAnimator[2].Play("Win", 0, num + 0.66f);
		this.mouseAnimator[0].Play("Idle", 1, 0f);
		this.mouseAnimator[1].Play("Idle", 1, 0.33f);
		this.mouseAnimator[2].Play("Idle", 1, 0.66f);
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0005B63F File Offset: 0x00059A3F
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.queen.StateChanged();
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0005B652 File Offset: 0x00059A52
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.chessQueenPattern_cr());
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0005B664 File Offset: 0x00059A64
	private IEnumerator chessQueenPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0005B680 File Offset: 0x00059A80
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessQueen.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.ChessQueen.Pattern.Lightning)
		{
			if (p != LevelProperties.ChessQueen.Pattern.Egg)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.egg_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.lightning_cr());
		}
		yield break;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0005B69C File Offset: 0x00059A9C
	public bool NextPatternIsEgg()
	{
		if (this.properties.CurrentState.PeekNextPattern == LevelProperties.ChessQueen.Pattern.Egg)
		{
			LevelProperties.ChessQueen.Pattern nextPattern = this.properties.CurrentState.NextPattern;
			return true;
		}
		return false;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0005B6D4 File Offset: 0x00059AD4
	private IEnumerator lightning_cr()
	{
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		this.queen.StartLightning();
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0005B6F0 File Offset: 0x00059AF0
	private IEnumerator egg_cr()
	{
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		this.queen.StartEgg();
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040003C5 RID: 965
	private LevelProperties.ChessQueen properties;

	// Token: 0x040003C6 RID: 966
	[SerializeField]
	private ChessQueenLevelQueen queen;

	// Token: 0x040003C7 RID: 967
	[SerializeField]
	private Animator[] mouseAnimator;

	// Token: 0x040003C8 RID: 968
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x040003CA RID: 970
	public bool cannonBlastFXVariant;
}
