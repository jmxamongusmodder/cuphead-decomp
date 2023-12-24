using UnityEngine;
using System;
using System.Collections.Generic;

public class CustomLanguageFont : MonoBehaviour
{
	[Serializable]
	public struct LanguageFont
	{
		public Localization.Languages languageApplied;
		public bool needSpacing;
		public float characterSpacing;
		public float lineSpacing;
		public float paragraphSpacing;
		public bool needFontSize;
		public float customFontSize;
		public bool needKerning;
	}

	[SerializeField]
	public List<CustomLanguageFont.LanguageFont> customLayouts;
}
