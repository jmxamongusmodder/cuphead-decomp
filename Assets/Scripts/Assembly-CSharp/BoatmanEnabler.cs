using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200092C RID: 2348
public class BoatmanEnabler : MapLevelDependentObstacle
{
	// Token: 0x060036F6 RID: 14070 RVA: 0x001FAD18 File Offset: 0x001F9118
	protected override void Start()
	{
		if (DLCManager.DLCEnabled())
		{
			base.StartCoroutine(this.check_cr());
		}
	}

	// Token: 0x060036F7 RID: 14071 RVA: 0x001FAD34 File Offset: 0x001F9134
	private IEnumerator check_cr()
	{
		if (this.forceBoatmanUnlocking || PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.OnConditionAlreadyMet();
		}
		else if (!PlayerData.Data.hasUnlockedBoatman)
		{
			if (PlayerData.Data.hasUnlockedFirstSuper)
			{
				PlayerData.Data.shouldShowBoatmanTooltip = true;
				while (!MapEventNotification.Current.showing)
				{
					yield return null;
				}
				while (MapEventNotification.Current.showing)
				{
					yield return null;
				}
				base.StartCoroutine(this.showAppear_cr());
			}
			else if (PlayerData.Data.GetLevelData(Levels.Mausoleum).completed)
			{
				while (AbstractEquipUI.Current.CurrentState == AbstractEquipUI.ActiveState.Inactive)
				{
					yield return null;
				}
				while (AbstractEquipUI.Current.CurrentState == AbstractEquipUI.ActiveState.Active)
				{
					yield return null;
				}
				base.StartCoroutine(this.showAppear_cr());
			}
		}
		else
		{
			this.OnConditionAlreadyMet();
		}
		yield break;
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x001FAD50 File Offset: 0x001F9150
	private IEnumerator showAppear_cr()
	{
		Map.Current.CurrentState = Map.State.Event;
		MapEventNotification.Current.showing = true;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		CupheadMapCamera cupheadMapCamera = UnityEngine.Object.FindObjectOfType<CupheadMapCamera>();
		Vector3 cameraStartPos = cupheadMapCamera.transform.position;
		if (this.panCamera)
		{
			yield return cupheadMapCamera.MoveToPosition(base.CameraPosition, 0.5f, 0.9f);
		}
		base.MapMeetCondition();
		while (base.CurrentState != AbstractMapLevelDependentEntity.State.Complete)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		if (this.panCamera)
		{
			cupheadMapCamera.MoveToPosition(cameraStartPos, 0.75f, 1f);
		}
		Map.Current.CurrentState = Map.State.Ready;
		MapEventNotification.Current.showing = false;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		yield break;
	}

	// Token: 0x04003F29 RID: 16169
	private bool forceBoatmanUnlocking;
}
