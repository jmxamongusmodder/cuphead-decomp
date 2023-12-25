using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BC7 RID: 3015
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x0026383D File Offset: 0x00261C3D
		// (set) Token: 0x060048CC RID: 18636 RVA: 0x00263845 File Offset: 0x00261C45
		public BloomModel.Settings settings
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

		// Token: 0x060048CD RID: 18637 RVA: 0x0026384E File Offset: 0x00261C4E
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04004E9A RID: 20122
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x02000BC8 RID: 3016
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x17000662 RID: 1634
			// (get) Token: 0x060048CF RID: 18639 RVA: 0x00263869 File Offset: 0x00261C69
			// (set) Token: 0x060048CE RID: 18638 RVA: 0x0026385B File Offset: 0x00261C5B
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x17000663 RID: 1635
			// (get) Token: 0x060048D0 RID: 18640 RVA: 0x00263878 File Offset: 0x00261C78
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04004E9B RID: 20123
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x04004E9C RID: 20124
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x04004E9D RID: 20125
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x04004E9E RID: 20126
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04004E9F RID: 20127
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x02000BC9 RID: 3017
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x17000664 RID: 1636
			// (get) Token: 0x060048D1 RID: 18641 RVA: 0x002638C8 File Offset: 0x00261CC8
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04004EA0 RID: 20128
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04004EA1 RID: 20129
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x02000BCA RID: 3018
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000665 RID: 1637
			// (get) Token: 0x060048D2 RID: 18642 RVA: 0x002638F4 File Offset: 0x00261CF4
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04004EA2 RID: 20130
			public BloomModel.BloomSettings bloom;

			// Token: 0x04004EA3 RID: 20131
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
