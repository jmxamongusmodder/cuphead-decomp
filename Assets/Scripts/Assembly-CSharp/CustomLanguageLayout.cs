using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class CustomLanguageLayout : MonoBehaviour
{
	// Token: 0x06003665 RID: 13925 RVA: 0x001F817C File Offset: 0x001F657C
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.textContainer = base.GetComponent<TextContainer>();
		this.englishBasicLayout = default(CustomLanguageLayout.LanguageLayout);
		this.englishBasicLayout.positionOffset = this.rectTransform.localPosition;
		this.englishBasicLayout.customWidth = this.rectTransform.sizeDelta.x;
		this.englishBasicLayout.customHeight = this.rectTransform.sizeDelta.y;
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x001F8202 File Offset: 0x001F6602
	private void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ReviewLayout;
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x001F8215 File Offset: 0x001F6615
	private void OnEnable()
	{
		Localization.OnLanguageChangedEvent += this.ReviewLayout;
		this.ReviewLayout();
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x001F822E File Offset: 0x001F662E
	private void OnDisable()
	{
		this.ResetToEnglish();
		Localization.OnLanguageChangedEvent -= this.ReviewLayout;
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x001F8248 File Offset: 0x001F6648
	private void ReviewLayout()
	{
		if (this.rectTransform == null)
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
			CustomLanguageLayout.LanguageLayout languageLayout = this.customLayouts[num];
			this.ApplylayoutChanges(languageLayout);
		}
		else
		{
			this.ResetToEnglish();
		}
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x001F82D0 File Offset: 0x001F66D0
	private void ResetToEnglish()
	{
		this.rectTransform.localPosition = this.englishBasicLayout.positionOffset;
		if (this.textContainer != null)
		{
			this.textContainer.height = this.englishBasicLayout.customHeight;
			this.textContainer.width = this.englishBasicLayout.customWidth;
		}
		else
		{
			this.rectTransform.sizeDelta = new Vector2(this.englishBasicLayout.customWidth, this.englishBasicLayout.customHeight);
		}
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x001F835C File Offset: 0x001F675C
	private void ApplylayoutChanges(CustomLanguageLayout.LanguageLayout languageLayout)
	{
		if (languageLayout.needCustomOffset)
		{
			this.rectTransform.localPosition = new Vector3(this.englishBasicLayout.positionOffset.x + languageLayout.positionOffset.x, this.englishBasicLayout.positionOffset.y + languageLayout.positionOffset.y, this.englishBasicLayout.positionOffset.z + languageLayout.positionOffset.z);
		}
		else
		{
			this.rectTransform.localPosition = new Vector3(this.englishBasicLayout.positionOffset.x, this.englishBasicLayout.positionOffset.y, this.englishBasicLayout.positionOffset.z);
		}
		if (this.textContainer != null)
		{
			this.textContainer.width = ((!languageLayout.needCustomWidth) ? this.englishBasicLayout.customWidth : languageLayout.customWidth);
			this.textContainer.height = ((!languageLayout.needCustomHeight) ? this.englishBasicLayout.customHeight : languageLayout.customHeight);
		}
		else
		{
			float x = (!languageLayout.needCustomWidth) ? this.englishBasicLayout.customWidth : languageLayout.customWidth;
			float y = (!languageLayout.needCustomHeight) ? this.englishBasicLayout.customHeight : languageLayout.customHeight;
			this.rectTransform.sizeDelta = new Vector2(x, y);
		}
	}

	// Token: 0x04003E5D RID: 15965
	[SerializeField]
	public List<CustomLanguageLayout.LanguageLayout> customLayouts;

	// Token: 0x04003E5E RID: 15966
	private RectTransform rectTransform;

	// Token: 0x04003E5F RID: 15967
	private CustomLanguageLayout.LanguageLayout englishBasicLayout;

	// Token: 0x04003E60 RID: 15968
	private TextContainer textContainer;

	// Token: 0x02000913 RID: 2323
	[Serializable]
	public struct LanguageLayout
	{
		// Token: 0x04003E61 RID: 15969
		public Localization.Languages languageApplied;

		// Token: 0x04003E62 RID: 15970
		public bool needCustomOffset;

		// Token: 0x04003E63 RID: 15971
		public Vector3 positionOffset;

		// Token: 0x04003E64 RID: 15972
		public bool needCustomWidth;

		// Token: 0x04003E65 RID: 15973
		public float customWidth;

		// Token: 0x04003E66 RID: 15974
		public bool needCustomHeight;

		// Token: 0x04003E67 RID: 15975
		public float customHeight;
	}
}
