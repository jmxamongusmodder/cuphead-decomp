using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x02000B72 RID: 2930
	[Serializable]
	public class DialoguerVariables
	{
		// Token: 0x060046A1 RID: 18081 RVA: 0x0024EC1F File Offset: 0x0024D01F
		public DialoguerVariables(List<bool> booleans, List<float> floats, List<string> strings)
		{
			this.booleans = booleans;
			this.floats = floats;
			this.strings = strings;
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x0024EC3C File Offset: 0x0024D03C
		public DialoguerVariables Clone()
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < this.booleans.Count; i++)
			{
				list.Add(this.booleans[i]);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < this.floats.Count; j++)
			{
				list2.Add(this.floats[j]);
			}
			List<string> list3 = new List<string>();
			for (int k = 0; k < this.strings.Count; k++)
			{
				list3.Add(this.strings[k]);
			}
			return new DialoguerVariables(list, list2, list3);
		}

		// Token: 0x04004C8A RID: 19594
		public readonly List<bool> booleans;

		// Token: 0x04004C8B RID: 19595
		public readonly List<float> floats;

		// Token: 0x04004C8C RID: 19596
		public readonly List<string> strings;
	}
}
