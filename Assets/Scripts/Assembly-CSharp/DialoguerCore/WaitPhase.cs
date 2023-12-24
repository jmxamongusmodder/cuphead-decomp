using DialoguerEditor;
using System.Collections.Generic;

namespace DialoguerCore
{
	public class WaitPhase : AbstractDialoguePhase
	{
		public WaitPhase(DialogueEditorWaitTypes type, float duration, List<int> outs) : base(default(List<int>))
		{
		}

	}
}
