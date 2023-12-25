using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class DicePalaceFlyingMemoryLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000379 RID: 889 RVA: 0x00060160 File Offset: 0x0005E560
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceFlyingMemory.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600037A RID: 890 RVA: 0x000601F6 File Offset: 0x0005E5F6
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceFlyingMemory;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600037B RID: 891 RVA: 0x000601FD File Offset: 0x0005E5FD
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceFlyingMemory;
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00060204 File Offset: 0x0005E604
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_flying_memory;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x0600037D RID: 893 RVA: 0x00060208 File Offset: 0x0005E608
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600037E RID: 894 RVA: 0x00060210 File Offset: 0x0005E610
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00060218 File Offset: 0x0005E618
	protected override void Start()
	{
		base.Start();
		this.gameManager.LevelInit(this.properties);
		this.stuffedToy.LevelInit(this.properties);
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00060242 File Offset: 0x0005E642
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceflyingmemoryPattern_cr());
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00060251 File Offset: 0x0005E651
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00060260 File Offset: 0x0005E660
	private IEnumerator dicepalaceflyingmemoryPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0006027C File Offset: 0x0005E67C
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceFlyingMemory.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000616 RID: 1558
	private LevelProperties.DicePalaceFlyingMemory properties;

	// Token: 0x04000617 RID: 1559
	[SerializeField]
	private DicePalaceFlyingMemoryLevelStuffedToy stuffedToy;

	// Token: 0x04000618 RID: 1560
	[SerializeField]
	private DicePalaceFlyingMemoryLevelGameManager gameManager;

	// Token: 0x04000619 RID: 1561
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400061A RID: 1562
	[SerializeField]
	private string _bossQuote;
}
