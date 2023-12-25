using System;

// Token: 0x02000942 RID: 2370
public class MapLockedEntity : AbstractMapInteractiveEntity
{
	// Token: 0x0600376E RID: 14190 RVA: 0x001FDEE4 File Offset: 0x001FC2E4
	protected override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("???");
	}
}
