using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomLanguageLayoutGroup : MonoBehaviour
{
	[Serializable]
	public struct LanguageLayoutGroup
	{
		public Localization.Languages languageApplied;
		public bool needPadding;
		public RectOffset padding;
		public bool needSpacing;
		public float spacing;
	}

	[SerializeField]
	private HorizontalOrVerticalLayoutGroup layoutComponent;
	[SerializeField]
	public List<CustomLanguageLayoutGroup.LanguageLayoutGroup> customLayouts;
}
