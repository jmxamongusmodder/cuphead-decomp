using System;
using UnityEngine;

// Token: 0x02000383 RID: 899
public static class Rand
{
	// Token: 0x06000AA3 RID: 2723 RVA: 0x0007F971 File Offset: 0x0007DD71
	public static bool Bool()
	{
		return UnityEngine.Random.Range(0, 2) == 1;
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0007F987 File Offset: 0x0007DD87
	public static int PosOrNeg()
	{
		return (!Rand.Bool()) ? -1 : 1;
	}
}
