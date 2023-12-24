using System;
using UnityEngine.UI;
using UnityEngine;

public class OptionsGUI : AbstractMonoBehaviour
{
	[Serializable]
	public class Button
	{
		public Text text;
		public LocalizationHelper localizationHelper;
		public string[] options;
		public int selection;
		public bool wrap;
	}

	[Serializable]
	public struct LanguageTranslation
	{
		[SerializeField]
		public Localization.Languages language;
		[SerializeField]
		public string translation;
	}

	[SerializeField]
	private GameObject mainObject;
	[SerializeField]
	private GameObject visualObject;
	[SerializeField]
	private GameObject audioObject;
	[SerializeField]
	private GameObject languageObject;
	[SerializeField]
	private Button[] mainObjectButtons;
	[SerializeField]
	private GameObject[] PcOnlyObjects;
	[SerializeField]
	private GameObject[] playStation4HideObjects;
	[SerializeField]
	private GameObject[] dlcHideObjects;
	[SerializeField]
	private GameObject[] FilterUnlockedOnlyObjects;
	[SerializeField]
	private GameObject bigCard;
	[SerializeField]
	private GameObject bigNoise;
	[SerializeField]
	private Button[] visualObjectButtons;
	[SerializeField]
	private Button[] audioObjectButtons;
	[SerializeField]
	private Button[] languageObjectButtons;
	[SerializeField]
	private LanguageTranslation[] languageTranslations;
	[SerializeField]
	private LocalizationHelper[] elementsToTranslate;
	[SerializeField]
	private float[] audioCenterPositions;
	[SerializeField]
	private float[] visualCenterPositions;
	[SerializeField]
	private CustomLanguageLayout[] customPositionning;
}
