using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CED RID: 3309
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
	internal class TiltShift : PostEffectsBase
	{
		// Token: 0x06005265 RID: 21093 RVA: 0x002A41B1 File Offset: 0x002A25B1
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x002A41EC File Offset: 0x002A25EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize >= 0f) ? this.maxBlurSize : 0f);
			this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
			source.filterMode = FilterMode.Bilinear;
			RenderTexture renderTexture = destination;
			if ((float)this.downsample > 0f)
			{
				renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
				renderTexture.filterMode = FilterMode.Bilinear;
			}
			int num = (int)this.quality;
			num *= 2;
			Graphics.Blit(source, renderTexture, this.tiltShiftMaterial, (this.mode != TiltShift.TiltShiftMode.TiltShiftMode) ? (num + 1) : num);
			if (this.downsample > 0)
			{
				this.tiltShiftMaterial.SetTexture("_Blurred", renderTexture);
				Graphics.Blit(source, destination, this.tiltShiftMaterial, 6);
			}
			if (renderTexture != destination)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040056E9 RID: 22249
		public TiltShift.TiltShiftMode mode;

		// Token: 0x040056EA RID: 22250
		public TiltShift.TiltShiftQuality quality = TiltShift.TiltShiftQuality.Normal;

		// Token: 0x040056EB RID: 22251
		[Range(0f, 15f)]
		public float blurArea = 1f;

		// Token: 0x040056EC RID: 22252
		[Range(0f, 25f)]
		public float maxBlurSize = 5f;

		// Token: 0x040056ED RID: 22253
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040056EE RID: 22254
		public Shader tiltShiftShader;

		// Token: 0x040056EF RID: 22255
		private Material tiltShiftMaterial;

		// Token: 0x02000CEE RID: 3310
		public enum TiltShiftMode
		{
			// Token: 0x040056F1 RID: 22257
			TiltShiftMode,
			// Token: 0x040056F2 RID: 22258
			IrisMode
		}

		// Token: 0x02000CEF RID: 3311
		public enum TiltShiftQuality
		{
			// Token: 0x040056F4 RID: 22260
			Preview,
			// Token: 0x040056F5 RID: 22261
			Normal,
			// Token: 0x040056F6 RID: 22262
			High
		}
	}
}
