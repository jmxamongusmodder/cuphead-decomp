using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CC3 RID: 3267
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Blur")]
	public class Blur : MonoBehaviour
	{
		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060051C0 RID: 20928 RVA: 0x0029D1BC File Offset: 0x0029B5BC
		protected Material material
		{
			get
			{
				if (Blur.m_Material == null)
				{
					Blur.m_Material = new Material(this.blurShader);
					Blur.m_Material.hideFlags = HideFlags.DontSave;
				}
				return Blur.m_Material;
			}
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x0029D1EF File Offset: 0x0029B5EF
		protected void OnDisable()
		{
			if (Blur.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(Blur.m_Material);
			}
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x0029D20C File Offset: 0x0029B60C
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.blurShader || !this.material.shader.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x0029D258 File Offset: 0x0029B658
		public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
		{
			float num = 0.5f + (float)iteration * this.blurSpread;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x0029D2D8 File Offset: 0x0029B6D8
		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			float num = 1f;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x0029D350 File Offset: 0x0029B750
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int width = source.width / 4;
			int height = source.height / 4;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
			this.DownSample4x(source, renderTexture);
			for (int i = 0; i < this.iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				this.FourTapCone(renderTexture, temporary, i);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0400559E RID: 21918
		public int iterations = 3;

		// Token: 0x0400559F RID: 21919
		public float blurSpread = 0.6f;

		// Token: 0x040055A0 RID: 21920
		public Shader blurShader;

		// Token: 0x040055A1 RID: 21921
		private static Material m_Material;
	}
}
