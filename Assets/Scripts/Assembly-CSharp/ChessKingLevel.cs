using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class ChessKingLevel : Level
{
	// Token: 0x060001D0 RID: 464 RVA: 0x0005A35C File Offset: 0x0005875C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessKing.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060001D1 RID: 465 RVA: 0x0005A3F2 File Offset: 0x000587F2
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessKing;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060001D2 RID: 466 RVA: 0x0005A3F9 File Offset: 0x000587F9
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_king;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060001D3 RID: 467 RVA: 0x0005A3FD File Offset: 0x000587FD
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060001D4 RID: 468 RVA: 0x0005A405 File Offset: 0x00058805
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0005A40D File Offset: 0x0005880D
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.king.LevelInit(this.properties);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0005A42C File Offset: 0x0005882C
	protected override void OnLevelStart()
	{
		this.king.StartGame();
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0005A439 File Offset: 0x00058839
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.king.StateChange();
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0005A44C File Offset: 0x0005884C
	private IEnumerator chesskingPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0005A468 File Offset: 0x00058868
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessKing.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000352 RID: 850
	private LevelProperties.ChessKing properties;

	// Token: 0x04000353 RID: 851
	[SerializeField]
	private ChessKingLevelKing king;

	// Token: 0x04000354 RID: 852
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000355 RID: 853
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
