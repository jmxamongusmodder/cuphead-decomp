using System;
using UnityEngine;

// Token: 0x02000951 RID: 2385
public class MapNPCFishgirl : MonoBehaviour
{
	// Token: 0x060037B8 RID: 14264 RVA: 0x001FFBCB File Offset: 0x001FDFCB
	private void Start()
	{
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(new Levels[]
		{
			Levels.Mausoleum
		}, Level.Mode.Easy))
		{
			Dialoguer.SetGlobalFloat(12, 1f);
		}
	}
}
