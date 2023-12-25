using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000B31 RID: 2865
public class TowerOfPowerScorecard : AbstractMonoBehaviour
{
	// Token: 0x06004568 RID: 17768 RVA: 0x002481E8 File Offset: 0x002465E8
	protected override void Awake()
	{
		base.Awake();
		if (Localization.language != Localization.Languages.English)
		{
			this.DisableEnglishMDHRSubtitles();
		}
		if (Localization.language == Localization.Languages.Japanese)
		{
			this.CenterResultTitles(this.japaneseTitleRoot);
		}
		else if (Localization.language == Localization.Languages.Korean)
		{
			this.CenterResultTitles(this.koreanTitleRoot);
		}
		else if (Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.German || Localization.language == Localization.Languages.SpanishSpain || Localization.language == Localization.Languages.SpanishAmerica || Localization.language == Localization.Languages.Russian || Localization.language == Localization.Languages.PortugueseBrazil)
		{
			this.CenterResultTitles(this.chineseTitleRoot);
		}
		Cuphead.Init(false);
		if (PlayerManager.Multiplayer)
		{
			if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French)
			{
				this.CenterResultTitles(this.japaneseTitleRoot);
			}
		}
		else if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French || Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.Japanese)
		{
			this.CenterResultTitles(this.playerOneOffCenterTitleRoot);
		}
		base.StartCoroutine(this.main_cr());
		this.continuePrompt.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
	}

	// Token: 0x06004569 RID: 17769 RVA: 0x00248340 File Offset: 0x00246740
	private void DisableEnglishMDHRSubtitles()
	{
		foreach (SpriteRenderer spriteRenderer in this.studioMHDRSubtitles)
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x0600456A RID: 17770 RVA: 0x00248374 File Offset: 0x00246774
	private void CenterResultTitles(Vector3 rootPosition)
	{
		foreach (Transform transform in this.resultsTitles)
		{
			transform.localPosition = rootPosition;
		}
	}

	// Token: 0x0600456B RID: 17771 RVA: 0x002483A8 File Offset: 0x002467A8
	private IEnumerator main_cr()
	{
		LevelScoringData data = Level.ScoringData;
		this.done = false;
		if (Localization.language == Localization.Languages.Korean)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.scoring.GetComponentsInChildren<TextMeshProUGUI>())
			{
				textMeshProUGUI.fontStyle = FontStyles.Bold;
			}
			this.gradeLabel.fontStyle = FontStyles.Bold;
		}
		if (!Level.IsTowerOfPowerMain && data.difficulty == Level.Mode.Easy && Level.PreviousDifficulty == Level.Mode.Easy && Level.PreviousLevelType == Level.Type.Battle && !Level.IsDicePalace && !Level.IsDicePalaceMain && Level.PreviousLevel != Levels.Devil)
		{
			Localization.Translation translation = Localization.Translate("ResultsTryRegular");
			this.tryRegular.SetActive(true);
			if (translation.image == null && !translation.hasSpriteAtlasImage)
			{
				this.tryRegular.GetComponent<SpriteRenderer>().enabled = false;
				this.tryRegularEnglishBackground.enabled = false;
				this.tryRegularText.text = translation.text;
				this.tryRegularText.font = translation.fonts.fontAsset;
				this.tryRegularText.fontSize = ((translation.fonts.fontAssetSize != 0f) ? translation.fonts.fontAssetSize : this.tryRegularText.fontSize);
				this.tryRegularText.outlineWidth = ((Localization.language != Localization.Languages.Korean) ? this.tryRegularText.outlineWidth : 0.07f);
				this.AlignBannerText(this.tryRegularText.gameObject);
				if (Localization.language == Localization.Languages.Korean || Localization.language == Localization.Languages.Japanese)
				{
					this.postProcessingScript.profile = this.asianProfile;
				}
				this.AlignBannerText(this.glowingText);
				this.glowScript.InitTMPText(new MaskableGraphic[]
				{
					this.tryRegularText
				});
				if (Localization.language != Localization.Languages.English)
				{
					this.glowScript.BeginGlow();
				}
			}
			else
			{
				this.tryRegularText.enabled = false;
			}
		}
		this.timeTicker.TargetValue = (int)data.time;
		this.timeTicker.MaxValue = (int)data.goalTime;
		this.hitsTicker.TargetValue = ((data.numTimesHit >= 3) ? 0 : (3 - data.numTimesHit));
		this.hitsTicker.MaxValue = 3;
		this.parriesTicker.TargetValue = Mathf.Min(data.numParries, (int)Cuphead.Current.ScoringProperties.parriesForHighestGrade);
		this.parriesTicker.MaxValue = (int)Cuphead.Current.ScoringProperties.parriesForHighestGrade;
		this.superMeterTicker.TargetValue = Mathf.Min(data.superMeterUsed, (int)Cuphead.Current.ScoringProperties.superMeterUsageForHighestGrade);
		this.superMeterTicker.MaxValue = (int)Cuphead.Current.ScoringProperties.superMeterUsageForHighestGrade;
		if (data.useCoinsInsteadOfSuperMeter)
		{
			this.superMeterTicker.TargetValue = data.coinsCollected;
			this.superMeterTicker.MaxValue = 5;
			this.spiritStockLabelLocalizationHelper.currentID = Localization.Find("ResultsMenuCoins").id;
		}
		this.difficultyTicker.TargetValue = ((data.difficulty != Level.Mode.Easy) ? ((data.difficulty != Level.Mode.Normal) ? 2 : 1) : 0);
		this.gradeDisplay.Grade = Level.Grade;
		this.gradeDisplay.Difficulty = data.difficulty;
		yield return new WaitForSeconds(this.introDelay);
		WinScreenTicker[] tickers = new WinScreenTicker[]
		{
			this.timeTicker,
			this.hitsTicker,
			this.parriesTicker,
			this.superMeterTicker,
			this.difficultyTicker
		};
		foreach (WinScreenTicker ticker in tickers)
		{
			ticker.StartCounting();
			while (!ticker.FinishedCounting)
			{
				yield return null;
			}
			if (ticker.TargetValue != 0)
			{
				yield return new WaitForSeconds(this.talliesDelay);
			}
		}
		InterruptingPrompt.SetCanInterrupt(true);
		float timer = 0f;
		while (timer < this.gradeDelay)
		{
			if (this.input.GetAnyButtonDown())
			{
				break;
			}
			if (!InterruptingPrompt.IsInterrupting())
			{
				timer += Time.deltaTime;
			}
			yield return null;
		}
		this.gradeDisplay.Show();
		while (!this.gradeDisplay.FinishedGrading)
		{
			yield return null;
		}
		timer = 0f;
		this.continuePrompt.SetActive(true);
		while (timer < this.advanceDelay)
		{
			if (this.input.GetActionButtonDown())
			{
				break;
			}
			if (!InterruptingPrompt.IsInterrupting())
			{
				timer += Time.deltaTime;
			}
			yield return null;
		}
		this.done = true;
		yield break;
	}

	// Token: 0x0600456C RID: 17772 RVA: 0x002483C4 File Offset: 0x002467C4
	private void AlignBannerText(GameObject bannerText)
	{
		bannerText.GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)TowerOfPowerScorecard.TryRegularCurveValues[(int)Localization.language];
		Vector3 localPosition = bannerText.transform.localPosition;
		localPosition.y = (float)TowerOfPowerScorecard.TryRegularCurveOffsets[(int)Localization.language];
		bannerText.transform.localPosition = localPosition;
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x00248414 File Offset: 0x00246814
	private string GetTriggerName(Localization.Languages language)
	{
		switch (language)
		{
		case Localization.Languages.French:
			return "useFrench";
		case Localization.Languages.Italian:
			return "useItalian";
		case Localization.Languages.German:
			return "useGerman";
		case Localization.Languages.SpanishSpain:
			return "useSpanishSpain";
		case Localization.Languages.SpanishAmerica:
			return "useSpanishAmerica";
		case Localization.Languages.Korean:
			return "useKorean";
		case Localization.Languages.Russian:
			return "useRussian";
		case Localization.Languages.Polish:
			return "usePolish";
		case Localization.Languages.PortugueseBrazil:
			return "usePortuguese";
		case Localization.Languages.Japanese:
			return "useJapanese";
		case Localization.Languages.SimplifiedChinese:
			return "useChinese";
		default:
			return "useEnglish";
		}
	}

	// Token: 0x04004B21 RID: 19233
	private static readonly int[] TryRegularCurveValues = new int[]
	{
		0,
		62,
		48,
		57,
		65,
		74,
		58,
		36,
		54,
		72,
		83,
		27
	};

	// Token: 0x04004B22 RID: 19234
	private static readonly int[] TryRegularCurveOffsets = new int[]
	{
		0,
		32,
		18,
		27,
		35,
		45,
		28,
		7,
		24,
		42,
		50,
		-3
	};

	// Token: 0x04004B23 RID: 19235
	private const float BOB_FRAME_TIME = 0.041666668f;

	// Token: 0x04004B24 RID: 19236
	[Header("Delays")]
	[SerializeField]
	private float introDelay = 10f;

	// Token: 0x04004B25 RID: 19237
	[SerializeField]
	private float talliesDelay = 0.5f;

	// Token: 0x04004B26 RID: 19238
	[SerializeField]
	private float gradeDelay = 0.7f;

	// Token: 0x04004B27 RID: 19239
	[SerializeField]
	private float advanceDelay = 10f;

	// Token: 0x04004B28 RID: 19240
	[SerializeField]
	private WinScreenTicker timeTicker;

	// Token: 0x04004B29 RID: 19241
	[SerializeField]
	private WinScreenTicker hitsTicker;

	// Token: 0x04004B2A RID: 19242
	[SerializeField]
	private WinScreenTicker parriesTicker;

	// Token: 0x04004B2B RID: 19243
	[SerializeField]
	private WinScreenTicker superMeterTicker;

	// Token: 0x04004B2C RID: 19244
	[SerializeField]
	private WinScreenTicker difficultyTicker;

	// Token: 0x04004B2D RID: 19245
	[SerializeField]
	private LocalizationHelper spiritStockLabelLocalizationHelper;

	// Token: 0x04004B2E RID: 19246
	[SerializeField]
	private WinScreenGradeDisplay gradeDisplay;

	// Token: 0x04004B2F RID: 19247
	[SerializeField]
	private GameObject continuePrompt;

	// Token: 0x04004B30 RID: 19248
	[Header("UI Scoring")]
	[SerializeField]
	private GameObject scoring;

	// Token: 0x04004B31 RID: 19249
	[SerializeField]
	private TextMeshProUGUI gradeLabel;

	// Token: 0x04004B32 RID: 19250
	[Header("Try Text")]
	[SerializeField]
	private GameObject tryRegular;

	// Token: 0x04004B33 RID: 19251
	[SerializeField]
	private TMP_Text tryRegularText;

	// Token: 0x04004B34 RID: 19252
	[SerializeField]
	private SpriteRenderer tryRegularEnglishBackground;

	// Token: 0x04004B35 RID: 19253
	[Header("Glow effect")]
	[SerializeField]
	private GameObject glowingText;

	// Token: 0x04004B36 RID: 19254
	[SerializeField]
	private GlowText glowScript;

	// Token: 0x04004B37 RID: 19255
	[SerializeField]
	private PostProcessingBehaviour postProcessingScript;

	// Token: 0x04004B38 RID: 19256
	[SerializeField]
	public PostProcessingProfile asianProfile;

	// Token: 0x04004B39 RID: 19257
	[SerializeField]
	private GameObject tryExpert;

	// Token: 0x04004B3A RID: 19258
	[SerializeField]
	private TMP_Text tryExpertText;

	// Token: 0x04004B3B RID: 19259
	[Space(10f)]
	[SerializeField]
	private SpriteRenderer[] studioMHDRSubtitles;

	// Token: 0x04004B3C RID: 19260
	[SerializeField]
	private Transform[] resultsTitles;

	// Token: 0x04004B3D RID: 19261
	[SerializeField]
	private Vector3 playerOneOffCenterTitleRoot;

	// Token: 0x04004B3E RID: 19262
	[SerializeField]
	private Vector3 japaneseTitleRoot;

	// Token: 0x04004B3F RID: 19263
	[SerializeField]
	private Vector3 koreanTitleRoot;

	// Token: 0x04004B40 RID: 19264
	[SerializeField]
	private Vector3 chineseTitleRoot;

	// Token: 0x04004B41 RID: 19265
	[Space(10f)]
	[SerializeField]
	private Canvas results;

	// Token: 0x04004B42 RID: 19266
	[Header("BannerCurve")]
	[SerializeField]
	private MinMax textWidthRange;

	// Token: 0x04004B43 RID: 19267
	[SerializeField]
	private MinMax curveScaleRange;

	// Token: 0x04004B44 RID: 19268
	[SerializeField]
	private float yOffsetDelta;

	// Token: 0x04004B45 RID: 19269
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04004B46 RID: 19270
	private const float BG_NORMAL_SPEED = 50f;

	// Token: 0x04004B47 RID: 19271
	private const float BG_FAST_SPEED = 150f;

	// Token: 0x04004B48 RID: 19272
	public bool done;
}
