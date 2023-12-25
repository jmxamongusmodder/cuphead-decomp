using System;

// Token: 0x0200092F RID: 2351
public class MapBakeryLoader : AbstractMapInteractiveEntity
{
	// Token: 0x06003701 RID: 14081 RVA: 0x001FB2F8 File Offset: 0x001F96F8
	private void Start()
	{
		if (PlayerData.Data.shouldShowChaliceTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Chalice);
			PlayerData.Data.shouldShowChaliceTooltip = false;
		}
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x001FB320 File Offset: 0x001F9720
	protected override void Activate(MapPlayerController player)
	{
		if (AbstractMapInteractiveEntity.HasPopupOpened)
		{
			return;
		}
		AbstractMapInteractiveEntity.HasPopupOpened = true;
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		if (PlayerData.Data.GetLevelData(Levels.Saltbaker).played && !this.HoldingLAndR())
		{
			MapDifficultySelectStartUI.Current.level = "Saltbaker";
			MapDifficultySelectStartUI.Current.In(player);
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent += this.OnBack;
			this.loadKitchen = false;
		}
		else
		{
			MapBasicStartUI.Current.level = "BakeryWorldMap";
			MapBasicStartUI.Current.In(player);
			MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent += this.OnBack;
			this.loadKitchen = true;
		}
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x001FB4C0 File Offset: 0x001F98C0
	private bool HoldingLAndR()
	{
		return (Map.Current.players[0] && Map.Current.players[0].input.actions.GetButton(11) && Map.Current.players[0].input.actions.GetButton(12)) || (Map.Current.players[1] && Map.Current.players[1].input.actions.GetButton(11) && Map.Current.players[1].input.actions.GetButton(12));
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x001FB586 File Offset: 0x001F9986
	private void OnLoadLevel()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		AudioNoiseHandler.Instance.BoingSound();
		if (this.loadKitchen)
		{
			SceneLoader.LoadScene(Scenes.scene_level_kitchen, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
		}
		else
		{
			SceneLoader.LoadScene(Scenes.scene_level_saltbaker, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x001FB5C0 File Offset: 0x001F99C0
	private void OnBack()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		MapConfirmStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapConfirmStartUI.Current.OnBackEvent -= this.OnBack;
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x04003F35 RID: 16181
	private MapPlayerController player1;

	// Token: 0x04003F36 RID: 16182
	private MapPlayerController player2;

	// Token: 0x04003F37 RID: 16183
	private bool p1InTrigger;

	// Token: 0x04003F38 RID: 16184
	private bool p2InTrigger;

	// Token: 0x04003F39 RID: 16185
	private bool loadKitchen;
}
