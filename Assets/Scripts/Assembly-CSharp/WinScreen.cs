using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000B32 RID: 2866
public class WinScreen : AbstractMonoBehaviour
{
	// Token: 0x06004570 RID: 17776 RVA: 0x00248C78 File Offset: 0x00247078
	protected override void Awake()
	{
		base.Awake();
		this.OnePlayerCuphead.SetActive(false);
		this.TwoPlayerCupheadMugman.SetActive(false);
		Cuphead.Init(false);
		LevelScoringData scoringData = Level.ScoringData;
		if (scoringData != null)
		{
			this.player1IsChalice = scoringData.player1IsChalice;
			this.player2IsChalice = scoringData.player2IsChalice;
		}
		if (!PlayerManager.Multiplayer)
		{
			this.player2IsChalice = false;
		}
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
		if (PlayerManager.Multiplayer)
		{
			Animator animator;
			if (PlayerManager.player1IsMugman)
			{
				if (this.player1IsChalice)
				{
					animator = this.TwoPlayerTitleChaliceCuphead;
					this.TwoPlayerChaliceCuphead.SetActive(true);
					this.results.transform.position = this.TwoPlayerChaliceCupheadUIRoot.transform.position;
				}
				else if (this.player2IsChalice)
				{
					animator = this.TwoPlayerTitleMugmanChalice;
					this.TwoPlayerMugmanChalice.SetActive(true);
					this.results.transform.position = this.TwoPlayerMugmanChaliceUIRoot.transform.position;
				}
				else
				{
					animator = this.TwoPlayerTitleMugman;
					this.TwoPlayerMugmanCuphead.SetActive(true);
					this.results.transform.position = this.TwoPlayerMugmanCupheadUIRoot.transform.position;
				}
			}
			else if (this.player1IsChalice)
			{
				animator = this.TwoPlayerTitleChaliceMugman;
				this.TwoPlayerChaliceMugman.SetActive(true);
				this.results.transform.position = this.TwoPlayerChaliceMugmanUIRoot.transform.position;
			}
			else if (this.player2IsChalice)
			{
				animator = this.TwoPlayerTitleCupheadChalice;
				this.TwoPlayerCupheadChalice.SetActive(true);
				this.results.transform.position = this.TwoPlayerCupheadChaliceUIRoot.transform.position;
			}
			else
			{
				animator = this.TwoPlayerTitleCuphead;
				this.TwoPlayerCupheadMugman.SetActive(true);
				this.results.transform.position = this.TwoPlayerCupheadMugmanUIRoot.transform.position;
			}
			if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French)
			{
				this.CenterResultTitles(this.japaneseTitleRoot);
			}
			if (Localization.language == Localization.Languages.English)
			{
				animator.SetBool("pickedA", Rand.Bool());
			}
			animator.SetTrigger(this.GetTriggerName(Localization.language));
		}
		else
		{
			Animator animator;
			if (this.player1IsChalice)
			{
				animator = this.OnePlayerTitleChalice;
				this.OnePlayerChalice.SetActive(true);
			}
			else if (PlayerManager.player1IsMugman)
			{
				animator = this.OnePlayerTitleMugman;
				this.OnePlayerMugman.SetActive(true);
			}
			else
			{
				animator = this.OnePlayerTitleCuphead;
				this.OnePlayerCuphead.SetActive(true);
			}
			this.results.transform.position = this.OnePlayerUIRoot.transform.position;
			if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French || Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.Japanese)
			{
				this.CenterResultTitles(this.playerOneOffCenterTitleRoot);
			}
			if (Localization.language == Localization.Languages.English)
			{
				animator.SetBool("pickedA", Rand.Bool());
			}
			animator.SetTrigger(this.GetTriggerName(Localization.language));
		}
		base.StartCoroutine(this.main_cr());
		this.continuePrompt.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		base.StartCoroutine(this.rotate_bg_cr());
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x0024907C File Offset: 0x0024747C
	private void DisableEnglishMDHRSubtitles()
	{
		foreach (SpriteRenderer spriteRenderer in this.studioMHDRSubtitles)
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x06004572 RID: 17778 RVA: 0x002490B0 File Offset: 0x002474B0
	private void CenterResultTitles(Vector3 rootPosition)
	{
		if (this.player1IsChalice)
		{
			rootPosition += ((!PlayerManager.Multiplayer) ? this.chaliceTitleOffset1P : this.chaliceTitleOffset2P);
		}
		foreach (Transform transform in this.resultsTitles)
		{
			transform.localPosition = rootPosition;
		}
	}

	// Token: 0x06004573 RID: 17779 RVA: 0x00249114 File Offset: 0x00247514
	private IEnumerator main_cr()
	{
		LevelScoringData data = Level.ScoringData;
		if (Localization.language == Localization.Languages.Korean)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.scoring.GetComponentsInChildren<TextMeshProUGUI>())
			{
				textMeshProUGUI.fontStyle = FontStyles.Bold;
			}
			this.gradeLabel.fontStyle = FontStyles.Bold;
		}
		if (data.difficulty == Level.Mode.Easy && Level.PreviousDifficulty == Level.Mode.Easy && Level.PreviousLevelType == Level.Type.Battle && !Level.IsDicePalace && !Level.IsDicePalaceMain && Level.PreviousLevel != Levels.Devil && Level.PreviousLevel != Levels.Saltbaker)
		{
			if (Array.IndexOf<Levels>(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0)
			{
				this.isDLCLevel = true;
			}
			else
			{
				this.isDLCLevel = false;
			}
			Localization.Translation translation = (!this.isDLCLevel) ? Localization.Translate("ResultsTryRegular") : Localization.Translate("WinScreen_Tooltip_SimpleIngredient");
			this.tryRegular.SetActive(true);
			if ((translation.image == null && !translation.hasSpriteAtlasImage) || this.isDLCLevel)
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
				if (Localization.language != Localization.Languages.English || this.isDLCLevel)
				{
					this.glowScript.BeginGlow();
				}
			}
			else
			{
				this.tryRegularText.enabled = false;
			}
		}
		if (data == null)
		{
			yield break;
		}
		this.timeTicker.TargetValue = (int)data.time;
		this.timeTicker.MaxValue = (int)data.goalTime;
		this.hitsTicker.TargetValue = Mathf.Clamp(data.finalHP, 0, 3);
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
		if (Level.PreviousLevel == Levels.Devil)
		{
			Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_outro, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		}
		else if (Level.PreviousLevel == Levels.Saltbaker)
		{
			Cutscene.Load(Scenes.scene_map_world_DLC, Scenes.scene_cutscene_dlc_ending, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		}
		else
		{
			SceneLoader.LoadLastMap();
		}
		yield break;
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x00249130 File Offset: 0x00247530
	private void AlignBannerText(GameObject bannerText)
	{
		bannerText.GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)((!this.isDLCLevel) ? WinScreen.TryRegularCurveValues[(int)Localization.language] : WinScreen.TryRegularCurveValuesDLC[(int)Localization.language]);
		Vector3 localPosition = bannerText.transform.localPosition;
		localPosition.y = (float)((!this.isDLCLevel) ? WinScreen.TryRegularCurveOffsets[(int)Localization.language] : WinScreen.TryRegularCurveOffsetsDLC[(int)Localization.language]);
		bannerText.transform.localPosition = localPosition;
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x002491B8 File Offset: 0x002475B8
	private IEnumerator rotate_bg_cr()
	{
		float frameTime = 0f;
		float normalTime = 0f;
		float speed = 50f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			while (frameTime > 0.041666668f)
			{
				frameTime -= 0.041666668f;
				this.Background.Rotate(0f, 0f, speed * CupheadTime.Delta);
				yield return null;
			}
			if (this.gradeDisplay.Celebration && speed < 150f)
			{
				normalTime += CupheadTime.Delta;
				speed = Mathf.Lerp(50f, 150f, normalTime / 0.5f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x002491D4 File Offset: 0x002475D4
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

	// Token: 0x04004B49 RID: 19273
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

	// Token: 0x04004B4A RID: 19274
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

	// Token: 0x04004B4B RID: 19275
	private static readonly int[] TryRegularCurveValuesDLC = new int[]
	{
		62,
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

	// Token: 0x04004B4C RID: 19276
	private static readonly int[] TryRegularCurveOffsetsDLC = new int[]
	{
		32,
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

	// Token: 0x04004B4D RID: 19277
	private const float BOB_FRAME_TIME = 0.041666668f;

	// Token: 0x04004B4E RID: 19278
	[Header("Delays")]
	[SerializeField]
	private float introDelay = 10f;

	// Token: 0x04004B4F RID: 19279
	[SerializeField]
	private float talliesDelay = 0.5f;

	// Token: 0x04004B50 RID: 19280
	[SerializeField]
	private float gradeDelay = 0.7f;

	// Token: 0x04004B51 RID: 19281
	[SerializeField]
	private float advanceDelay = 10f;

	// Token: 0x04004B52 RID: 19282
	[SerializeField]
	private WinScreenTicker timeTicker;

	// Token: 0x04004B53 RID: 19283
	[SerializeField]
	private WinScreenTicker hitsTicker;

	// Token: 0x04004B54 RID: 19284
	[SerializeField]
	private WinScreenTicker parriesTicker;

	// Token: 0x04004B55 RID: 19285
	[SerializeField]
	private WinScreenTicker superMeterTicker;

	// Token: 0x04004B56 RID: 19286
	[SerializeField]
	private WinScreenTicker difficultyTicker;

	// Token: 0x04004B57 RID: 19287
	[SerializeField]
	private LocalizationHelper spiritStockLabelLocalizationHelper;

	// Token: 0x04004B58 RID: 19288
	[SerializeField]
	private WinScreenGradeDisplay gradeDisplay;

	// Token: 0x04004B59 RID: 19289
	[SerializeField]
	private GameObject continuePrompt;

	// Token: 0x04004B5A RID: 19290
	private bool player1IsChalice;

	// Token: 0x04004B5B RID: 19291
	private bool player2IsChalice;

	// Token: 0x04004B5C RID: 19292
	[Header("UI Scoring")]
	[SerializeField]
	private GameObject scoring;

	// Token: 0x04004B5D RID: 19293
	[SerializeField]
	private TextMeshProUGUI gradeLabel;

	// Token: 0x04004B5E RID: 19294
	[Header("Try Text")]
	[SerializeField]
	private GameObject tryRegular;

	// Token: 0x04004B5F RID: 19295
	[SerializeField]
	private TMP_Text tryRegularText;

	// Token: 0x04004B60 RID: 19296
	[SerializeField]
	private SpriteRenderer tryRegularEnglishBackground;

	// Token: 0x04004B61 RID: 19297
	[Header("Glow effect")]
	[SerializeField]
	private GameObject glowingText;

	// Token: 0x04004B62 RID: 19298
	[SerializeField]
	private GlowText glowScript;

	// Token: 0x04004B63 RID: 19299
	[SerializeField]
	private PostProcessingBehaviour postProcessingScript;

	// Token: 0x04004B64 RID: 19300
	[SerializeField]
	public PostProcessingProfile asianProfile;

	// Token: 0x04004B65 RID: 19301
	[SerializeField]
	private GameObject tryExpert;

	// Token: 0x04004B66 RID: 19302
	[SerializeField]
	private TMP_Text tryExpertText;

	// Token: 0x04004B67 RID: 19303
	[Header("Background")]
	[SerializeField]
	private Transform Background;

	// Token: 0x04004B68 RID: 19304
	[Header("DifferentLayouts")]
	[SerializeField]
	private GameObject OnePlayerCuphead;

	// Token: 0x04004B69 RID: 19305
	[SerializeField]
	private GameObject OnePlayerMugman;

	// Token: 0x04004B6A RID: 19306
	[SerializeField]
	private Transform OnePlayerUIRoot;

	// Token: 0x04004B6B RID: 19307
	[SerializeField]
	private Animator OnePlayerTitleCuphead;

	// Token: 0x04004B6C RID: 19308
	[SerializeField]
	private Animator OnePlayerTitleMugman;

	// Token: 0x04004B6D RID: 19309
	[Space(10f)]
	[SerializeField]
	private GameObject TwoPlayerCupheadMugman;

	// Token: 0x04004B6E RID: 19310
	[SerializeField]
	private GameObject TwoPlayerMugmanCuphead;

	// Token: 0x04004B6F RID: 19311
	[SerializeField]
	private Transform TwoPlayerCupheadMugmanUIRoot;

	// Token: 0x04004B70 RID: 19312
	[SerializeField]
	private Transform TwoPlayerMugmanCupheadUIRoot;

	// Token: 0x04004B71 RID: 19313
	[SerializeField]
	private Animator TwoPlayerTitleCuphead;

	// Token: 0x04004B72 RID: 19314
	[SerializeField]
	private Animator TwoPlayerTitleMugman;

	// Token: 0x04004B73 RID: 19315
	[Space(10f)]
	[SerializeField]
	private GameObject OnePlayerChalice;

	// Token: 0x04004B74 RID: 19316
	[SerializeField]
	private GameObject TwoPlayerChaliceCuphead;

	// Token: 0x04004B75 RID: 19317
	[SerializeField]
	private GameObject TwoPlayerCupheadChalice;

	// Token: 0x04004B76 RID: 19318
	[SerializeField]
	private GameObject TwoPlayerMugmanChalice;

	// Token: 0x04004B77 RID: 19319
	[SerializeField]
	private GameObject TwoPlayerChaliceMugman;

	// Token: 0x04004B78 RID: 19320
	[SerializeField]
	private Transform OnePlayerChaliceUIRoot;

	// Token: 0x04004B79 RID: 19321
	[SerializeField]
	private Transform TwoPlayerChaliceCupheadUIRoot;

	// Token: 0x04004B7A RID: 19322
	[SerializeField]
	private Transform TwoPlayerCupheadChaliceUIRoot;

	// Token: 0x04004B7B RID: 19323
	[SerializeField]
	private Transform TwoPlayerMugmanChaliceUIRoot;

	// Token: 0x04004B7C RID: 19324
	[SerializeField]
	private Transform TwoPlayerChaliceMugmanUIRoot;

	// Token: 0x04004B7D RID: 19325
	[SerializeField]
	private Animator OnePlayerTitleChalice;

	// Token: 0x04004B7E RID: 19326
	[SerializeField]
	private Animator TwoPlayerTitleChaliceCuphead;

	// Token: 0x04004B7F RID: 19327
	[SerializeField]
	private Animator TwoPlayerTitleCupheadChalice;

	// Token: 0x04004B80 RID: 19328
	[SerializeField]
	private Animator TwoPlayerTitleMugmanChalice;

	// Token: 0x04004B81 RID: 19329
	[SerializeField]
	private Animator TwoPlayerTitleChaliceMugman;

	// Token: 0x04004B82 RID: 19330
	[Space(10f)]
	[SerializeField]
	private SpriteRenderer[] studioMHDRSubtitles;

	// Token: 0x04004B83 RID: 19331
	[SerializeField]
	private Transform[] resultsTitles;

	// Token: 0x04004B84 RID: 19332
	[SerializeField]
	private Vector3 playerOneOffCenterTitleRoot;

	// Token: 0x04004B85 RID: 19333
	[SerializeField]
	private Vector3 japaneseTitleRoot;

	// Token: 0x04004B86 RID: 19334
	[SerializeField]
	private Vector3 koreanTitleRoot;

	// Token: 0x04004B87 RID: 19335
	[SerializeField]
	private Vector3 chineseTitleRoot;

	// Token: 0x04004B88 RID: 19336
	[Space(10f)]
	[SerializeField]
	private Vector3 chaliceTitleOffset1P;

	// Token: 0x04004B89 RID: 19337
	[SerializeField]
	private Vector3 chaliceTitleOffset2P;

	// Token: 0x04004B8A RID: 19338
	[Space(10f)]
	[SerializeField]
	private Canvas results;

	// Token: 0x04004B8B RID: 19339
	[Header("BannerCurve")]
	[SerializeField]
	private MinMax textWidthRange;

	// Token: 0x04004B8C RID: 19340
	[SerializeField]
	private MinMax curveScaleRange;

	// Token: 0x04004B8D RID: 19341
	[SerializeField]
	private float yOffsetDelta;

	// Token: 0x04004B8E RID: 19342
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04004B8F RID: 19343
	private const float BG_NORMAL_SPEED = 50f;

	// Token: 0x04004B90 RID: 19344
	private const float BG_FAST_SPEED = 150f;

	// Token: 0x04004B91 RID: 19345
	private bool isDLCLevel;
}
