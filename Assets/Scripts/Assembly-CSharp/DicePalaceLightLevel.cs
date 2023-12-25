using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class DicePalaceLightLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000392 RID: 914 RVA: 0x00060474 File Offset: 0x0005E874
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceLight.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000393 RID: 915 RVA: 0x0006050A File Offset: 0x0005E90A
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceLight;
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000394 RID: 916 RVA: 0x00060511 File Offset: 0x0005E911
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceLight;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000395 RID: 917 RVA: 0x00060518 File Offset: 0x0005E918
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_light;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000396 RID: 918 RVA: 0x0006051C File Offset: 0x0005E91C
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000397 RID: 919 RVA: 0x00060524 File Offset: 0x0005E924
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0006052C File Offset: 0x0005E92C
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00060534 File Offset: 0x0005E934
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacelightPattern_cr());
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00060544 File Offset: 0x0005E944
	private IEnumerator dicepalacelightPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00060560 File Offset: 0x0005E960
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceLight.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000630 RID: 1584
	private LevelProperties.DicePalaceLight properties;

	// Token: 0x04000631 RID: 1585
	[SerializeField]
	private RumRunnersLevelWorm lightBoss;

	// Token: 0x04000632 RID: 1586
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	private string _bossQuote;
}
