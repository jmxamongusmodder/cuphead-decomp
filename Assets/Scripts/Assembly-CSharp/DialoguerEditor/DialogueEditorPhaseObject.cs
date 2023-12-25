using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguerEditor
{
	// Token: 0x02000B46 RID: 2886
	[Serializable]
	public class DialogueEditorPhaseObject
	{
		// Token: 0x060045CC RID: 17868 RVA: 0x0024D084 File Offset: 0x0024B484
		public DialogueEditorPhaseObject()
		{
			this.type = DialogueEditorPhaseTypes.EmptyPhase;
			this.position = Vector2.zero;
			this.text = string.Empty;
			this.outs = new List<int>();
			this.choices = new List<string>();
			this.waitType = DialogueEditorWaitTypes.Seconds;
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x0024D0D1 File Offset: 0x0024B4D1
		public void addNewOut()
		{
			this.outs.Add(-1);
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x0024D0DF File Offset: 0x0024B4DF
		public void removeOut()
		{
			this.outs.RemoveAt(this.outs.Count - 1);
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x0024D0F9 File Offset: 0x0024B4F9
		public void addNewChoice()
		{
			this.addNewOut();
			this.choices.Add(string.Empty);
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x0024D111 File Offset: 0x0024B511
		public void removeChoice()
		{
			this.removeOut();
			this.choices.RemoveAt(this.choices.Count - 1);
		}

		// Token: 0x04004BF8 RID: 19448
		public int id;

		// Token: 0x04004BF9 RID: 19449
		public DialogueEditorPhaseTypes type;

		// Token: 0x04004BFA RID: 19450
		public object paramaters;

		// Token: 0x04004BFB RID: 19451
		public string theme;

		// Token: 0x04004BFC RID: 19452
		public Vector2 position;

		// Token: 0x04004BFD RID: 19453
		public List<int> outs;

		// Token: 0x04004BFE RID: 19454
		public bool advanced;

		// Token: 0x04004BFF RID: 19455
		public string metadata;

		// Token: 0x04004C00 RID: 19456
		public string text;

		// Token: 0x04004C01 RID: 19457
		public string name;

		// Token: 0x04004C02 RID: 19458
		public string portrait;

		// Token: 0x04004C03 RID: 19459
		public string audio;

		// Token: 0x04004C04 RID: 19460
		public float audioDelay;

		// Token: 0x04004C05 RID: 19461
		public Rect rect;

		// Token: 0x04004C06 RID: 19462
		public bool newWindow;

		// Token: 0x04004C07 RID: 19463
		public List<string> choices;

		// Token: 0x04004C08 RID: 19464
		public DialogueEditorWaitTypes waitType;

		// Token: 0x04004C09 RID: 19465
		public float waitDuration;

		// Token: 0x04004C0A RID: 19466
		public VariableEditorScopes variableScope;

		// Token: 0x04004C0B RID: 19467
		public VariableEditorTypes variableType;

		// Token: 0x04004C0C RID: 19468
		public int variableId;

		// Token: 0x04004C0D RID: 19469
		public Vector2 variableScrollPosition;

		// Token: 0x04004C0E RID: 19470
		public VariableEditorSetEquation variableSetEquation;

		// Token: 0x04004C0F RID: 19471
		public string variableSetValue;

		// Token: 0x04004C10 RID: 19472
		public VariableEditorGetEquation variableGetEquation;

		// Token: 0x04004C11 RID: 19473
		public string variableGetValue;

		// Token: 0x04004C12 RID: 19474
		public string messageName;
	}
}
