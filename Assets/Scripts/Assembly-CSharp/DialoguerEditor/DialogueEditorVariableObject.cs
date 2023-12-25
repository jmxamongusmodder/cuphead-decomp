using System;

namespace DialoguerEditor
{
	// Token: 0x02000B4D RID: 2893
	[Serializable]
	public class DialogueEditorVariableObject
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x0024D7C6 File Offset: 0x0024BBC6
		public DialogueEditorVariableObject()
		{
			this.name = string.Empty;
			this.variable = string.Empty;
		}

		// Token: 0x04004C29 RID: 19497
		public string name;

		// Token: 0x04004C2A RID: 19498
		public string variable;

		// Token: 0x04004C2B RID: 19499
		public int id;
	}
}
