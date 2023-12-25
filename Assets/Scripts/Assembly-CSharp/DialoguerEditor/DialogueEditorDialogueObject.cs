using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x02000B41 RID: 2881
	[Serializable]
	public class DialogueEditorDialogueObject
	{
		// Token: 0x060045BD RID: 17853 RVA: 0x0024C694 File Offset: 0x0024AA94
		public DialogueEditorDialogueObject()
		{
			this.name = string.Empty;
			this.phases = new List<DialogueEditorPhaseObject>();
			this.floats = new DialogueEditorVariablesContainer();
			this.strings = new DialogueEditorVariablesContainer();
			this.booleans = new DialogueEditorVariablesContainer();
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x0024C6E8 File Offset: 0x0024AAE8
		public void addPhase(DialogueEditorPhaseTypes phaseType, Vector2 newPhasePosition)
		{
			switch (phaseType)
			{
			case DialogueEditorPhaseTypes.TextPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newTextPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.BranchedTextPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newBranchedTextPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.WaitPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newWaitPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.SetVariablePhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newSetVariablePhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.ConditionalPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newConditionalPhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.SendMessagePhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newSendMessagePhase(this.phases.Count));
				break;
			case DialogueEditorPhaseTypes.EndPhase:
				this.phases.Add(DialogueEditorPhaseTemplates.newEndPhase(this.phases.Count));
				break;
			}
			this.phases[this.phases.Count - 1].position = newPhasePosition;
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x0024C81C File Offset: 0x0024AC1C
		public void removePhase(int phaseId)
		{
			for (int i = 0; i < this.phases.Count; i++)
			{
				DialogueEditorPhaseObject dialogueEditorPhaseObject = this.phases[i];
				for (int j = 0; j < dialogueEditorPhaseObject.outs.Count; j++)
				{
					if (dialogueEditorPhaseObject.outs[j] >= 0 && dialogueEditorPhaseObject.outs[j] > phaseId)
					{
						List<int> outs;
						int index;
						(outs = dialogueEditorPhaseObject.outs)[index = j] = outs[index] - 1;
					}
					else if (dialogueEditorPhaseObject.outs[j] >= 0 && dialogueEditorPhaseObject.outs[j] == phaseId)
					{
						dialogueEditorPhaseObject.outs[j] = -1;
					}
				}
				if (this.startPage >= 0 && this.startPage == phaseId)
				{
					this.startPage = -1;
				}
				if (i > phaseId)
				{
					dialogueEditorPhaseObject.id--;
				}
			}
			this.phases.RemoveAt(phaseId);
		}

		// Token: 0x04004BE4 RID: 19428
		public int id;

		// Token: 0x04004BE5 RID: 19429
		public string name;

		// Token: 0x04004BE6 RID: 19430
		public int startPage = -1;

		// Token: 0x04004BE7 RID: 19431
		public Vector2 scrollPosition;

		// Token: 0x04004BE8 RID: 19432
		public List<DialogueEditorPhaseObject> phases;

		// Token: 0x04004BE9 RID: 19433
		public DialogueEditorVariablesContainer floats;

		// Token: 0x04004BEA RID: 19434
		public DialogueEditorVariablesContainer strings;

		// Token: 0x04004BEB RID: 19435
		public DialogueEditorVariablesContainer booleans;
	}
}
