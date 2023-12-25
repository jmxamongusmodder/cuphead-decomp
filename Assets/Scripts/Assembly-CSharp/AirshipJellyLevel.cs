using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class AirshipJellyLevel : Level
{
	// Token: 0x0600008B RID: 139 RVA: 0x000547C4 File Offset: 0x00052BC4
	protected override void PartialInit()
	{
		this.properties = LevelProperties.AirshipJelly.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600008C RID: 140 RVA: 0x0005485A File Offset: 0x00052C5A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipJelly;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600008D RID: 141 RVA: 0x0005485D File Offset: 0x00052C5D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_jelly;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600008E RID: 142 RVA: 0x00054861 File Offset: 0x00052C61
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600008F RID: 143 RVA: 0x00054869 File Offset: 0x00052C69
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00054871 File Offset: 0x00052C71
	protected override void Start()
	{
		base.Start();
		this.jelly.LevelInit(this.properties);
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0005488A File Offset: 0x00052C8A
	protected override void OnLevelStart()
	{
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0005488C File Offset: 0x00052C8C
	private IEnumerator airshipPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x000548A8 File Offset: 0x00052CA8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipJelly.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000136 RID: 310
	private LevelProperties.AirshipJelly properties;

	// Token: 0x04000137 RID: 311
	[SerializeField]
	private AirshipJellyLevelJelly jelly;

	// Token: 0x04000138 RID: 312
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000139 RID: 313
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x020004D3 RID: 1235
	[Serializable]
	public class Prefabs
	{
	}
}
