using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x02000B79 RID: 2937
	public class SendMessagePhase : AbstractDialoguePhase
	{
		// Token: 0x060046BE RID: 18110 RVA: 0x0024F4B7 File Offset: 0x0024D8B7
		public SendMessagePhase(string message, string metadata, List<int> outs) : base(outs)
		{
			this.message = message;
			this.metadata = metadata;
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x0024F4CE File Offset: 0x0024D8CE
		protected override void onStart()
		{
			DialoguerEventManager.dispatchOnMessageEvent(this.message, this.metadata);
			base.state = PhaseState.Complete;
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x0024F4E8 File Offset: 0x0024D8E8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Send Message Phase\nMessage: ",
				this.message,
				"\nMetadata: ",
				this.metadata,
				"\n"
			});
		}

		// Token: 0x04004C9E RID: 19614
		public readonly string message;

		// Token: 0x04004C9F RID: 19615
		public readonly string metadata;
	}
}
