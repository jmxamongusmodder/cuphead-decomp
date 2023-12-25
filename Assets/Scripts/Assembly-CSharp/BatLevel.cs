using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class BatLevel : Level
{
	// Token: 0x060000E6 RID: 230 RVA: 0x000558FC File Offset: 0x00053CFC
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Bat.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000E7 RID: 231 RVA: 0x00055992 File Offset: 0x00053D92
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Bat;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000E8 RID: 232 RVA: 0x00055995 File Offset: 0x00053D95
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_bat;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000E9 RID: 233 RVA: 0x00055999 File Offset: 0x00053D99
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000EA RID: 234 RVA: 0x000559A1 File Offset: 0x00053DA1
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000559A9 File Offset: 0x00053DA9
	protected override void Start()
	{
		base.Start();
		this.bat.LevelInit(this.properties);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000559C2 File Offset: 0x00053DC2
	protected override void OnLevelStart()
	{
		base.OnLevelStart();
		base.StartCoroutine(this.batPattern_cr());
		base.StartCoroutine(this.goblins_cr());
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000559E4 File Offset: 0x00053DE4
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Bat.States.Wolf)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_3_cr());
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00055A50 File Offset: 0x00053E50
	private IEnumerator batPattern_cr()
	{
		yield return new WaitForSeconds(1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00055A6C File Offset: 0x00053E6C
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Bat.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Bat.Pattern.Bouncer)
		{
			if (p != LevelProperties.Bat.Pattern.Lightning)
			{
				yield return new WaitForSeconds(1f);
			}
			else
			{
				yield return base.StartCoroutine(this.lightning_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.bouncer_cr());
		}
		yield break;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00055A88 File Offset: 0x00053E88
	private IEnumerator bouncer_cr()
	{
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		this.bat.StartBouncer();
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00055AA4 File Offset: 0x00053EA4
	private IEnumerator lightning_cr()
	{
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		this.bat.StartLightning();
		while (this.bat.state != BatLevelBat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00055AC0 File Offset: 0x00053EC0
	private IEnumerator phase_2_cr()
	{
		this.bat.StartPhase2();
		yield return null;
		yield break;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00055ADC File Offset: 0x00053EDC
	private IEnumerator phase_3_cr()
	{
		this.bat.StartPhase3();
		yield return null;
		yield break;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00055AF8 File Offset: 0x00053EF8
	private IEnumerator goblins_cr()
	{
		if (!this.properties.CurrentState.goblins.Enabled)
		{
			yield return null;
		}
		else
		{
			this.bat.StartGoblin();
		}
		yield return null;
		yield break;
	}

	// Token: 0x04000210 RID: 528
	private LevelProperties.Bat properties;

	// Token: 0x04000211 RID: 529
	[Space(10f)]
	[SerializeField]
	private BatLevelBat bat;

	// Token: 0x04000212 RID: 530
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x020004FF RID: 1279
	[Serializable]
	public class Prefabs
	{
	}
}
