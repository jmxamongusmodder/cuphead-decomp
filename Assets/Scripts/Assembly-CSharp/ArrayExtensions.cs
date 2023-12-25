using System;
using UnityEngine;

// Token: 0x02000367 RID: 871
public static class ArrayExtensions
{
	// Token: 0x060009BC RID: 2492 RVA: 0x0007CBD2 File Offset: 0x0007AFD2
	public static T GetRandom<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0007CBE3 File Offset: 0x0007AFE3
	public static T GetLast<T>(this T[] array)
	{
		return array[array.Length - 1];
	}
}
