using System;
using System.Collections;

// Token: 0x0200059C RID: 1436
public class DiceGateLevelToPrevWorld : AbstractLevelInteractiveEntity
{
	// Token: 0x06001B84 RID: 7044 RVA: 0x000FB4E3 File Offset: 0x000F98E3
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000FB504 File Offset: 0x000F9904
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000FB518 File Offset: 0x000F9918
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
		PlayerData.Data.CurrentMapData.hasVisitedDieHouse = true;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000FB533 File Offset: 0x000F9933
	protected override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Left, false);
	}

	// Token: 0x040024A9 RID: 9385
	private bool activated;
}
