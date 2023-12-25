using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CCD RID: 3277
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
	public class ContrastStretch : MonoBehaviour
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060051EC RID: 20972 RVA: 0x0029F0D1 File Offset: 0x0029D4D1
		protected Material materialLum
		{
			get
			{
				if (this.m_materialLum == null)
				{
					this.m_materialLum = new Material(this.shaderLum);
					this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialLum;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060051ED RID: 20973 RVA: 0x0029F108 File Offset: 0x0029D508
		protected Material materialReduce
		{
			get
			{
				if (this.m_materialReduce == null)
				{
					this.m_materialReduce = new Material(this.shaderReduce);
					this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialReduce;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060051EE RID: 20974 RVA: 0x0029F13F File Offset: 0x0029D53F
		protected Material materialAdapt
		{
			get
			{
				if (this.m_materialAdapt == null)
				{
					this.m_materialAdapt = new Material(this.shaderAdapt);
					this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialAdapt;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060051EF RID: 20975 RVA: 0x0029F176 File Offset: 0x0029D576
		protected Material materialApply
		{
			get
			{
				if (this.m_materialApply == null)
				{
					this.m_materialApply = new Material(this.shaderApply);
					this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialApply;
			}
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x0029F1B0 File Offset: 0x0029D5B0
		private void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x0029F218 File Offset: 0x0029D618
		private void OnEnable()
		{
			for (int i = 0; i < 2; i++)
			{
				if (!this.adaptRenderTex[i])
				{
					this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
					this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
				}
			}
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x0029F268 File Offset: 0x0029D668
		private void OnDisable()
		{
			for (int i = 0; i < 2; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
				this.adaptRenderTex[i] = null;
			}
			if (this.m_materialLum)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialLum);
			}
			if (this.m_materialReduce)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
			}
			if (this.m_materialAdapt)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
			}
			if (this.m_materialApply)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialApply);
			}
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x0029F30C File Offset: 0x0029D70C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, renderTexture, this.materialLum);
			while (renderTexture.width > 1 || renderTexture.height > 1)
			{
				int num = renderTexture.width / 2;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = renderTexture.height / 2;
				if (num2 < 1)
				{
					num2 = 1;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
				Graphics.Blit(renderTexture, temporary, this.materialReduce);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.CalculateAdaptation(renderTexture);
			this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
			Graphics.Blit(source, destination, this.materialApply);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x0029F3D0 File Offset: 0x0029D7D0
		private void CalculateAdaptation(Texture curTexture)
		{
			int num = this.curAdaptIndex;
			this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
			float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
			num2 = Mathf.Clamp(num2, 0.01f, 1f);
			this.materialAdapt.SetTexture("_CurTex", curTexture);
			this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
			Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
			GL.Clear(false, true, Color.black);
			Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
		}

		// Token: 0x040055F6 RID: 22006
		public float adaptationSpeed = 0.02f;

		// Token: 0x040055F7 RID: 22007
		public float limitMinimum = 0.2f;

		// Token: 0x040055F8 RID: 22008
		public float limitMaximum = 0.6f;

		// Token: 0x040055F9 RID: 22009
		private RenderTexture[] adaptRenderTex = new RenderTexture[2];

		// Token: 0x040055FA RID: 22010
		private int curAdaptIndex;

		// Token: 0x040055FB RID: 22011
		public Shader shaderLum;

		// Token: 0x040055FC RID: 22012
		private Material m_materialLum;

		// Token: 0x040055FD RID: 22013
		public Shader shaderReduce;

		// Token: 0x040055FE RID: 22014
		private Material m_materialReduce;

		// Token: 0x040055FF RID: 22015
		public Shader shaderAdapt;

		// Token: 0x04005600 RID: 22016
		private Material m_materialAdapt;

		// Token: 0x04005601 RID: 22017
		public Shader shaderApply;

		// Token: 0x04005602 RID: 22018
		private Material m_materialApply;
	}
}
