using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000CAC RID: 3244
	public struct Extents
	{
		// Token: 0x06005197 RID: 20887 RVA: 0x0029A01F File Offset: 0x0029841F
		public Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x0029A030 File Offset: 0x00298430
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Min (",
				this.min.x.ToString("f2"),
				", ",
				this.min.y.ToString("f2"),
				")   Max (",
				this.max.x.ToString("f2"),
				", ",
				this.max.y.ToString("f2"),
				")"
			});
		}

		// Token: 0x040054A0 RID: 21664
		public Vector2 min;

		// Token: 0x040054A1 RID: 21665
		public Vector2 max;
	}
}
