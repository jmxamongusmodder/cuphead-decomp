using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CF5 RID: 3317
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
	public class VignetteAndChromaticAberration : PostEffectsBase
	{
		// Token: 0x06005276 RID: 21110 RVA: 0x002A4D78 File Offset: 0x002A3178
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
			this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
			this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x002A4DEC File Offset: 0x002A31EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			bool flag = Mathf.Abs(this.blur) > 0f || Mathf.Abs(this.intensity) > 0f;
			float num = 1f * (float)width / (1f * (float)height);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (flag)
			{
				renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				if (Mathf.Abs(this.blur) > 0f)
				{
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(source, renderTexture2, this.m_ChromAberrationMaterial, 0);
					for (int i = 0; i < 2; i++)
					{
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
						RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(renderTexture2, temporary, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(renderTexture2);
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 0.001953125f / num, 0f, 0f, 0f));
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(temporary, renderTexture2, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(temporary);
					}
				}
				this.m_VignetteMaterial.SetFloat("_Intensity", this.intensity);
				this.m_VignetteMaterial.SetFloat("_Blur", this.blur);
				this.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
				Graphics.Blit(source, renderTexture, this.m_VignetteMaterial, 0);
			}
			this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
			this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
			this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
			this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
			if (flag)
			{
				renderTexture.wrapMode = TextureWrapMode.Clamp;
			}
			else
			{
				source.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit((!flag) ? source : renderTexture, destination, this.m_ChromAberrationMaterial, (this.mode != VignetteAndChromaticAberration.AberrationMode.Advanced) ? 1 : 2);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture2);
		}

		// Token: 0x04005719 RID: 22297
		public VignetteAndChromaticAberration.AberrationMode mode;

		// Token: 0x0400571A RID: 22298
		public float intensity = 0.375f;

		// Token: 0x0400571B RID: 22299
		public float chromaticAberration = 0.2f;

		// Token: 0x0400571C RID: 22300
		public float axialAberration = 0.5f;

		// Token: 0x0400571D RID: 22301
		public float blur;

		// Token: 0x0400571E RID: 22302
		public float blurSpread = 0.75f;

		// Token: 0x0400571F RID: 22303
		public float luminanceDependency = 0.25f;

		// Token: 0x04005720 RID: 22304
		public float blurDistance = 2.5f;

		// Token: 0x04005721 RID: 22305
		public Shader vignetteShader;

		// Token: 0x04005722 RID: 22306
		public Shader separableBlurShader;

		// Token: 0x04005723 RID: 22307
		public Shader chromAberrationShader;

		// Token: 0x04005724 RID: 22308
		private Material m_VignetteMaterial;

		// Token: 0x04005725 RID: 22309
		private Material m_SeparableBlurMaterial;

		// Token: 0x04005726 RID: 22310
		private Material m_ChromAberrationMaterial;

		// Token: 0x02000CF6 RID: 3318
		public enum AberrationMode
		{
			// Token: 0x04005728 RID: 22312
			Simple,
			// Token: 0x04005729 RID: 22313
			Advanced
		}
	}
}
