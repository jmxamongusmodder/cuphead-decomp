using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class DicePalaceRouletteLevel : AbstractDicePalaceLevel
{
	// Token: 0x060003F8 RID: 1016 RVA: 0x0006153C File Offset: 0x0005F93C
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceRoulette.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000615D2 File Offset: 0x0005F9D2
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceRoulette;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x000615D9 File Offset: 0x0005F9D9
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceRoulette;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x000615E0 File Offset: 0x0005F9E0
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_roulette;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060003FC RID: 1020 RVA: 0x000615E4 File Offset: 0x0005F9E4
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x000615EC File Offset: 0x0005F9EC
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x000615F4 File Offset: 0x0005F9F4
	protected override void Start()
	{
		base.Start();
		this.roulette.LevelInit(this.properties);
		foreach (DicePalaceRouletteLevelPlatform dicePalaceRouletteLevelPlatform in this.platforms)
		{
			dicePalaceRouletteLevelPlatform.Init(this.properties.CurrentState.platform);
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0006164D File Offset: 0x0005FA4D
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceroulettePattern_cr());
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0006165C File Offset: 0x0005FA5C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0006166C File Offset: 0x0005FA6C
	private IEnumerator dicepalaceroulettePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00061688 File Offset: 0x0005FA88
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceRoulette.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceRoulette.Pattern.Twirl)
		{
			if (p != LevelProperties.DicePalaceRoulette.Pattern.Marble)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.marble_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.twirl_cr());
		}
		yield break;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x000616A4 File Offset: 0x0005FAA4
	private IEnumerator twirl_cr()
	{
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		this.roulette.StartTwirl();
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x000616C0 File Offset: 0x0005FAC0
	private IEnumerator marble_cr()
	{
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		this.roulette.StartMarbleDrop();
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040006AA RID: 1706
	private LevelProperties.DicePalaceRoulette properties;

	// Token: 0x040006AB RID: 1707
	[SerializeField]
	private DicePalaceRouletteLevelRoulette roulette;

	// Token: 0x040006AC RID: 1708
	[SerializeField]
	private DicePalaceRouletteLevelPlatform[] platforms;

	// Token: 0x040006AD RID: 1709
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private string _bossQuote;
}
