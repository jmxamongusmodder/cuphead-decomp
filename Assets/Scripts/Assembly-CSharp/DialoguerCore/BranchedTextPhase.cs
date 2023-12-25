using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x02000B75 RID: 2933
	public class BranchedTextPhase : TextPhase
	{
		// Token: 0x060046B5 RID: 18101 RVA: 0x0024EFA4 File Offset: 0x0024D3A4
		public BranchedTextPhase(string text, List<string> choices, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, int dialogueID, int nodeID) : base(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, outs, choices, dialogueID, nodeID)
		{
			this.choices = choices;
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0024EFD8 File Offset: 0x0024D3D8
		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < this.choices.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					i,
					": ",
					this.choices[i],
					" : Out ",
					this.outs[i],
					"\n"
				});
			}
			return "Branched Text Phase" + this.data.ToString() + "\n" + text;
		}

		// Token: 0x04004C92 RID: 19602
		public readonly List<string> choices;
	}
}
