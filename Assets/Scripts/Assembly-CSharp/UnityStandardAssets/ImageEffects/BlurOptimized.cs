using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CC4 RID: 3268
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		// Token: 0x060051C8 RID: 20936 RVA: 0x0029D3E4 File Offset: 0x0029B7E4
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x0029D41D File Offset: 0x0029B81D
		public void OnDisable()
		{
			if (this.blurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.blurMaterial);
			}
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x0029D43C File Offset: 0x0029B83C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = (float)destination.width / (float)destination.height;
			float num2 = (num >= 1.7777778f) ? 1f : (num / 1.7777778f);
			num2 *= 1f - 0.1f * SettingsData.Data.overscan;
			float num3 = (float)destination.height / 1080f * 1f / (1f * (float)(1 << this.downsample));
			num3 *= num2;
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num3, -this.blurSize * num3, 0f, 0f));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width >> this.downsample;
			int height = source.height >> this.downsample;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
			int num4 = (this.blurType != BlurOptimized.BlurType.StandardGauss) ? 2 : 0;
			for (int i = 0; i < this.blurIterations; i++)
			{
				float num5 = (float)i * 1f;
				this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num3 + num5, -this.blurSize * num3 - num5, 0f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040055A2 RID: 21922
		[Range(0f, 2f)]
		public int downsample = 1;

		// Token: 0x040055A3 RID: 21923
		[Range(0f, 10f)]
		public float blurSize = 3f;

		// Token: 0x040055A4 RID: 21924
		[Range(1f, 4f)]
		public int blurIterations = 2;

		// Token: 0x040055A5 RID: 21925
		public BlurOptimized.BlurType blurType;

		// Token: 0x040055A6 RID: 21926
		public Shader blurShader;

		// Token: 0x040055A7 RID: 21927
		private Material blurMaterial;

		// Token: 0x02000CC5 RID: 3269
		public enum BlurType
		{
			// Token: 0x040055A9 RID: 21929
			StandardGauss,
			// Token: 0x040055AA RID: 21930
			SgxGauss
		}
	}
}
