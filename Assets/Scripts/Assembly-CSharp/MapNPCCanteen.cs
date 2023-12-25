using System;
using UnityEngine;

// Token: 0x0200094B RID: 2379
public class MapNPCCanteen : MonoBehaviour
{
	// Token: 0x06003797 RID: 14231 RVA: 0x001FEFBC File Offset: 0x001FD3BC
	private void Start()
	{
		if (PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Weapon.plane_weapon_bomb) && (PlayerManager.Multiplayer || PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb)))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
		}
		this.AddDialoguerEvents();
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x001FF013 File Offset: 0x001FD413
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x001FF01B File Offset: 0x001FD41B
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x001FF033 File Offset: 0x001FD433
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x001FF04C File Offset: 0x001FD44C
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "CanteenWeaponTwo")
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Canteen);
			if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Weapon.plane_weapon_bomb))
			{
				PlayerData.Data.Gift(PlayerId.PlayerOne, Weapon.plane_weapon_bomb);
				if (!PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).HasEquippedSecondarySHMUPWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).MustNotifySwitchSHMUPWeapon = true;
				}
				PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).HasEquippedSecondarySHMUPWeapon = true;
			}
			if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb))
			{
				PlayerData.Data.Gift(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb);
				if (!PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).HasEquippedSecondarySHMUPWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).MustNotifySwitchSHMUPWeapon = true;
				}
				PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).HasEquippedSecondarySHMUPWeapon = true;
			}
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x04003FA4 RID: 16292
	[SerializeField]
	private int dialoguerVariableID = 13;

	// Token: 0x04003FA5 RID: 16293
	[HideInInspector]
	public bool SkipDialogueEvent;
}
