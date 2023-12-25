using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CB4 RID: 3252
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : PostEffectsBase
	{
		// Token: 0x060051A9 RID: 20905 RVA: 0x0029B5B0 File Offset: 0x002999B0
		public Material CurrentAAMaterial()
		{
			Material result;
			switch (this.mode)
			{
			case AAMode.FXAA2:
				result = this.materialFXAAII;
				break;
			case AAMode.FXAA3Console:
				result = this.materialFXAAIII;
				break;
			case AAMode.FXAA1PresetA:
				result = this.materialFXAAPreset2;
				break;
			case AAMode.FXAA1PresetB:
				result = this.materialFXAAPreset3;
				break;
			case AAMode.NFAA:
				result = this.nfaa;
				break;
			case AAMode.SSAA:
				result = this.ssaa;
				break;
			case AAMode.DLAA:
				result = this.dlaa;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x0029B64C File Offset: 0x00299A4C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
			this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
			this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
			this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
			this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
			this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
			this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
			if (!this.ssaaShader.isSupported)
			{
				base.NotSupported();
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x0029B72C File Offset: 0x00299B2C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.mode == AAMode.FXAA3Console && this.materialFXAAIII != null)
			{
				this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
				this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
				this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
				Graphics.Blit(source, destination, this.materialFXAAIII);
			}
			else if (this.mode == AAMode.FXAA1PresetB && this.materialFXAAPreset3 != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAPreset3);
			}
			else if (this.mode == AAMode.FXAA1PresetA && this.materialFXAAPreset2 != null)
			{
				source.anisoLevel = 4;
				Graphics.Blit(source, destination, this.materialFXAAPreset2);
				source.anisoLevel = 0;
			}
			else if (this.mode == AAMode.FXAA2 && this.materialFXAAII != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAII);
			}
			else if (this.mode == AAMode.SSAA && this.ssaa != null)
			{
				Graphics.Blit(source, destination, this.ssaa);
			}
			else if (this.mode == AAMode.DLAA && this.dlaa != null)
			{
				source.anisoLevel = 0;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(source, temporary, this.dlaa, 0);
				Graphics.Blit(temporary, destination, this.dlaa, (!this.dlaaSharp) ? 1 : 2);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else if (this.mode == AAMode.NFAA && this.nfaa != null)
			{
				source.anisoLevel = 0;
				this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
				this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
				Graphics.Blit(source, destination, this.nfaa, (!this.showGeneratedNormals) ? 0 : 1);
			}
			else
			{
				Graphics.Blit(source, destination);
			}
		}

		// Token: 0x0400551B RID: 21787
		public AAMode mode = AAMode.FXAA3Console;

		// Token: 0x0400551C RID: 21788
		public bool showGeneratedNormals;

		// Token: 0x0400551D RID: 21789
		public float offsetScale = 0.2f;

		// Token: 0x0400551E RID: 21790
		public float blurRadius = 18f;

		// Token: 0x0400551F RID: 21791
		public float edgeThresholdMin = 0.05f;

		// Token: 0x04005520 RID: 21792
		public float edgeThreshold = 0.2f;

		// Token: 0x04005521 RID: 21793
		public float edgeSharpness = 4f;

		// Token: 0x04005522 RID: 21794
		public bool dlaaSharp;

		// Token: 0x04005523 RID: 21795
		public Shader ssaaShader;

		// Token: 0x04005524 RID: 21796
		private Material ssaa;

		// Token: 0x04005525 RID: 21797
		public Shader dlaaShader;

		// Token: 0x04005526 RID: 21798
		private Material dlaa;

		// Token: 0x04005527 RID: 21799
		public Shader nfaaShader;

		// Token: 0x04005528 RID: 21800
		private Material nfaa;

		// Token: 0x04005529 RID: 21801
		public Shader shaderFXAAPreset2;

		// Token: 0x0400552A RID: 21802
		private Material materialFXAAPreset2;

		// Token: 0x0400552B RID: 21803
		public Shader shaderFXAAPreset3;

		// Token: 0x0400552C RID: 21804
		private Material materialFXAAPreset3;

		// Token: 0x0400552D RID: 21805
		public Shader shaderFXAAII;

		// Token: 0x0400552E RID: 21806
		private Material materialFXAAII;

		// Token: 0x0400552F RID: 21807
		public Shader shaderFXAAIII;

		// Token: 0x04005530 RID: 21808
		private Material materialFXAAIII;
	}
}
