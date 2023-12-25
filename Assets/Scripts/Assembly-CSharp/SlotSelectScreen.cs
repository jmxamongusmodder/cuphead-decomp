using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009AA RID: 2474
public class SlotSelectScreen : AbstractMonoBehaviour
{
	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06003A01 RID: 14849 RVA: 0x0020F881 File Offset: 0x0020DC81
	private bool RespondToDeadPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x0020F884 File Offset: 0x0020DC84
	protected override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.isConsole = PlatformHelper.IsConsole;
		PlayerData.inGame = false;
		List<Text> list = new List<Text>(this.mainMenuItems);
		List<SlotSelectScreen.MainMenuItem> list2 = new List<SlotSelectScreen.MainMenuItem>((SlotSelectScreen.MainMenuItem[])Enum.GetValues(typeof(SlotSelectScreen.MainMenuItem)));
		if (this.isConsole)
		{
			this.mainMenuItems[4].gameObject.SetActive(false);
			list.RemoveAt(4);
			list2.RemoveAt(4);
		}
		if (!PlatformHelper.ShowDLCMenuItem)
		{
			this.mainMenuItems[3].gameObject.SetActive(false);
			list.RemoveAt(3);
			list2.RemoveAt(3);
		}
		if (!PlatformHelper.ShowAchievements)
		{
			this.mainMenuItems[1].gameObject.SetActive(false);
			list.RemoveAt(1);
			list2.RemoveAt(1);
		}
		this.mainMenuItems = list.ToArray();
		this._availableMainMenuItems = list2.ToArray();
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x0020F97C File Offset: 0x0020DD7C
	private void Update()
	{
		if (this.dataStatus == SlotSelectScreen.SaveDataStatus.Received)
		{
			this.dataStatus = SlotSelectScreen.SaveDataStatus.Initialized;
			base.StartCoroutine(this.allDataLoaded_cr());
		}
		this.timeSinceStart += Time.deltaTime;
		switch (this.state)
		{
		case SlotSelectScreen.State.MainMenu:
			this.UpdateMainMenu();
			break;
		case SlotSelectScreen.State.AchievementsMenu:
			this.UpdateAchievementsMenu();
			break;
		case SlotSelectScreen.State.OptionsMenu:
			this.UpdateOptionsMenu();
			break;
		case SlotSelectScreen.State.DLC:
			this.UpdateDLCMenu();
			break;
		case SlotSelectScreen.State.SlotSelect:
			this.UpdateSlotSelect();
			break;
		case SlotSelectScreen.State.ConfirmDelete:
			this.UpdateConfirmDelete();
			break;
		case SlotSelectScreen.State.PlayerSelect:
			this.UpdatePlayerSelect();
			break;
		}
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x0020FA38 File Offset: 0x0020DE38
	private void Start()
	{
		if (StartScreenAudio.Instance == null)
		{
			UnityEngine.Object.Instantiate(Resources.Load("Audio/TitleScreenAudio"));
			SceneLoader.OnLoaderCompleteEvent += this.PlayMusic;
		}
		CupheadLevelCamera.Current.StartSmoothShake(8f, 3f, 2);
		this.SetState(SlotSelectScreen.State.InitializeStorage);
		PlayerData.Init(new PlayerData.PlayerDataInitHandler(this.OnPlayerDataInitialized));
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x0020FAA3 File Offset: 0x0020DEA3
	private void PlayMusic()
	{
		AudioManager.PlayBGMPlaylistManually(true);
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x0020FAAB File Offset: 0x0020DEAB
	private void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.PlayMusic;
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x0020FAC0 File Offset: 0x0020DEC0
	private void SetState(SlotSelectScreen.State state)
	{
		this.state = state;
		this.mainMenuChild.gameObject.SetActive(state == SlotSelectScreen.State.MainMenu);
		this.LoadingChild.gameObject.SetActive(state == SlotSelectScreen.State.InitializeStorage);
		this.slotSelectChild.gameObject.SetActive(state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.ConfirmDelete || state == SlotSelectScreen.State.PlayerSelect);
		this.confirmDeleteChild.gameObject.SetActive(state == SlotSelectScreen.State.ConfirmDelete);
		this.confirmPrompt.gameObject.SetActive(state == SlotSelectScreen.State.MainMenu || state == SlotSelectScreen.State.OptionsMenu || state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.ConfirmDelete || state == SlotSelectScreen.State.PlayerSelect);
		this.confirmGlyph.gameObject.SetActive(this.confirmPrompt.gameObject.activeSelf);
		this.confirmSpacer.gameObject.SetActive(this.confirmPrompt.gameObject.activeSelf);
		this.backPrompt.gameObject.SetActive(state == SlotSelectScreen.State.OptionsMenu || state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.ConfirmDelete || state == SlotSelectScreen.State.PlayerSelect || state == SlotSelectScreen.State.AchievementsMenu || state == SlotSelectScreen.State.DLC);
		this.backGlyph.gameObject.SetActive(this.backPrompt.gameObject.activeSelf);
		this.backSpacer.gameObject.SetActive(this.backPrompt.gameObject.activeSelf);
		this.deletePrompt.gameObject.SetActive(state == SlotSelectScreen.State.SlotSelect);
		this.deleteGlyph.gameObject.SetActive(this.deletePrompt.gameObject.activeSelf);
		this.deleteSpacer.gameObject.SetActive(this.deletePrompt.gameObject.activeSelf);
		this.storePrompt.gameObject.SetActive(state == SlotSelectScreen.State.DLC && DLCManager.CanRedirectToStore() && !DLCManager.DLCEnabled());
		this.storeGlyph.gameObject.SetActive(this.storePrompt.gameObject.activeSelf);
		this.storeSpacer.gameObject.SetActive(this.storePrompt.gameObject.activeSelf);
		this.playerProfiles.gameObject.SetActive(state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.MainMenu);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.MainMenu);
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x0020FD14 File Offset: 0x0020E114
	private void UpdateMainMenu()
	{
		if (this.timeSinceStart < 0.75f)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("level_menu_move");
			this._mainMenuSelection = (this._mainMenuSelection + 1) % this.mainMenuItems.Length;
		}
		if (this.GetButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("level_menu_move");
			this._mainMenuSelection--;
			if (this._mainMenuSelection < 0)
			{
				this._mainMenuSelection = this.mainMenuItems.Length - 1;
			}
		}
		for (int i = 0; i < this.mainMenuItems.Length; i++)
		{
			this.mainMenuItems[i].color = ((this._mainMenuSelection != i) ? this.mainMenuUnselectedColor : this.mainMenuSelectedColor);
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			AudioManager.Play("level_menu_select");
			switch (this._availableMainMenuItems[this._mainMenuSelection])
			{
			case SlotSelectScreen.MainMenuItem.Start:
				this.SetState(SlotSelectScreen.State.SlotSelect);
				for (int j = 0; j < 3; j++)
				{
					this.slots[j].Init(j);
				}
				break;
			case SlotSelectScreen.MainMenuItem.Achievements:
				this.SetState(SlotSelectScreen.State.AchievementsMenu);
				this.achievements.ShowAchievements();
				break;
			case SlotSelectScreen.MainMenuItem.Options:
				this.SetState(SlotSelectScreen.State.OptionsMenu);
				this.options.ShowMainOptionMenu();
				break;
			case SlotSelectScreen.MainMenuItem.DLC:
				this.SetState(SlotSelectScreen.State.DLC);
				this.dlcMenu.ShowDLCMenu();
				break;
			case SlotSelectScreen.MainMenuItem.Exit:
				Application.Quit();
				break;
			}
		}
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0020FEA4 File Offset: 0x0020E2A4
	private void UpdateOptionsMenu()
	{
		this.prompts.gameObject.SetActive(!Cuphead.Current.controlMapper.isOpen);
		if (!this.options.optionMenuOpen && !this.options.justClosed)
		{
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x0020FEFA File Offset: 0x0020E2FA
	private void UpdateAchievementsMenu()
	{
		if (!this.achievements.achievementsMenuOpen && !this.achievements.justClosed)
		{
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x0020FF23 File Offset: 0x0020E323
	private void UpdateDLCMenu()
	{
		if (!this.dlcMenu.dlcMenuOpen && !this.dlcMenu.justClosed)
		{
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0020FF4C File Offset: 0x0020E34C
	private void UpdatePlayerSelect()
	{
		if (PlayerData.inGame)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.MenuLeft) || this.GetButtonDown(CupheadButton.MenuRight))
		{
			AudioManager.Play("level_menu_move");
			this.slots[this._slotSelection].SwapSprite();
		}
		else if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			for (int i = 0; i < this.slots.Length; i++)
			{
				if (i != this._slotSelection)
				{
					base.StartCoroutine(this.activate_noise_cr(i));
				}
			}
			this.slots[this._slotSelection].StopSelectingPlayer();
			this.SetState(SlotSelectScreen.State.SlotSelect);
		}
		else if (this.GetButtonDown(CupheadButton.Accept))
		{
			AudioManager.Play("ui_menu_confirm");
			this.slots[this._slotSelection].PlayAnimation(this._slotSelection);
			base.StartCoroutine(this.game_start_cr());
		}
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x00210044 File Offset: 0x0020E444
	private void UpdateSlotSelect()
	{
		if (PlayerData.inGame)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("ui_saveslot_move");
			this._slotSelection = (this._slotSelection + 1) % 3;
		}
		if (this.GetButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("ui_saveslot_move");
			this._slotSelection--;
			if (this._slotSelection < 0)
			{
				this._slotSelection = 2;
			}
		}
		for (int i = 0; i < 3; i++)
		{
			this.slots[i].SetSelected(this._slotSelection == i);
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			AudioManager.Play("level_select");
			for (int j = 0; j < this.slots.Length; j++)
			{
				if (j != this._slotSelection)
				{
					this.slots[j].noise.gameObject.SetActive(false);
				}
			}
			this.slots[this._slotSelection].EnterSelectMenu();
			this.SetState(SlotSelectScreen.State.PlayerSelect);
		}
		else if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
		else if (!this.slots[this._slotSelection].IsEmpty && this.GetButtonDown(CupheadButton.EquipMenu))
		{
			AudioManager.Play("level_menu_select");
			this.SetState(SlotSelectScreen.State.ConfirmDelete);
			this._confirmDeleteSelection = 1;
			this.confirmDeleteSlotTitle.text = this.slots[this._slotSelection].GetSlotTitle();
			this.confirmDeleteSlotTitle.font = this.slots[this._slotSelection].GetSlotTitleFont();
			this.confirmDeleteSlotSeparator.text = this.slots[this._slotSelection].GetSlotSeparator();
			this.confirmDeleteSlotSeparator.font = this.slots[this._slotSelection].GetSlotSeparatorFont();
			this.confirmDeleteSlotPercentage.text = this.slots[this._slotSelection].GetSlotPercentage() + "?";
			this.confirmDeleteSlotPercentage.font = this.slots[this._slotSelection].GetSlotPercentageFont();
		}
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x0021026C File Offset: 0x0020E66C
	private IEnumerator game_start_cr()
	{
		PlayerData.inGame = true;
		for (int i = 0; i < 45; i++)
		{
			yield return null;
		}
		this.EnterGame();
		yield break;
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x00210288 File Offset: 0x0020E688
	private IEnumerator activate_noise_cr(int index)
	{
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		this.slots[index].noise.gameObject.SetActive(true);
		yield return null;
		yield break;
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x002102AC File Offset: 0x0020E6AC
	private void EnterGame()
	{
		DLCManager.RefreshDLC();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerData.CurrentSaveFileIndex = this._slotSelection;
		PlayerManager.player1IsMugman = this.slots[this._slotSelection].isPlayer1Mugman;
		PlayerData.GetDataForSlot(this._slotSelection).isPlayer1Mugman = PlayerManager.player1IsMugman;
		if (!DLCManager.DLCEnabled())
		{
			PlayerData data = PlayerData.Data;
			for (int i = 0; i < 2; i++)
			{
				PlayerId player = (PlayerId)i;
				PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = data.Loadouts.GetPlayerLoadout(player);
				if (Array.IndexOf<Weapon>(PlayerData.WeaponsDLC, playerLoadout.secondaryWeapon) >= 0)
				{
					playerLoadout.secondaryWeapon = Weapon.None;
				}
				if (Array.IndexOf<Weapon>(PlayerData.WeaponsDLC, playerLoadout.primaryWeapon) >= 0)
				{
					playerLoadout.primaryWeapon = Weapon.level_weapon_peashot;
					if (playerLoadout.secondaryWeapon == Weapon.level_weapon_peashot)
					{
						playerLoadout.secondaryWeapon = Weapon.None;
					}
				}
				if (Array.IndexOf<Charm>(PlayerData.CharmsDLC, playerLoadout.charm) >= 0)
				{
					playerLoadout.charm = Charm.None;
				}
			}
		}
		Level.ResetPreviousLevelInfo();
		if (!this.slots[this._slotSelection].IsEmpty)
		{
			if (!DLCManager.DLCEnabled() && PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
			{
				PlayerData.Data.CurrentMap = Scenes.scene_map_world_1;
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = false;
			}
			SceneLoader.LoadScene(PlayerData.Data.CurrentMap, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			PlayerData.Data.CurrentMap = Scenes.scene_map_world_1;
			PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = false;
			Cutscene.Load(Scenes.scene_level_house_elder_kettle, Scenes.scene_cutscene_intro, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		}
		PlayerData.inGame = true;
		if (StartScreenAudio.Instance != null)
		{
			UnityEngine.Object.Destroy(StartScreenAudio.Instance.gameObject);
		}
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x00210464 File Offset: 0x0020E864
	private void UpdateConfirmDelete()
	{
		if (this.GetButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("level_menu_move");
			this._confirmDeleteSelection = (this._confirmDeleteSelection + 1) % 2;
		}
		if (this.GetButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("level_menu_move");
			this._confirmDeleteSelection--;
			if (this._confirmDeleteSelection < 0)
			{
				this._confirmDeleteSelection = 1;
			}
		}
		for (int i = 0; i < 2; i++)
		{
			this.confirmDeleteItems[i].color = ((this._confirmDeleteSelection != i) ? this.confirmDeleteUnselectedColor : this.confirmDeleteSelectedColor);
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			SlotSelectScreen.ConfirmDeleteItem confirmDeleteSelection = (SlotSelectScreen.ConfirmDeleteItem)this._confirmDeleteSelection;
			if (confirmDeleteSelection != SlotSelectScreen.ConfirmDeleteItem.Yes)
			{
				if (confirmDeleteSelection == SlotSelectScreen.ConfirmDeleteItem.No)
				{
					AudioManager.Play("level_menu_select");
					this.SetState(SlotSelectScreen.State.SlotSelect);
				}
			}
			else
			{
				AudioManager.Play("level_menu_select");
				PlayerData.ClearSlot(this._slotSelection);
				this.slots[this._slotSelection].Init(this._slotSelection);
				this.SetState(SlotSelectScreen.State.SlotSelect);
			}
		}
		if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			this.SetState(SlotSelectScreen.State.SlotSelect);
		}
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x002105A0 File Offset: 0x0020E9A0
	private void OnPlayerDataInitialized(bool success)
	{
		if (!success)
		{
			PlayerData.Init(new PlayerData.PlayerDataInitHandler(this.OnPlayerDataInitialized));
			return;
		}
		if (PlatformHelper.IsConsole && !PlatformHelper.PreloadSettingsData)
		{
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
		}
		else
		{
			this.dataStatus = SlotSelectScreen.SaveDataStatus.Received;
		}
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x002105F6 File Offset: 0x0020E9F6
	private void OnSettingsDataLoaded(bool success)
	{
		if (!success)
		{
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
			return;
		}
		SettingsData.ApplySettingsOnStartup();
		base.StartCoroutine(this.allDataLoaded_cr());
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x00210624 File Offset: 0x0020EA24
	private IEnumerator allDataLoaded_cr()
	{
		yield return null;
		this.SetState(SlotSelectScreen.State.MainMenu);
		for (int i = 0; i < 3; i++)
		{
			this.slots[i].Init(i);
		}
		ControllerDisconnectedPrompt.Instance.allowedToShow = true;
		this.options = this.optionsPrefab.InstantiatePrefab<OptionsGUI>();
		this.options.rectTransform.SetParent(this.optionsRoot, false);
		this.options.Init(false);
		if (PlatformHelper.ShowAchievements)
		{
			this.achievements = this.achievementsPrefab.InstantiatePrefab<AchievementsGUI>();
			this.achievements.rectTransform.SetParent(this.achievementsRoot, false);
			this.achievements.Init(false);
		}
		if (PlatformHelper.ShowDLCMenuItem)
		{
			this.dlcMenu = this.dlcMenuPrefab.InstantiatePrefab<DLCGUI>();
			this.dlcMenu.rectTransform.SetParent(this.dlcMenuRoot, false);
			this.dlcMenu.Init(false);
		}
		if (PlatformHelper.IsConsole)
		{
			PlayerManager.LoadControllerMappings(PlayerId.PlayerOne);
		}
		this.SetRichPresence();
		yield break;
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x0021063F File Offset: 0x0020EA3F
	protected bool GetButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x00210655 File Offset: 0x0020EA55
	private void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "SlotSelect", true);
	}

	// Token: 0x040041EB RID: 16875
	private SlotSelectScreen.State state;

	// Token: 0x040041EC RID: 16876
	[SerializeField]
	private RectTransform LoadingChild;

	// Token: 0x040041ED RID: 16877
	[SerializeField]
	private RectTransform mainMenuChild;

	// Token: 0x040041EE RID: 16878
	[SerializeField]
	private RectTransform slotSelectChild;

	// Token: 0x040041EF RID: 16879
	[SerializeField]
	private RectTransform confirmDeleteChild;

	// Token: 0x040041F0 RID: 16880
	[SerializeField]
	private Text[] mainMenuItems;

	// Token: 0x040041F1 RID: 16881
	[SerializeField]
	private SlotSelectScreenSlot[] slots;

	// Token: 0x040041F2 RID: 16882
	[SerializeField]
	private Text[] confirmDeleteItems;

	// Token: 0x040041F3 RID: 16883
	[SerializeField]
	private RectTransform playerProfiles;

	// Token: 0x040041F4 RID: 16884
	[SerializeField]
	private RectTransform confirmPrompt;

	// Token: 0x040041F5 RID: 16885
	[SerializeField]
	private RectTransform confirmGlyph;

	// Token: 0x040041F6 RID: 16886
	[SerializeField]
	private RectTransform confirmSpacer;

	// Token: 0x040041F7 RID: 16887
	[SerializeField]
	private RectTransform backPrompt;

	// Token: 0x040041F8 RID: 16888
	[SerializeField]
	private RectTransform backGlyph;

	// Token: 0x040041F9 RID: 16889
	[SerializeField]
	private RectTransform backSpacer;

	// Token: 0x040041FA RID: 16890
	[SerializeField]
	private RectTransform storePrompt;

	// Token: 0x040041FB RID: 16891
	[SerializeField]
	private RectTransform storeGlyph;

	// Token: 0x040041FC RID: 16892
	[SerializeField]
	private RectTransform storeSpacer;

	// Token: 0x040041FD RID: 16893
	[SerializeField]
	private RectTransform deletePrompt;

	// Token: 0x040041FE RID: 16894
	[SerializeField]
	private RectTransform deleteGlyph;

	// Token: 0x040041FF RID: 16895
	[SerializeField]
	private RectTransform deleteSpacer;

	// Token: 0x04004200 RID: 16896
	[SerializeField]
	private RectTransform prompts;

	// Token: 0x04004201 RID: 16897
	[SerializeField]
	private Color mainMenuSelectedColor;

	// Token: 0x04004202 RID: 16898
	[SerializeField]
	private Color mainMenuUnselectedColor;

	// Token: 0x04004203 RID: 16899
	[SerializeField]
	private Color confirmDeleteSelectedColor;

	// Token: 0x04004204 RID: 16900
	[SerializeField]
	private Color confirmDeleteUnselectedColor;

	// Token: 0x04004205 RID: 16901
	[SerializeField]
	private OptionsGUI optionsPrefab;

	// Token: 0x04004206 RID: 16902
	[SerializeField]
	private RectTransform optionsRoot;

	// Token: 0x04004207 RID: 16903
	[SerializeField]
	private AchievementsGUI achievementsPrefab;

	// Token: 0x04004208 RID: 16904
	[SerializeField]
	private RectTransform achievementsRoot;

	// Token: 0x04004209 RID: 16905
	[SerializeField]
	private DLCGUI dlcMenuPrefab;

	// Token: 0x0400420A RID: 16906
	[SerializeField]
	private RectTransform dlcMenuRoot;

	// Token: 0x0400420B RID: 16907
	[SerializeField]
	private TMP_Text confirmDeleteSlotTitle;

	// Token: 0x0400420C RID: 16908
	[SerializeField]
	private TMP_Text confirmDeleteSlotSeparator;

	// Token: 0x0400420D RID: 16909
	[SerializeField]
	private TMP_Text confirmDeleteSlotPercentage;

	// Token: 0x0400420E RID: 16910
	private OptionsGUI options;

	// Token: 0x0400420F RID: 16911
	private AchievementsGUI achievements;

	// Token: 0x04004210 RID: 16912
	private DLCGUI dlcMenu;

	// Token: 0x04004211 RID: 16913
	private int _slotSelection;

	// Token: 0x04004212 RID: 16914
	private int _mainMenuSelection;

	// Token: 0x04004213 RID: 16915
	private SlotSelectScreen.MainMenuItem[] _availableMainMenuItems;

	// Token: 0x04004214 RID: 16916
	private int _confirmDeleteSelection;

	// Token: 0x04004215 RID: 16917
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04004216 RID: 16918
	private bool isConsole;

	// Token: 0x04004217 RID: 16919
	private const string PATH = "Audio/TitleScreenAudio";

	// Token: 0x04004218 RID: 16920
	private float timeSinceStart;

	// Token: 0x04004219 RID: 16921
	private SlotSelectScreen.SaveDataStatus dataStatus;

	// Token: 0x020009AB RID: 2475
	public enum State
	{
		// Token: 0x0400421B RID: 16923
		InitializeStorage,
		// Token: 0x0400421C RID: 16924
		MainMenu,
		// Token: 0x0400421D RID: 16925
		AchievementsMenu,
		// Token: 0x0400421E RID: 16926
		OptionsMenu,
		// Token: 0x0400421F RID: 16927
		DLC,
		// Token: 0x04004220 RID: 16928
		SlotSelect,
		// Token: 0x04004221 RID: 16929
		ConfirmDelete,
		// Token: 0x04004222 RID: 16930
		PlayerSelect
	}

	// Token: 0x020009AC RID: 2476
	public enum MainMenuItem
	{
		// Token: 0x04004224 RID: 16932
		Start,
		// Token: 0x04004225 RID: 16933
		Achievements,
		// Token: 0x04004226 RID: 16934
		Options,
		// Token: 0x04004227 RID: 16935
		DLC,
		// Token: 0x04004228 RID: 16936
		Exit
	}

	// Token: 0x020009AD RID: 2477
	public enum ConfirmDeleteItem
	{
		// Token: 0x0400422A RID: 16938
		Yes,
		// Token: 0x0400422B RID: 16939
		No
	}

	// Token: 0x020009AE RID: 2478
	private enum SaveDataStatus
	{
		// Token: 0x0400422D RID: 16941
		Uninitialized,
		// Token: 0x0400422E RID: 16942
		Received,
		// Token: 0x0400422F RID: 16943
		Initialized
	}
}
