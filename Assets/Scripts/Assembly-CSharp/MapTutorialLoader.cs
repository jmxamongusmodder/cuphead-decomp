using System;

// Token: 0x02000965 RID: 2405
public class MapTutorialLoader : AbstractMapInteractiveEntity
{
	// Token: 0x06003811 RID: 14353 RVA: 0x0020154C File Offset: 0x001FF94C
	protected override void Activate(MapPlayerController player)
	{
		base.Activate(player);
		AudioManager.Play("world_map_level_difficulty_appear");
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		Map.Current.OnLoadShop();
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		MapBasicStartUI.Current.level = "ElderKettleLevel";
		MapBasicStartUI.Current.In(player);
		MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent += this.OnBack;
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x00201668 File Offset: 0x001FFA68
	private void OnLoadLevel()
	{
		AudioNoiseHandler.Instance.BoingSound();
		SceneLoader.LoadScene(Scenes.scene_level_house_elder_kettle, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x0020167F File Offset: 0x001FFA7F
	private void OnBack()
	{
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}
}
