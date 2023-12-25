using System;
using System.Collections.Generic;
using DialoguerCore;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x02000B44 RID: 2884
	[Serializable]
	public class DialogueEditorMasterObject
	{
		// Token: 0x060045C2 RID: 17858 RVA: 0x0024C964 File Offset: 0x0024AD64
		public DialogueEditorMasterObject()
		{
			this.dialogues = new List<DialogueEditorDialogueObject>();
			this.globals = new DialogueEditorGlobalVariablesContainer();
			this.themes = new DialogueEditorThemesContainer();
			this.selectorScrollPosition = Vector2.zero;
			this.currentDialogueId = -1;
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060045C3 RID: 17859 RVA: 0x0024C9B1 File Offset: 0x0024ADB1
		public int count
		{
			get
			{
				return this.dialogues.Count;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060045C4 RID: 17860 RVA: 0x0024C9BE File Offset: 0x0024ADBE
		// (set) Token: 0x060045C5 RID: 17861 RVA: 0x0024C9C6 File Offset: 0x0024ADC6
		public int currentDialogueId
		{
			get
			{
				return this.__currentDialogueId;
			}
			set
			{
				this.__currentDialogueId = Mathf.Clamp(value, 0, this.count - 1);
			}
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x0024C9E0 File Offset: 0x0024ADE0
		public void addDialogue(int count)
		{
			for (int i = 0; i < count; i++)
			{
				int count2 = this.dialogues.Count;
				this.dialogues.Add(new DialogueEditorDialogueObject());
				this.dialogues[count2].id = count2;
				this.currentDialogueId = this.dialogues[count2].id;
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x0024CA44 File Offset: 0x0024AE44
		public void removeDialogue(int removeCount)
		{
			if (this.count < 1)
			{
				return;
			}
			for (int i = 0; i < removeCount; i++)
			{
				int index = this.dialogues.Count - 1;
				this.dialogues.RemoveAt(index);
			}
			this.currentDialogueId = this.currentDialogueId;
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0024CA96 File Offset: 0x0024AE96
		public string[] getThemeNames()
		{
			return this.getThemeNames(false);
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0024CAA0 File Offset: 0x0024AEA0
		public string[] getThemeNames(bool includeId)
		{
			string[] array = new string[this.themes.themes.Count];
			for (int i = 0; i < this.themes.themes.Count; i++)
			{
				array[i] = string.Empty;
				string[] array2;
				if (includeId)
				{
					int num;
					(array2 = array)[num = i] = array2[num] + this.themes.themes[i].id + " ";
				}
				int num2;
				(array2 = array)[num2 = i] = array2[num2] + this.themes.themes[i].name;
			}
			return array;
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x0024CB4C File Offset: 0x0024AF4C
		public DialoguerData getDialoguerData()
		{
			List<bool> list = new List<bool>();
			List<float> list2 = new List<float>();
			List<string> list3 = new List<string>();
			for (int i = 0; i < this.globals.booleans.variables.Count; i++)
			{
				bool item;
				if (!bool.TryParse(this.globals.booleans.variables[i].variable, out item))
				{
				}
				list.Add(item);
			}
			for (int j = 0; j < this.globals.floats.variables.Count; j++)
			{
				float item2;
				if (!float.TryParse(this.globals.floats.variables[j].variable, out item2))
				{
				}
				list2.Add(item2);
			}
			for (int k = 0; k < this.globals.strings.variables.Count; k++)
			{
				list3.Add(this.globals.strings.variables[k].variable);
			}
			DialoguerGlobalVariables globalVariables = new DialoguerGlobalVariables(list, list2, list3);
			List<DialoguerDialogue> list4 = new List<DialoguerDialogue>();
			for (int l = 0; l < this.dialogues.Count; l++)
			{
				DialogueEditorDialogueObject dialogueEditorDialogueObject = this.dialogues[l];
				List<AbstractDialoguePhase> list5 = new List<AbstractDialoguePhase>();
				for (int m = 0; m < dialogueEditorDialogueObject.phases.Count; m++)
				{
					DialogueEditorPhaseObject dialogueEditorPhaseObject = dialogueEditorDialogueObject.phases[m];
					switch (dialogueEditorPhaseObject.type)
					{
					case DialogueEditorPhaseTypes.TextPhase:
						list5.Add(new TextPhase(dialogueEditorPhaseObject.text, dialogueEditorPhaseObject.theme, dialogueEditorPhaseObject.newWindow, dialogueEditorPhaseObject.name, dialogueEditorPhaseObject.portrait, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.audio, dialogueEditorPhaseObject.audioDelay, dialogueEditorPhaseObject.rect, dialogueEditorPhaseObject.outs, null, dialogueEditorDialogueObject.id, dialogueEditorPhaseObject.id));
						break;
					case DialogueEditorPhaseTypes.BranchedTextPhase:
						list5.Add(new BranchedTextPhase(dialogueEditorPhaseObject.text, dialogueEditorPhaseObject.choices, dialogueEditorPhaseObject.theme, dialogueEditorPhaseObject.newWindow, dialogueEditorPhaseObject.name, dialogueEditorPhaseObject.portrait, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.audio, dialogueEditorPhaseObject.audioDelay, dialogueEditorPhaseObject.rect, dialogueEditorPhaseObject.outs, dialogueEditorDialogueObject.id, dialogueEditorPhaseObject.id));
						break;
					case DialogueEditorPhaseTypes.WaitPhase:
						list5.Add(new WaitPhase(dialogueEditorPhaseObject.waitType, dialogueEditorPhaseObject.waitDuration, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.SetVariablePhase:
						list5.Add(new SetVariablePhase(dialogueEditorPhaseObject.variableScope, dialogueEditorPhaseObject.variableType, dialogueEditorPhaseObject.variableId, dialogueEditorPhaseObject.variableSetEquation, dialogueEditorPhaseObject.variableSetValue, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.ConditionalPhase:
						list5.Add(new ConditionalPhase(dialogueEditorPhaseObject.variableScope, dialogueEditorPhaseObject.variableType, dialogueEditorPhaseObject.variableId, dialogueEditorPhaseObject.variableGetEquation, dialogueEditorPhaseObject.variableGetValue, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.SendMessagePhase:
						list5.Add(new SendMessagePhase(dialogueEditorPhaseObject.messageName, dialogueEditorPhaseObject.metadata, dialogueEditorPhaseObject.outs));
						break;
					case DialogueEditorPhaseTypes.EndPhase:
						list5.Add(new EndPhase());
						break;
					default:
						list5.Add(new EmptyPhase());
						break;
					}
				}
				List<bool> list6 = new List<bool>();
				for (int n = 0; n < dialogueEditorDialogueObject.booleans.variables.Count; n++)
				{
					bool item3;
					if (!bool.TryParse(dialogueEditorDialogueObject.booleans.variables[n].variable, out item3))
					{
					}
					list6.Add(item3);
				}
				List<float> list7 = new List<float>();
				for (int num = 0; num < dialogueEditorDialogueObject.floats.variables.Count; num++)
				{
					float item4;
					if (!float.TryParse(dialogueEditorDialogueObject.floats.variables[num].variable, out item4))
					{
					}
					list7.Add(item4);
				}
				List<string> list8 = new List<string>();
				for (int num2 = 0; num2 < dialogueEditorDialogueObject.strings.variables.Count; num2++)
				{
					list8.Add(dialogueEditorDialogueObject.strings.variables[num2].variable);
				}
				DialoguerVariables localVariables = new DialoguerVariables(list6, list7, list8);
				DialoguerDialogue item5 = new DialoguerDialogue(dialogueEditorDialogueObject.name, dialogueEditorDialogueObject.startPage, localVariables, list5);
				list4.Add(item5);
			}
			List<DialoguerTheme> list9 = new List<DialoguerTheme>();
			for (int num3 = 0; num3 < this.themes.themes.Count; num3++)
			{
				list9.Add(new DialoguerTheme(this.themes.themes[num3].name, this.themes.themes[num3].linkage));
			}
			return new DialoguerData(globalVariables, list4, list9);
		}

		// Token: 0x04004BF1 RID: 19441
		private int __currentDialogueId;

		// Token: 0x04004BF2 RID: 19442
		public bool generateEnum = true;

		// Token: 0x04004BF3 RID: 19443
		public List<DialogueEditorDialogueObject> dialogues;

		// Token: 0x04004BF4 RID: 19444
		public DialogueEditorGlobalVariablesContainer globals;

		// Token: 0x04004BF5 RID: 19445
		public DialogueEditorThemesContainer themes;

		// Token: 0x04004BF6 RID: 19446
		public Vector2 selectorScrollPosition;
	}
}
