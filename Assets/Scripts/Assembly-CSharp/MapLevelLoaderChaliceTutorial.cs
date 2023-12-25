using System;

// Token: 0x0200093E RID: 2366
public class MapLevelLoaderChaliceTutorial : MapLevelLoader
{
	// Token: 0x0600375F RID: 14175 RVA: 0x001FD907 File Offset: 0x001FBD07
	protected override void Activate(MapPlayerController player)
	{
		if (PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).charm == Charm.charm_chalice)
		{
			base.Activate(player);
		}
		else
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ChaliceTutorialEquipCharm);
		}
	}
}
