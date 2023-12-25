using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BD0 RID: 3024
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060048DD RID: 18653 RVA: 0x00263A6D File Offset: 0x00261E6D
		// (set) Token: 0x060048DE RID: 18654 RVA: 0x00263A75 File Offset: 0x00261E75
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x060048DF RID: 18655 RVA: 0x00263A7E File Offset: 0x00261E7E
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x04004EBA RID: 20154
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x02000BD1 RID: 3025
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700066C RID: 1644
			// (get) Token: 0x060048E0 RID: 18656 RVA: 0x00263A8C File Offset: 0x00261E8C
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x04004EBB RID: 20155
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x04004EBC RID: 20156
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
