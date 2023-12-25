using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000C72 RID: 3186
	internal static class TMP_ListPool<T>
	{
		// Token: 0x06004FD8 RID: 20440 RVA: 0x00294ACB File Offset: 0x00292ECB
		public static List<T> Get()
		{
			return TMP_ListPool<T>.s_ListPool.Get();
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x00294AD7 File Offset: 0x00292ED7
		public static void Release(List<T> toRelease)
		{
			TMP_ListPool<T>.s_ListPool.Release(toRelease);
		}

		// Token: 0x04005292 RID: 21138
		private static readonly TMP_ObjectPool<List<T>> s_ListPool = new TMP_ObjectPool<List<T>>(null, delegate(List<T> l)
		{
			l.Clear();
		});
	}
}
