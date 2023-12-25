using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x02000B6E RID: 2926
	public class DialoguerDialogue
	{
		// Token: 0x06004696 RID: 18070 RVA: 0x0024E82C File Offset: 0x0024CC2C
		public DialoguerDialogue(string name, int startPhaseId, DialoguerVariables localVariables, List<AbstractDialoguePhase> phases)
		{
			this.name = name;
			this.startPhaseId = startPhaseId;
			this.phases = phases;
			this._originalLocalVariables = localVariables;
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x0024E851 File Offset: 0x0024CC51
		public void Reset()
		{
			this.localVariables = this._originalLocalVariables.Clone();
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x0024E864 File Offset: 0x0024CC64
		public override string ToString()
		{
			string text = "Dialogue: " + this.name + "\n-";
			text = text + "\nLocal Booleans: " + this._originalLocalVariables.booleans.Count;
			text = text + "\nLocal Floats: " + this._originalLocalVariables.floats.Count;
			text = text + "\nLocal Strings: " + this._originalLocalVariables.strings.Count;
			text += "\n";
			for (int i = 0; i < this.phases.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\nPhase ",
					i,
					": ",
					this.phases[i].ToString()
				});
			}
			return text;
		}

		// Token: 0x04004C73 RID: 19571
		public readonly string name;

		// Token: 0x04004C74 RID: 19572
		public readonly int startPhaseId;

		// Token: 0x04004C75 RID: 19573
		public readonly List<AbstractDialoguePhase> phases;

		// Token: 0x04004C76 RID: 19574
		private readonly DialoguerVariables _originalLocalVariables;

		// Token: 0x04004C77 RID: 19575
		public DialoguerVariables localVariables;
	}
}
