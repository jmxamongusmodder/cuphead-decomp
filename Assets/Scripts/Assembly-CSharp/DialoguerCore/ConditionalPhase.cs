using DialoguerEditor;
using System.Collections.Generic;

namespace DialoguerCore
{
	public class ConditionalPhase : AbstractDialoguePhase
	{
		public ConditionalPhase(VariableEditorScopes scope, VariableEditorTypes type, int variableId, VariableEditorGetEquation equation, string getValue, List<int> outs) : base(default(List<int>))
		{
		}

	}
}
