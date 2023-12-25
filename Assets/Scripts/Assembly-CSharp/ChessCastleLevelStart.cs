using System;

// Token: 0x02000534 RID: 1332
public class ChessCastleLevelStart : AbstractLevelInteractiveEntity
{
	// Token: 0x06001818 RID: 6168 RVA: 0x000D9D9F File Offset: 0x000D819F
	protected override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		this.activated = true;
		base.Activate();
		((ChessCastleLevel)Level.Current).StartChessLevel();
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x000D9DC9 File Offset: 0x000D81C9
	protected override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, false);
	}

	// Token: 0x04002147 RID: 8519
	private bool activated;
}
