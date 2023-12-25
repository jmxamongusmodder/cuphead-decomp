using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CCE RID: 3278
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
	internal class CreaseShading : PostEffectsBase
	{
		// Token: 0x060051F6 RID: 20982 RVA: 0x0029F4C4 File Offset: 0x0029D8C4
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			this.depthFetchMaterial = base.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
			this.creaseApplyMaterial = base.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x0029F538 File Offset: 0x0029D938
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			float num = 1f * (float)width / (1f * (float)height);
			float num2 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			RenderTexture renderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0);
			Graphics.Blit(source, temporary, this.depthFetchMaterial);
			Graphics.Blit(temporary, renderTexture);
			for (int i = 0; i < this.softness; i++)
			{
				RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				this.blurMaterial.SetVector("offsets", new Vector4(0f, this.spread * num2, 0f, 0f));
				Graphics.Blit(renderTexture, temporary2, this.blurMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary2;
				temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, temporary2, this.blurMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary2;
			}
			this.creaseApplyMaterial.SetTexture("_HrDepthTex", temporary);
			this.creaseApplyMaterial.SetTexture("_LrDepthTex", renderTexture);
			this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
			Graphics.Blit(source, destination, this.creaseApplyMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04005603 RID: 22019
		public float intensity = 0.5f;

		// Token: 0x04005604 RID: 22020
		public int softness = 1;

		// Token: 0x04005605 RID: 22021
		public float spread = 1f;

		// Token: 0x04005606 RID: 22022
		public Shader blurShader;

		// Token: 0x04005607 RID: 22023
		private Material blurMaterial;

		// Token: 0x04005608 RID: 22024
		public Shader depthFetchShader;

		// Token: 0x04005609 RID: 22025
		private Material depthFetchMaterial;

		// Token: 0x0400560A RID: 22026
		public Shader creaseApplyShader;

		// Token: 0x0400560B RID: 22027
		private Material creaseApplyMaterial;
	}
}
