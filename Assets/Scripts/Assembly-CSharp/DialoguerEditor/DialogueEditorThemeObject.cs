using System;

namespace DialoguerEditor
{
	// Token: 0x02000B4B RID: 2891
	[Serializable]
	public class DialogueEditorThemeObject
	{
		// Token: 0x060045E5 RID: 17893 RVA: 0x0024D70A File Offset: 0x0024BB0A
		public DialogueEditorThemeObject()
		{
			this.name = string.Empty;
			this.linkage = string.Empty;
		}

		// Token: 0x04004C24 RID: 19492
		public int id;

		// Token: 0x04004C25 RID: 19493
		public string name;

		// Token: 0x04004C26 RID: 19494
		public string linkage;
	}
}
