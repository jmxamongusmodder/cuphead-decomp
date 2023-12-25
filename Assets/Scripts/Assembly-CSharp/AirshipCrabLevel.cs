using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class AirshipCrabLevel : Level
{
	// Token: 0x06000076 RID: 118 RVA: 0x00054504 File Offset: 0x00052904
	protected override void PartialInit()
	{
		this.properties = LevelProperties.AirshipCrab.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000077 RID: 119 RVA: 0x0005459A File Offset: 0x0005299A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipCrab;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000078 RID: 120 RVA: 0x000545A1 File Offset: 0x000529A1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_crab;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000079 RID: 121 RVA: 0x000545A5 File Offset: 0x000529A5
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600007A RID: 122 RVA: 0x000545AD File Offset: 0x000529AD
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000545B5 File Offset: 0x000529B5
	protected override void Start()
	{
		base.Start();
		this.crab.LevelInit(this.properties);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000545CE File Offset: 0x000529CE
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.airshipcrabPattern_cr());
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000545E0 File Offset: 0x000529E0
	private IEnumerator airshipcrabPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000545FC File Offset: 0x000529FC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipCrab.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000125 RID: 293
	private LevelProperties.AirshipCrab properties;

	// Token: 0x04000126 RID: 294
	[SerializeField]
	private AirshipCrabLevelCrab crab;

	// Token: 0x04000127 RID: 295
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000128 RID: 296
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
