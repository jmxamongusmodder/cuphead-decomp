using System;

namespace DialoguerEditor
{
	// Token: 0x02000B43 RID: 2883
	[Serializable]
	public class DialogueEditorGlobalVariablesContainer
	{
		// Token: 0x060045C1 RID: 17857 RVA: 0x0024C939 File Offset: 0x0024AD39
		public DialogueEditorGlobalVariablesContainer()
		{
			this.booleans = new DialogueEditorVariablesContainer();
			this.floats = new DialogueEditorVariablesContainer();
			this.strings = new DialogueEditorVariablesContainer();
		}

		// Token: 0x04004BEE RID: 19438
		public DialogueEditorVariablesContainer booleans;

		// Token: 0x04004BEF RID: 19439
		public DialogueEditorVariablesContainer floats;

		// Token: 0x04004BF0 RID: 19440
		public DialogueEditorVariablesContainer strings;
	}
}
