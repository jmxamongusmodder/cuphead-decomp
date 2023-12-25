using System;
using System.Collections.Generic;

namespace DialoguerEditor
{
	// Token: 0x02000B4E RID: 2894
	[Serializable]
	public class DialogueEditorVariablesContainer
	{
		// Token: 0x060045EA RID: 17898 RVA: 0x0024D7E4 File Offset: 0x0024BBE4
		public DialogueEditorVariablesContainer()
		{
			this.selection = 0;
			this.variables = new List<DialogueEditorVariableObject>();
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x0024D800 File Offset: 0x0024BC00
		public void addVariable()
		{
			int count = this.variables.Count;
			this.variables.Add(new DialogueEditorVariableObject());
			this.variables[count].id = count;
			this.selection = this.variables.Count - 1;
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x0024D850 File Offset: 0x0024BC50
		public void removeVariable()
		{
			if (this.variables.Count < 1)
			{
				return;
			}
			this.variables.RemoveAt(this.variables.Count - 1);
			if (this.selection > this.variables.Count - 1)
			{
				this.selection = this.variables.Count - 1;
			}
		}

		// Token: 0x04004C2C RID: 19500
		public List<DialogueEditorVariableObject> variables;

		// Token: 0x04004C2D RID: 19501
		public int selection;
	}
}
