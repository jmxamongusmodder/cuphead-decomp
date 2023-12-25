using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class LevelCoinVisual : AbstractPausableComponent
{
	// Token: 0x06001353 RID: 4947 RVA: 0x000AAC7F File Offset: 0x000A907F
	private void OnDeathAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
