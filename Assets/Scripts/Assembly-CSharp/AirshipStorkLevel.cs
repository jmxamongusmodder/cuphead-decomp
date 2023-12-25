using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class AirshipStorkLevel : Level
{
	// Token: 0x060000A2 RID: 162 RVA: 0x00054A78 File Offset: 0x00052E78
	protected override void PartialInit()
	{
		this.properties = LevelProperties.AirshipStork.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000A3 RID: 163 RVA: 0x00054B0E File Offset: 0x00052F0E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipStork;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000A4 RID: 164 RVA: 0x00054B15 File Offset: 0x00052F15
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_stork;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060000A5 RID: 165 RVA: 0x00054B19 File Offset: 0x00052F19
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000A6 RID: 166 RVA: 0x00054B21 File Offset: 0x00052F21
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00054B29 File Offset: 0x00052F29
	protected override void Start()
	{
		base.Start();
		this.stork.LevelInit(this.properties);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00054B42 File Offset: 0x00052F42
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.airshipstorkPattern_cr());
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00054B54 File Offset: 0x00052F54
	private IEnumerator airshipstorkPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00054B70 File Offset: 0x00052F70
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipStork.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000158 RID: 344
	private LevelProperties.AirshipStork properties;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private AirshipStorkLevelStork stork;

	// Token: 0x0400015A RID: 346
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	[Multiline]
	private string _bossQuote;
}
