using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099B RID: 2459
public class MapDifficultySelectStartUI : AbstractMapSceneStartUI
{
	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06003988 RID: 14728 RVA: 0x0020A3C9 File Offset: 0x002087C9
	// (set) Token: 0x06003989 RID: 14729 RVA: 0x0020A3D0 File Offset: 0x002087D0
	public static MapDifficultySelectStartUI Current { get; protected set; }

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x0600398A RID: 14730 RVA: 0x0020A3D8 File Offset: 0x002087D8
	// (set) Token: 0x0600398B RID: 14731 RVA: 0x0020A3DF File Offset: 0x002087DF
	public static Level.Mode Mode { get; private set; }

	// Token: 0x0600398C RID: 14732 RVA: 0x0020A3E8 File Offset: 0x002087E8
	protected override void Awake()
	{
		base.Awake();
		MapDifficultySelectStartUI.Current = this;
		switch (Level.CurrentMode)
		{
		case Level.Mode.Easy:
			this.index = 0;
			break;
		case Level.Mode.Normal:
			this.index = 1;
			break;
		case Level.Mode.Hard:
			this.index = 2;
			break;
		}
		this.options = new Level.Mode[]
		{
			Level.Mode.Easy,
			Level.Mode.Normal,
			Level.Mode.Hard
		};
		this.SetDifficultyAvailability();
		this.difficulyTexts = new TMP_Text[3];
		this.difficulyTexts[0] = this.easy.GetComponent<TMP_Text>();
		this.difficulyTexts[1] = this.normal.GetComponent<TMP_Text>();
		this.difficulyTexts[2] = this.hard.GetComponent<TMP_Text>();
		if (this.bossImage != null && this.bossImage.textComponent != null)
		{
			this.initialMaxFontSize = this.bossImage.textComponent.resizeTextMaxSize;
		}
		this.initialinImagePosX = this.inAnimated.rectTransform.offsetMin;
		this.initialinImagePosY = this.inAnimated.rectTransform.offsetMax;
		this.initialinDifficultyPos = this.difficultyImage.rectTransform.anchoredPosition;
		this.initialDifficultyPos = this.difficultySelectionText.rectTransform.anchoredPosition;
		this.initialBossNamePos = this.bossNameImage.rectTransform.anchoredPosition;
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x0020A554 File Offset: 0x00208954
	private void SetDifficultyAvailability()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
		{
			if (!PlayerData.Data.IsHardModeAvailable)
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Normal
				};
				this.hard.gameObject.SetActive(false);
				this.hardSeparator.gameObject.SetActive(false);
			}
			else
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Normal,
					Level.Mode.Hard
				};
			}
			this.index = Mathf.Max(0, this.index - 1);
			this.easy.gameObject.SetActive(false);
			this.normalSeparator.gameObject.SetActive(false);
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			if (this.level == "Saltbaker")
			{
				if (!PlayerData.Data.IsHardModeAvailableDLC)
				{
					this.options = new Level.Mode[]
					{
						Level.Mode.Normal
					};
				}
				else
				{
					this.options = new Level.Mode[]
					{
						Level.Mode.Normal,
						Level.Mode.Hard
					};
				}
			}
			else if (!PlayerData.Data.IsHardModeAvailableDLC)
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Easy,
					Level.Mode.Normal
				};
			}
			else
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Easy,
					Level.Mode.Normal,
					Level.Mode.Hard
				};
			}
			this.easy.gameObject.SetActive(this.level != "Saltbaker");
			this.normalSeparator.gameObject.SetActive(this.level != "Saltbaker");
			this.hard.gameObject.SetActive(PlayerData.Data.IsHardModeAvailableDLC);
			this.hardSeparator.gameObject.SetActive(PlayerData.Data.IsHardModeAvailableDLC);
		}
		else
		{
			if (!PlayerData.Data.IsHardModeAvailable)
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Easy,
					Level.Mode.Normal
				};
			}
			this.hard.gameObject.SetActive(PlayerData.Data.IsHardModeAvailable);
			this.hardSeparator.gameObject.SetActive(PlayerData.Data.IsHardModeAvailable);
		}
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x0020A770 File Offset: 0x00208B70
	public new void In(MapPlayerController playerController)
	{
		base.In(playerController);
		if (Level.CurrentMode == Level.Mode.Easy && PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
		{
			Level.SetCurrentMode(Level.Mode.Normal);
			switch (Level.CurrentMode)
			{
			case Level.Mode.Easy:
				this.index = 0;
				break;
			case Level.Mode.Normal:
				this.index = 1;
				break;
			case Level.Mode.Hard:
				this.index = 2;
				break;
			}
		}
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.SetDifficultyAvailability();
			if (this.level == "Saltbaker" && Level.CurrentMode == Level.Mode.Easy)
			{
				Level.SetCurrentMode(Level.Mode.Normal);
			}
		}
		if (base.animator != null)
		{
			base.animator.SetTrigger("ZoomIn");
			AudioManager.Play("world_map_level_menu_open");
		}
		this.InWordSetup();
		this.difficultyImage.enabled = (Localization.language == Localization.Languages.Japanese);
		this.difficultyImage.rectTransform.anchoredPosition = this.initialinDifficultyPos;
		for (int i = 0; i < this.separatorsAnimated.Length; i++)
		{
			this.separatorsAnimated[i].sprite = this.separatorsSprites[UnityEngine.Random.Range(0, this.separatorsSprites.Length)];
		}
		bool flag = Localization.language == Localization.Languages.Korean || Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.Japanese;
		this.bossTitleImage.enabled = (Localization.language == Localization.Languages.English || flag || PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC);
		this.glowScript.StopGlow();
		this.glowScript.DisableTMPText();
		this.glowScript.DisableImages();
		if (Localization.language == Localization.Languages.SimplifiedChinese)
		{
			this.difficultySelectionText.rectTransform.anchoredPosition = new Vector2(this.difficultySelectionText.rectTransform.anchoredPosition.x, -70f);
		}
		else
		{
			this.difficultySelectionText.rectTransform.anchoredPosition = this.initialDifficultyPos;
		}
		TranslationElement translationElement = Localization.Find(this.level + "Selection");
		if (this.bossImage != null && translationElement != null)
		{
			this.bossImage.ApplyTranslation(translationElement, null);
			if (this.bossImage.textComponent != null)
			{
				if (Localization.language == Localization.Languages.Korean)
				{
					this.bossImage.textComponent.resizeTextMaxSize = 100;
				}
				else
				{
					this.bossImage.textComponent.resizeTextMaxSize = this.initialMaxFontSize;
				}
			}
			if (flag)
			{
				this.SetupAsianBossCard(translationElement, this.bossTitleImage);
			}
			else
			{
				this.bossImage.transform.localScale = Vector3.one;
				this.bossImage.transform.localPosition = Vector3.zero;
				this.bossTitleImage.rectTransform.offsetMax = new Vector2(this.bossTitleImage.rectTransform.offsetMax.x, 0.5f);
				this.bossTitleImage.rectTransform.offsetMin = new Vector2(this.bossTitleImage.rectTransform.offsetMin.x, 0.5f);
				this.inAnimated.rectTransform.offsetMin = this.initialinImagePosX;
				this.inAnimated.rectTransform.offsetMax = this.initialinImagePosY;
				this.inText.fontStyle = FontStyle.Italic;
			}
		}
		TranslationElement translationElement2 = Localization.Find(this.level + "WorldMap");
		if (translationElement2 != null)
		{
			this.bossName.ApplyTranslation(translationElement2, null);
			if (this.bossName.textComponent != null && this.bossName.textComponent.enabled)
			{
				this.bossName.textComponent.font = FontLoader.GetFont(FontLoader.FontType.CupheadHenriette_A_merged);
			}
			this.bossNameImage.transform.localScale = Vector3.one;
			this.bossNameImage.rectTransform.anchoredPosition = this.initialBossNamePos;
			if (flag)
			{
				this.bossNameImage.material = this.bossCardWhiteMaterial;
				if (Localization.language == Localization.Languages.Korean || Localization.language == Localization.Languages.Japanese)
				{
					this.bossNameImage.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
					if (Localization.language == Localization.Languages.Japanese)
					{
						this.bossNameImage.rectTransform.anchoredPosition = new Vector2(0f, 214.2f);
					}
				}
			}
			this.bossName.gameObject.SetActive(Localization.language != Localization.Languages.English && !flag && PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC);
		}
		TranslationElement translationElement3 = Localization.Find(this.level + "Glow");
		if (Localization.language != Localization.Languages.English)
		{
			if (translationElement3 != null && flag)
			{
				this.bossGlow.ApplyTranslation(translationElement3, null);
			}
			else
			{
				this.glowScript.InitTMPText(new MaskableGraphic[]
				{
					this.bossImage.textMeshProComponent,
					this.bossName.textComponent
				});
				this.glowScript.BeginGlow();
			}
		}
		this.bossGlow.gameObject.SetActive(flag && PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC);
		for (int j = 0; j < this.difficulyTexts.Length; j++)
		{
			this.difficulyTexts[j].color = this.unselectedColor;
		}
		this.difficulyTexts[(int)Level.CurrentMode].color = this.selectedColor;
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x0020AD26 File Offset: 0x00209126
	private void OnDestroy()
	{
		this.bossNameImage.sprite = null;
		this.bossTitleImage.sprite = null;
		this.asianGlow.sprite = null;
		if (MapDifficultySelectStartUI.Current == this)
		{
			MapDifficultySelectStartUI.Current = null;
		}
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x0020AD62 File Offset: 0x00209162
	private void Update()
	{
		this.UpdateCursor();
		if (base.CurrentState == AbstractMapSceneStartUI.State.Active)
		{
			this.CheckInput();
		}
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x0020AD7C File Offset: 0x0020917C
	private void CheckInput()
	{
		if (!base.Able)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.MenuLeft))
		{
			this.Next(-1);
		}
		if (base.GetButtonDown(CupheadButton.MenuRight))
		{
			this.Next(1);
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			base.Out();
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.LoadLevel();
		}
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x0020ADE4 File Offset: 0x002091E4
	private void Next(int direction)
	{
		if ((this.index != this.options.Length - 1 && direction != -1) || (this.index != 0 && direction != 1))
		{
			AudioManager.Play("world_map_level_difficulty_hover");
		}
		this.index = Mathf.Clamp(this.index + direction, 0, this.options.Length - 1);
		Level.SetCurrentMode(this.options[this.index]);
		this.UpdateCursor();
		for (int i = 0; i < this.difficulyTexts.Length; i++)
		{
			this.difficulyTexts[i].color = this.unselectedColor;
		}
		this.difficulyTexts[(int)Level.CurrentMode].color = this.selectedColor;
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x0020AEA4 File Offset: 0x002092A4
	private void UpdateCursor()
	{
		Vector3 position = this.cursor.transform.position;
		position.y = this.normal.position.y;
		Level.Mode mode = Level.CurrentMode;
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4 && mode == Level.Mode.Easy)
		{
			mode = Level.Mode.Normal;
		}
		switch (mode)
		{
		case Level.Mode.Easy:
			position.x = this.easy.position.x;
			this.cursor.sizeDelta = new Vector2(this.easy.sizeDelta.x + 30f, this.easy.sizeDelta.y + 20f);
			break;
		case Level.Mode.Normal:
			position.x = this.normal.position.x;
			this.cursor.sizeDelta = new Vector2(this.normal.sizeDelta.x + 30f, this.normal.sizeDelta.y + 20f);
			break;
		case Level.Mode.Hard:
			position.x = this.hard.position.x;
			this.cursor.sizeDelta = new Vector2(this.hard.sizeDelta.x + 30f, this.hard.sizeDelta.y + 20f);
			break;
		}
		this.cursor.transform.position = position;
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x0020B054 File Offset: 0x00209454
	private void SetupAsianBossCard(TranslationElement translation, Image image)
	{
		image.material = this.bossCardWhiteMaterial;
		image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 0f);
		image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 0f);
		this.SetupAsianDifficulty();
		image.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
		if (Localization.language == Localization.Languages.Korean)
		{
			if (PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC)
			{
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 40f);
				image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 40f);
			}
			this.SetupKoreanInWord();
		}
		else if (Localization.language == Localization.Languages.SimplifiedChinese)
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -140f);
			if (this.level.Equals("FlyingBlimp"))
			{
				image.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 40f);
				image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 40f);
				this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -100f);
			}
		}
		else if (Localization.language == Localization.Languages.Japanese)
		{
			if (this.level.Equals("Flower") || this.level.Equals("FlyingBird") || this.level.Equals("Mouse") || this.level.Equals("SallyStagePlay"))
			{
				image.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -90f);
			}
			else if (this.level.Equals("Train"))
			{
				image.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 70f);
			}
			else if (this.level.Equals("Bee"))
			{
				image.transform.localScale = Vector3.one;
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -60f);
			}
			else if (this.level.Equals("DicePalaceMain"))
			{
				image.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -70f);
			}
			else
			{
				image.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 55f);
			}
			this.difficultyImage.rectTransform.anchoredPosition = new Vector2(0f, -70f);
			this.SetupJapaneseInWord();
		}
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x0020B488 File Offset: 0x00209888
	private void SetupAsianDifficulty()
	{
		this.difficultyText.textComponent.fontSize = 29;
		this.easy.gameObject.GetComponent<TMP_Text>().fontSize = 37f;
		this.normal.gameObject.GetComponent<TMP_Text>().fontSize = 37f;
		this.hard.gameObject.GetComponent<TMP_Text>().fontSize = 37f;
	}

	// Token: 0x06003996 RID: 14742 RVA: 0x0020B4F8 File Offset: 0x002098F8
	private void SetupJapaneseInWord()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.inAnimated.enabled = false;
			this.inText.enabled = false;
			return;
		}
		this.inAnimated.preserveAspect = true;
		if (this.level.Equals("Flower") || this.level.Equals("FlyingBird") || this.level.Equals("Mouse") || this.level.Equals("SallyStagePlay") || this.level.Equals("Bee") || this.level.Equals("DicePalaceMain"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, this.initialinImagePosY.y - 15.9f);
			this.inAnimated.rectTransform.offsetMin = new Vector2(this.inAnimated.rectTransform.offsetMin.x, this.initialinImagePosX.y - 15.9f);
		}
		else
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, this.initialinImagePosY.y + 6.5f);
			this.inAnimated.rectTransform.offsetMin = new Vector2(this.inAnimated.rectTransform.offsetMin.x, this.initialinImagePosX.y + 6.5f);
		}
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x0020B6B8 File Offset: 0x00209AB8
	private void SetupKoreanInWord()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.inAnimated.enabled = false;
			this.inText.enabled = false;
			return;
		}
		this.inText.fontStyle = FontStyle.Normal;
		if (this.level.Equals("Bird") || this.level.Equals("Dragon") || this.level.Equals("Devil"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -160f);
		}
		else if (this.level.Equals("Flower"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -140f);
		}
		else if (this.level.Equals("Bee"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -130f);
		}
		else if (this.level.Equals("KingDiceTop"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -155f);
		}
		else if (this.level.Equals("Frogs") || this.level.Equals("FlyingBlimp") || this.level.Equals("Baroness") || this.level.Equals("FlyingGenie") || this.level.Equals("Clown") || this.level.Equals("SallyStagePlay") || this.level.Equals("FlyingMermaid"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -110f);
		}
		else
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -150f);
		}
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x0020B954 File Offset: 0x00209D54
	private void InWordSetup()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.inAnimated.enabled = false;
			this.inText.enabled = false;
			return;
		}
		if (Localization.language == Localization.Languages.English)
		{
			this.inAnimated.sprite = this.inSprites[UnityEngine.Random.Range(0, this.inSprites.Length)];
		}
		this.inAnimated.enabled = (Localization.language != Localization.Languages.Korean && Localization.language != Localization.Languages.SimplifiedChinese && PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC);
		this.inAnimated.transform.localScale = Vector3.one;
		if (Localization.language == Localization.Languages.French)
		{
			this.inAnimated.transform.localScale = Vector3.one * 1.5f;
		}
		else if (Localization.language == Localization.Languages.PortugueseBrazil || Localization.language == Localization.Languages.SpanishSpain || Localization.language == Localization.Languages.SpanishAmerica)
		{
			this.inAnimated.transform.localScale = Vector3.one * 1.2f;
		}
		else if (Localization.language == Localization.Languages.Russian)
		{
			this.inAnimated.transform.localScale = Vector3.one * 2f;
		}
	}

	// Token: 0x0400412D RID: 16685
	private const int KoreanUpscaleSize = 100;

	// Token: 0x0400412E RID: 16686
	private const float AsianImageScale = 0.9f;

	// Token: 0x0400412F RID: 16687
	private const float KoreanBossTitleScale = 1.2f;

	// Token: 0x04004130 RID: 16688
	private const int KoreanDifficultyFontSize = 29;

	// Token: 0x04004131 RID: 16689
	private const int KoreanDifficultyOptionsFontSize = 37;

	// Token: 0x04004134 RID: 16692
	[SerializeField]
	private Image inAnimated;

	// Token: 0x04004135 RID: 16693
	[SerializeField]
	private Image bossTitleImage;

	// Token: 0x04004136 RID: 16694
	[SerializeField]
	private Image bossNameImage;

	// Token: 0x04004137 RID: 16695
	[SerializeField]
	private Image difficultyImage;

	// Token: 0x04004138 RID: 16696
	[SerializeField]
	private Sprite[] inSprites;

	// Token: 0x04004139 RID: 16697
	[SerializeField]
	private Image[] separatorsAnimated;

	// Token: 0x0400413A RID: 16698
	[SerializeField]
	private Sprite[] separatorsSprites;

	// Token: 0x0400413B RID: 16699
	[SerializeField]
	private RectTransform cursor;

	// Token: 0x0400413C RID: 16700
	[Header("Options")]
	[SerializeField]
	private RectTransform easy;

	// Token: 0x0400413D RID: 16701
	[SerializeField]
	private RectTransform normal;

	// Token: 0x0400413E RID: 16702
	[SerializeField]
	private RectTransform normalSeparator;

	// Token: 0x0400413F RID: 16703
	[SerializeField]
	private RectTransform hard;

	// Token: 0x04004140 RID: 16704
	[SerializeField]
	private RectTransform hardSeparator;

	// Token: 0x04004141 RID: 16705
	[SerializeField]
	private RectTransform box;

	// Token: 0x04004142 RID: 16706
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04004143 RID: 16707
	[SerializeField]
	private Color unselectedColor;

	// Token: 0x04004144 RID: 16708
	[Header("Stage")]
	[SerializeField]
	private LocalizationHelper bossImage;

	// Token: 0x04004145 RID: 16709
	[SerializeField]
	private LocalizationHelper bossName;

	// Token: 0x04004146 RID: 16710
	[SerializeField]
	private LocalizationHelper difficultyText;

	// Token: 0x04004147 RID: 16711
	[SerializeField]
	private Material bossCardWhiteMaterial;

	// Token: 0x04004148 RID: 16712
	[SerializeField]
	private Image difficultySelectionText;

	// Token: 0x04004149 RID: 16713
	[SerializeField]
	private Text inText;

	// Token: 0x0400414A RID: 16714
	[Header("Glow")]
	[SerializeField]
	private GlowText glowScript;

	// Token: 0x0400414B RID: 16715
	[SerializeField]
	private LocalizationHelper bossGlow;

	// Token: 0x0400414C RID: 16716
	[SerializeField]
	private Image asianGlow;

	// Token: 0x0400414D RID: 16717
	private TMP_Text[] difficulyTexts;

	// Token: 0x0400414E RID: 16718
	private Level.Mode[] options;

	// Token: 0x0400414F RID: 16719
	private int index = 1;

	// Token: 0x04004150 RID: 16720
	private float cursorY;

	// Token: 0x04004151 RID: 16721
	private int initialMaxFontSize;

	// Token: 0x04004152 RID: 16722
	private Vector2 initialinImagePosX;

	// Token: 0x04004153 RID: 16723
	private Vector2 initialinImagePosY;

	// Token: 0x04004154 RID: 16724
	private Vector2 initialinDifficultyPos;

	// Token: 0x04004155 RID: 16725
	private Vector2 initialDifficultyPos;

	// Token: 0x04004156 RID: 16726
	private Vector2 initialBossNamePos;
}
