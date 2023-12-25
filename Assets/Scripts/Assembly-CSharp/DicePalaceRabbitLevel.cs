using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class DicePalaceRabbitLevel : AbstractDicePalaceLevel
{
	// Token: 0x060003DD RID: 989 RVA: 0x00060FD8 File Offset: 0x0005F3D8
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceRabbit.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060003DE RID: 990 RVA: 0x0006106E File Offset: 0x0005F46E
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceRabbit;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060003DF RID: 991 RVA: 0x00061075 File Offset: 0x0005F475
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceRabbit;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060003E0 RID: 992 RVA: 0x0006107C File Offset: 0x0005F47C
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_rabbit;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060003E1 RID: 993 RVA: 0x00061080 File Offset: 0x0005F480
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x00061088 File Offset: 0x0005F488
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00061090 File Offset: 0x0005F490
	protected override void Start()
	{
		base.Start();
		this.rabbit.LevelInit(this.properties);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x000610A9 File Offset: 0x0005F4A9
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacerabbitPattern_cr());
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x000610B8 File Offset: 0x0005F4B8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x000610C8 File Offset: 0x0005F4C8
	private IEnumerator dicepalacerabbitPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x000610E4 File Offset: 0x0005F4E4
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceRabbit.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceRabbit.Pattern.MagicWand)
		{
			if (p != LevelProperties.DicePalaceRabbit.Pattern.MagicParry)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.magicparry_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.magicwand_cr());
		}
		yield break;
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00061100 File Offset: 0x0005F500
	private IEnumerator magicwand_cr()
	{
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		this.rabbit.OnMagicWand();
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0006111C File Offset: 0x0005F51C
	private IEnumerator magicparry_cr()
	{
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		this.rabbit.OnMagicParry();
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400068C RID: 1676
	private LevelProperties.DicePalaceRabbit properties;

	// Token: 0x0400068D RID: 1677
	[SerializeField]
	private DicePalaceRabbitLevelRabbit rabbit;

	// Token: 0x0400068E RID: 1678
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private string _bossQuote;
}
