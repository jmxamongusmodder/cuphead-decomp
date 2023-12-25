using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B34 RID: 2868
public class WinScreenGradeDisplay : AbstractMonoBehaviour
{
	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x0600457C RID: 17788 RVA: 0x00249E1A File Offset: 0x0024821A
	// (set) Token: 0x0600457D RID: 17789 RVA: 0x00249E22 File Offset: 0x00248222
	public LevelScoringData.Grade Grade { get; set; }

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x0600457E RID: 17790 RVA: 0x00249E2B File Offset: 0x0024822B
	// (set) Token: 0x0600457F RID: 17791 RVA: 0x00249E33 File Offset: 0x00248233
	public Level.Mode Difficulty { get; set; }

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06004580 RID: 17792 RVA: 0x00249E3C File Offset: 0x0024823C
	// (set) Token: 0x06004581 RID: 17793 RVA: 0x00249E44 File Offset: 0x00248244
	public bool Celebration { get; private set; }

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06004582 RID: 17794 RVA: 0x00249E4D File Offset: 0x0024824D
	// (set) Token: 0x06004583 RID: 17795 RVA: 0x00249E55 File Offset: 0x00248255
	public bool FinishedGrading { get; private set; }

	// Token: 0x06004584 RID: 17796 RVA: 0x00249E5E File Offset: 0x0024825E
	protected override void Awake()
	{
		base.Awake();
		this.input = new CupheadInput.AnyPlayerInput(false);
	}

	// Token: 0x06004585 RID: 17797 RVA: 0x00249E74 File Offset: 0x00248274
	private void Start()
	{
		this.Celebration = false;
		if (Level.PreviouslyWon)
		{
			this.topGradeLabel.fontStyle = ((Localization.language != Localization.Languages.Korean) ? this.topGradeLabel.fontStyle : FontStyles.Bold);
			this.topGradeValue.text = " " + this.grades[(int)Level.PreviousGrade];
		}
	}

