using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BE0 RID: 3040
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060048F8 RID: 18680 RVA: 0x00264099 File Offset: 0x00262499
		// (set) Token: 0x060048F9 RID: 18681 RVA: 0x002640A1 File Offset: 0x002624A1
		public DitheringModel.Settings settings
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

		// Token: 0x060048FA RID: 18682 RVA: 0x002640AA File Offset: 0x002624AA
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x04004EFE RID: 20222
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x02000BE1 RID: 3041
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700067B RID: 1659
			// (get) Token: 0x060048FB RID: 18683 RVA: 0x002640B8 File Offset: 0x002624B8
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
