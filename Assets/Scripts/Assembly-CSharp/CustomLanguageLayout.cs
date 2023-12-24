using UnityEngine;
using System;
using System.Collections.Generic;

public class CustomLanguageLayout : MonoBehaviour
{
	[Serializable]
	public struct LanguageLayout
	{
		public Localization.Languages languageApplied;
		public bool needCustomOffset;
		public Vector3 positionOffset;
		public bool needCustomWidth;
		public float customWidth;
		public bool needCustomHeight;
		public float customHeight;
	}

	[SerializeField]
	public List<CustomLanguageLayout.LanguageLayout> customLayouts;
}
