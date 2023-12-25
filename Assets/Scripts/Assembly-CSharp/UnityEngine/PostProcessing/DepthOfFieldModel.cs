using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BDD RID: 3037
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060048F3 RID: 18675 RVA: 0x0026401D File Offset: 0x0026241D
		// (set) Token: 0x060048F4 RID: 18676 RVA: 0x00264025 File Offset: 0x00262425
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x060048F5 RID: 18677 RVA: 0x0026402E File Offset: 0x0026242E
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x04004EF3 RID: 20211
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x02000BDE RID: 3038
		public enum KernelSize
		{
			// Token: 0x04004EF5 RID: 20213
			Small,
			// Token: 0x04004EF6 RID: 20214
			Medium,
			// Token: 0x04004EF7 RID: 20215
			Large,
			// Token: 0x04004EF8 RID: 20216
			VeryLarge
		}

		// Token: 0x02000BDF RID: 3039
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000679 RID: 1657
			// (get) Token: 0x060048F6 RID: 18678 RVA: 0x0026403C File Offset: 0x0026243C
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x04004EF9 RID: 20217
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04004EFA RID: 20218
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04004EFB RID: 20219
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04004EFC RID: 20220
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04004EFD RID: 20221
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
