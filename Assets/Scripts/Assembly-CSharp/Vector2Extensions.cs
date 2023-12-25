using System;
using UnityEngine;

// Token: 0x02000371 RID: 881
public static class Vector2Extensions
{
	// Token: 0x06000A28 RID: 2600 RVA: 0x0007E390 File Offset: 0x0007C790
	public static Vector2 Set(this Vector2 v, float? x = null, float? y = null)
	{
		Vector2 result = v;
		if (x != null)
		{
			result.x = x.Value;
		}
		if (y != null)
		{
			result.y = y.Value;
		}
		return result;
	}
}