	// Token: 0x06004586 RID: 17798 RVA: 0x00249EDA File Offset: 0x002482DA
	public void Show()
	{
		base.StartCoroutine(this.grade_tally_up_cr());
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x00249EEC File Offset: 0x002482EC
	private IEnumerator grade_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int counter = 0;
		this.text.text = this.grades[this.grades.Length - 1].Substring(0, 1) + " ";
		while (counter <= (int)this.Grade && isTallying)
		{
			if (counter >= (int)this.Grade)
			{
				break;
			}
			AudioManager.Play("win_score_tick");
			counter++;
			this.text.text = this.grades[counter].Substring(0, 1) + " ";
			while (t < 0.02f)
			{
				if (this.input.GetButtonDown(CupheadButton.Accept))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
		}
		AudioManager.Play("win_grade_chalk");
		this.circle.SetTrigger("Circle");
		this.text.GetComponent<Animator>().SetTrigger("MakeBig");
		this.text.text = this.grades[(int)this.Grade];
		if (counter == this.grades.Length - 1)
		{
			this.text.color = ColorUtils.HexToColor("FCC93D");
		}
		LevelScoringData.Grade PerfectGrade = (this.Difficulty != Level.Mode.Hard) ? LevelScoringData.Grade.APlus : LevelScoringData.Grade.S;
		bool english = Localization.language == Localization.Languages.English;
		if (!english)
		{
			this.AlignBannerText();
		}
		if (!Level.IsTowerOfPower)
		{
			if (this.Grade == PerfectGrade)
			{
				base.StartCoroutine(this.fade_text_cr());
				yield return CupheadTime.WaitForSeconds(this, 0.16f);
				this.gollyBanner.SetTrigger("OnBanner");
				this.Celebration = true;
				this.LanguageUpdate(english);
				this.gollyBannerEnglish.enabled = english;
				this.gollyBannerOther.enabled = !english;
				yield return this.gollyBanner.WaitForAnimationToEnd(this, "Golly", false, true);
			}
			else if (this.Grade > Level.PreviousGrade || !Level.PreviouslyWon)
			{
				base.StartCoroutine(this.fade_text_cr());
				yield return CupheadTime.WaitForSeconds(this, 0.16f);
				this.recordBanner.SetTrigger("OnBanner");
				this.Celebration = true;
				this.LanguageUpdate(english);
				this.recordBannerEnglish.enabled = english;
				this.recordBannerOther.enabled = !english;
				yield return this.recordBanner.WaitForAnimationToEnd(this, "Record", false, true);
			}
		}
		if (Level.IsTowerOfPower && this.Grade >= (LevelScoringData.Grade)TowerOfPowerLevelGameInfo.MIN_RANK_NEED_TO_GET_TOKEN)
		{
			TowerOfPowerLevelGameInfo.AddToken();
		}
		this.FinishedGrading = true;
		yield return null;
		yield break;
	}

	// Token: 0x06004588 RID: 17800 RVA: 0x00249F08 File Offset: 0x00248308
	private void AlignBannerText()
	{
		for (int i = 0; i < this.normalBannerTexts.Length; i++)
		{
			this.normalBannerTexts[i].GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)WinScreenGradeDisplay.NormalCurveValues[(int)Localization.language];
			Vector3 localPosition = this.normalBannerTexts[i].transform.localPosition;
			localPosition.y = (float)(-(float)WinScreenGradeDisplay.NormalCurveOffsets[(int)Localization.language]);
			if (i == this.normalBannerTexts.Length - 1)
			{
				localPosition.y += 2f;
			}
			this.normalBannerTexts[i].transform.localPosition = localPosition;
		}
		for (int j = 0; j < this.topScoreBannerTexts.Length; j++)
		{
			this.topScoreBannerTexts[j].GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)WinScreenGradeDisplay.GollyCurveValues[(int)Localization.language];
			Vector3 localPosition2 = this.topScoreBannerTexts[j].transform.localPosition;
			localPosition2.y = (float)(-(float)WinScreenGradeDisplay.GollyCurveOffsets[(int)Localization.language]);
			if (j == this.topScoreBannerTexts.Length - 1)
			{
				localPosition2.y -= 2f;
			}
			this.topScoreBannerTexts[j].transform.localPosition = localPosition2;
		}
	}

	// Token: 0x06004589 RID: 17801 RVA: 0x0024A040 File Offset: 0x00248440
	private void LanguageUpdate(bool english)
	{
		for (int i = 0; i < this.recordEnglish.Length; i++)
		{
			this.recordEnglish[i].SetActive(english);
		}
		for (int j = 0; j < this.gollyEnglish.Length; j++)
		{
			this.gollyEnglish[j].SetActive(english);
		}
		for (int k = 0; k < this.recordOther.Length; k++)
		{
			this.recordOther[k].SetActive(!english);
		}
		for (int l = 0; l < this.gollyOther.Length; l++)
		{
			this.gollyOther[l].SetActive(!english);
		}
	}

	// Token: 0x0600458A RID: 17802 RVA: 0x0024A0F0 File Offset: 0x002484F0
	private IEnumerator fade_text_cr()
	{
		float t = 0f;
		float fadeTime = 0.29f;
		Color topGradeLabelColor = this.topGradeLabel.color;
		Color topGradeValColor = this.topGradeValue.color;
		while (t < fadeTime)
		{
			t += CupheadTime.Delta;
			this.topGradeLabel.color = new Color(topGradeLabelColor.r, topGradeLabelColor.g, topGradeLabelColor.b, 1f - t / fadeTime);
			this.topGradeValue.color = new Color(topGradeValColor.r, topGradeValColor.g, topGradeValColor.b, 1f - t / fadeTime);
			if (this.tryExpert.gameObject.activeSelf)
			{
				foreach (SpriteRenderer spriteRenderer in this.tryExpert.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (RawImage rawImage in this.tryExpert.GetComponentsInChildren<RawImage>())
				{
					rawImage.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (TextMeshCurveAndJitter textMeshCurveAndJitter in this.tryExpert.GetComponentsInChildren<TextMeshCurveAndJitter>())
				{
					float value = Mathf.Clamp(255f - t / fadeTime * 255f, 0f, 255f);
					textMeshCurveAndJitter.AlphaValue = Convert.ToByte(value);
				}
			}
			if (this.tryRegular.gameObject.activeSelf)
			{
				foreach (SpriteRenderer spriteRenderer2 in this.tryRegular.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer2.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (RawImage rawImage2 in this.tryRegular.GetComponentsInChildren<RawImage>())
				{
					rawImage2.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (TextMeshCurveAndJitter textMeshCurveAndJitter2 in this.tryRegular.GetComponentsInChildren<TextMeshCurveAndJitter>())
				{
					float value2 = Mathf.Clamp(255f - t / fadeTime * 255f, 0f, 255f);
					textMeshCurveAndJitter2.AlphaValue = Convert.ToByte(value2);
				}
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04004B9B RID: 19355
	private static readonly int[] NormalCurveValues = new int[]
	{
		0,
		38,
		28,
		65,
		25,
		36,
		8,
		28,
		26,
		40,
		5,
		5
	};

	// Token: 0x04004B9C RID: 19356
	private static readonly int[] NormalCurveOffsets = new int[]
	{
		0,
		21,
		16,
		37,
		17,
		23,
		6,
		17,
		15,
		22,
		6,
		6
	};

	// Token: 0x04004B9D RID: 19357
	private static readonly int[] GollyCurveValues = new int[]
	{
		0,
		53,
		47,
		51,
		54,
		54,
		20,
		51,
		49,
		49,
		51,
		26
	};

	// Token: 0x04004B9E RID: 19358
	private static readonly int[] GollyCurveOffsets = new int[]
	{
		0,
		30,
		28,
		31,
		31,
		31,
		16,
		29,
		29,
		29,
		29,
		16
	};

	// Token: 0x04004B9F RID: 19359
	[SerializeField]
	private Text text;

	// Token: 0x04004BA0 RID: 19360
	[SerializeField]
	private TextMeshProUGUI topGradeLabel;

	// Token: 0x04004BA1 RID: 19361
	[SerializeField]
	private Text topGradeValue;

	// Token: 0x04004BA2 RID: 19362
	[SerializeField]
	private string[] grades;

	// Token: 0x04004BA3 RID: 19363
	[SerializeField]
	private Animator circle;

	// Token: 0x04004BA4 RID: 19364
	[SerializeField]
	private Animator recordBanner;

	// Token: 0x04004BA5 RID: 19365
	[SerializeField]
	private GameObject[] recordEnglish;

	// Token: 0x04004BA6 RID: 19366
	[SerializeField]
	private GameObject[] recordOther;

	// Token: 0x04004BA7 RID: 19367
	[SerializeField]
	private Image recordBannerEnglish;

	// Token: 0x04004BA8 RID: 19368
	[SerializeField]
	private Image recordBannerOther;

	// Token: 0x04004BA9 RID: 19369
	[SerializeField]
	private Animator gollyBanner;

	// Token: 0x04004BAA RID: 19370
	[SerializeField]
	private GameObject[] gollyEnglish;

	// Token: 0x04004BAB RID: 19371
	[SerializeField]
	private GameObject[] gollyOther;

	// Token: 0x04004BAC RID: 19372
	[SerializeField]
	private Image gollyBannerEnglish;

	// Token: 0x04004BAD RID: 19373
	[SerializeField]
	private Image gollyBannerOther;

	// Token: 0x04004BAE RID: 19374
	[SerializeField]
	private SpriteRenderer tryRegular;

	// Token: 0x04004BAF RID: 19375
	[SerializeField]
	private SpriteRenderer tryExpert;

	// Token: 0x04004BB0 RID: 19376
	[SerializeField]
	private GameObject[] normalBannerTexts;

	// Token: 0x04004BB1 RID: 19377
	[SerializeField]
	private GameObject[] topScoreBannerTexts;

	// Token: 0x04004BB4 RID: 19380
	private const float COUNTER_TIME = 0.02f;

	// Token: 0x04004BB5 RID: 19381
	private const float BANNER_FLASH_Y_OFFSET = 2f;

	// Token: 0x04004BB8 RID: 19384
	private CupheadInput.AnyPlayerInput input;
}
