using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000CA6 RID: 3238
	[Serializable]
	public struct VertexGradient
	{
		// Token: 0x06005191 RID: 20881 RVA: 0x00299EAE File Offset: 0x002982AE
		public VertexGradient(Color color)
		{
			this.topLeft = color;
			this.topRight = color;
			this.bottomLeft = color;
			this.bottomRight = color;
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x00299ECC File Offset: 0x002982CC
		public VertexGradient(Color color0, Color color1, Color color2, Color color3)
		{
			this.topLeft = color0;
			this.topRight = color1;
			this.bottomLeft = color2;
			this.bottomRight = color3;
		}

		// Token: 0x04005476 RID: 21622
		public Color topLeft;

		// Token: 0x04005477 RID: 21623
		public Color topRight;

		// Token: 0x04005478 RID: 21624
		public Color bottomLeft;

		// Token: 0x04005479 RID: 21625
		public Color bottomRight;
	}
}
