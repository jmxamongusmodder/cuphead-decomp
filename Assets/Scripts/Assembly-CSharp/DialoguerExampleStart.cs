using System;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class DialoguerExampleStart : MonoBehaviour
{
	// Token: 0x060046D7 RID: 18135 RVA: 0x0024FE9D File Offset: 0x0024E29D
	private void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x0024FEA4 File Offset: 0x0024E2A4
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Start!"))
		{
			Dialoguer.StartDialogue(3);
		}
		string text = "Open this file (DialoguerExampleStart.cs) to see how to start using Dialoguer";
		GUI.Label(new Rect(10f, 50f, 500f, 500f), text);
	}
}
