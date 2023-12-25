using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x02000B49 RID: 2889
	public class DialogueEditorPhaseTemplates
	{
		// Token: 0x060045D6 RID: 17878 RVA: 0x0024D320 File Offset: 0x0024B720
		public static DialogueEditorPhaseObject newTextPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.TextPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				name = string.Empty,
				portrait = string.Empty,
				audio = string.Empty,
				audioDelay = 0f,
				rect = new Rect(0f, 0f, 0f, 0f),
				newWindow = false,
				outs = new List<int>(),
				outs = 
				{
					-1
				}
			};
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x0024D3C8 File Offset: 0x0024B7C8
		public static DialogueEditorPhaseObject newBranchedTextPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.BranchedTextPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				name = string.Empty,
				portrait = string.Empty,
				audio = string.Empty,
				audioDelay = 0f,
				rect = new Rect(0f, 0f, 0f, 0f),
				newWindow = false,
				outs = new List<int>(),
				outs = 
				{
					-1,
					-1
				},
				choices = new List<string>(),
				choices = 
				{
					string.Empty,
					string.Empty
				}
			};
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x0024D4A8 File Offset: 0x0024B8A8
		public static DialogueEditorPhaseObject newWaitPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.WaitPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1
				}
			};
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x0024D500 File Offset: 0x0024B900
		public static DialogueEditorPhaseObject newSetVariablePhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.SetVariablePhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1
				},
				variableScope = VariableEditorScopes.Local,
				variableType = VariableEditorTypes.Boolean,
				variableSetEquation = VariableEditorSetEquation.Equals,
				variableScrollPosition = default(Vector2),
				variableId = 0,
				variableSetValue = string.Empty
			};
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x0024D58C File Offset: 0x0024B98C
		public static DialogueEditorPhaseObject newConditionalPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.ConditionalPhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1,
					-1
				}
			};
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x0024D5F0 File Offset: 0x0024B9F0
		public static DialogueEditorPhaseObject newSendMessagePhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.SendMessagePhase,
				position = Vector2.zero,
				advanced = false,
				metadata = string.Empty,
				outs = new List<int>(),
				outs = 
				{
					-1
				},
				messageName = string.Empty
			};
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x0024D654 File Offset: 0x0024BA54
		public static DialogueEditorPhaseObject newEndPhase(int id)
		{
			return new DialogueEditorPhaseObject
			{
				id = id,
				type = DialogueEditorPhaseTypes.EndPhase,
				position = Vector2.zero
			};
		}
	}
}
