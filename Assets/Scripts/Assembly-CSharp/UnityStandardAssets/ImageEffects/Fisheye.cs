using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CD9 RID: 3289
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Displacement/Fisheye")]
	internal class Fisheye : PostEffectsBase
	{
		// Token: 0x06005219 RID: 21017 RVA: 0x002A18FA File Offset: 0x0029FCFA
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x002A1934 File Offset: 0x0029FD34
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 0.15625f;
			float num2 = (float)source.width * 1f / ((float)source.height * 1f);
			this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num, this.strengthY * num, this.strengthX * num2 * num, this.strengthY * num));
			Graphics.Blit(source, destination, this.fisheyeMaterial);
		}

		// Token: 0x04005676 RID: 22134
		public float strengthX = 0.05f;

		// Token: 0x04005677 RID: 22135
		public float strengthY = 0.05f;

		// Token: 0x04005678 RID: 22136
		public Shader fishEyeShader;

		// Token: 0x04005679 RID: 22137
		private Material fisheyeMaterial;
	}
}
