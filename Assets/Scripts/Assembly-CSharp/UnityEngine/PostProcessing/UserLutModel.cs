using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BF2 RID: 3058
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06004916 RID: 18710 RVA: 0x002643CD File Offset: 0x002627CD
		// (set) Token: 0x06004917 RID: 18711 RVA: 0x002643D5 File Offset: 0x002627D5
		public UserLutModel.Settings settings
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

		// Token: 0x06004918 RID: 18712 RVA: 0x002643DE File Offset: 0x002627DE
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x04004F30 RID: 20272
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x02000BF3 RID: 3059
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000687 RID: 1671
			// (get) Token: 0x06004919 RID: 18713 RVA: 0x002643EC File Offset: 0x002627EC
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04004F31 RID: 20273
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04004F32 RID: 20274
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
