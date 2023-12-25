using System;
using UnityEngine;

// Token: 0x020006CE RID: 1742
public class HouseLevelMusicNotes : AbstractPausableComponent
{
	// Token: 0x0600251C RID: 9500 RVA: 0x0015C491 File Offset: 0x0015A891
	private void ChangeAnimation()
	{
		base.animator.SetInteger("Type", UnityEngine.Random.Range(0, 4));
	}
}
