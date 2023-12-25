using System;

// Token: 0x02000988 RID: 2440
public class MapEquipUI : AbstractEquipUI
{
	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x0600390E RID: 14606 RVA: 0x00205F80 File Offset: 0x00204380
	protected override bool CanPause
	{
		get
		{
			return Map.Current.CurrentState == Map.State.Ready && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard);
		}
	}

	// Token: 0x0600390F RID: 14607 RVA: 0x00205FF5 File Offset: 0x002043F5
	protected override void OnPause()
	{
		base.OnPause();
		CupheadMapCamera.Current.StartBlur();
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x00206007 File Offset: 0x00204407
	protected override void OnUnpause()
	{
		base.OnUnpause();
		CupheadMapCamera.Current.EndBlur();
	}

	// Token: 0x06003911 RID: 14609 RVA: 0x0020601C File Offset: 0x0020441C
	protected override void OnPauseAudio()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.EquipMenu.ToString(), 0.15f);
		AudioManager.PauseAllSFX();
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x00206048 File Offset: 0x00204448
	protected override void OnUnpauseAudio()
	{
		AudioManager.SnapshotReset(SceneLoader.SceneName, 0.1f);
	}
}
