using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000833 RID: 2099
public class TutorialLevelDoor : AbstractLevelInteractiveEntity
{
	// Token: 0x060030AF RID: 12463 RVA: 0x001CA2A3 File Offset: 0x001C86A3
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x001CA2C4 File Offset: 0x001C86C4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x001CA2D8 File Offset: 0x001C86D8
	private IEnumerator go_cr()
	{
		this.activated = true;
		LevelCoin.OnLevelComplete();
		if (this.isChaliceTutorial)
		{
			PlayerData.Data.IsChaliceTutorialCompleted = true;
		}
		else
		{
			PlayerData.Data.IsTutorialCompleted = true;
		}
		PlayerData.SaveCurrentFile();
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.DisableInput();
				levelPlayerController.PauseAll();
			}
		}
		TutorialLevel level = Level.Current as TutorialLevel;
		if (level)
		{
			level.GoBackToHouse();
		}
		else
		{
			ChaliceTutorialLevel chaliceTutorialLevel = Level.Current as ChaliceTutorialLevel;
			if (chaliceTutorialLevel)
			{
				chaliceTutorialLevel.Exit();
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		if (this.isChaliceTutorial)
		{
			SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			SceneLoader.LoadScene(Scenes.scene_level_house_elder_kettle, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		yield break;
	}

	// Token: 0x04003952 RID: 14674
	private bool activated;

	// Token: 0x04003953 RID: 14675
	[SerializeField]
	private bool isChaliceTutorial;
}
