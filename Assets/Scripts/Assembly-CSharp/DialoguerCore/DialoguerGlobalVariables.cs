using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x02000B6F RID: 2927
	[Serializable]
	public class DialoguerGlobalVariables
	{
		// Token: 0x06004699 RID: 18073 RVA: 0x0024E952 File Offset: 0x0024CD52
		public DialoguerGlobalVariables()
		{
			this.booleans = new List<bool>();
			this.floats = new List<float>();
			this.strings = new List<string>();
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0024E97B File Offset: 0x0024CD7B
		public DialoguerGlobalVariables(List<bool> booleans, List<float> floats, List<string> strings)
		{
			this.booleans = booleans;
			this.floats = floats;
			this.strings = strings;
		}

		// Token: 0x04004C78 RID: 19576
		public List<bool> booleans;

		// Token: 0x04004C79 RID: 19577
		public List<float> floats;

		// Token: 0x04004C7A RID: 19578
		public List<string> strings;
	}
}
