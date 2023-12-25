using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CDB RID: 3291
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
	public class Grayscale : ImageEffectBase
	{
		// Token: 0x06005220 RID: 21024 RVA: 0x002A1EB9 File Offset: 0x002A02B9
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			base.material.SetFloat("_RampOffset", this.rampOffset);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x04005682 RID: 22146
		public Texture textureRamp;

		// Token: 0x04005683 RID: 22147
		public float rampOffset;
	}
}
