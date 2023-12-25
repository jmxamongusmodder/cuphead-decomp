using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000CAD RID: 3245
	[Serializable]
	public struct Mesh_Extents
	{
		// Token: 0x06005199 RID: 20889 RVA: 0x0029A0D3 File Offset: 0x002984D3
		public Mesh_Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x0029A0E4 File Offset: 0x002984E4
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

		// Token: 0x040054A2 RID: 21666
		public Vector2 min;

		// Token: 0x040054A3 RID: 21667
		public Vector2 max;
	}
}
