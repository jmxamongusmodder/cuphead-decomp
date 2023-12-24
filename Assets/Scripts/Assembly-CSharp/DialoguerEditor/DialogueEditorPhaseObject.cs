using System;
using UnityEngine;
using System.Collections.Generic;

namespace DialoguerEditor
{
	[Serializable]
	public class DialogueEditorPhaseObject
	{
		public int id;
		public DialogueEditorPhaseTypes type;
		public string theme;
		public Vector2 position;
		public List<int> outs;
		public bool advanced;
		public string metadata;
		public string text;
		public string name;
		public string portrait;
		public string audio;
		public float audioDelay;
		public Rect rect;
		public bool newWindow;
		public List<string> choices;
		public DialogueEditorWaitTypes waitType;
		public float waitDuration;
		public VariableEditorScopes variableScope;
		public VariableEditorTypes variableType;
		public int variableId;
		public Vector2 variableScrollPosition;
		public VariableEditorSetEquation variableSetEquation;
		public string variableSetValue;
		public VariableEditorGetEquation variableGetEquation;
		public string variableGetValue;
		public string messageName;
	}
}
