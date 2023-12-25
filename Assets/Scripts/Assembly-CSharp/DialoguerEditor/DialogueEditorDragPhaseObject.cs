using System;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x02000B42 RID: 2882
	public class DialogueEditorDragPhaseObject
	{
		// Token: 0x060045C0 RID: 17856 RVA: 0x0024C923 File Offset: 0x0024AD23
		public DialogueEditorDragPhaseObject(int phaseId, Vector2 mouseOffset)
		{
			this.phaseId = phaseId;
			this.mouseOffset = mouseOffset;
		}

		// Token: 0x04004BEC RID: 19436
		public int phaseId;

		// Token: 0x04004BED RID: 19437
		public Vector2 mouseOffset;
	}
}
