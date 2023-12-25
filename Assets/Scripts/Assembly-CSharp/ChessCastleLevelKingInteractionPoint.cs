using System;

// Token: 0x02000533 RID: 1331
public class ChessCastleLevelKingInteractionPoint : DialogueInteractionPoint
{
	// Token: 0x06001814 RID: 6164 RVA: 0x000D9D61 File Offset: 0x000D8161
	public void BeginDialogue()
	{
		this.Activate();
		this.speechBubble.waitForRealease = false;
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x000D9D75 File Offset: 0x000D8175
	protected override void StartAnimation()
	{
		((ChessCastleLevel)Level.Current).StartTalkAnimation();
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000D9D86 File Offset: 0x000D8186
	protected override void EndAnimation()
	{
		((ChessCastleLevel)Level.Current).EndTalkAnimation();
	}
}
