using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public class ShmupTutorialExitSign : AbstractLevelInteractiveEntity
{
	// Token: 0x060030A6 RID: 12454 RVA: 0x001C9FBE File Offset: 0x001C83BE
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x001C9FDF File Offset: 0x001C83DF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x001C9FF4 File Offset: 0x001C83F4
	private IEnumerator go_cr()
	{
		this.activated = true;
		PlayerData.SaveCurrentFile();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0f);
		foreach (PlanePlayerController planePlayerController in UnityEngine.Object.FindObjectsOfType<PlanePlayerController>())
		{
			planePlayerController.PauseAll();
		}
		foreach (PlaneSuperBomb planeSuperBomb in UnityEngine.Object.FindObjectsOfType<PlaneSuperBomb>())
		{
			planeSuperBomb.Pause();
		}
		foreach (PlaneSuperChalice planeSuperChalice in UnityEngine.Object.FindObjectsOfType<PlaneSuperChalice>())
		{
			planeSuperChalice.Pause();
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		SceneLoader.LoadScene(Scenes.scene_map_world_1, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x0400394C RID: 14668
	private bool activated;
}
