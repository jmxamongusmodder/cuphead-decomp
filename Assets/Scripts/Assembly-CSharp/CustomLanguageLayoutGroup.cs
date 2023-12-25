using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000914 RID: 2324
public class CustomLanguageLayoutGroup : MonoBehaviour
{
	// Token: 0x0600366D RID: 13933 RVA: 0x001F84F8 File Offset: 0x001F68F8
	private void Awake()
	{
		this.englishBasicLayout = default(CustomLanguageLayoutGroup.LanguageLayoutGroup);
		this.englishBasicLayout.needPadding = true;
		this.englishBasicLayout.padding = this.layoutComponent.padding;
		this.englishBasicLayout.needSpacing = true;
		this.englishBasicLayout.spacing = this.layoutComponent.spacing;
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x001F8558 File Offset: 0x001F6958
	private void Start()
	{
		Localization.OnLanguageChangedEvent += this.ReviewLayout;
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x001F856B File Offset: 0x001F696B
	private void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ReviewLayout;
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x001F857E File Offset: 0x001F697E
	private void OnEnable()
	{
		this.ReviewLayout();
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x001F8588 File Offset: 0x001F6988
	private void ReviewLayout()
	{
		if (this.layoutComponent == null)
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
			if (this.customLayouts[num].needPadding)
			{
				this.ApplyPaddingChanges(this.customLayouts[num]);
			}
		}
		else
		{
			this.ApplySpacingChanges(this.englishBasicLayout);
			this.ApplyPaddingChanges(this.englishBasicLayout);
		}
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x001F8665 File Offset: 0x001F6A65
	private void ApplySpacingChanges(CustomLanguageLayoutGroup.LanguageLayoutGroup languageLayout)
	{
		this.layoutComponent.spacing = languageLayout.spacing;
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x001F8679 File Offset: 0x001F6A79
	private void ApplyPaddingChanges(CustomLanguageLayoutGroup.LanguageLayoutGroup languageLayout)
	{
		this.layoutComponent.padding = languageLayout.padding;
	}

	// Token: 0x04003E68 RID: 15976
	[SerializeField]
	private HorizontalOrVerticalLayoutGroup layoutComponent;

	// Token: 0x04003E69 RID: 15977
	[SerializeField]
	public List<CustomLanguageLayoutGroup.LanguageLayoutGroup> customLayouts;

	// Token: 0x04003E6A RID: 15978
	private CustomLanguageLayoutGroup.LanguageLayoutGroup englishBasicLayout;

	// Token: 0x02000915 RID: 2325
	[Serializable]
	public struct LanguageLayoutGroup
	{
		// Token: 0x04003E6B RID: 15979
		public Localization.Languages languageApplied;

		// Token: 0x04003E6C RID: 15980
		public bool needPadding;

		// Token: 0x04003E6D RID: 15981
		public RectOffset padding;

		// Token: 0x04003E6E RID: 15982
		public bool needSpacing;

		// Token: 0x04003E6F RID: 15983
		public float spacing;
	}
}
