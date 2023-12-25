using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class DicePalaceMainLevel : AbstractDicePalaceLevel
{
	// Token: 0x060003A9 RID: 937 RVA: 0x00060728 File Offset: 0x0005EB28
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceMain.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060003AA RID: 938 RVA: 0x000607BE File Offset: 0x0005EBBE
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceMain;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060003AB RID: 939 RVA: 0x000607C5 File Offset: 0x0005EBC5
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceMain;
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060003AC RID: 940 RVA: 0x000607CC File Offset: 0x0005EBCC
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_main;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060003AD RID: 941 RVA: 0x000607D0 File Offset: 0x0005EBD0
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060003AE RID: 942 RVA: 0x000607D8 File Offset: 0x0005EBD8
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060003AF RID: 943 RVA: 0x000607E0 File Offset: 0x0005EBE0
	public DicePalaceMainLevelGameManager GameManager
	{
		get
		{
			return this.gameManager;
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x000607E8 File Offset: 0x0005EBE8
	protected override void Start()
	{
		base.Start();
		this.gameManager.LevelInit(this.properties);
		this.kingDice.LevelInit(this.properties);
		if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice)
		{
			DicePalaceMainLevelGameInfo.CHALICE_PLAYER = 0;
		}
		else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)
		{
			DicePalaceMainLevelGameInfo.CHALICE_PLAYER = 1;
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00060869 File Offset: 0x0005EC69
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacemainPattern_cr());
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00060878 File Offset: 0x0005EC78
	protected override void CheckIfInABossesHub()
	{
		base.CheckIfInABossesHub();
		if (!this.isTowerOfPower)
		{
			Level.IsDicePalaceMain = true;
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00060891 File Offset: 0x0005EC91
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000608A0 File Offset: 0x0005ECA0
	private IEnumerator dicepalacemainPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x000608BC File Offset: 0x0005ECBC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceMain.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400064B RID: 1611
	private LevelProperties.DicePalaceMain properties;

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private DicePalaceMainLevelGameManager gameManager;

	// Token: 0x0400064D RID: 1613
	[SerializeField]
	private DicePalaceMainLevelKingDice kingDice;

	// Token: 0x0400064E RID: 1614
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private string _bossQuote;
}
