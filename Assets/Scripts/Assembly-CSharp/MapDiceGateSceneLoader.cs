using System;
using UnityEngine;

// Token: 0x02000935 RID: 2357
public class MapDiceGateSceneLoader : AbstractMapInteractiveEntity
{
	// Token: 0x0600371C RID: 14108 RVA: 0x001FBD88 File Offset: 0x001FA188
	protected override void Activate(MapPlayerController player)
	{
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		Map.Current.OnLoadLevel();
		base.SetPlayerReturnPos();
		if (this.nextWorld == Scenes.scene_map_world_1)
		{
			MapBasicStartUI.Current.level = "MapWorld_1";
		}
		else if (this.nextWorld == Scenes.scene_map_world_2)
		{
			MapBasicStartUI.Current.level = ((!PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted) ? "DieHouse" : "MapWorld_2");
		}
		else if (this.nextWorld == Scenes.scene_map_world_3)
		{
			MapBasicStartUI.Current.level = ((!PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted) ? "DieHouse" : "MapWorld_3");
		}
		else if (this.nextWorld == Scenes.scene_map_world_4)
		{
			MapBasicStartUI.Current.level = "Inkwell";
		}
		MapBasicStartUI.Current.In(player);
		MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent += this.OnBack;
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x001FBEB0 File Offset: 0x001FA2B0
	private void OnLoadLevel()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5f);
		AudioNoiseHandler.Instance.BoingSound();
		this.CheckSceneToLoad();
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x001FBEE8 File Offset: 0x001FA2E8
	private void CheckSceneToLoad()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseLeft;
				SceneLoader.LoadScene(this.nextWorld, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				SceneLoader.LoadScene(this.diceGate, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_3).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseLeft;
				SceneLoader.LoadScene(this.nextWorld, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				SceneLoader.LoadScene(this.diceGate, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
		}
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x001FBFAC File Offset: 0x001FA3AC
	private void OnBack()
	{
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x001FBFE8 File Offset: 0x001FA3E8
	protected override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>");
	}

	// Token: 0x04003F4E RID: 16206
	[SerializeField]
	private Scenes nextWorld;

	// Token: 0x04003F4F RID: 16207
	private readonly Scenes diceGate = Scenes.scene_level_dice_gate;

	// Token: 0x04003F50 RID: 16208
	[SerializeField]
	private bool askDifficulty;
}
