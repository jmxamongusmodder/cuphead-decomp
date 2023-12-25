using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class ChessBOldBLevel : Level
{
	// Token: 0x0600017E RID: 382 RVA: 0x00058258 File Offset: 0x00056658
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessBOldB.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600017F RID: 383 RVA: 0x000582EE File Offset: 0x000566EE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessBOldB;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000180 RID: 384 RVA: 0x000582F5 File Offset: 0x000566F5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_boldb;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000181 RID: 385 RVA: 0x000582F9 File Offset: 0x000566F9
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000182 RID: 386 RVA: 0x00058301 File Offset: 0x00056701
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00058309 File Offset: 0x00056709
	protected override void Start()
	{
		base.Start();
		this.boss.LevelInit(this.properties);
		this.gameManager.SetupGameManager(this.properties);
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00058333 File Offset: 0x00056733
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.gameManager.OnStateChanged();
		this.boss.OnStateChanged();
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00058351 File Offset: 0x00056751
	protected override void OnLevelStart()
	{
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00058354 File Offset: 0x00056754
	private IEnumerator ChessBOldBPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00058370 File Offset: 0x00056770
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessBOldB.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040002FE RID: 766
	private LevelProperties.ChessBOldB properties;

	// Token: 0x040002FF RID: 767
	[SerializeField]
	private ChessBOldBLevelBoss boss;

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private ChessBOldBLevelGameManager gameManager;

	// Token: 0x04000301 RID: 769
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000302 RID: 770
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
