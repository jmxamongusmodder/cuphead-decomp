using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CF4 RID: 3316
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	public class Twirl : ImageEffectBase
	{
		// Token: 0x06005274 RID: 21108 RVA: 0x002A4CFD File Offset: 0x002A30FD
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x04005716 RID: 22294
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		// Token: 0x04005717 RID: 22295
		public float angle = 50f;

		// Token: 0x04005718 RID: 22296
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
