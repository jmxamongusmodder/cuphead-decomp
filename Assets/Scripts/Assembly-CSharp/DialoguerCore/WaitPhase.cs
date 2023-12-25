using System;
using System.Collections.Generic;
using DialoguerEditor;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x02000B7C RID: 2940
	public class WaitPhase : AbstractDialoguePhase
	{
		// Token: 0x060046C8 RID: 18120 RVA: 0x0024F9FD File Offset: 0x0024DDFD
		public WaitPhase(DialogueEditorWaitTypes type, float duration, List<int> outs) : base(outs)
		{
			this.type = type;
			this.duration = duration;
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x0024FA14 File Offset: 0x0024DE14
		protected override void onStart()
		{
			DialoguerEventManager.dispatchOnWaitStart();
			if (this.type == DialogueEditorWaitTypes.Continue)
			{
				return;
			}
			GameObject gameObject = new GameObject("Dialoguer WaitPhaseTimer");
			WaitPhaseComponent waitPhaseComponent = gameObject.AddComponent<WaitPhaseComponent>();
			waitPhaseComponent.Init(this, this.type, this.duration);
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x0024FA58 File Offset: 0x0024DE58
		public void waitComplete()
		{
			DialoguerEventManager.dispatchOnWaitComplete();
			base.state = PhaseState.Complete;
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x0024FA66 File Offset: 0x0024DE66
		public override void Continue(int outId)
		{
			if (this.type != DialogueEditorWaitTypes.Continue)
			{
				return;
			}
			DialoguerEventManager.dispatchOnWaitComplete();
			base.Continue(outId);
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x0024FA84 File Offset: 0x0024DE84
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Wait Phase\nType: ",
				this.type.ToString(),
				"\nDuration: ",
				this.duration,
				"\n"
			});
		}

		// Token: 0x04004CA9 RID: 19625
		public readonly DialogueEditorWaitTypes type;

		// Token: 0x04004CAA RID: 19626
		public readonly float duration;
	}
}
