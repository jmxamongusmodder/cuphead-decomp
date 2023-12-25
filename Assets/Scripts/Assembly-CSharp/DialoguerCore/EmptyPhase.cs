using System;

namespace DialoguerCore
{
	// Token: 0x02000B77 RID: 2935
	public class EmptyPhase : AbstractDialoguePhase
	{
		// Token: 0x060046BA RID: 18106 RVA: 0x0024F497 File Offset: 0x0024D897
		public EmptyPhase() : base(null)
		{
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x0024F4A0 File Offset: 0x0024D8A0
		public override string ToString()
		{
			return "Empty Phase\nEmpty Phases should not be generated, something went wrong.\n";
		}
	}
}
