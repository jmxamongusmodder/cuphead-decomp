using DialoguerEditor;
using System.Collections.Generic;

namespace DialoguerCore
{
	public class SetVariablePhase : AbstractDialoguePhase
	{
		public SetVariablePhase(VariableEditorScopes scope, VariableEditorTypes type, int variableId, VariableEditorSetEquation equation, string setValue, List<int> outs) : base(default(List<int>))
		{
		}

	}
}
