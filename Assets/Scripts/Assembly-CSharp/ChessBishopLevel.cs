using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class ChessBishopLevel : ChessLevel
{
	// Token: 0x0600014D RID: 333 RVA: 0x00057BE0 File Offset: 0x00055FE0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessBishop.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600014E RID: 334 RVA: 0x00057C76 File Offset: 0x00056076
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessBishop;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x0600014F RID: 335 RVA: 0x00057C7D File Offset: 0x0005607D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_bishop;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000150 RID: 336 RVA: 0x00057C81 File Offset: 0x00056081
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000151 RID: 337 RVA: 0x00057C89 File Offset: 0x00056089
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00057C91 File Offset: 0x00056091
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.bishop.LevelInit(this.properties);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00057CB0 File Offset: 0x000560B0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bishop = null;
		this._bossPortrait = null;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00057CC6 File Offset: 0x000560C6
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.bishop.StartNewPhase();
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00057CDC File Offset: 0x000560DC
	private IEnumerator bishopPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00057CF8 File Offset: 0x000560F8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessBishop.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040002B3 RID: 691
	private LevelProperties.ChessBishop properties;

	// Token: 0x040002B4 RID: 692
	[SerializeField]
	private ChessBishopLevelBishop bishop;

	// Token: 0x040002B5 RID: 693
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
