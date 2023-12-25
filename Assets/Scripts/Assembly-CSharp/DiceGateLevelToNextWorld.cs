using System;
using System.Collections;

// Token: 0x0200059B RID: 1435
public class DiceGateLevelToNextWorld : AbstractLevelInteractiveEntity
{
	// Token: 0x06001B7F RID: 7039 RVA: 0x000FB2AD File Offset: 0x000F96AD
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x000FB2CE File Offset: 0x000F96CE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000FB2E4 File Offset: 0x000F96E4
	private IEnumerator go_cr()
	{
		this.activated = true;
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0f);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.DisableInput();
				levelPlayerController.PauseAll();
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted)
			{
				SceneLoader.LoadScene(Scenes.scene_map_world_2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				Cutscene.Load(Scenes.scene_map_world_2, Scenes.scene_cutscene_world2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass);
			}
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted)
			{
				SceneLoader.LoadScene(Scenes.scene_map_world_3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				Cutscene.Load(Scenes.scene_map_world_3, Scenes.scene_cutscene_world3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass);
			}
		}
		yield break;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x000FB2FF File Offset: 0x000F96FF
	protected override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Right, false);
	}

	// Token: 0x040024A8 RID: 9384
	private bool activated;
}
