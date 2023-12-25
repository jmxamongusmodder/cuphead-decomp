using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class DicePalaceEightBallLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000348 RID: 840 RVA: 0x0005FB58 File Offset: 0x0005DF58
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceEightBall.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000349 RID: 841 RVA: 0x0005FBEE File Offset: 0x0005DFEE
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceEightBall;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600034A RID: 842 RVA: 0x0005FBF5 File Offset: 0x0005DFF5
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceEightBall;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600034B RID: 843 RVA: 0x0005FBFC File Offset: 0x0005DFFC
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_eight_ball;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600034C RID: 844 RVA: 0x0005FC00 File Offset: 0x0005E000
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600034D RID: 845 RVA: 0x0005FC08 File Offset: 0x0005E008
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x0005FC10 File Offset: 0x0005E010
	protected override void Start()
	{
		base.Start();
		this.eightBall.LevelInit(this.properties);
	}

	// Token: 0x0600034F RID: 847 RVA: 0x0005FC29 File Offset: 0x0005E029
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceeightballPattern_cr());
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0005FC38 File Offset: 0x0005E038
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x0005FC48 File Offset: 0x0005E048
	private IEnumerator dicepalaceeightballPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0005FC64 File Offset: 0x0005E064
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceEightBall.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceEightBall.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040005CB RID: 1483
	private LevelProperties.DicePalaceEightBall properties;

	// Token: 0x040005CC RID: 1484
	[SerializeField]
	private DicePalaceEightBallLevelEightBall eightBall;

	// Token: 0x040005CD RID: 1485
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	private string _bossQuote;
}
