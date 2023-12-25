using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class SymbolPicker : MonoBehaviour
{
	// Token: 0x06001153 RID: 4435 RVA: 0x000A465C File Offset: 0x000A2A5C
	private void OnEnable()
	{
		this.ApplySymbol();
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x000A4664 File Offset: 0x000A2A64
	public void ApplySymbol()
	{
		TranslationElement translationElement = Localization.Find(this.button.ToString());
		this.localizationHelper.ApplyTranslation(translationElement, null);
	}

	// Token: 0x04001AC6 RID: 6854
	[SerializeField]
	private LocalizationHelper localizationHelper;

	// Token: 0x04001AC7 RID: 6855
	public CupheadButton button;
}
