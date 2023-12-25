using System;
using UnityEngine;

// Token: 0x020008FA RID: 2298
public class PlatformingLevelParallaxChild : AbstractMonoBehaviour
{
	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x060035E5 RID: 13797 RVA: 0x001F6444 File Offset: 0x001F4844
	public int SortingOrderOffset
	{
		get
		{
			return this._sortingOrderOffset;
		}
	}

	// Token: 0x04003DF9 RID: 15865
	[SerializeField]
	private int _sortingOrderOffset;
}
