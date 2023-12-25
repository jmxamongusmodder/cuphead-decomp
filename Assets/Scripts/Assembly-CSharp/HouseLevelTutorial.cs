using System;
using System.Collections;

// Token: 0x020006CF RID: 1743
public class HouseLevelTutorial : AbstractLevelInteractiveEntity
{
	// Token: 0x0600251E RID: 9502 RVA: 0x0015C4B2 File Offset: 0x0015A8B2
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x0015C4D3 File Offset: 0x0015A8D3
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x0015C4E8 File Offset: 0x0015A8E8
	private IEnumerator go_cr()
	{
		this.activated = true;
		HouseLevel level = Level.Current as HouseLevel;
		if (level)
		{
			level.StartTutorial();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		SceneLoader.LoadScene(Scenes.scene_level_tutorial, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x0015C503 File Offset: 0x0015A903
	protected override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, false);
	}

	// Token: 0x04002DCB RID: 11723
	private bool activated;
}
