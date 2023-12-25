using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000369 RID: 873
public static class ListExtensions
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x0007CC80 File Offset: 0x0007B080
	public static void Move<T>(this List<T> list, int index, int direction)
	{
		if (direction < 0)
		{
			if (index == 0)
			{
				return;
			}
			T value = list[index - 1];
			list[index - 1] = list[index];
			list[index] = value;
		}
		else if (direction > 0)
		{
			if (index >= list.Count - 1)
			{
				return;
			}
			T value2 = list[index + 1];
			list[index + 1] = list[index];
			list[index] = value2;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0007CCFC File Offset: 0x0007B0FC
	public static void Shuffle<T>(this IList<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			int index = UnityEngine.Random.Range(i, list.Count);
			T value = list[i];
			list[i] = list[index];
			list[index] = value;
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0007CD4B File Offset: 0x0007B14B
	public static T RandomChoice<T>(this IList<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}
}
