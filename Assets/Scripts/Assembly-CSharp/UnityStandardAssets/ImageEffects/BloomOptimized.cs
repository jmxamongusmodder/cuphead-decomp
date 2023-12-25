using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CC0 RID: 3264
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	public class BloomOptimized : PostEffectsBase
	{
		// Token: 0x060051BC RID: 20924 RVA: 0x0029CF94 File Offset: 0x0029B394
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x0029CFCD File Offset: 0x0029B3CD
		private void OnDisable()
		{
			if (this.fastBloomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.fastBloomMaterial);
			}
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x0029CFEC File Offset: 0x0029B3EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = (this.resolution != BloomOptimized.Resolution.Low) ? 2 : 4;
			float num2 = (this.resolution != BloomOptimized.Resolution.Low) ? 1f : 0.5f;
			this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.fastBloomMaterial, 1);
			int num3 = (this.blurType != BloomOptimized.BlurType.Standard) ? 2 : 0;
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2 + (float)i * 1f, 0f, this.threshold, this.intensity));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 2 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 3 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.fastBloomMaterial.SetTexture("_Bloom", renderTexture);
			Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04005590 RID: 21904
		[Range(0f, 1.5f)]
		public float threshold = 0.25f;

		// Token: 0x04005591 RID: 21905
		[Range(0f, 2.5f)]
		public float intensity = 0.75f;

		// Token: 0x04005592 RID: 21906
		[Range(0.25f, 5.5f)]
		public float blurSize = 1f;

		// Token: 0x04005593 RID: 21907
		private BloomOptimized.Resolution resolution;

		// Token: 0x04005594 RID: 21908
		[Range(1f, 4f)]
		public int blurIterations = 1;

		// Token: 0x04005595 RID: 21909
		public BloomOptimized.BlurType blurType;

		// Token: 0x04005596 RID: 21910
		public Shader fastBloomShader;

		// Token: 0x04005597 RID: 21911
		private Material fastBloomMaterial;

		// Token: 0x02000CC1 RID: 3265
		public enum Resolution
		{
			// Token: 0x04005599 RID: 21913
			Low,
			// Token: 0x0400559A RID: 21914
			High
		}

		// Token: 0x02000CC2 RID: 3266
		public enum BlurType
		{
			// Token: 0x0400559C RID: 21916
			Standard,
			// Token: 0x0400559D RID: 21917
			Sgx
		}
	}
}
