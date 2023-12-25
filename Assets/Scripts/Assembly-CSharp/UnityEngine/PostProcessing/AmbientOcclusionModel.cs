using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BBC RID: 3004
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060048BD RID: 18621 RVA: 0x0026343A File Offset: 0x0026183A
		// (set) Token: 0x060048BE RID: 18622 RVA: 0x00263442 File Offset: 0x00261842
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x060048BF RID: 18623 RVA: 0x0026344B File Offset: 0x0026184B
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x04004E72 RID: 20082
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x02000BBD RID: 3005
		public enum SampleCount
		{
			// Token: 0x04004E74 RID: 20084
			Lowest = 3,
			// Token: 0x04004E75 RID: 20085
			Low = 6,
			// Token: 0x04004E76 RID: 20086
			Medium = 10,
			// Token: 0x04004E77 RID: 20087
			High = 16
		}

		// Token: 0x02000BBE RID: 3006
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700065C RID: 1628
			// (get) Token: 0x060048C0 RID: 18624 RVA: 0x00263458 File Offset: 0x00261858
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x04004E78 RID: 20088
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x04004E79 RID: 20089
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04004E7A RID: 20090
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04004E7B RID: 20091
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x04004E7C RID: 20092
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x04004E7D RID: 20093
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x04004E7E RID: 20094
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
