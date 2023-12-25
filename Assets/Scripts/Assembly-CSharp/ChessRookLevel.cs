using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class ChessRookLevel : ChessLevel
{
	// Token: 0x06000245 RID: 581 RVA: 0x0005BB10 File Offset: 0x00059F10
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessRook.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000246 RID: 582 RVA: 0x0005BBA6 File Offset: 0x00059FA6
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessRook;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000247 RID: 583 RVA: 0x0005BBAD File Offset: 0x00059FAD
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_rook;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000248 RID: 584 RVA: 0x0005BBB1 File Offset: 0x00059FB1
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000249 RID: 585 RVA: 0x0005BBB9 File Offset: 0x00059FB9
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0005BBC1 File Offset: 0x00059FC1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this.rook = null;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0005BBD7 File Offset: 0x00059FD7
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.rook.LevelInit(this.properties);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0005BBF6 File Offset: 0x00059FF6
	protected override void OnLevelStart()
	{
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0005BBF8 File Offset: 0x00059FF8
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.rook.OnPhaseChange();
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0005BC0C File Offset: 0x0005A00C
	private IEnumerator chessrookPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0005BC28 File Offset: 0x0005A028
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessRook.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040003F4 RID: 1012
	private LevelProperties.ChessRook properties;

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	private ChessRookLevelRook rook;

	// Token: 0x040003F6 RID: 1014
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private string _bossQuoteMain;
}
