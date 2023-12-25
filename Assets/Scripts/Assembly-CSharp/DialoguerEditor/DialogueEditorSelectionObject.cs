using System;

namespace DialoguerEditor
{
	// Token: 0x02000B4A RID: 2890
	public class DialogueEditorSelectionObject
	{
		// Token: 0x060045DD RID: 17885 RVA: 0x0024D681 File Offset: 0x0024BA81
		public DialogueEditorSelectionObject(int phaseId, int outputIndex)
		{
			if (phaseId < 0)
			{
				phaseId = 0;
			}
			if (outputIndex < 0)
			{
				outputIndex = 0;
			}
			this.phaseId = phaseId;
			this.outputIndex = outputIndex;
			this.isStart = false;
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x0024D6B2 File Offset: 0x0024BAB2
		public DialogueEditorSelectionObject(bool isStart)
		{
			this.isStart = true;
			this.phaseId = int.MinValue;
			this.outputIndex = int.MinValue;
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x0024D6D7 File Offset: 0x0024BAD7
		// (set) Token: 0x060045E0 RID: 17888 RVA: 0x0024D6DF File Offset: 0x0024BADF
		public int phaseId { get; private set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060045E1 RID: 17889 RVA: 0x0024D6E8 File Offset: 0x0024BAE8
		// (set) Token: 0x060045E2 RID: 17890 RVA: 0x0024D6F0 File Offset: 0x0024BAF0
		public int outputIndex { get; private set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x0024D6F9 File Offset: 0x0024BAF9
		// (set) Token: 0x060045E4 RID: 17892 RVA: 0x0024D701 File Offset: 0x0024BB01
		public bool isStart { get; private set; }
	}
}
