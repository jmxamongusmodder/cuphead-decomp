using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BE9 RID: 3049
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600490C RID: 18700 RVA: 0x00264261 File Offset: 0x00262661
		// (set) Token: 0x0600490D RID: 18701 RVA: 0x00264269 File Offset: 0x00262669
		public MotionBlurModel.Settings settings
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

		// Token: 0x0600490E RID: 18702 RVA: 0x00264272 File Offset: 0x00262672
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x04004F15 RID: 20245
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x02000BEA RID: 3050
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000683 RID: 1667
			// (get) Token: 0x0600490F RID: 18703 RVA: 0x00264280 File Offset: 0x00262680
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x04004F16 RID: 20246
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x04004F17 RID: 20247
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x04004F18 RID: 20248
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
