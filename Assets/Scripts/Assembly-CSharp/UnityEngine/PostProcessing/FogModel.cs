using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BE5 RID: 3045
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06004902 RID: 18690 RVA: 0x0026419E File Offset: 0x0026259E
		// (set) Token: 0x06004903 RID: 18691 RVA: 0x002641A6 File Offset: 0x002625A6
		public FogModel.Settings settings
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

		// Token: 0x06004904 RID: 18692 RVA: 0x002641AF File Offset: 0x002625AF
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04004F0E RID: 20238
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x02000BE6 RID: 3046
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700067F RID: 1663
			// (get) Token: 0x06004905 RID: 18693 RVA: 0x002641BC File Offset: 0x002625BC
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04004F0F RID: 20239
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
