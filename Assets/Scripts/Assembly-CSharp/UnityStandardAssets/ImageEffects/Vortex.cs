using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CF7 RID: 3319
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Vortex")]
	public class Vortex : ImageEffectBase
	{
		// Token: 0x06005279 RID: 21113 RVA: 0x002A50CF File Offset: 0x002A34CF
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x0400572A RID: 22314
		public Vector2 radius = new Vector2(0.4f, 0.4f);

		// Token: 0x0400572B RID: 22315
		public float angle = 50f;

		// Token: 0x0400572C RID: 22316
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
