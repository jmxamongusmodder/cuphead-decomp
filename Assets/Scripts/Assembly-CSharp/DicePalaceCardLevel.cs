using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class DicePalaceCardLevel : AbstractDicePalaceLevel
{
	// Token: 0x060002E5 RID: 741 RVA: 0x0005EBC4 File Offset: 0x0005CFC4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceCard.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060002E6 RID: 742 RVA: 0x0005EC5A File Offset: 0x0005D05A
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceCard;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x0005EC61 File Offset: 0x0005D061
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceCard;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x0005EC68 File Offset: 0x0005D068
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_card;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060002E9 RID: 745 RVA: 0x0005EC6C File Offset: 0x0005D06C
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060002EA RID: 746 RVA: 0x0005EC74 File Offset: 0x0005D074
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0005EC7C File Offset: 0x0005D07C
	protected override void Start()
	{
		base.Start();
		this.card.LevelInit(this.properties);
		this.gameManager.GameSetup(this.properties);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0005ECA6 File Offset: 0x0005D0A6
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacecardPattern_cr());
		base.StartCoroutine(this.gameManager.start_game_cr());
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0005ECC8 File Offset: 0x0005D0C8
	private IEnumerator dicepalacecardPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0005ECE4 File Offset: 0x0005D0E4
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceCard.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400054E RID: 1358
	private LevelProperties.DicePalaceCard properties;

	// Token: 0x0400054F RID: 1359
	[SerializeField]
	private DicePalaceCardGameManager gameManager;

	// Token: 0x04000550 RID: 1360
	[SerializeField]
	private DicePalaceCardLevelCard card;

	// Token: 0x04000551 RID: 1361
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000552 RID: 1362
	[SerializeField]
	private string _bossQuote;
}
