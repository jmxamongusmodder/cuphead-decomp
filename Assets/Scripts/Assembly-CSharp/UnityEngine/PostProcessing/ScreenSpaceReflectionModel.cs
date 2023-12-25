using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BEB RID: 3051
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06004911 RID: 18705 RVA: 0x002642CA File Offset: 0x002626CA
		// (set) Token: 0x06004912 RID: 18706 RVA: 0x002642D2 File Offset: 0x002626D2
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x002642DB File Offset: 0x002626DB
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x04004F19 RID: 20249
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x02000BEC RID: 3052
		public enum SSRResolution
		{
			// Token: 0x04004F1B RID: 20251
			High,
			// Token: 0x04004F1C RID: 20252
			Low = 2
		}

		// Token: 0x02000BED RID: 3053
		public enum SSRReflectionBlendType
		{
			// Token: 0x04004F1E RID: 20254
			PhysicallyBased,
			// Token: 0x04004F1F RID: 20255
			Additive
		}

		// Token: 0x02000BEE RID: 3054
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04004F20 RID: 20256
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04004F21 RID: 20257
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04004F22 RID: 20258
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04004F23 RID: 20259
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x02000BEF RID: 3055
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x04004F24 RID: 20260
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x04004F25 RID: 20261
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x04004F26 RID: 20262
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x04004F27 RID: 20263
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x04004F28 RID: 20264
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x04004F29 RID: 20265
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x04004F2A RID: 20266
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x04004F2B RID: 20267
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x02000BF0 RID: 3056
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04004F2C RID: 20268
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x02000BF1 RID: 3057
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000685 RID: 1669
			// (get) Token: 0x06004914 RID: 18708 RVA: 0x002642E8 File Offset: 0x002626E8
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04004F2D RID: 20269
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04004F2E RID: 20270
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04004F2F RID: 20271
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
