using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CE4 RID: 3300
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Screen Overlay")]
	public class ScreenOverlay : PostEffectsBase
	{
		// Token: 0x06005251 RID: 21073 RVA: 0x002A3460 File Offset: 0x002A1860
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x002A349C File Offset: 0x002A189C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector4 value = new Vector4(1f, 0f, 0f, 1f);
			this.overlayMaterial.SetVector("_UV_Transform", value);
			this.overlayMaterial.SetFloat("_Intensity", this.intensity);
			this.overlayMaterial.SetTexture("_Overlay", this.texture);
			Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
		}

		// Token: 0x040056B2 RID: 22194
		public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;

		// Token: 0x040056B3 RID: 22195
		public float intensity = 1f;

		// Token: 0x040056B4 RID: 22196
		public Texture2D texture;

		// Token: 0x040056B5 RID: 22197
		public Shader overlayShader;

		// Token: 0x040056B6 RID: 22198
		private Material overlayMaterial;

		// Token: 0x02000CE5 RID: 3301
		public enum OverlayBlendMode
		{
			// Token: 0x040056B8 RID: 22200
			Additive,
			// Token: 0x040056B9 RID: 22201
			ScreenBlend,
			// Token: 0x040056BA RID: 22202
			Multiply,
			// Token: 0x040056BB RID: 22203
			Overlay,
			// Token: 0x040056BC RID: 22204
			AlphaBlend
		}
	}
}
