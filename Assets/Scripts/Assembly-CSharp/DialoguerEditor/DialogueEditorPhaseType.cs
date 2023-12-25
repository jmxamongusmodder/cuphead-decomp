using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x02000B48 RID: 2888
	public class DialogueEditorPhaseType
	{
		// Token: 0x060045D1 RID: 17873 RVA: 0x0024D131 File Offset: 0x0024B531
		public DialogueEditorPhaseType(DialogueEditorPhaseTypes type, string name, string info, Texture iconDark, Texture iconLight)
		{
			this.type = type;
			this.name = name;
			this.info = info;
			this.iconDark = iconDark;
			this.iconLight = iconLight;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0024D160 File Offset: 0x0024B560
		public static Dictionary<int, DialogueEditorPhaseType> getPhases()
		{
			Dictionary<int, DialogueEditorPhaseType> dictionary = new Dictionary<int, DialogueEditorPhaseType>();
			DialogueEditorPhaseType value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.TextPhase, "Text", "A simple text page with one out-path.", DialogueEditorPhaseType.getDarkIcon("textPhase"), DialogueEditorPhaseType.getLightIcon("textPhase"));
			dictionary.Add(0, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.BranchedTextPhase, "Branched Text", "A text page with multiple, selectable out-paths.", DialogueEditorPhaseType.getDarkIcon("branchedTextPhase"), DialogueEditorPhaseType.getLightIcon("branchedTextPhase"));
			dictionary.Add(1, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.WaitPhase, "Wait", "Wait X seconds before progressing.", DialogueEditorPhaseType.getDarkIcon("waitPhase"), DialogueEditorPhaseType.getLightIcon("waitPhase"));
			dictionary.Add(2, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.SetVariablePhase, "Set Variable", "Set a local or global variable.", DialogueEditorPhaseType.getDarkIcon("setVariablePhase"), DialogueEditorPhaseType.getLightIcon("setVariablePhase"));
			dictionary.Add(3, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.ConditionalPhase, "Condition", "Moves to an out-path based on a condition.", DialogueEditorPhaseType.getDarkIcon("conditionalPhase"), DialogueEditorPhaseType.getLightIcon("conditionalPhase"));
			dictionary.Add(4, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.SendMessagePhase, "Message Event", "Dispatch an event which can be easily listened to and handled.", DialogueEditorPhaseType.getDarkIcon("sendMessagePhase"), DialogueEditorPhaseType.getLightIcon("sendMessagePhase"));
			dictionary.Add(5, value);
			value = new DialogueEditorPhaseType(DialogueEditorPhaseTypes.EndPhase, "End", "Ends the dialogue and calls the dialogue's callback.", DialogueEditorPhaseType.getDarkIcon("endPhase"), DialogueEditorPhaseType.getLightIcon("endPhase"));
			dictionary.Add(6, value);
			return dictionary;
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x0024D2B0 File Offset: 0x0024B6B0
		private static Texture getDarkIcon(string icon)
		{
			string str = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			str += "Dark/";
			str = str + "icon_" + icon + ".png";
			return null;
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x0024D2E4 File Offset: 0x0024B6E4
		private static Texture getLightIcon(string icon)
		{
			string str = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			str += "Light/";
			str = str + "icon_" + icon + ".png";
			return null;
		}

		// Token: 0x04004C1C RID: 19484
		public DialogueEditorPhaseTypes type;

		// Token: 0x04004C1D RID: 19485
		public string name;

		// Token: 0x04004C1E RID: 19486
		public string info;

		// Token: 0x04004C1F RID: 19487
		public Texture iconDark;

		// Token: 0x04004C20 RID: 19488
		public Texture iconLight;
	}
}
