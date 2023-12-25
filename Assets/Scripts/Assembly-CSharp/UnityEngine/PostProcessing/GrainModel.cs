using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BE7 RID: 3047
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06004907 RID: 18695 RVA: 0x002641ED File Offset: 0x002625ED
		// (set) Token: 0x06004908 RID: 18696 RVA: 0x002641F5 File Offset: 0x002625F5
		public GrainModel.Settings settings
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

		// Token: 0x06004909 RID: 18697 RVA: 0x002641FE File Offset: 0x002625FE
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x04004F10 RID: 20240
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x02000BE8 RID: 3048
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000681 RID: 1665
			// (get) Token: 0x0600490A RID: 18698 RVA: 0x0026420C File Offset: 0x0026260C
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04004F11 RID: 20241
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04004F12 RID: 20242
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x04004F13 RID: 20243
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x04004F14 RID: 20244
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
