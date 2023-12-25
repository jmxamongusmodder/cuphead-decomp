using System;
using System.Collections.Generic;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000460 RID: 1120
public class OptionsGUI : AbstractMonoBehaviour
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060010FE RID: 4350 RVA: 0x000A286E File Offset: 0x000A0C6E
	// (set) Token: 0x060010FF RID: 4351 RVA: 0x000A2876 File Offset: 0x000A0C76
	public OptionsGUI.State state { get; private set; }

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06001100 RID: 4352 RVA: 0x000A287F File Offset: 0x000A0C7F
	// (set) Token: 0x06001101 RID: 4353 RVA: 0x000A2886 File Offset: 0x000A0C86
	public static Color COLOR_SELECTED { get; private set; }

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06001102 RID: 4354 RVA: 0x000A288E File Offset: 0x000A0C8E
	// (set) Token: 0x06001103 RID: 4355 RVA: 0x000A2895 File Offset: 0x000A0C95
	public static Color COLOR_INACTIVE { get; private set; }

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06001104 RID: 4356 RVA: 0x000A289D File Offset: 0x000A0C9D
	// (set) Token: 0x06001105 RID: 4357 RVA: 0x000A28A5 File Offset: 0x000A0CA5
	public bool optionMenuOpen { get; private set; }

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06001106 RID: 4358 RVA: 0x000A28AE File Offset: 0x000A0CAE
	// (set) Token: 0x06001107 RID: 4359 RVA: 0x000A28B6 File Offset: 0x000A0CB6
	public bool inputEnabled { get; private set; }

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06001108 RID: 4360 RVA: 0x000A28BF File Offset: 0x000A0CBF
	// (set) Token: 0x06001109 RID: 4361 RVA: 0x000A28C8 File Offset: 0x000A0CC8
	private int verticalSelection
	{
		get
		{
			return this._verticalSelection;
		}
		set
		{
			bool flag = value > this._verticalSelection;
			int num = (int)Mathf.Repeat((float)value, (float)this.currentItems.Count);
			while (!this.currentItems[num].text.gameObject.activeSelf)
			{
				num = ((!flag) ? (num - 1) : (num + 1));
				num = (int)Mathf.Repeat((float)num, (float)this.currentItems.Count);
			}
			this._verticalSelection = num;
			this.UpdateVerticalSelection();
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x0600110A RID: 4362 RVA: 0x000A294D File Offset: 0x000A0D4D
	// (set) Token: 0x0600110B RID: 4363 RVA: 0x000A2955 File Offset: 0x000A0D55
	public bool justClosed { get; private set; }

	// Token: 0x0600110C RID: 4364 RVA: 0x000A2960 File Offset: 0x000A0D60
	protected override void Awake()
	{
		base.Awake();
		this.isConsole = PlatformHelper.IsConsole;
		this.showAlignOption = true;
		this.showTitleScreenOption = DLCManager.DLCEnabled();
		this.optionMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		this.currentItems = new List<OptionsGUI.Button>(this.mainObjectButtons);
		this.resolutions = new List<Resolution>();
		foreach (Resolution resolution in Screen.resolutions)
		{
			Resolution item = default(Resolution);
			item.width = resolution.width;
			item.height = resolution.height;
			item.refreshRate = 60;
			if (!this.resolutions.Contains(item))
			{
				this.resolutions.Add(item);
			}
		}
		this.SetupButtons();
		OptionsGUI.COLOR_SELECTED = this.currentItems[0].text.color;
		OptionsGUI.COLOR_INACTIVE = this.currentItems[this.currentItems.Count - 1].text.color;
		this.initialAudioCenter = this.audioObject.transform.localPosition.x;
		this.initialVisualCenter = this.visualObject.transform.localPosition.x;
		this.initialAudioBackCenter = this.audioObjectButtons[4].text.transform.localPosition.x;
		this.initialVisualBackCenter = this.visualObjectButtons[7].text.transform.localPosition.x;
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x000A2B12 File Offset: 0x000A0F12
	private void Start()
	{
		Localization.OnLanguageChangedEvent += this.UpdateLanguages;
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000A2B25 File Offset: 0x000A0F25
	private void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.UpdateLanguages;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x000A2B38 File Offset: 0x000A0F38
	private void UpdateLanguages()
	{
		for (int i = 0; i < this.audioObjectButtons.Length; i++)
		{
			if (this.audioObjectButtons[i].localizationHelper != null)
			{
				this.audioObjectButtons[i].localizationHelper.ApplyTranslation();
			}
		}
		for (int j = 0; j < this.visualObjectButtons.Length; j++)
		{
			if (this.visualObjectButtons[j].localizationHelper != null)
			{
				this.visualObjectButtons[j].localizationHelper.ApplyTranslation();
			}
		}
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x000A2BCC File Offset: 0x000A0FCC
	public void SetupButtons()
	{
		string[] array = new string[this.resolutions.Count];
		int index = 0;
		for (int i = 0; i < this.resolutions.Count; i++)
		{
			array[i] = this.resolutions[i].width + "x" + this.resolutions[i].height;
			if (Screen.width == this.resolutions[i].width && Screen.height == this.resolutions[i].height)
			{
				index = i;
			}
		}
		if (this.isConsole)
		{
			foreach (GameObject gameObject in this.PcOnlyObjects)
			{
				gameObject.SetActive(false);
			}
		}
		if (!DLCManager.DLCEnabled())
		{
			foreach (GameObject gameObject2 in this.dlcHideObjects)
			{
				gameObject2.SetActive(false);
			}
		}
		bool active = PlayerData.inGame && (PlayerData.Data.unlockedBlackAndWhite || PlayerData.Data.unlocked2Strip || PlayerData.Data.unlockedChaliceRecolor);
		foreach (GameObject gameObject3 in this.FilterUnlockedOnlyObjects)
		{
			gameObject3.SetActive(active);
		}
		if (!this.isConsole)
		{
			this.visualObjectButtons[0].options = array;
			this.visualObjectButtons[1].options = new string[]
			{
				"OptionMenuDisplayWindowed",
				"OptionMenuDisplayFullscreen"
			};
			this.visualObjectButtons[2].options = new string[]
			{
				"OptionMenuOn",
				"OptionMenuOff"
			};
		}
		if (this.showAlignOption)
		{
			this.visualObjectButtons[3].options = this.slider;
			this.visualObjectButtons[3].wrap = false;
		}
		this.visualObjectButtons[4].options = this.slider;
		this.visualObjectButtons[4].wrap = false;
		this.visualObjectButtons[5].options = this.slider;
		this.visualObjectButtons[5].wrap = false;
		if (this.showTitleScreenOption)
		{
			this.visualObjectButtons[6].options = new string[]
			{
				"TitleScreenOptionsMenuOriginal",
				"TitleScreenOptionsMenuDLC"
			};
			this.visualObjectButtons[6].wrap = true;
		}
		List<string> list = new List<string>();
		this.unlockedFilters = new List<BlurGamma.Filter>();
		this.unlockedFilters.Add(BlurGamma.Filter.None);
		list.Add("OptionMenuFilterNone");
		if (PlayerData.Data.unlocked2Strip)
		{
			list.Add("OptionMenuFilter2Strip");
			this.unlockedFilters.Add(BlurGamma.Filter.TwoStrip);
		}
		if (PlayerData.Data.unlockedBlackAndWhite)
		{
			list.Add("OptionMenuFilterBlackWhite");
			this.unlockedFilters.Add(BlurGamma.Filter.BW);
		}
		if (PlayerData.Data.unlockedChaliceRecolor)
		{
			list.Add("ChaliceCostumeOptionsMenu");
			this.unlockedFilters.Add(BlurGamma.Filter.Chalice);
		}
		this.visualObjectButtons[7].options = list.ToArray();
		if (!this.isConsole)
		{
			this.visualObjectButtons[0].updateSelection(index);
			this.visualObjectButtons[1].updateSelection((!Screen.fullScreen) ? 0 : 1);
			this.visualObjectButtons[2].updateSelection((QualitySettings.vSyncCount <= 0) ? 1 : 0);
		}
		if (this.showAlignOption)
		{
			this.visualObjectButtons[3].updateSelection(this.floatToSliderIndex(SettingsData.Data.overscan, 0f, 1f));
		}
		this.visualObjectButtons[4].updateSelection(this.floatToSliderIndex(SettingsData.Data.Brightness, -1f, 1f));
		this.visualObjectButtons[5].updateSelection(this.floatToSliderIndex(SettingsData.Data.chromaticAberration, 0.5f, 1.5f));
		if (this.showTitleScreenOption)
		{
			this.visualObjectButtons[6].updateSelection((!SettingsData.Data.forceOriginalTitleScreen) ? 1 : 0);
		}
		this.visualObjectButtons[7].updateSelection(Mathf.Min((int)SettingsData.Data.filter, list.Count - 1));
		this.audioObjectButtons[0].options = this.slider;
		this.audioObjectButtons[0].wrap = false;
		this.audioObjectButtons[1].options = this.slider;
		this.audioObjectButtons[1].wrap = false;
		this.audioObjectButtons[2].options = this.slider;
		this.audioObjectButtons[2].wrap = false;
		this.audioObjectButtons[3].options = new string[]
		{
			"OptionMenuOff",
			"OptionMenuOn"
		};
		this.audioObjectButtons[0].updateSelection(this.floatToSliderIndex(SettingsData.Data.masterVolume, -48f, 0f));
		this.audioObjectButtons[1].updateSelection(this.floatToSliderIndex(SettingsData.Data.sFXVolume, -48f, 0f));
		this.audioObjectButtons[2].updateSelection(this.floatToSliderIndex(SettingsData.Data.musicVolume, -48f, 0f));
		this.audioObjectButtons[3].updateSelection((!SettingsData.Data.vintageAudioEnabled) ? 0 : 1);
		int index2 = 0;
		for (int m = 0; m < this.languageTranslations.Length; m++)
		{
			if (this.languageTranslations[m].language == Localization.language)
			{
				index2 = m;
				break;
			}
		}
		string[] array3 = new string[this.languageTranslations.Length];
		for (int n = 0; n < this.languageTranslations.Length; n++)
		{
			array3[n] = "Language" + this.languageTranslations[n].translation;
		}
		this.languageObjectButtons[0].options = array3;
		this.languageObjectButtons[0].updateSelection(index2);
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x000A321C File Offset: 0x000A161C
	public void ChangeStateCustomLayoutScripts()
	{
		string text = this.visualObjectButtons[1].text.text;
		string value = Localization.Find(this.visualObjectButtons[1].options[0]).translation.SanitizedText();
		bool enabled = text.Equals(value);
		for (int i = 0; i < this.customPositionning.Length; i++)
		{
			this.customPositionning[i].enabled = enabled;
		}
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x000A3292 File Offset: 0x000A1692
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x000A32A0 File Offset: 0x000A16A0
	private void Update()
	{
		this.justClosed = false;
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.state == OptionsGUI.State.Controls)
		{
			if (Cuphead.Current.controlMapper.isOpen)
			{
				return;
			}
			this.state = OptionsGUI.State.MainOptions;
			this.canvasGroup.alpha = 1f;
			this.ToggleSubMenu(OptionsGUI.State.MainOptions);
			PlayerManager.ControlsChanged();
			return;
		}
		else
		{
			if (this.GetButtonDown(CupheadButton.Pause) || this.GetButtonDown(CupheadButton.Cancel))
			{
				if (this.state == OptionsGUI.State.MainOptions)
				{
					this.MenuSelectSound();
					this.HideMainOptionMenu();
				}
				else
				{
					this.MenuSelectSound();
					this.ToMainOptions();
				}
				return;
			}
			if (this.GetButtonDown(CupheadButton.Accept))
			{
				switch (this.state)
				{
				case OptionsGUI.State.MainOptions:
					this.OptionSelect();
					break;
				case OptionsGUI.State.Visual:
					this.VisualSelect();
					break;
				case OptionsGUI.State.Audio:
					this.AudioSelect();
					break;
				case OptionsGUI.State.Language:
					this.LanguageSelect();
					break;
				}
				return;
			}
			if (this._selectionTimer >= 0.15f)
			{
				if (this.GetButton(CupheadButton.MenuUp))
				{
					this.MenuMoveSound();
					this.verticalSelection--;
				}
				if (this.GetButton(CupheadButton.MenuDown))
				{
					this.MenuMoveSound();
					this.verticalSelection++;
				}
				if (this.GetButton(CupheadButton.MenuRight) && this.currentItems[this.verticalSelection].options.Length > 0)
				{
					this.currentItems[this.verticalSelection].incrementSelection();
					this.UpdateHorizontalSelection();
				}
				if (this.GetButton(CupheadButton.MenuLeft) && this.currentItems[this.verticalSelection].options.Length > 0)
				{
					this.currentItems[this.verticalSelection].decrementSelection();
					this.UpdateHorizontalSelection();
				}
			}
			else
			{
				this._selectionTimer += Time.deltaTime;
			}
			return;
		}
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x000A34A0 File Offset: 0x000A18A0
	private void UpdateVerticalSelection()
	{
		this._selectionTimer = 0f;
		if (this.state == OptionsGUI.State.Controls)
		{
			return;
		}
		if (this.state == OptionsGUI.State.Visual && this.isConsole && this.showAlignOption && this._verticalSelection < 3)
		{
			this._verticalSelection = 3;
		}
		if (this.state == OptionsGUI.State.Visual && this.isConsole && !this.showAlignOption && this._verticalSelection < 4)
		{
			this._verticalSelection = 4;
		}
		for (int i = 0; i < this.currentItems.Count; i++)
		{
			OptionsGUI.Button button = this.currentItems[i];
			if (i == this.verticalSelection)
			{
				button.text.color = OptionsGUI.COLOR_SELECTED;
			}
			else
			{
				button.text.color = OptionsGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x000A358C File Offset: 0x000A198C
	private void UpdateHorizontalSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.currentItems.Count; i++)
		{
			OptionsGUI.Button button = this.currentItems[i];
			if (i == this.verticalSelection && this.currentItems[i].options.Length > 0)
			{
				OptionsGUI.State state = this.state;
				if (state != OptionsGUI.State.Audio)
				{
					if (state != OptionsGUI.State.Visual)
					{
						if (state == OptionsGUI.State.Language)
						{
							this.LanguageHorizontalSelect(this.currentItems[i]);
						}
					}
					else
					{
						this.VisualHorizontalSelect(this.currentItems[i]);
					}
				}
				else
				{
					this.AudioHorizontalSelect(this.currentItems[i]);
				}
			}
		}
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x000A3658 File Offset: 0x000A1A58
	public void ShowMainOptionMenu()
	{
		this.state = OptionsGUI.State.MainOptions;
		this.ToggleSubMenu(this.state);
		this.optionMenuOpen = true;
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 1f;
		base.FrameDelayedCallback(new Action(this.Interactable), 1);
		this.UpdateVerticalSelection();
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x000A36B0 File Offset: 0x000A1AB0
	public void HideMainOptionMenu()
	{
		SettingsData.Save();
		if (PlatformHelper.IsConsole)
		{
			SettingsData.SaveToCloud();
		}
		if (this.savePlayerData)
		{
			PlayerData.SaveCurrentFile();
		}
		this.savePlayerData = false;
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.optionMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x000A372C File Offset: 0x000A1B2C
	private void Interactable()
	{
		this.verticalSelection = 0;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x000A3754 File Offset: 0x000A1B54
	private void OptionSelect()
	{
		this.MenuSelectSound();
		switch (this.verticalSelection)
		{
		case 0:
			this.ToAudio();
			break;
		case 1:
			this.ToVisual();
			break;
		case 2:
			this.ToControls();
			break;
		case 3:
			this.ToLanguage();
			break;
		case 4:
			this.ToPauseMenu();
			break;
		}
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x000A37C4 File Offset: 0x000A1BC4
	protected void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x000A37D0 File Offset: 0x000A1BD0
	protected void MenuMoveSound()
	{
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x000A37DC File Offset: 0x000A1BDC
	private void ToVisual()
	{
		this.state = OptionsGUI.State.Visual;
		this.CenterVisual();
		if (!this.isConsole)
		{
			this.ChangeStateCustomLayoutScripts();
		}
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x000A3808 File Offset: 0x000A1C08
	private void ToAudio()
	{
		this.state = OptionsGUI.State.Audio;
		this.CenterAudio();
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x000A3823 File Offset: 0x000A1C23
	private void ToLanguage()
	{
		this.state = OptionsGUI.State.Language;
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x000A3838 File Offset: 0x000A1C38
	private void ToControls()
	{
		this.state = OptionsGUI.State.Controls;
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x000A384D File Offset: 0x000A1C4D
	private void ToPauseMenu()
	{
		this.optionMenuOpen = false;
		this.HideMainOptionMenu();
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x000A385C File Offset: 0x000A1C5C
	private void ToggleSubMenu(OptionsGUI.State state)
	{
		this.currentItems.Clear();
		switch (state)
		{
		case OptionsGUI.State.MainOptions:
			this.mainObject.SetActive(true);
			this.visualObject.SetActive(false);
			this.audioObject.SetActive(false);
			this.languageObject.SetActive(false);
			this.bigCard.SetActive(false);
			this.bigNoise.SetActive(false);
			this.currentItems.AddRange(this.mainObjectButtons);
			break;
		case OptionsGUI.State.Visual:
			this.mainObject.SetActive(false);
			this.visualObject.SetActive(true);
			this.audioObject.SetActive(false);
			this.bigCard.SetActive(true);
			this.bigNoise.SetActive(true);
			this.currentItems.AddRange(this.visualObjectButtons);
			break;
		case OptionsGUI.State.Audio:
			this.mainObject.SetActive(false);
			this.visualObject.SetActive(false);
			this.audioObject.SetActive(true);
			this.languageObject.SetActive(false);
			this.bigCard.SetActive(true);
			this.bigNoise.SetActive(true);
			this.currentItems.AddRange(this.audioObjectButtons);
			break;
		case OptionsGUI.State.Controls:
			this.mainObject.SetActive(false);
			this.visualObject.SetActive(false);
			this.audioObject.SetActive(false);
			this.languageObject.SetActive(false);
			this.ShowControlMapper();
			break;
		case OptionsGUI.State.Language:
			this.languageObjectButtons[0].updateSelection((int)Localization.language);
			this.mainObject.SetActive(false);
			this.audioObject.SetActive(false);
			this.languageObject.SetActive(true);
			this.bigCard.SetActive(false);
			this.bigNoise.SetActive(false);
			this.currentItems.AddRange(this.languageObjectButtons);
			break;
		}
		if (state != OptionsGUI.State.Controls)
		{
			this.verticalSelection = 0;
			this.UpdateVerticalSelection();
		}
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x000A3A54 File Offset: 0x000A1E54
	private void ShowControlMapper()
	{
		ControlMapper controlMapper = Cuphead.Current.controlMapper;
		Canvas componentInChildren = controlMapper.GetComponentInChildren<Canvas>(true);
		CupheadUICamera cupheadUICamera = UnityEngine.Object.FindObjectOfType<CupheadUICamera>();
		if (cupheadUICamera != null && componentInChildren != null)
		{
			componentInChildren.worldCamera = cupheadUICamera.GetComponent<Camera>();
		}
		controlMapper.showPlayers = true;
		if (PlatformHelper.IsConsole)
		{
			controlMapper.showKeyboard = false;
			controlMapper.showControllerGroupButtons = false;
		}
		controlMapper.showControllerGroupButtons = !PlatformHelper.IsConsole;
		controlMapper.Reset();
		controlMapper.Open();
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x000A3AFF File Offset: 0x000A1EFF
	private void ToMainOptions()
	{
		this.state = OptionsGUI.State.MainOptions;
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x000A3B14 File Offset: 0x000A1F14
	private void VisualHorizontalSelect(OptionsGUI.Button button)
	{
		switch (this.verticalSelection)
		{
		case 0:
			this.MenuSelectSound();
			if (button.selection < this.resolutions.Count)
			{
				SettingsData.Data.screenWidth = this.resolutions[button.selection].width;
				SettingsData.Data.screenHeight = this.resolutions[button.selection].height;
				Screen.SetResolution(SettingsData.Data.screenWidth, SettingsData.Data.screenHeight, Screen.fullScreen, 60);
			}
			break;
		case 1:
			this.MenuSelectSound();
			SettingsData.Data.fullScreen = (button.selection == 1);
			if (!this.isConsole)
			{
				this.ChangeStateCustomLayoutScripts();
			}
			Screen.fullScreen = SettingsData.Data.fullScreen;
			break;
		case 2:
			this.MenuSelectSound();
			SettingsData.Data.vSyncCount = ((button.selection != 0) ? 0 : 1);
			QualitySettings.vSyncCount = SettingsData.Data.vSyncCount;
			break;
		case 3:
			SettingsData.Data.overscan = this.sliderIndexToFloat(button.selection, 0f, 1f);
			break;
		case 4:
			SettingsData.Data.Brightness = this.sliderIndexToFloat(button.selection, -1f, 1f);
			break;
		case 5:
			SettingsData.Data.chromaticAberration = this.sliderIndexToFloat(button.selection, 0.5f, 1.5f);
			break;
		case 6:
			SettingsData.Data.forceOriginalTitleScreen = !SettingsData.Data.forceOriginalTitleScreen;
			break;
		case 7:
			this.MenuSelectSound();
			PlayerData.Data.filter = this.unlockedFilters[button.selection];
			EventManager.Instance.Raise(new ChaliceRecolorEvent(this.unlockedFilters[button.selection] == BlurGamma.Filter.Chalice));
			this.savePlayerData = true;
			break;
		}
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x000A3D2C File Offset: 0x000A212C
	private void VisualSelect()
	{
		AudioManager.Play("level_menu_select");
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 8)
		{
			this.ToMainOptions();
		}
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x000A3D64 File Offset: 0x000A2164
	private void LanguageSelect()
	{
		AudioManager.Play("level_menu_select");
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 1)
		{
			this.ToMainOptions();
		}
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x000A3D9C File Offset: 0x000A219C
	private void LanguageHorizontalSelect(OptionsGUI.Button button)
	{
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 0)
		{
			Localization.language = this.languageTranslations[button.selection].language;
			button.updateSelection(button.selection);
			for (int i = 0; i < this.elementsToTranslate.Length; i++)
			{
				this.elementsToTranslate[i].ApplyTranslation();
			}
		}
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x000A3E10 File Offset: 0x000A2210
	private void AudioHorizontalSelect(OptionsGUI.Button button)
	{
		switch (this.verticalSelection)
		{
		case 0:
			AudioManager.masterVolume = ((button.selection > 0) ? this.sliderIndexToFloat(button.selection, -48f, 0f) : -80f);
			SettingsData.Data.masterVolume = AudioManager.masterVolume;
			break;
		case 1:
			AudioManager.sfxOptionsVolume = ((button.selection > 0) ? this.sliderIndexToFloat(button.selection, -48f, 0f) : -80f);
			SettingsData.Data.sFXVolume = AudioManager.sfxOptionsVolume;
			break;
		case 2:
			AudioManager.bgmOptionsVolume = ((button.selection > 0) ? this.sliderIndexToFloat(button.selection, -48f, 0f) : -80f);
			SettingsData.Data.musicVolume = AudioManager.bgmOptionsVolume;
			break;
		case 3:
			this.MenuSelectSound();
			if (button.selection == 0)
			{
				PlayerData.Data.vintageAudioEnabled = false;
			}
			else if (button.options[button.selection] == button.options[1])
			{
				PlayerData.Data.vintageAudioEnabled = true;
			}
			this.savePlayerData = true;
			break;
		}
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x000A3F65 File Offset: 0x000A2365
	private void MasterVolume(string option)
	{
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x000A3F68 File Offset: 0x000A2368
	private void AudioSelect()
	{
		AudioManager.Play("level_menu_select");
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 4)
		{
			this.ToMainOptions();
		}
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x000A3F9D File Offset: 0x000A239D
	protected bool GetButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x000A3FB3 File Offset: 0x000A23B3
	protected bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x000A3FC9 File Offset: 0x000A23C9
	private float sliderIndexToFloat(int index, float min, float max)
	{
		if (index != this.lastIndex)
		{
			this.MenuSelectSound();
		}
		this.lastIndex = index;
		return (float)index / (float)(this.slider.Length - 1) * (max - min) + min;
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x000A3FF8 File Offset: 0x000A23F8
	private int floatToSliderIndex(float value, float min, float max)
	{
		int num = Mathf.RoundToInt((value - min) / (max - min) * (float)(this.slider.Length - 1));
		if (num > this.slider.Length - 1)
		{
			num = this.slider.Length - 1;
		}
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x000A4044 File Offset: 0x000A2444
	private void CenterAudio()
	{
		float num = this.audioCenterPositions[(int)Localization.language];
		this.audioObject.transform.SetLocalPosition(new float?(this.initialAudioCenter + num), null, null);
		this.audioObjectButtons[4].text.transform.SetLocalPosition(new float?(this.initialAudioBackCenter - num), null, null);
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x000A40C4 File Offset: 0x000A24C4
	private void CenterVisual()
	{
		float num = this.visualCenterPositions[(int)Localization.language];
		this.visualObject.transform.SetLocalPosition(new float?(this.initialVisualCenter + num), null, null);
		this.visualObjectButtons[7].text.transform.SetLocalPosition(new float?(this.initialVisualBackCenter - num), null, null);
	}

	// Token: 0x04001A66 RID: 6758
	private const float BRIGHTNESS_MAX = 1f;

	// Token: 0x04001A67 RID: 6759
	private const float VOLUME_MIN = -48f;

	// Token: 0x04001A68 RID: 6760
	private const float CHROMATIC_ABERRATION_MIN = 0.5f;

	// Token: 0x04001A69 RID: 6761
	private const float CHROMATIC_ABERRATION_MAX = 1.5f;

	// Token: 0x04001A6A RID: 6762
	private const float VOLUME_NONE = -80f;

	// Token: 0x04001A70 RID: 6768
	[SerializeField]
	private GameObject mainObject;

	// Token: 0x04001A71 RID: 6769
	[SerializeField]
	private GameObject visualObject;

	// Token: 0x04001A72 RID: 6770
	[SerializeField]
	private GameObject audioObject;

	// Token: 0x04001A73 RID: 6771
	[SerializeField]
	private GameObject languageObject;

	// Token: 0x04001A74 RID: 6772
	[SerializeField]
	private OptionsGUI.Button[] mainObjectButtons;

	// Token: 0x04001A75 RID: 6773
	[SerializeField]
	private GameObject[] PcOnlyObjects;

	// Token: 0x04001A76 RID: 6774
	[SerializeField]
	private GameObject[] playStation4HideObjects;

	// Token: 0x04001A77 RID: 6775
	[SerializeField]
	private GameObject[] dlcHideObjects;

	// Token: 0x04001A78 RID: 6776
	[SerializeField]
	private GameObject[] FilterUnlockedOnlyObjects;

	// Token: 0x04001A79 RID: 6777
	[SerializeField]
	private GameObject bigCard;

	// Token: 0x04001A7A RID: 6778
	[SerializeField]
	private GameObject bigNoise;

	// Token: 0x04001A7B RID: 6779
	[SerializeField]
	private OptionsGUI.Button[] visualObjectButtons;

	// Token: 0x04001A7C RID: 6780
	[SerializeField]
	private OptionsGUI.Button[] audioObjectButtons;

	// Token: 0x04001A7D RID: 6781
	[SerializeField]
	private OptionsGUI.Button[] languageObjectButtons;

	// Token: 0x04001A7E RID: 6782
	[SerializeField]
	private OptionsGUI.LanguageTranslation[] languageTranslations;

	// Token: 0x04001A7F RID: 6783
	[SerializeField]
	private LocalizationHelper[] elementsToTranslate;

	// Token: 0x04001A80 RID: 6784
	[SerializeField]
	private float[] audioCenterPositions;

	// Token: 0x04001A81 RID: 6785
	[SerializeField]
	private float[] visualCenterPositions;

	// Token: 0x04001A82 RID: 6786
	[SerializeField]
	private CustomLanguageLayout[] customPositionning;

	// Token: 0x04001A83 RID: 6787
	private List<OptionsGUI.Button> currentItems;

	// Token: 0x04001A84 RID: 6788
	private List<BlurGamma.Filter> unlockedFilters;

	// Token: 0x04001A85 RID: 6789
	private bool isConsole;

	// Token: 0x04001A86 RID: 6790
	private bool showAlignOption;

	// Token: 0x04001A87 RID: 6791
	private bool showTitleScreenOption;

	// Token: 0x04001A88 RID: 6792
	private string[] slider = new string[]
	{
		"|----------",
		"-|---------",
		"--|--------",
		"---|-------",
		"----|------",
		"-----|-----",
		"------|----",
		"-------|---",
		"--------|--",
		"---------|-",
		"----------|"
	};

	// Token: 0x04001A89 RID: 6793
	private CanvasGroup canvasGroup;

	// Token: 0x04001A8A RID: 6794
	private AbstractPauseGUI pauseMenu;

	// Token: 0x04001A8B RID: 6795
	private float _selectionTimer;

	// Token: 0x04001A8C RID: 6796
	private const float _SELECTION_TIME = 0.15f;

	// Token: 0x04001A8D RID: 6797
	private int _verticalSelection;

	// Token: 0x04001A8E RID: 6798
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001A8F RID: 6799
	private int lastIndex;

	// Token: 0x04001A90 RID: 6800
	private List<Resolution> resolutions;

	// Token: 0x04001A92 RID: 6802
	private float initialAudioCenter;

	// Token: 0x04001A93 RID: 6803
	private float initialVisualCenter;

	// Token: 0x04001A94 RID: 6804
	private float initialAudioBackCenter;

	// Token: 0x04001A95 RID: 6805
	private float initialVisualBackCenter;

	// Token: 0x04001A96 RID: 6806
	private bool savePlayerData;

	// Token: 0x02000461 RID: 1121
	[Serializable]
	public struct LanguageTranslation
	{
		// Token: 0x04001A97 RID: 6807
		[SerializeField]
		public Localization.Languages language;

		// Token: 0x04001A98 RID: 6808
		[SerializeField]
		public string translation;
	}

	// Token: 0x02000462 RID: 1122
	public enum State
	{
		// Token: 0x04001A9A RID: 6810
		MainOptions,
		// Token: 0x04001A9B RID: 6811
		Visual,
		// Token: 0x04001A9C RID: 6812
		Audio,
		// Token: 0x04001A9D RID: 6813
		Controls,
		// Token: 0x04001A9E RID: 6814
		Language
	}

	// Token: 0x02000463 RID: 1123
	private enum VisualOptions
	{
		// Token: 0x04001AA0 RID: 6816
		Resolution,
		// Token: 0x04001AA1 RID: 6817
		Display,
		// Token: 0x04001AA2 RID: 6818
		VSync,
		// Token: 0x04001AA3 RID: 6819
		Align,
		// Token: 0x04001AA4 RID: 6820
		Brightness,
		// Token: 0x04001AA5 RID: 6821
		ChromaticAberration,
		// Token: 0x04001AA6 RID: 6822
		TitleScreen,
		// Token: 0x04001AA7 RID: 6823
		Filter
	}

	// Token: 0x02000464 RID: 1124
	private enum AudioOptions
	{
		// Token: 0x04001AA9 RID: 6825
		MasterVol,
		// Token: 0x04001AAA RID: 6826
		SFXVol,
		// Token: 0x04001AAB RID: 6827
		MusicVol,
		// Token: 0x04001AAC RID: 6828
		Vintage
	}

	// Token: 0x02000465 RID: 1125
	private enum LanguageOptions
	{
		// Token: 0x04001AAE RID: 6830
		Language
	}

	// Token: 0x02000466 RID: 1126
	[Serializable]
	public class Button
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x000A4154 File Offset: 0x000A2554
		public void updateSelection(int index)
		{
			this.selection = index;
			if (this.localizationHelper == null)
			{
				this.text.text = this.options[index];
			}
			else
			{
				this.localizationHelper.ApplyTranslation(Localization.Find(this.options[index]), null);
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x000A41AA File Offset: 0x000A25AA
		public void incrementSelection()
		{
			if (this.wrap || this.selection < this.options.Length - 1)
			{
				this.updateSelection((this.selection + 1) % this.options.Length);
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000A41E4 File Offset: 0x000A25E4
		public void decrementSelection()
		{
			if (this.wrap || this.selection > 0)
			{
				this.updateSelection((this.selection != 0) ? (this.selection - 1) : (this.options.Length - 1));
			}
		}

		// Token: 0x04001AAF RID: 6831
		public Text text;

		// Token: 0x04001AB0 RID: 6832
		public LocalizationHelper localizationHelper;

		// Token: 0x04001AB1 RID: 6833
		public string[] options;

		// Token: 0x04001AB2 RID: 6834
		public int selection;

		// Token: 0x04001AB3 RID: 6835
		public bool wrap = true;
	}
}
