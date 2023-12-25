using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class FlyingBlimpLevel : Level
{
	// Token: 0x060004A9 RID: 1193 RVA: 0x00064B64 File Offset: 0x00062F64
	protected override void PartialInit()
	{
		this.properties = LevelProperties.FlyingBlimp.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060004AA RID: 1194 RVA: 0x00064BFA File Offset: 0x00062FFA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingBlimp;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060004AB RID: 1195 RVA: 0x00064C01 File Offset: 0x00063001
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_blimp;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060004AC RID: 1196 RVA: 0x00064C08 File Offset: 0x00063008
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBlimp.States.Main:
			case LevelProperties.FlyingBlimp.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.FlyingBlimp.States.Moon:
				return this._bossPortraitMoon;
			case LevelProperties.FlyingBlimp.States.Sagittarius:
			case LevelProperties.FlyingBlimp.States.Taurus:
			case LevelProperties.FlyingBlimp.States.Gemini:
			case LevelProperties.FlyingBlimp.States.SagOrGem:
				return this._bossPortraitMagical;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060004AD RID: 1197 RVA: 0x00064C94 File Offset: 0x00063094
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBlimp.States.Main:
			case LevelProperties.FlyingBlimp.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.FlyingBlimp.States.Moon:
				return this._bossQuoteMoon;
			case LevelProperties.FlyingBlimp.States.Sagittarius:
			case LevelProperties.FlyingBlimp.States.Taurus:
			case LevelProperties.FlyingBlimp.States.Gemini:
			case LevelProperties.FlyingBlimp.States.SagOrGem:
				return this._bossQuoteMagical;
			default:
				global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00064D1E File Offset: 0x0006311E
	protected override void Start()
	{
		base.Start();
		this.blimpLady.LevelInit(this.properties);
		this.moonLady.LevelInit(this.properties);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00064D48 File Offset: 0x00063148
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.flyingblimpPattern_cr());
		base.StartCoroutine(this.enemies_cr());
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00064D64 File Offset: 0x00063164
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		this.changingStates = true;
		this.StopAllCoroutines();
		base.StopCoroutine(this.blimpLady.spawnEnemy_cr());
		base.StartCoroutine(this.enemies_cr());
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Moon)
		{
			base.StartCoroutine(this.morph_to_moon_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Sagittarius)
		{
			base.StartCoroutine(this.sagittarius_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Taurus)
		{
			base.StartCoroutine(this.taurus_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Gemini)
		{
			base.StartCoroutine(this.gemini_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.SagOrGem)
		{
			if (Rand.Bool())
			{
				base.StartCoroutine(this.sagittarius_cr());
			}
			else
			{
				base.StartCoroutine(this.gemini_cr());
			}
		}
		else
		{
			base.StartCoroutine(this.flyingblimpPattern_cr());
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00064E93 File Offset: 0x00063293
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMagical = null;
		this._bossPortraitMain = null;
		this._bossPortraitMoon = null;
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00064EB0 File Offset: 0x000632B0
	private IEnumerator flyingblimpPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00064ECC File Offset: 0x000632CC
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.FlyingBlimp.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.FlyingBlimp.Pattern.Tornado)
		{
			if (p != LevelProperties.FlyingBlimp.Pattern.Shoot)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.shoot_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.tornado_cr());
		}
		yield break;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00064EE8 File Offset: 0x000632E8
	private IEnumerator tornado_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartTornado();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00064F04 File Offset: 0x00063304
	private IEnumerator sagittarius_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartSagittarius();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00064F20 File Offset: 0x00063320
	private IEnumerator taurus_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartTaurus();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00064F3C File Offset: 0x0006333C
	private IEnumerator gemini_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartGemini();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00064F58 File Offset: 0x00063358
	private IEnumerator shoot_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartShoot();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00064F74 File Offset: 0x00063374
	private IEnumerator enemies_cr()
	{
		if (!this.properties.CurrentState.enemy.active)
		{
			yield return null;
		}
		else
		{
			base.StartCoroutine(this.blimpLady.spawnEnemy_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00064F90 File Offset: 0x00063390
	private IEnumerator morph_to_moon_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.SpawnMoonLady();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000832 RID: 2098
	private LevelProperties.FlyingBlimp properties;

	// Token: 0x04000833 RID: 2099
	[Space(10f)]
	[SerializeField]
	private FlyingBlimpLevelBlimpLady blimpLady;

	// Token: 0x04000834 RID: 2100
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04000835 RID: 2101
	public bool changingStates;

	// Token: 0x04000836 RID: 2102
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private Sprite _bossPortraitMagical;

	// Token: 0x04000838 RID: 2104
	[SerializeField]
	private Sprite _bossPortraitMoon;

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private string _bossQuoteMagical;

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	private string _bossQuoteMoon;
}
