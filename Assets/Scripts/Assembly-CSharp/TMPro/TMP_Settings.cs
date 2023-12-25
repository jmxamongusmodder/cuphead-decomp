using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C77 RID: 3191
	[ExecuteInEditMode]
	[Serializable]
	public class TMP_Settings : ScriptableObject
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06004FF1 RID: 20465 RVA: 0x00295260 File Offset: 0x00293660
		public static bool enableWordWrapping
		{
			get
			{
				return TMP_Settings.instance.m_enableWordWrapping;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06004FF2 RID: 20466 RVA: 0x0029526C File Offset: 0x0029366C
		public static bool enableKerning
		{
			get
			{
				return TMP_Settings.instance.m_enableKerning;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06004FF3 RID: 20467 RVA: 0x00295278 File Offset: 0x00293678
		public static bool enableExtraPadding
		{
			get
			{
				return TMP_Settings.instance.m_enableExtraPadding;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06004FF4 RID: 20468 RVA: 0x00295284 File Offset: 0x00293684
		public static bool enableTintAllSprites
		{
			get
			{
				return TMP_Settings.instance.m_enableTintAllSprites;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06004FF5 RID: 20469 RVA: 0x00295290 File Offset: 0x00293690
		public static bool warningsDisabled
		{
			get
			{
				return TMP_Settings.instance.m_warningsDisabled;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06004FF6 RID: 20470 RVA: 0x0029529C File Offset: 0x0029369C
		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontAsset;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06004FF7 RID: 20471 RVA: 0x002952A8 File Offset: 0x002936A8
		public static List<TMP_FontAsset> fallbackFontAssets
		{
			get
			{
				return TMP_Settings.instance.m_fallbackFontAssets;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06004FF8 RID: 20472 RVA: 0x002952B4 File Offset: 0x002936B4
		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultSpriteAsset;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06004FF9 RID: 20473 RVA: 0x002952C0 File Offset: 0x002936C0
		public static TMP_StyleSheet defaultStyleSheet
		{
			get
			{
				return TMP_Settings.instance.m_defaultStyleSheet;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06004FFA RID: 20474 RVA: 0x002952CC File Offset: 0x002936CC
		public static TMP_Settings instance
		{
			get
			{
				if (TMP_Settings.s_Instance == null)
				{
					TMP_Settings.s_Instance = (Resources.Load("TMP Settings") as TMP_Settings);
				}
				return TMP_Settings.s_Instance;
			}
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x002952F8 File Offset: 0x002936F8
		public static TMP_Settings LoadDefaultSettings()
		{
			if (TMP_Settings.s_Instance == null)
			{
				TMP_Settings x = Resources.Load("TMP Settings") as TMP_Settings;
				if (x != null)
				{
					TMP_Settings.s_Instance = x;
				}
			}
			return TMP_Settings.s_Instance;
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x0029533C File Offset: 0x0029373C
		public static TMP_Settings GetSettings()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance;
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x00295355 File Offset: 0x00293755
		public static TMP_FontAsset GetFontAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultFontAsset;
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x00295373 File Offset: 0x00293773
		public static TMP_SpriteAsset GetSpriteAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultSpriteAsset;
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x00295391 File Offset: 0x00293791
		public static TMP_StyleSheet GetStyleSheet()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultStyleSheet;
		}

		// Token: 0x040052A1 RID: 21153
		private static TMP_Settings s_Instance;

		// Token: 0x040052A2 RID: 21154
		[SerializeField]
		private bool m_enableWordWrapping;

		// Token: 0x040052A3 RID: 21155
		[SerializeField]
		private bool m_enableKerning;

		// Token: 0x040052A4 RID: 21156
		[SerializeField]
		private bool m_enableExtraPadding;

		// Token: 0x040052A5 RID: 21157
		[SerializeField]
		private bool m_enableTintAllSprites;

		// Token: 0x040052A6 RID: 21158
		[SerializeField]
		private bool m_warningsDisabled;

		// Token: 0x040052A7 RID: 21159
		[SerializeField]
		private TMP_FontAsset m_defaultFontAsset;

		// Token: 0x040052A8 RID: 21160
		[SerializeField]
		private List<TMP_FontAsset> m_fallbackFontAssets;

		// Token: 0x040052A9 RID: 21161
		[SerializeField]
		private TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x040052AA RID: 21162
		[SerializeField]
		private TMP_StyleSheet m_defaultStyleSheet;
	}
}
