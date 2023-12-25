using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class DicePalaceBoozeLevel : AbstractDicePalaceLevel
{
	// Token: 0x060002CD RID: 717 RVA: 0x0005E608 File Offset: 0x0005CA08
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceBooze.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060002CE RID: 718 RVA: 0x0005E69E File Offset: 0x0005CA9E
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceBooze;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060002CF RID: 719 RVA: 0x0005E6A5 File Offset: 0x0005CAA5
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceBooze;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060002D0 RID: 720 RVA: 0x0005E6AC File Offset: 0x0005CAAC
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_booze;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060002D1 RID: 721 RVA: 0x0005E6B0 File Offset: 0x0005CAB0
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060002D2 RID: 722 RVA: 0x0005E6B8 File Offset: 0x0005CAB8
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0005E6C0 File Offset: 0x0005CAC0
	protected override void Start()
	{
		base.Start();
		this.decanter.LevelInit(this.properties);
		this.martini.LevelInit(this.properties);
		this.tumbler.LevelInit(this.properties);
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = this.properties.CurrentState.decanter.decanterHP + this.properties.CurrentState.martini.martiniHP + this.properties.CurrentState.tumbler.tumblerHP;
		foreach (Transform lamp in this.lamps)
		{
			base.StartCoroutine(this.lamps_cr(lamp));
		}
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0005E7A6 File Offset: 0x0005CBA6
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceboozePattern_cr());
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0005E7B5 File Offset: 0x0005CBB5
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0005E7C4 File Offset: 0x0005CBC4
	private IEnumerator dicepalaceboozePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0005E7E0 File Offset: 0x0005CBE0
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceBooze.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceBooze.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0005E7FC File Offset: 0x0005CBFC
	private IEnumerator lamps_cr(Transform lamp)
	{
		float t = 0f;
		float maxSpeed = 0f;
		float speed = maxSpeed;
		for (;;)
		{
			t = 0f;
			maxSpeed = UnityEngine.Random.Range(5f, 15f);
			speed = maxSpeed;
			while (!CupheadLevelCamera.Current.isShaking)
			{
				yield return null;
			}
			bool movingRight = Rand.Bool();
			while (speed > 0f)
			{
				t = ((!movingRight) ? (t - CupheadTime.Delta) : (t + CupheadTime.Delta));
				float phase = Mathf.Sin(t);
				lamp.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * speed));
				speed -= 0.05f;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000537 RID: 1335
	private LevelProperties.DicePalaceBooze properties;

	// Token: 0x04000538 RID: 1336
	[SerializeField]
	private Transform[] lamps;

	// Token: 0x04000539 RID: 1337
	[SerializeField]
	private DicePalaceBoozeLevelDecanter decanter;

	// Token: 0x0400053A RID: 1338
	[SerializeField]
	private DicePalaceBoozeLevelMartini martini;

	// Token: 0x0400053B RID: 1339
	[SerializeField]
	private DicePalaceBoozeLevelTumbler tumbler;

	// Token: 0x0400053C RID: 1340
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400053D RID: 1341
	[SerializeField]
	private string _bossQuote;
}
