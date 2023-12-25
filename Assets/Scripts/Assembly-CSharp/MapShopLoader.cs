using System;
using UnityEngine;

// Token: 0x02000961 RID: 2401
public class MapShopLoader : AbstractMapInteractiveEntity
{
	// Token: 0x06003807 RID: 14343 RVA: 0x002012E0 File Offset: 0x001FF6E0
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
		Map.Current.OnLoadShop();
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		MapBasicStartUI.Current.level = "Shop";
		MapBasicStartUI.Current.In(player);
		MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent += this.OnBack;
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x0020140D File Offset: 0x001FF80D
	private void OnLoadLevel()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		AudioNoiseHandler.Instance.BoingSound();
		SceneLoader.LoadScene((!this.isDLCShop) ? Scenes.scene_shop : Scenes.scene_shop_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x0020143C File Offset: 0x001FF83C
	private void OnBack()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x04003FEC RID: 16364
	[SerializeField]
	private bool isDLCShop;
}
