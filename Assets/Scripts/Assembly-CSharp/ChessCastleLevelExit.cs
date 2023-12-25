using System;

// Token: 0x02000531 RID: 1329
public class ChessCastleLevelExit : AbstractLevelInteractiveEntity
{
	// Token: 0x0600180F RID: 6159 RVA: 0x000D9CE9 File Offset: 0x000D80E9
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		this.activated = true;
		((ChessCastleLevel)Level.Current).Exit();
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000D9D13 File Offset: 0x000D8113
	protected override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, false);
	}

	// Token: 0x04002146 RID: 8518
	private bool activated;
}
