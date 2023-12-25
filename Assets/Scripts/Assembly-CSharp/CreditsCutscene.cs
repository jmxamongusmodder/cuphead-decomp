using System;
using System.Collections;

// Token: 0x020003F6 RID: 1014
public class CreditsCutscene : Cutscene
{
	// Token: 0x06000DCD RID: 3533 RVA: 0x0008F6EA File Offset: 0x0008DAEA
	protected override void Start()
	{
		base.Start();
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.music_cr());
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x0008F710 File Offset: 0x0008DB10
	private IEnumerator music_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (CreditsScreen.goodEnding)
		{
			AudioManager.PlayBGM();
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "GoodEnding");
		}
		else
		{
			AudioManager.PlayBGMPlaylistManually(true);
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "BadEnding");
		}
		yield break;
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0008F72B File Offset: 0x0008DB2B
	protected override void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Ending", true);
	}
}
