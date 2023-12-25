using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class FlyingTestLevel : Level
{
	// Token: 0x06000543 RID: 1347 RVA: 0x00067B50 File Offset: 0x00065F50
	protected override void PartialInit()
	{
		this.properties = LevelProperties.FlyingTest.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x00067BE6 File Offset: 0x00065FE6
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingTest;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x00067BED File Offset: 0x00065FED
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_test;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00067BF1 File Offset: 0x00065FF1
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000547 RID: 1351 RVA: 0x00067BF9 File Offset: 0x00065FF9
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00067C01 File Offset: 0x00066001
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00067C09 File Offset: 0x00066009
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.flyingtestPattern_cr());
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00067C18 File Offset: 0x00066018
	private IEnumerator flyingtestPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00067C34 File Offset: 0x00066034
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.FlyingTest.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040009F7 RID: 2551
	private LevelProperties.FlyingTest properties;

	// Token: 0x040009F8 RID: 2552
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040009F9 RID: 2553
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x020004AA RID: 1194
	[Serializable]
	public class Prefabs
	{
	}
}
