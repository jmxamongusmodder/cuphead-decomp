using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class FontReplacer : MonoBehaviour
{
	// Token: 0x06003677 RID: 13943 RVA: 0x001F87F7 File Offset: 0x001F6BF7
	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003E70 RID: 15984
	public Localization localizationAsset;

	// Token: 0x04003E71 RID: 15985
	public Localization.Languages sourceLanguage;

	// Token: 0x04003E72 RID: 15986
	public Localization.Languages destinationLanguage;

	// Token: 0x04003E73 RID: 15987
	public List<Font> allSourceFonts;

	// Token: 0x04003E74 RID: 15988
	public List<Font> allDestinationFonts;

	// Token: 0x04003E75 RID: 15989
	public List<TMP_FontAsset> allSourceFontAssets;

	// Token: 0x04003E76 RID: 15990
	public List<TMP_FontAsset> allDestinationFontAssets;
}
