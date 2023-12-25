using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class TestLevel : Level
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x000779C0 File Offset: 0x00075DC0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Test.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00077A56 File Offset: 0x00075E56
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Test;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00077A59 File Offset: 0x00075E59
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_test;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00077A5D File Offset: 0x00075E5D
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00077A65 File Offset: 0x00075E65
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00077A6D File Offset: 0x00075E6D
	protected override void Start()
	{
		base.Start();
		this.jared.LevelInit(this.properties);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00077A88 File Offset: 0x00075E88
	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			LevelPlayerController player = PlayerManager.GetPlayer<LevelPlayerController>(PlayerId.PlayerOne);
			player.animationController.SetColorOverTime(Color.blue, 1f);
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00077AC3 File Offset: 0x00075EC3
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.testPattern_cr());
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00077AD2 File Offset: 0x00075ED2
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00077ADC File Offset: 0x00075EDC
	private IEnumerator testPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00077AF8 File Offset: 0x00075EF8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Test.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04001028 RID: 4136
	private LevelProperties.Test properties;

	// Token: 0x04001029 RID: 4137
	[SerializeField]
	private TestLevelFlyingJared jared;

	// Token: 0x0400102A RID: 4138
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400102B RID: 4139
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x020004AB RID: 1195
	[Serializable]
	public class Prefabs
	{
	}
}
