using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CE0 RID: 3296
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	public class NoiseAndScratches : MonoBehaviour
	{
		// Token: 0x06005232 RID: 21042 RVA: 0x002A2840 File Offset: 0x002A0C40
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (this.shaderRGB == null || this.shaderYUV == null)
			{
				base.enabled = false;
			}
			else if (!this.shaderRGB.isSupported)
			{
				base.enabled = false;
			}
			else if (!this.shaderYUV.isSupported)
			{
				this.rgbFallback = true;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x002A28C0 File Offset: 0x002A0CC0
		protected Material material
		{
			get
			{
				if (this.m_MaterialRGB == null)
				{
					this.m_MaterialRGB = new Material(this.shaderRGB);
					this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.m_MaterialYUV == null && !this.rgbFallback)
				{
					this.m_MaterialYUV = new Material(this.shaderYUV);
					this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
				}
				return (this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV;
			}
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x002A295D File Offset: 0x002A0D5D
		protected void OnDisable()
		{
			if (this.m_MaterialRGB)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialRGB);
			}
			if (this.m_MaterialYUV)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialYUV);
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x002A2998 File Offset: 0x002A0D98
		private void SanitizeParameters()
		{
			this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
			this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
			this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
			this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
			this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
			this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
			this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x002A2A64 File Offset: 0x002A0E64
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SanitizeParameters();
			if (this.scratchTimeLeft <= 0f)
			{
				this.scratchTimeLeft = UnityEngine.Random.value * 2f / this.scratchFPS;
				this.scratchX = UnityEngine.Random.value;
				this.scratchY = UnityEngine.Random.value;
			}
			this.scratchTimeLeft -= Time.deltaTime;
			Material material = this.material;
			material.SetTexture("_GrainTex", this.grainTexture);
			material.SetTexture("_ScratchTex", this.scratchTexture);
			float num = 1f / this.grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + UnityEngine.Random.value * this.scratchJitter, this.scratchY + UnityEngine.Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(UnityEngine.Random.Range(this.grainIntensityMin, this.grainIntensityMax), UnityEngine.Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0400569B RID: 22171
		public bool monochrome = true;

		// Token: 0x0400569C RID: 22172
		private bool rgbFallback;

		// Token: 0x0400569D RID: 22173
		public float grainIntensityMin = 0.1f;

		// Token: 0x0400569E RID: 22174
		public float grainIntensityMax = 0.2f;

		// Token: 0x0400569F RID: 22175
		public float grainSize = 2f;

		// Token: 0x040056A0 RID: 22176
		public float scratchIntensityMin = 0.05f;

		// Token: 0x040056A1 RID: 22177
		public float scratchIntensityMax = 0.25f;

		// Token: 0x040056A2 RID: 22178
		public float scratchFPS = 10f;

		// Token: 0x040056A3 RID: 22179
		public float scratchJitter = 0.01f;

		// Token: 0x040056A4 RID: 22180
		public Texture grainTexture;

		// Token: 0x040056A5 RID: 22181
		public Texture scratchTexture;

		// Token: 0x040056A6 RID: 22182
		public Shader shaderRGB;

		// Token: 0x040056A7 RID: 22183
		public Shader shaderYUV;

		// Token: 0x040056A8 RID: 22184
		private Material m_MaterialRGB;

		// Token: 0x040056A9 RID: 22185
		private Material m_MaterialYUV;

		// Token: 0x040056AA RID: 22186
		private float scratchTimeLeft;

		// Token: 0x040056AB RID: 22187
		private float scratchX;

		// Token: 0x040056AC RID: 22188
		private float scratchY;
	}
}
