using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000483 RID: 1155
public class LevelPauseGUI : AbstractPauseGUI
{
	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x060011E9 RID: 4585 RVA: 0x000A788A File Offset: 0x000A5C8A
	// (set) Token: 0x060011EA RID: 4586 RVA: 0x000A7891 File Offset: 0x000A5C91
	public static Color COLOR_SELECTED { get; private set; }

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x060011EB RID: 4587 RVA: 0x000A7899 File Offset: 0x000A5C99
	// (set) Token: 0x060011EC RID: 4588 RVA: 0x000A78A0 File Offset: 0x000A5CA0
	public static Color COLOR_INACTIVE { get; private set; }

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x060011ED RID: 4589 RVA: 0x000A78A8 File Offset: 0x000A5CA8
	// (remove) Token: 0x060011EE RID: 4590 RVA: 0x000A78DC File Offset: 0x000A5CDC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnPauseEvent;

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x060011EF RID: 4591 RVA: 0x000A7910 File Offset: 0x000A5D10
	// (remove) Token: 0x060011F0 RID: 4592 RVA: 0x000A7944 File Offset: 0x000A5D44
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnUnpauseEvent;

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x060011F1 RID: 4593 RVA: 0x000A7978 File Offset: 0x000A5D78
	// (set) Token: 0x060011F2 RID: 4594 RVA: 0x000A7980 File Offset: 0x000A5D80
	private int selection
	{
		get
		{
			return this._selection;
		}
		set
		{
			bool flag = value > this._selection;
			int num = (int)Mathf.Repeat((float)value, (float)this.menuItems.Length);
			while (!this.menuItems[num].gameObject.activeSelf)
			{
				num = ((!flag) ? (num - 1) : (num + 1));
				num = (int)Mathf.Repeat((float)num, (float)this.menuItems.Length);
			}
			this._selection = num;
			this.UpdateSelection();
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x060011F3 RID: 4595 RVA: 0x000A79F8 File Offset: 0x000A5DF8
	protected override bool CanPause
	{
		get
		{
			return Level.Current.Started && !Level.Current.Ending && PauseManager.state != PauseManager.State.Paused && !SceneLoader.CurrentlyLoading && !this.forceDisablePause;
		}
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x000A7A44 File Offset: 0x000A5E44
	private void OnEnable()
	{
		Localization.OnLanguageChangedEvent += this.onLanguageChangedEventHandler;
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x000A7A57 File Offset: 0x000A5E57
	private void OnDisable()
	{
		Localization.OnLanguageChangedEvent -= this.onLanguageChangedEventHandler;
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x000A7A6A File Offset: 0x000A5E6A
	protected override void Awake()
	{
		base.Awake();
		LevelPauseGUI.COLOR_SELECTED = this.menuItems[0].color;
		LevelPauseGUI.COLOR_INACTIVE = this.menuItems[this.menuItems.Length - 1].color;
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x000A7A9F File Offset: 0x000A5E9F
	public override void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements)
	{
		this.Init(checkIfDead, options, achievements, null);
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x000A7AAC File Offset: 0x000A5EAC
	public override void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements, RestartTowerConfirmGUI restartTowerConfirm)
	{
		base.Init(checkIfDead, options, achievements);
		this.options = options;
		this.achievements = achievements;
		this.restartTowerConfirm = restartTowerConfirm;
		if (PlatformHelper.IsConsole && this.menuItems.Length > 7)
		{
			this.menuItems[7].gameObject.SetActive(false);
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			this.menuItems[2].gameObject.SetActive(true);
			this.updateRotateControlsToggleVisualValue();
		}
		else if (!PlatformHelper.ShowAchievements && this.menuItems.Length > 2)
		{
			this.menuItems[2].gameObject.SetActive(false);
		}
		if (Level.IsTowerOfPower)
		{
			this.ReplaceRestartWRestartTowerOfPower();
		}
		options.Init(checkIfDead);
		if (achievements != null)
		{
			achievements.Init(checkIfDead);
		}
		if (restartTowerConfirm != null)
		{
			restartTowerConfirm.Init(checkIfDead);
		}
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x000A7BAF File Offset: 0x000A5FAF
	public void ForceDisablePause(bool value)
	{
		this.forceDisablePause = value;
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x000A7BB8 File Offset: 0x000A5FB8
	protected override void OnPause()
	{
		base.OnPause();
		if (CupheadLevelCamera.Current != null)
		{
			CupheadLevelCamera.Current.StartBlur();
		}
		else
		{
			CupheadMapCamera.Current.StartBlur();
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		this.menuItems[4].gameObject.SetActive(PlayerManager.Multiplayer);
		if (LevelPauseGUI.OnPauseEvent != null)
		{
			LevelPauseGUI.OnPauseEvent();
		}
		this.selection = 0;
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x000A7C44 File Offset: 0x000A6044
	protected override void OnUnpause()
	{
		base.OnUnpause();
		if (CupheadLevelCamera.Current != null)
		{
			CupheadLevelCamera.Current.EndBlur();
		}
		else
		{
			CupheadMapCamera.Current.EndBlur();
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			SettingsData.Save();
			if (PlatformHelper.IsConsole)
			{
				SettingsData.SaveToCloud();
			}
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		if (LevelPauseGUI.OnUnpauseEvent != null)
		{
			LevelPauseGUI.OnUnpauseEvent();
		}
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x000A7CE2 File Offset: 0x000A60E2
	private void OnDestroy()
	{
		PauseManager.Unpause();
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x000A7CEC File Offset: 0x000A60EC
	protected override void Update()
	{
		base.Update();
		if (base.state != AbstractPauseGUI.State.Paused || this.options.optionMenuOpen || this.options.justClosed || (this.achievements != null && (this.achievements.achievementsMenuOpen || this.achievements.justClosed)) || (this.restartTowerConfirm != null && (this.restartTowerConfirm.restartTowerConfirmMenuOpen || this.restartTowerConfirm.justClosed)))
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Pause) || base.GetButtonDown(CupheadButton.Cancel))
		{
			this.Unpause();
			return;
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane && this.selection == 2 && (base.GetButtonDown(CupheadButton.Accept) || base.GetButtonDown(CupheadButton.MenuLeft) || base.GetButtonDown(CupheadButton.MenuRight)))
		{
			base.MenuSelectSound();
			this.ToggleRotateControls();
			return;
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.MenuSelectSound();
			this.Select();
			return;
		}
		if (this._selectionTimer >= 0.15f)
		{
			if (base.GetButton(CupheadButton.MenuUp))
			{
				base.MenuMoveSound();
				this.selection--;
			}
			if (base.GetButton(CupheadButton.MenuDown))
			{
				base.MenuMoveSound();
				this.selection++;
			}
		}
		else
		{
			this._selectionTimer += Time.deltaTime;
		}
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x000A7E98 File Offset: 0x000A6298
	private void Select()
	{
		switch (this.selection)
		{
		case 0:
			this.Unpause();
			break;
		case 1:
			this.Restart();
			break;
		case 2:
			this.Achievements();
			break;
		case 3:
			this.Options();
			break;
		case 4:
			this.Player2Leave();
			break;
		case 5:
			this.Exit();
			break;
		case 6:
			this.ExitToTitle();
			break;
		case 7:
			this.ExitToDesktop();
			break;
		}
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x000A7F2F File Offset: 0x000A632F
	protected override void OnUnpauseSound()
	{
		base.OnUnpauseSound();
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000A7F38 File Offset: 0x000A6338
	private void UpdateSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = LevelPauseGUI.COLOR_SELECTED;
			}
			else
			{
				text.color = LevelPauseGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x000A7F9C File Offset: 0x000A639C
	private void Restart()
	{
		if (Level.IsTowerOfPower)
		{
			this.RestartTowerConfirm();
		}
		else
		{
			this.OnUnpauseSound();
			base.state = AbstractPauseGUI.State.Animating;
			PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
			PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
			SceneLoader.ReloadLevel();
			Dialoguer.EndDialogue();
			if (Level.IsDicePalaceMain || Level.IsDicePalace)
			{
				DicePalaceMainLevelGameInfo.CleanUpRetry();
			}
		}
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x000A7FFC File Offset: 0x000A63FC
	private void ReplaceRestartWRestartTowerOfPower()
	{
		this.retryLocHelper.currentID = Localization.Find("OptionMenuRestartTower").id;
		this.retryLocHelper.ApplyTranslation();
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x000A8023 File Offset: 0x000A6423
	private void Exit()
	{
		base.state = AbstractPauseGUI.State.Animating;
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		Dialoguer.EndDialogue();
		if (Level.IsDicePalaceMain || Level.IsDicePalace)
		{
			DicePalaceMainLevelGameInfo.CleanUpRetry();
		}
		SceneLoader.LoadLastMap();
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x000A805D File Offset: 0x000A645D
	private void Player2Leave()
	{
		PlayerManager.PlayerLeave(PlayerId.PlayerTwo);
		this.Unpause();
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x000A806B File Offset: 0x000A646B
	private void ExitToTitle()
	{
		base.state = AbstractPauseGUI.State.Animating;
		PlayerManager.ResetPlayers();
		Dialoguer.EndDialogue();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x000A8088 File Offset: 0x000A6488
	private void ExitToDesktop()
	{
		Dialoguer.EndDialogue();
		Application.Quit();
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x000A8094 File Offset: 0x000A6494
	private void Options()
	{
		base.StartCoroutine(this.in_options_cr());
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x000A80A4 File Offset: 0x000A64A4
	private IEnumerator in_options_cr()
	{
		this.HideImmediate();
		this.options.ShowMainOptionMenu();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		while (this.options.optionMenuOpen)
		{
			yield return null;
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		this.selection = 0;
		this.ShowImmediate();
		yield return null;
		yield break;
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x000A80BF File Offset: 0x000A64BF
	private void RestartTowerConfirm()
	{
		base.StartCoroutine(this.in_restarttowerconfirm_cr());
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000A80D0 File Offset: 0x000A64D0
	private IEnumerator in_restarttowerconfirm_cr()
	{
		this.HideImmediate();
		this.restartTowerConfirm.ShowMenu();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		while (this.restartTowerConfirm.restartTowerConfirmMenuOpen)
		{
			yield return null;
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		this.selection = 0;
		this.ShowImmediate();
		yield return null;
		yield break;
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x000A80EB File Offset: 0x000A64EB
	private void Achievements()
	{
		base.StartCoroutine(this.in_achievements_cr());
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x000A80FC File Offset: 0x000A64FC
	private IEnumerator in_achievements_cr()
	{
		this.HideImmediate();
		this.achievements.ShowAchievements();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		while (this.achievements.achievementsMenuOpen)
		{
			yield return null;
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		this.selection = 0;
		this.ShowImmediate();
		yield return null;
		yield break;
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x000A8117 File Offset: 0x000A6517
	private void ToggleRotateControls()
	{
		SettingsData.Data.rotateControlsWithCamera = !SettingsData.Data.rotateControlsWithCamera;
		this.updateRotateControlsToggleVisualValue();
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x000A8138 File Offset: 0x000A6538
	private void updateRotateControlsToggleVisualValue()
	{
		Text text = this.menuItems[2];
		text.GetComponent<LocalizationHelper>().ApplyTranslation(Localization.Find("CameraRotationControl"), null);
		text.text = string.Format(text.text, (!SettingsData.Data.rotateControlsWithCamera) ? "A" : "B");
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x000A8193 File Offset: 0x000A6593
	private void onLanguageChangedEventHandler()
	{
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			base.StartCoroutine(this.changeRotationToggleLanguage_cr());
		}
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x000A81C8 File Offset: 0x000A65C8
	private IEnumerator changeRotationToggleLanguage_cr()
	{
		yield return null;
		yield return null;
		yield return null;
		this.updateRotateControlsToggleVisualValue();
		yield break;
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x000A81E3 File Offset: 0x000A65E3
	protected override void InAnimation(float i)
	{
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x000A81E5 File Offset: 0x000A65E5
	protected override void OutAnimation(float i)
	{
	}

	// Token: 0x04001B73 RID: 7027
	[SerializeField]
	private Text[] menuItems;

	// Token: 0x04001B74 RID: 7028
	private OptionsGUI options;

	// Token: 0x04001B75 RID: 7029
	private AchievementsGUI achievements;

	// Token: 0x04001B76 RID: 7030
	private RestartTowerConfirmGUI restartTowerConfirm;

	// Token: 0x04001B77 RID: 7031
	private float _selectionTimer;

	// Token: 0x04001B78 RID: 7032
	private const float _SELECTION_TIME = 0.15f;

	// Token: 0x04001B79 RID: 7033
	[SerializeField]
	private LocalizationHelper retryLocHelper;

	// Token: 0x04001B7A RID: 7034
	private int _selection;

	// Token: 0x04001B7B RID: 7035
	private bool forceDisablePause;

	// Token: 0x02000484 RID: 1156
	private enum MenuItems
	{
		// Token: 0x04001B7D RID: 7037
		Unpause,
		// Token: 0x04001B7E RID: 7038
		Restart,
		// Token: 0x04001B7F RID: 7039
		Achievements,
		// Token: 0x04001B80 RID: 7040
		Options,
		// Token: 0x04001B81 RID: 7041
		Player2Leave,
		// Token: 0x04001B82 RID: 7042
		ExitToMap,
		// Token: 0x04001B83 RID: 7043
		ExitToTitle,
		// Token: 0x04001B84 RID: 7044
		ExitToDesktop
	}
}
