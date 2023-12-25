using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000CA1 RID: 3233
	[Serializable]
	public class LineBreakingTable
	{
		// Token: 0x06005188 RID: 20872 RVA: 0x002998BB File Offset: 0x00297CBB
		public LineBreakingTable()
		{
			this.leadingCharacters = new Dictionary<int, char>();
			this.followingCharacters = new Dictionary<int, char>();
		}

		// Token: 0x0400543C RID: 21564
		public Dictionary<int, char> leadingCharacters;

		// Token: 0x0400543D RID: 21565
		public Dictionary<int, char> followingCharacters;
	}
}
