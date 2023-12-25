using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class ChessBOldALevel : Level
{
	// Token: 0x06000167 RID: 359 RVA: 0x00057EC0 File Offset: 0x000562C0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessBOldA.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000168 RID: 360 RVA: 0x00057F56 File Offset: 0x00056356
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessBOldA;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000169 RID: 361 RVA: 0x00057F5D File Offset: 0x0005635D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_bolda;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600016A RID: 362 RVA: 0x00057F61 File Offset: 0x00056361
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600016B RID: 363 RVA: 0x00057F69 File Offset: 0x00056369
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00057F71 File Offset: 0x00056371
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00057F80 File Offset: 0x00056380
	protected override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.bishop.LevelInit(this.properties);
		foreach (Transform transform in this.topPlatforms)
		{
			transform.transform.SetPosition(null, new float?(-360f + this.properties.CurrentState.stage.platformHeight * 2f), null);
		}
		foreach (Transform transform2 in this.bottomPlatforms)
		{
			transform2.transform.SetPosition(null, new float?(-360f + this.properties.CurrentState.stage.platformHeight), null);
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00058072 File Offset: 0x00056472
	protected override void OnLevelStart()
	{
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00058074 File Offset: 0x00056474
	private IEnumerator chessbishopPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00058090 File Offset: 0x00056490
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessBOldA.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040002DF RID: 735
	private LevelProperties.ChessBOldA properties;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	private ChessBOldALevelBishop bishop;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private Transform[] topPlatforms;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private Transform[] bottomPlatforms;

	// Token: 0x040002E3 RID: 739
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040002E4 RID: 740
	[SerializeField]
	private string _bossQuoteMain;
}
