using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class DicePalaceDominoLevel : AbstractDicePalaceLevel
{
	// Token: 0x0600032E RID: 814 RVA: 0x0005F5F4 File Offset: 0x0005D9F4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceDomino.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600032F RID: 815 RVA: 0x0005F68A File Offset: 0x0005DA8A
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceDomino;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000330 RID: 816 RVA: 0x0005F691 File Offset: 0x0005DA91
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceDomino;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000331 RID: 817 RVA: 0x0005F698 File Offset: 0x0005DA98
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_domino;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000332 RID: 818 RVA: 0x0005F69C File Offset: 0x0005DA9C
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000333 RID: 819 RVA: 0x0005F6A4 File Offset: 0x0005DAA4
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x0005F6AC File Offset: 0x0005DAAC
	protected override void Start()
	{
		base.Start();
		this.domino.LevelInit(this.properties);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x0005F6C5 File Offset: 0x0005DAC5
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacedominoPattern_cr());
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0005F6D4 File Offset: 0x0005DAD4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0005F6E4 File Offset: 0x0005DAE4
	private IEnumerator dicepalacedominoPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0005F700 File Offset: 0x0005DB00
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceDomino.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceDomino.Pattern.Boomerang)
		{
			if (p != LevelProperties.DicePalaceDomino.Pattern.BouncyBall)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.bouncyball_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.boomerang_cr());
		}
		yield break;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0005F71C File Offset: 0x0005DB1C
	private IEnumerator boomerang_cr()
	{
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		this.domino.OnBoomerang();
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x0005F738 File Offset: 0x0005DB38
	private IEnumerator bouncyball_cr()
	{
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		this.domino.OnBouncyBall();
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040005A3 RID: 1443
	private LevelProperties.DicePalaceDomino properties;

	// Token: 0x040005A4 RID: 1444
	[SerializeField]
	private DicePalaceDominoLevelDomino domino;

	// Token: 0x040005A5 RID: 1445
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	private string _bossQuote;
}
