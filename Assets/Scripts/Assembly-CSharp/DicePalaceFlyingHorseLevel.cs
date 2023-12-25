using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class DicePalaceFlyingHorseLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000360 RID: 864 RVA: 0x0005FE5C File Offset: 0x0005E25C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceFlyingHorse.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000361 RID: 865 RVA: 0x0005FEF2 File Offset: 0x0005E2F2
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceFlyingHorse;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000362 RID: 866 RVA: 0x0005FEF9 File Offset: 0x0005E2F9
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceFlyingHorse;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000363 RID: 867 RVA: 0x0005FF00 File Offset: 0x0005E300
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_flying_horse;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000364 RID: 868 RVA: 0x0005FF04 File Offset: 0x0005E304
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000365 RID: 869 RVA: 0x0005FF0C File Offset: 0x0005E30C
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0005FF14 File Offset: 0x0005E314
	protected override void Start()
	{
		base.Start();
		this.horse.LevelInit(this.properties);
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0005FF2D File Offset: 0x0005E32D
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceflyinghorsePattern_cr());
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0005FF3C File Offset: 0x0005E33C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0005FF4C File Offset: 0x0005E34C
	private IEnumerator dicepalaceflyinghorsePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0005FF68 File Offset: 0x0005E368
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceFlyingHorse.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceFlyingHorse.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040005E9 RID: 1513
	private LevelProperties.DicePalaceFlyingHorse properties;

	// Token: 0x040005EA RID: 1514
	[SerializeField]
	private DicePalaceFlyingHorseLevelHorse horse;

	// Token: 0x040005EB RID: 1515
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040005EC RID: 1516
	[SerializeField]
	private string _bossQuote;
}
