using System;
using UnityEngine;
using System.Collections.Generic;

namespace DialoguerEditor
{
	[Serializable]
	public class DialogueEditorDialogueObject
	{
		public int id;
		public string name;
		public int startPage;
		public Vector2 scrollPosition;
		public List<DialogueEditorPhaseObject> phases;
		public DialogueEditorVariablesContainer floats;
		public DialogueEditorVariablesContainer strings;
		public DialogueEditorVariablesContainer booleans;
	}
}
