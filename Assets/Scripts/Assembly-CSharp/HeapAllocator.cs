using System;

// Token: 0x02000B28 RID: 2856
public static class HeapAllocator
{
	// Token: 0x06004536 RID: 17718 RVA: 0x002479F8 File Offset: 0x00245DF8
	public static void Allocate(int iterations)
	{
		object[] array = new object[iterations];
		for (int i = 0; i < iterations; i++)
		{
			object[] array2 = new object[HeapAllocator.AllocationsPerIteration];
			for (int j = 0; j < HeapAllocator.AllocationsPerIteration; j++)
			{
				array2[j] = new byte[HeapAllocator.BytesPerAllocation];
			}
			array[i] = array2;
		}
	}

	// Token: 0x04004AE8 RID: 19176
	private static readonly int BytesPerAllocation = 1024;

	// Token: 0x04004AE9 RID: 19177
	private static readonly int AllocationsPerIteration = 1024;
}
