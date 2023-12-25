using System;
using UnityEngine;

// Token: 0x0200093D RID: 2365
public class MapLevelLoader : AbstractMapInteractiveEntity
{
	// Token: 0x06003759 RID: 14169 RVA: 0x001FD4CE File Offset: 0x001FB8CE
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x001FD4D8 File Offset: 0x001FB8D8
	protected override void Activate(MapPlayerController player)
	{
		if (AbstractMapInteractiveEntity.HasPopupOpened)
		{
			return;
		}
		if (PlatformHelper.ManuallyRefreshDLCAvailability)
		{
			DLCManager.CheckInstallationStatusChanged();
			if (DLCManager.showAvailabilityPrompt)
			{
				DLCManager.ResetAvailabilityPrompt();
				MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DLCAvailable);
				return;
			}
		}
		AbstractMapInteractiveEntity.HasPopupOpened = true;
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		Map.Current.OnLoadLevel();
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		if (this.askDifficulty)
		{
			MapDifficultySelectStartUI.Current.level = this.level.ToString();
			MapDifficultySelectStartUI.Current.In(player);
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent += this.OnBack;
		}
		else
		{
			string a = this.level.ToString();
			if (a != "Mausoleum" && a != "Devil" && a != "ShmupTutorial" && a != "ChaliceTutorial" && a != "ChessCastle")
			{
				MapConfirmStartUI.Current.level = a;
				MapConfirmStartUI.Current.In(player);
				MapConfirmStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
				MapConfirmStartUI.Current.OnBackEvent += this.OnBack;
				return;
			}
			if (a == "Mausoleum")
			{
				if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
				{
					a = "Mausoleum_1";
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
				{
					a = "Mausoleum_2";
				}
				else
				{
					a = "Mausoleum_3";
				}
			}
			else if (a == "ChessCastle")
			{
				a = "KingOfGamesWorldMap";
			}
			MapBasicStartUI.Current.level = a;
			MapBasicStartUI.Current.In(player);
			MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent += this.OnBack;
		}
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x001FD79C File Offset: 0x001FBB9C
	private void OnLoadLevel()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5f);
		AudioNoiseHandler.Instance.BoingSound();
		if (this.level == Levels.Devil)
		{
			SceneLoader.LoadScene(Scenes.scene_cutscene_devil, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			SceneLoader.LoadLevel(this.level, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x001FD800 File Offset: 0x001FBC00
	private void OnBack()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		if (this.askDifficulty)
		{
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		}
		else
		{
			MapConfirmStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
			MapConfirmStartUI.Current.OnBackEvent -= this.OnBack;
			MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
		}
		if (this.OnBackCallback != null)
		{
			this.OnBackCallback();
			this.OnBackCallback = (Action)Delegate.Remove(this.OnBackCallback, this.OnBackCallback);
		}
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x001FD8E7 File Offset: 0x001FBCE7
	protected override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>");
	}

	// Token: 0x04003F73 RID: 16243
	[SerializeField]
	private Levels level;

	// Token: 0x04003F74 RID: 16244
	[SerializeField]
	private bool askDifficulty;

	// Token: 0x04003F75 RID: 16245
	public Action OnBackCallback;
}
