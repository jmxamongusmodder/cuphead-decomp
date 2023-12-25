using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BA7 RID: 2983
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06004873 RID: 18547 RVA: 0x00260C7B File Offset: 0x0025F07B
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x00260CB8 File Offset: 0x0025F0B8
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x00260CBF File Offset: 0x0025F0BF
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x00260CC2 File Offset: 0x0025F0C2
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterImageEffectsOpaque;
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x00260CC8 File Offset: 0x0025F0C8
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color value = (!GraphicsUtils.isLinearColorSpace) ? RenderSettings.fogColor : RenderSettings.fogColor.linear;
			material.SetColor(FogComponent.Uniforms._FogColor, value);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			FogMode fogMode = RenderSettings.fogMode;
			if (fogMode != FogMode.Linear)
			{
				if (fogMode != FogMode.Exponential)
				{
					if (fogMode == FogMode.ExponentialSquared)
					{
						material.EnableKeyword("FOG_EXP2");
					}
				}
				else
				{
					material.EnableKeyword("FOG_EXP");
				}
			}
			else
			{
				material.EnableKeyword("FOG_LINEAR");
			}
			RenderTextureFormat format = (!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, (!settings.excludeSkybox) ? 0 : 1);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x04004DEF RID: 19951
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x02000BA8 RID: 2984
		private static class Uniforms
		{
			// Token: 0x04004DF0 RID: 19952
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x04004DF1 RID: 19953
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x04004DF2 RID: 19954
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x04004DF3 RID: 19955
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x04004DF4 RID: 19956
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
