using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	[Serializable]
	public class DialogueEditorMasterObject
	{
		public bool generateEnum;
		public List<DialogueEditorDialogueObject> dialogues;
		public DialogueEditorGlobalVariablesContainer globals;
		public DialogueEditorThemesContainer themes;
		public Vector2 selectorScrollPosition;
	}
}
