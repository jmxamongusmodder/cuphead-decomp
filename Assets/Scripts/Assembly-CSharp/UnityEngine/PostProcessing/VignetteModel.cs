using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BF4 RID: 3060
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600491B RID: 18715 RVA: 0x00264429 File Offset: 0x00262829
		// (set) Token: 0x0600491C RID: 18716 RVA: 0x00264431 File Offset: 0x00262831
		public VignetteModel.Settings settings
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

		// Token: 0x0600491D RID: 18717 RVA: 0x0026443A File Offset: 0x0026283A
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x04004F33 RID: 20275
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x02000BF5 RID: 3061
		public enum Mode
		{
			// Token: 0x04004F35 RID: 20277
			Classic,
			// Token: 0x04004F36 RID: 20278
			Masked
		}

		// Token: 0x02000BF6 RID: 3062
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000689 RID: 1673
			// (get) Token: 0x0600491E RID: 18718 RVA: 0x00264448 File Offset: 0x00262848
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x04004F37 RID: 20279
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x04004F38 RID: 20280
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x04004F39 RID: 20281
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x04004F3A RID: 20282
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x04004F3B RID: 20283
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x04004F3C RID: 20284
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04004F3D RID: 20285
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04004F3E RID: 20286
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04004F3F RID: 20287
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
