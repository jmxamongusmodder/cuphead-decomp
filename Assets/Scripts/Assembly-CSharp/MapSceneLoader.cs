using System;
using UnityEngine;

// Token: 0x0200095D RID: 2397
public class MapSceneLoader : AbstractMapInteractiveEntity
{
	// Token: 0x060037F8 RID: 14328 RVA: 0x001FC008 File Offset: 0x001FA408
	protected override void Activate(MapPlayerController player)
	{
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		base.SetPlayerReturnPos();
		if (this.askDifficulty)
		{
			if (this.scene == Scenes.scene_cutscene_kingdice)
			{
				MapDifficultySelectStartUI.Current.level = "DicePalaceMain";
			}
			MapDifficultySelectStartUI.Current.In(player);
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent += this.OnBack;
		}
		else
		{
			if (this.scene == Scenes.scene_map_world_1)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseRight;
				MapBasicStartUI.Current.level = "MapWorld_1";
			}
			else if (this.scene == Scenes.scene_map_world_2)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseRight;
				MapBasicStartUI.Current.level = "MapWorld_2";
			}
			else if (this.scene == Scenes.scene_map_world_3)
			{
				if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
				{
					MapBasicStartUI.Current.level = "KingDiceToWorld3WorldMap";
				}
				else
				{
					MapBasicStartUI.Current.level = "MapWorld_3";
				}
			}
			else if (this.scene == Scenes.scene_map_world_4)
			{
				MapBasicStartUI.Current.level = "Inkwell";
			}
			else if (this.scene == Scenes.scene_cutscene_kingdice)
			{
				MapBasicStartUI.Current.level = "KingDice";
			}
			MapBasicStartUI.Current.In(player);
			MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent += this.OnBack;
		}
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x001FC1B0 File Offset: 0x001FA5B0
	private void OnLoadLevel()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5f);
		AudioNoiseHandler.Instance.BoingSound();
		this.LoadScene();
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x001FC1E8 File Offset: 0x001FA5E8
	protected virtual void LoadScene()
	{
		MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
		SceneLoader.LoadScene(this.scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x001FC25C File Offset: 0x001FA65C
	private void OnBack()
	{
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x001FC2CF File Offset: 0x001FA6CF
	protected override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>");
	}

	// Token: 0x04003FE3 RID: 16355
	[SerializeField]
	protected Scenes scene;

	// Token: 0x04003FE4 RID: 16356
	[SerializeField]
	private bool askDifficulty;
}
