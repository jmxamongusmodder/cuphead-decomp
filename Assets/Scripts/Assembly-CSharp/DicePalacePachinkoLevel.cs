using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class DicePalacePachinkoLevel : AbstractDicePalaceLevel
{
	// Token: 0x060003C4 RID: 964 RVA: 0x00060A84 File Offset: 0x0005EE84
	protected override void PartialInit()
	{
		this.properties = LevelProperties.DicePalacePachinko.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060003C5 RID: 965 RVA: 0x00060B1A File Offset: 0x0005EF1A
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalacePachinko;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060003C6 RID: 966 RVA: 0x00060B21 File Offset: 0x0005EF21
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalacePachinko;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060003C7 RID: 967 RVA: 0x00060B28 File Offset: 0x0005EF28
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_pachinko;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060003C8 RID: 968 RVA: 0x00060B2C File Offset: 0x0005EF2C
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060003C9 RID: 969 RVA: 0x00060B34 File Offset: 0x0005EF34
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00060B3C File Offset: 0x0005EF3C
	protected override void Start()
	{
		base.Start();
		this.pipes.LevelInit(this.properties);
		this.pachinko.LevelInit(this.properties);
		foreach (Transform disc in this.starDiscs)
		{
			base.StartCoroutine(this.star_disc_cr(disc));
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00060B9E File Offset: 0x0005EF9E
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacepachinkoPattern_cr());
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00060BB0 File Offset: 0x0005EFB0
	private IEnumerator star_disc_cr(Transform disc)
	{
		bool fadingOut = Rand.Bool();
		for (;;)
		{
			float fadeTime = UnityEngine.Random.Range(0.1f, 0.3f);
			float holdTime = UnityEngine.Random.Range(0.1f, 0.3f);
			yield return CupheadTime.WaitForSeconds(this, holdTime);
			if (fadingOut)
			{
				float t = 0f;
				while (t < fadeTime)
				{
					disc.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
					t += CupheadTime.Delta;
					yield return null;
				}
			}
			else
			{
				float t2 = 0f;
				while (t2 < fadeTime)
				{
					disc.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t2 / fadeTime);
					t2 += CupheadTime.Delta;
					yield return null;
				}
			}
			fadingOut = !fadingOut;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00060BD4 File Offset: 0x0005EFD4
	private IEnumerator dicepalacepachinkoPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00060BF0 File Offset: 0x0005EFF0
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalacePachinko.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400066A RID: 1642
	private LevelProperties.DicePalacePachinko properties;

	// Token: 0x0400066B RID: 1643
	[SerializeField]
	private Transform[] starDiscs;

	// Token: 0x0400066C RID: 1644
	[SerializeField]
	private DicePalacePachinkoLevelPipes pipes;

	// Token: 0x0400066D RID: 1645
	[SerializeField]
	private DicePalacePachinkoLevelPachinko pachinko;

	// Token: 0x0400066E RID: 1646
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x0400066F RID: 1647
	[SerializeField]
	private string _bossQuote;
}
