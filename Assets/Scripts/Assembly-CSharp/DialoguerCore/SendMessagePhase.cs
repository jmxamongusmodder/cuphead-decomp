using System.Collections.Generic;

namespace DialoguerCore
{
	public class SendMessagePhase : AbstractDialoguePhase
	{
		public SendMessagePhase(string message, string metadata, List<int> outs) : base(default(List<int>))
		{
		}

	}
}
