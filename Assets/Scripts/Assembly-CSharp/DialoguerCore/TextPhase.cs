using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x02000B7B RID: 2939
	public class TextPhase : AbstractDialoguePhase
	{
		// Token: 0x060046C4 RID: 18116 RVA: 0x0024EEE0 File Offset: 0x0024D2E0
		public TextPhase(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, List<string> choices, int dialogueID, int nodeID) : base(outs)
		{
			this.data = new DialoguerTextData(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, choices, dialogueID, nodeID);
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x0024EF15 File Offset: 0x0024D315
		protected override void onStart()
		{
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0024EF18 File Offset: 0x0024D318
		public override void Continue(int nextPhaseId)
		{
			if (this.data.newWindow)
			{
				DialoguerEventManager.dispatchOnWindowClose();
			}
			base.Continue(nextPhaseId);
			base.state = PhaseState.Complete;
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x0024EF4C File Offset: 0x0024D34C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Text Phase",
				this.data.ToString(),
				"\nOut: ",
				this.outs[0],
				"\n"
			});
		}

		// Token: 0x04004CA8 RID: 19624
		public readonly DialoguerTextData data;
	}
}
