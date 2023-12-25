using System;
using UnityEngine.UI;

// Token: 0x02000923 RID: 2339
public class TextAutoLocalize : Text
{
	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x060036AA RID: 13994 RVA: 0x001FA0F3 File Offset: 0x001F84F3
	// (set) Token: 0x060036AB RID: 13995 RVA: 0x001FA0FC File Offset: 0x001F84FC
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			TranslationElement translationElement = Localization.Find(value);
			base.text = ((translationElement == null) ? value : translationElement.translations[(int)Localization.language].text);
		}
	}
}
