using UnityEngine;
using System;
using System.Collections.Generic;

public class Localization : ScriptableObject
{
	[Serializable]
	public class CategoryLanguageFont
	{
		public int fontSize;
		public FontLoader.FontType fontType;
		public float fontAssetSize;
		public FontLoader.TMPFontType tmpFontType;
		public float charSpacing;
	}

	[Serializable]
	public struct Translation
	{
		[SerializeField]
		public bool hasImage;
		[SerializeField]
		public string text;
		[SerializeField]
		public Localization.CategoryLanguageFont fonts;
		[SerializeField]
		public Sprite image;
		[SerializeField]
		public string spriteAtlasName;
		[SerializeField]
		public string spriteAtlasImageName;
	}

	[Serializable]
	public struct CategoryLanguageFonts
	{
		[SerializeField]
		public Localization.CategoryLanguageFont[] fonts;
	}

	public enum Categories
	{
		NoCategory = 0,
		LevelSelectionName = 1,
		LevelSelectionIn = 2,
		LevelSelectionStage = 3,
		LevelSelectionDifficultyHeader = 4,
		LevelSelectionDifficultys = 5,
		EquipCategoryNames = 6,
		EquipWeaponNames = 7,
		EquipCategoryBackName = 8,
		EquipCategoryBackTitle = 9,
		EquipCategoryBackSubtitle = 10,
		EquipCategoryBackDescription = 11,
		ChecklistTitle = 12,
		ChecklistWorldNames = 13,
		ChecklistContractHeaders = 14,
		ChecklistContracts = 15,
		PauseMenuItems = 16,
		DeathMenuQuote = 17,
		DeathMenuItems = 18,
		ResultsMenuTitle = 19,
		ResultsMenuCategories = 20,
		ResultsMenuGrade = 21,
		ResultsMenuNewRecord = 22,
		ResultsMenuTryNormal = 23,
		IntroEndingText = 24,
		IntroEndingAction = 25,
		CutScenesText = 26,
		SpeechBalloons = 27,
		WorldMapTitles = 28,
		Glyphs = 29,
		TitleScreenSelection = 30,
		Notifications = 31,
		Tutorials = 32,
		OptionMenu = 33,
		RemappingMenu = 34,
		RemappingButton = 35,
		XboxNotification = 36,
		AttractScreen = 37,
		JoinPrompt = 38,
		ConfirmMenu = 39,
		DifficultyMenu = 40,
		ShopElement = 41,
		StageTitles = 42,
		NintendoSwitchNotification = 43,
		Achievements = 44,
	}

	public enum Languages
	{
		English = 0,
		French = 1,
		Italian = 2,
		German = 3,
		SpanishSpain = 4,
		SpanishAmerica = 5,
		Korean = 6,
		Russian = 7,
		Polish = 8,
		PortugueseBrazil = 9,
		Japanese = 10,
		SimplifiedChinese = 11,
	}

	[SerializeField]
	private List<TranslationElement> m_TranslationElements;
	[SerializeField]
	public CategoryLanguageFonts[] m_Fonts;
}
