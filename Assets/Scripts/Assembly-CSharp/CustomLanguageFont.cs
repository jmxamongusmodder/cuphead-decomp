using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000910 RID: 2320
public class CustomLanguageFont : MonoBehaviour
{
	// Token: 0x0600365D RID: 13917 RVA: 0x001F7EC4 File Offset: 0x001F62C4
	private void Awake()
	{
		this.textMeshProComponent = base.GetComponent<TMP_Text>();
		this.textComponent = base.GetComponent<Text>();
		this.englishBasicFont = default(CustomLanguageFont.LanguageFont);
		this.englishBasicFont.characterSpacing = this.textMeshProComponent.characterSpacing;
		this.englishBasicFont.lineSpacing = this.textMeshProComponent.lineSpacing;
		this.englishBasicFont.paragraphSpacing = this.textMeshProComponent.paragraphSpacing;
		this.englishBasicFont.needFontSize = true;
		this.englishBasicFont.customFontSize = this.textMeshProComponent.fontSize;
		this.englishBasicFont.needKerning = this.textMeshProComponent.enableKerning;
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x001F7F72 File Offset: 0x001F6372
	private void Start()
	{
		Localization.OnLanguageChangedEvent += this.ReviewFont;
		this.ReviewFont();
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x001F7F8B File Offset: 0x001F638B
	private void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ReviewFont;
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x001F7F9E File Offset: 0x001F639E
	private void OnEnable()
	{
		this.ReviewFont();
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x001F7FA8 File Offset: 0x001F63A8
	private void ReviewFont()
	{
		if (this.textMeshProComponent == null && this.textComponent == null)
		{
			return;
		}
		int num = 0;
		bool flag = false;
		while (!flag && num < this.customLayouts.Count)
		{
			flag = (this.customLayouts[num].languageApplied == Localization.language);
			num++;
		}
		num--;
		if (flag)
		{
			if (this.customLayouts[num].needSpacing)
			{
				this.ApplySpacingChanges(this.customLayouts[num]);
			}
			if (this.customLayouts[num].needFontSize)
			{
				this.ApplyFontSizeChanges(this.customLayouts[num]);
			}
			this.textMeshProComponent.enableKerning = this.customLayouts[num].needKerning;
		}
		else
		{
			this.ApplySpacingChanges(this.englishBasicFont);
			this.ApplyFontSizeChanges(this.englishBasicFont);
			this.textMeshProComponent.enableKerning = this.englishBasicFont.needKerning;
		}
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x001F80CC File Offset: 0x001F64CC
	private void ApplySpacingChanges(CustomLanguageFont.LanguageFont languageLayout)
	{
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.characterSpacing = languageLayout.characterSpacing;
			this.textMeshProComponent.lineSpacing = languageLayout.lineSpacing;
			this.textMeshProComponent.paragraphSpacing = languageLayout.paragraphSpacing;
		}
		else
		{
			this.textComponent.lineSpacing = languageLayout.lineSpacing;
		}
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x001F8137 File Offset: 0x001F6537
	private void ApplyFontSizeChanges(CustomLanguageFont.LanguageFont languageLayout)
	{
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.fontSize = languageLayout.customFontSize;
		}
		else
		{
			this.textComponent.fontSize = (int)languageLayout.customFontSize;
		}
	}

	// Token: 0x04003E51 RID: 15953
	[SerializeField]
	public List<CustomLanguageFont.LanguageFont> customLayouts;

	// Token: 0x04003E52 RID: 15954
	private CustomLanguageFont.LanguageFont englishBasicFont;

	// Token: 0x04003E53 RID: 15955
	private TMP_Text textMeshProComponent;

	// Token: 0x04003E54 RID: 15956
	private Text textComponent;

	// Token: 0x02000911 RID: 2321
	[Serializable]
	public struct LanguageFont
	{
		// Token: 0x04003E55 RID: 15957
		public Localization.Languages languageApplied;

		// Token: 0x04003E56 RID: 15958
		public bool needSpacing;

		// Token: 0x04003E57 RID: 15959
		public float characterSpacing;

		// Token: 0x04003E58 RID: 15960
		public float lineSpacing;

		// Token: 0x04003E59 RID: 15961
		public float paragraphSpacing;

		// Token: 0x04003E5A RID: 15962
		public bool needFontSize;

		// Token: 0x04003E5B RID: 15963
		public float customFontSize;

		// Token: 0x04003E5C RID: 15964
		public bool needKerning;
	}
}
