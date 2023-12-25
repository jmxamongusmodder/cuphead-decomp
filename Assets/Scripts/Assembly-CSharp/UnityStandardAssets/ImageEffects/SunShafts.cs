using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CEA RID: 3306
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
	public class SunShafts : PostEffectsBase
	{
		// Token: 0x06005262 RID: 21090 RVA: 0x002A3D34 File Offset: 0x002A2134
		public override bool CheckResources()
		{
			base.CheckSupport(this.useDepthTexture);
			this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
			this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x002A3D98 File Offset: 0x002A2198
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.useDepthTexture)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			int num = 4;
			if (this.resolution == SunShafts.SunShaftsResolution.Normal)
			{
				num = 2;
			}
			else if (this.resolution == SunShafts.SunShaftsResolution.High)
			{
				num = 1;
			}
			Vector3 vector = Vector3.one * 0.5f;
			if (this.sunTransform)
			{
				vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
			}
			else
			{
				vector = new Vector3(0.5f, 0.5f, 0f);
			}
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
			if (!this.useDepthTexture)
			{
				RenderTextureFormat format = (!base.GetComponent<Camera>().allowHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, format);
				RenderTexture.active = temporary2;
				GL.ClearWithSkybox(false, base.GetComponent<Camera>());
				this.sunShaftsMaterial.SetTexture("_Skybox", temporary2);
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary2);
			}
			else
			{
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
			}
			base.DrawBorder(temporary, this.simpleClearMaterial);
			this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
			float num2 = this.sunShaftBlurRadius * 0.0013020834f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			for (int i = 0; i < this.radialBlurIterations; i++)
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0);
				Graphics.Blit(temporary, temporary3, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
				num2 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
				temporary = RenderTexture.GetTemporary(width, height, 0);
				Graphics.Blit(temporary3, temporary, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary3);
				num2 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			}
			if (vector.z >= 0f)
			{
				this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
			}
			else
			{
				this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
			Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != SunShafts.ShaftsScreenBlendMode.Screen) ? 4 : 0);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x040056D4 RID: 22228
		public SunShafts.SunShaftsResolution resolution = SunShafts.SunShaftsResolution.Normal;

		// Token: 0x040056D5 RID: 22229
		public SunShafts.ShaftsScreenBlendMode screenBlendMode;

		// Token: 0x040056D6 RID: 22230
		public Transform sunTransform;

		// Token: 0x040056D7 RID: 22231
		public int radialBlurIterations = 2;

		// Token: 0x040056D8 RID: 22232
		public Color sunColor = Color.white;

		// Token: 0x040056D9 RID: 22233
		public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

		// Token: 0x040056DA RID: 22234
		public float sunShaftBlurRadius = 2.5f;

		// Token: 0x040056DB RID: 22235
		public float sunShaftIntensity = 1.15f;

		// Token: 0x040056DC RID: 22236
		public float maxRadius = 0.75f;

		// Token: 0x040056DD RID: 22237
		public bool useDepthTexture = true;

		// Token: 0x040056DE RID: 22238
		public Shader sunShaftsShader;

		// Token: 0x040056DF RID: 22239
		private Material sunShaftsMaterial;

		// Token: 0x040056E0 RID: 22240
		public Shader simpleClearShader;

		// Token: 0x040056E1 RID: 22241
		private Material simpleClearMaterial;

		// Token: 0x02000CEB RID: 3307
		public enum SunShaftsResolution
		{
			// Token: 0x040056E3 RID: 22243
			Low,
			// Token: 0x040056E4 RID: 22244
			Normal,
			// Token: 0x040056E5 RID: 22245
			High
		}

		// Token: 0x02000CEC RID: 3308
		public enum ShaftsScreenBlendMode
		{
			// Token: 0x040056E7 RID: 22247
			Screen,
			// Token: 0x040056E8 RID: 22248
			Add
		}
	}
}
