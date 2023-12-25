using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CE7 RID: 3303
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
	public class ScreenSpaceAmbientOcclusion : MonoBehaviour
	{
		// Token: 0x06005258 RID: 21080 RVA: 0x002A386C File Offset: 0x002A1C6C
		private static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x002A3896 File Offset: 0x002A1C96
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x002A38AC File Offset: 0x002A1CAC
		private void OnDisable()
		{
			ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x002A38BC File Offset: 0x002A1CBC
		private void Start()
		{
			if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.m_Supported = true;
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x002A392A File Offset: 0x002A1D2A
		private void OnEnable()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x002A3940 File Offset: 0x002A1D40
		private void CreateMaterials()
		{
			if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
			{
				this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
				this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
			}
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x002A3994 File Offset: 0x002A1D94
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Supported || !this.m_SSAOShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
			this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
			this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
			this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
			this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
			this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
			float fieldOfView = base.GetComponent<Camera>().fieldOfView;
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			float num = Mathf.Tan(fieldOfView * 0.017453292f * 0.5f) * farClipPlane;
			float x = num * base.GetComponent<Camera>().aspect;
			this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(x, num, farClipPlane));
			int num2;
			int num3;
			if (this.m_RandomTexture)
			{
				num2 = this.m_RandomTexture.width;
				num3 = this.m_RandomTexture.height;
			}
			else
			{
				num2 = 1;
				num3 = 1;
			}
			this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num2, (float)renderTexture.height / (float)num3, 0f));
			this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
			bool flag = this.m_Blur > 0;
			Graphics.Blit((!flag) ? source : null, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
			if (flag)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
				Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
				Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary);
				renderTexture = temporary2;
			}
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040056C5 RID: 22213
		public float m_Radius = 0.4f;

		// Token: 0x040056C6 RID: 22214
		public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;

		// Token: 0x040056C7 RID: 22215
		public float m_OcclusionIntensity = 1.5f;

		// Token: 0x040056C8 RID: 22216
		public int m_Blur = 2;

		// Token: 0x040056C9 RID: 22217
		public int m_Downsampling = 2;

		// Token: 0x040056CA RID: 22218
		public float m_OcclusionAttenuation = 1f;

		// Token: 0x040056CB RID: 22219
		public float m_MinZ = 0.01f;

		// Token: 0x040056CC RID: 22220
		public Shader m_SSAOShader;

		// Token: 0x040056CD RID: 22221
		private Material m_SSAOMaterial;

		// Token: 0x040056CE RID: 22222
		public Texture2D m_RandomTexture;

		// Token: 0x040056CF RID: 22223
		private bool m_Supported;

		// Token: 0x02000CE8 RID: 3304
		public enum SSAOSamples
		{
			// Token: 0x040056D1 RID: 22225
			Low,
			// Token: 0x040056D2 RID: 22226
			Medium,
			// Token: 0x040056D3 RID: 22227
			High
		}
	}
}
