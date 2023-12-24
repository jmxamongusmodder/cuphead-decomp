using System;
using UnityEngine;
using System.Collections.Generic;

namespace TMPro
{
	[Serializable]
	public class TMP_Settings : ScriptableObject
	{
		[SerializeField]
		private bool m_enableWordWrapping;
		[SerializeField]
		private bool m_enableKerning;
		[SerializeField]
		private bool m_enableExtraPadding;
		[SerializeField]
		private bool m_enableTintAllSprites;
		[SerializeField]
		private bool m_warningsDisabled;
		[SerializeField]
		private TMP_FontAsset m_defaultFontAsset;
		[SerializeField]
		private List<TMP_FontAsset> m_fallbackFontAssets;
		[SerializeField]
		private TMP_SpriteAsset m_defaultSpriteAsset;
		[SerializeField]
		private TMP_StyleSheet m_defaultStyleSheet;
	}
}
