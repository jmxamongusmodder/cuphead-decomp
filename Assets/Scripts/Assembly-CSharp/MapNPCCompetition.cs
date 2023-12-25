using System;
using UnityEngine;

// Token: 0x0200094F RID: 2383
public class MapNPCCompetition : MonoBehaviour
{
	// Token: 0x060037B4 RID: 14260 RVA: 0x001FFAA0 File Offset: 0x001FDEA0
	private void Start()
	{
		int[] curseCharmPuzzleOrder = PlayerData.Data.curseCharmPuzzleOrder;
		foreach (int num in PlayerData.Data.curseCharmPuzzleOrder)
		{
		}
		for (int j = 0; j < curseCharmPuzzleOrder.Length; j++)
		{
			Dialoguer.SetGlobalFloat(this.dialogueVarIndices[j], (float)curseCharmPuzzleOrder[j]);
		}
	}

	// Token: 0x04003FB8 RID: 16312
	private int[] dialogueVarIndices = new int[]
	{
		26,
		27,
		28
	};
}
