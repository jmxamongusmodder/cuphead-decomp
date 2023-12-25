using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class DicePalaceCigarLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000315 RID: 789 RVA: 0x0005F2F0 File Offset: 0x0005D6F0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceCigar.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000316 RID: 790 RVA: 0x0005F386 File Offset: 0x0005D786
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceCigar;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000317 RID: 791 RVA: 0x0005F38D File Offset: 0x0005D78D
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceCigar;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000318 RID: 792 RVA: 0x0005F394 File Offset: 0x0005D794
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_cigar;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000319 RID: 793 RVA: 0x0005F398 File Offset: 0x0005D798
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600031A RID: 794 RVA: 0x0005F3A0 File Offset: 0x0005D7A0
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0005F3A8 File Offset: 0x0005D7A8
	protected override void Start()
	{
		base.Start();
		this.cigar.LevelInit(this.properties);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0005F3C1 File Offset: 0x0005D7C1
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacecigarPattern_cr());
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0005F3D0 File Offset: 0x0005D7D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0005F3E0 File Offset: 0x0005D7E0
	private IEnumerator dicepalacecigarPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0005F3FC File Offset: 0x0005D7FC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceCigar.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceCigar.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400057F RID: 1407
	private LevelProperties.DicePalaceCigar properties;

	// Token: 0x04000580 RID: 1408
	[SerializeField]
	private DicePalaceCigarLevelCigar cigar;

	// Token: 0x04000581 RID: 1409
	[SerializeField]
	private Animator fire;

	// Token: 0x04000582 RID: 1410
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000583 RID: 1411
	[SerializeField]
	private string _bossQuote;
}
