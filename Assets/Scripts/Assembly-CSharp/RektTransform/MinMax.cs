using System;
using UnityEngine;

namespace RektTransform
{
	// Token: 0x0200036C RID: 876
	public struct MinMax
	{
		// Token: 0x060009C6 RID: 2502 RVA: 0x0007D020 File Offset: 0x0007B420
		public MinMax(Vector2 min, Vector2 max)
		{
			this.min = new Vector2(Mathf.Clamp01(min.x), Mathf.Clamp01(min.y));
			this.max = new Vector2(Mathf.Clamp01(max.x), Mathf.Clamp01(max.y));
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0007D073 File Offset: 0x0007B473
		public MinMax(float minx, float miny, float maxx, float maxy)
		{
			this.min = new Vector2(Mathf.Clamp01(minx), Mathf.Clamp01(miny));
			this.max = new Vector2(Mathf.Clamp01(maxx), Mathf.Clamp01(maxy));
		}

		// Token: 0x0400145B RID: 5211
		public Vector2 min;

		// Token: 0x0400145C RID: 5212
		public Vector2 max;
	}
}
