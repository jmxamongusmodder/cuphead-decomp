using UnityEngine;
using System.Collections.Generic;

namespace DialoguerCore
{
	public class TextPhase : AbstractDialoguePhase
	{
		public TextPhase(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, List<string> choices, int dialogueID, int nodeID) : base(default(List<int>))
		{
		}

	}
}
