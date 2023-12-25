using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x02000918 RID: 2328
[CreateAssetMenu(fileName = "LocalizationAsset", menuName = "Localization Asset", order = 1)]
public class Localization : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06003679 RID: 13945 RVA: 0x001F8817 File Offset: 0x001F6C17
	public static Localization Instance
	{
		get
		{
			if (Localization._instance == null)
			{
				Localization._instance = Resources.Load<Localization>("LocalizationAsset");
			}
			return Localization._instance;
		}
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x0600367A RID: 13946 RVA: 0x001F8840 File Offset: 0x001F6C40
	// (remove) Token: 0x0600367B RID: 13947 RVA: 0x001F8874 File Offset: 0x001F6C74
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Localization.LanguageChanged OnLanguageChangedEvent;

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x0600367C RID: 13948 RVA: 0x001F88A8 File Offset: 0x001F6CA8
	// (set) Token: 0x0600367D RID: 13949 RVA: 0x001F88D3 File Offset: 0x001F6CD3
	public static Localization.Languages language
	{
		get
		{
			if (SettingsData.Data.language == -1)
			{
				SettingsData.Data.language = (int)DetectLanguage.GetDefaultLanguage();
			}
			return (Localization.Languages)SettingsData.Data.language;
		}
		set
		{
			SettingsData.Data.language = (int)value;
			if (Localization.OnLanguageChangedEvent != null)
			{
				Localization.OnLanguageChangedEvent();
			}
		}
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x001F88F4 File Offset: 0x001F6CF4
	public static Localization.Translation Translate(string key)
	{
		int id;
		if (Parser.IntTryParse(key, out id))
		{
			return Localization.Translate(id);
		}
		Localization.Translation result = default(Localization.Translation);
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].key == key)
			{
				TranslationElement translationElement = Localization._instance.m_TranslationElements[i];
				result = translationElement.translation;
			}
		}
		return result;
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x001F8978 File Offset: 0x001F6D78
	public static Localization.Translation Translate(int id)
	{
		Localization.Translation result = default(Localization.Translation);
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].id == id)
			{
				TranslationElement translationElement = Localization._instance.m_TranslationElements[i];
				result = translationElement.translation;
			}
		}
		return result;
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x001F89E4 File Offset: 0x001F6DE4
	public static TranslationElement Find(string key)
	{
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].key == key)
			{
				return Localization._instance.m_TranslationElements[i];
			}
		}
		return null;
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x001F8A44 File Offset: 0x001F6E44
	public static TranslationElement Find(int id)
	{
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].id == id)
			{
				return Localization._instance.m_TranslationElements[i];
			}
		}
		return null;
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x001F8AA0 File Offset: 0x001F6EA0
	public static void ExportCsv(string path)
	{
		string text = "|lang|";
		string text2 = "|lang|_cuphead";
		string text3 = "|lang|_mugman";
		char value = '@';
		string value2 = "\r\n";
		StringBuilder stringBuilder = new StringBuilder();
		int num = Enum.GetNames(typeof(Localization.Languages)).Length;
		int num2 = Enum.GetNames(typeof(Localization.Categories)).Length;
		for (int i = 0; i < Localization.csvKeys.Length; i++)
		{
			if (Localization.csvKeys[i].Contains(text))
			{
				string value3 = Localization.csvKeys[i].Replace(text, string.Empty);
				for (int j = 0; j < num; j++)
				{
					if (i > 0)
					{
						stringBuilder.Append(value);
					}
					StringBuilder stringBuilder2 = stringBuilder;
					Localization.Languages languages = (Localization.Languages)j;
					stringBuilder2.Append(languages.ToString());
					stringBuilder.Append(value3);
				}
			}
			else
			{
				if (i > 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Append(Localization.csvKeys[i]);
			}
		}
		stringBuilder.Append(value2);
		string text4 = string.Empty;
		for (int k = 0; k < Localization.Instance.m_TranslationElements.Count; k++)
		{
			TranslationElement translationElement = Localization._instance.m_TranslationElements[k];
			if (translationElement.depth != -1)
			{
				for (int l = 0; l < Localization.csvKeys.Length; l++)
				{
					if (Localization.csvKeys[l].Contains(text))
					{
						for (int m = 0; m < num; m++)
						{
							if (l > 0)
							{
								stringBuilder.Append(value);
							}
							text4 = string.Empty;
							string a;
							Localization.Translation translation;
							if (Localization.csvKeys[l].Contains(text2))
							{
								a = Localization.csvKeys[l].Replace(text2, string.Empty);
								if (translationElement.translationsCuphead == null || translationElement.translationsCuphead.Length == 0)
								{
									translation = default(Localization.Translation);
								}
								else
								{
									translation = translationElement.translationsCuphead[m];
								}
							}
							else if (Localization.csvKeys[l].Contains(text3))
							{
								a = Localization.csvKeys[l].Replace(text3, string.Empty);
								if (translationElement.translationsMugman == null || translationElement.translationsMugman.Length == 0)
								{
									translation = default(Localization.Translation);
								}
								else
								{
									translation = translationElement.translationsMugman[m];
								}
							}
							else
							{
								a = Localization.csvKeys[l].Replace(text, string.Empty);
								translation = translationElement.translations[m];
							}
							if (a == "_text")
							{
								text4 = translation.text;
								if (!string.IsNullOrEmpty(text4))
								{
									text4 = text4.Replace('\n'.ToString(), '\\' + "n");
								}
							}
							else if (a == "_image")
							{
								if (translation.image != null)
								{
									text4 = translation.image.name;
								}
							}
							else if (a == "_spriteAtlasName")
							{
								text4 = translation.spriteAtlasName;
							}
							else if (a == "_spriteAtlasImageName")
							{
								text4 = translation.spriteAtlasImageName;
							}
							else if (a == "_font")
							{
								if (translation.fonts.fontType != FontLoader.FontType.None)
								{
									text4 = FontLoader.GetFilename(translation.fonts.fontType);
								}
							}
							else if (a == "_fontSize")
							{
								if (translation.fonts.fontSize > 0)
								{
									text4 = translation.fonts.fontSize.ToString();
								}
								else
								{
									text4 = string.Empty;
								}
							}
							else if (a == "_fontAsset")
							{
								if (translation.fonts.tmpFontType != FontLoader.TMPFontType.None)
								{
									text4 = FontLoader.GetFilename(translation.fonts.tmpFontType);
								}
							}
							else if (a == "_fontAssetSize")
							{
								if (translation.fonts.fontAssetSize > 0f)
								{
									text4 = translation.fonts.fontAssetSize.ToString();
								}
								else
								{
									text4 = string.Empty;
								}
							}
							if (text4 != null)
							{
								stringBuilder.Append(text4);
							}
						}
					}
					else
					{
						if (l > 0)
						{
							stringBuilder.Append(value);
						}
						text4 = string.Empty;
						string a = Localization.csvKeys[l];
						if (a == "id")
						{
							text4 = translationElement.id.ToString();
						}
						else if (a == "key")
						{
							text4 = translationElement.key;
						}
						else if (a == "category")
						{
							text4 = translationElement.category.ToString();
						}
						else if (a == "description")
						{
							text4 = translationElement.description;
						}
						if (text4 != null)
						{
							stringBuilder.Append(text4);
						}
					}
				}
				stringBuilder.Append(value2);
			}
		}
		Encoding encoding = new UTF8Encoding(true);
		byte[] bytes = encoding.GetBytes(stringBuilder.ToString());
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] preamble = encoding.GetPreamble();
		fileStream.Write(preamble, 0, preamble.Length);
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Dispose();
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x001F9068 File Offset: 0x001F7468
	public static void ImportCsv(string path)
	{
		char c = '@';
		string text = "\r\n";
		Encoding encoding = new UTF8Encoding(true);
		FileStream fileStream = new FileStream(path, FileMode.Open);
		byte[] preamble = encoding.GetPreamble();
		byte[] array = new byte[preamble.Length];
		fileStream.Read(array, 0, preamble.Length);
		bool flag = true;
		for (int i = 0; i < preamble.Length; i++)
		{
			if (preamble[i] != array[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			array = new byte[fileStream.Length - (long)preamble.Length];
			fileStream.Read(array, 0, array.Length);
		}
		else
		{
			array = new byte[fileStream.Length];
			fileStream.Position = 0L;
			fileStream.Read(array, 0, (int)fileStream.Length);
		}
		fileStream.Dispose();
		string @string = encoding.GetString(array);
		string[] array2 = @string.Split(new string[]
		{
			text
		}, StringSplitOptions.RemoveEmptyEntries);
		string[] headers = array2[0].Split(new char[]
		{
			c
		});
		Localization.processImportedLines(headers, array2, c);
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x001F917C File Offset: 0x001F757C
	private static void processImportedLines(string[] headers, string[] lines, char separator)
	{
		string text = "_cuphead";
		string text2 = "_mugman";
		string[] names = Enum.GetNames(typeof(Localization.Languages));
		string[] names2 = Enum.GetNames(typeof(Localization.Categories));
		Dictionary<string, Font> dictionary = new Dictionary<string, Font>();
		Dictionary<string, TMP_FontAsset> dictionary2 = new Dictionary<string, TMP_FontAsset>();
		Localization.Instance.m_TranslationElements.Clear();
		TranslationElement translationElement = new TranslationElement("Root", -1, 0);
		Localization._instance.m_TranslationElements.Add(translationElement);
		for (int i = 1; i < lines.Length; i++)
		{
			string[] array = lines[i].Split(new char[]
			{
				separator
			});
			if (array.Length != headers.Length)
			{
				if (lines[i] != string.Empty)
				{
				}
			}
			else
			{
				translationElement = Localization.Instance.AddKey();
				for (int j = 0; j < array.Length; j++)
				{
					if (!string.IsNullOrEmpty(array[j]))
					{
						string text3 = headers[j];
						if (text3 == "id")
						{
							translationElement.id = Parser.IntParse(array[j]);
						}
						else if (text3 == "key")
						{
							translationElement.key = array[j];
						}
						else if (text3 == "category")
						{
							int category = -1;
							for (int k = 0; k < names2.Length; k++)
							{
								if (names2[k] == array[j])
								{
									category = k;
								}
							}
							translationElement.category = (Localization.Categories)category;
						}
						else if (text3 == "description")
						{
							translationElement.description = array[j];
						}
						else
						{
							for (int l = 0; l < names.Length; l++)
							{
								if (text3.Contains(names[l]))
								{
									text3 = text3.Replace(names[l], string.Empty);
									bool flag = false;
									bool flag2 = false;
									Localization.Translation translation;
									if (text3.Contains(text))
									{
										flag = true;
										text3 = text3.Replace(text, string.Empty);
										if (translationElement.translationsCuphead == null || translationElement.translationsCuphead.Length == 0)
										{
											translationElement.translationsCuphead = new Localization.Translation[names.Length];
											translationElement.translationsMugman = new Localization.Translation[names.Length];
										}
										translation = translationElement.translationsCuphead[l];
									}
									else if (text3.Contains(text2))
									{
										flag2 = true;
										text3 = text3.Replace(text2, string.Empty);
										if (translationElement.translationsCuphead == null || translationElement.translationsCuphead.Length == 0)
										{
											translationElement.translationsCuphead = new Localization.Translation[names.Length];
											translationElement.translationsMugman = new Localization.Translation[names.Length];
										}
										translation = translationElement.translationsMugman[l];
									}
									else
									{
										translation = translationElement.translations[l];
									}
									if (translation.fonts == null)
									{
										translation.fonts = new Localization.CategoryLanguageFont();
									}
									if (text3 == "_text")
									{
										translation.text = array[j];
									}
									else if (text3 == "_image")
									{
										if (string.IsNullOrEmpty(array[j]))
										{
											break;
										}
									}
									else if (text3 == "_spriteAtlasName")
									{
										translation.spriteAtlasName = array[j];
									}
									else if (text3 == "_spriteAtlasImageName")
									{
										translation.spriteAtlasImageName = array[j];
									}
									else if (text3 == "_font")
									{
										if (string.IsNullOrEmpty(array[j]))
										{
											break;
										}
									}
									else if (text3 == "_fontSize")
									{
										if (!string.IsNullOrEmpty(array[j]))
										{
											int num = Convert.ToInt32(array[j]);
											if (num == 0)
											{
												break;
											}
											translation.fonts.fontSize = num;
										}
									}
									else if (text3 == "_fontAsset")
									{
										if (string.IsNullOrEmpty(array[j]))
										{
											break;
										}
									}
									else if (text3 == "_fontAssetSize" && !string.IsNullOrEmpty(array[j]))
									{
										float num2 = Convert.ToSingle(array[j]);
										if (num2 == 0f)
										{
											break;
										}
										translation.fonts.fontAssetSize = num2;
									}
									if (flag)
									{
										translationElement.translationsCuphead[l] = translation;
									}
									else if (flag2)
									{
										translationElement.translationsMugman[l] = translation;
									}
									else
									{
										translationElement.translations[l] = translation;
									}
									break;
								}
							}
						}
					}
				}
			}
		}
		if (Localization.OnLanguageChangedEvent != null)
		{
			Localization.OnLanguageChangedEvent();
		}
	}

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06003685 RID: 13957 RVA: 0x001F9678 File Offset: 0x001F7A78
	// (set) Token: 0x06003686 RID: 13958 RVA: 0x001F96EF File Offset: 0x001F7AEF
	[SerializeField]
	public Localization.CategoryLanguageFonts[] fonts
	{
		get
		{
			if (this.m_Fonts == null)
			{
				int num = Enum.GetNames(typeof(Localization.Languages)).Length;
				int num2 = Enum.GetNames(typeof(Localization.Categories)).Length;
				this.m_Fonts = new Localization.CategoryLanguageFonts[num];
				for (int i = 0; i < num; i++)
				{
					this.m_Fonts[i].fonts = new Localization.CategoryLanguageFont[num2];
				}
			}
			return this.m_Fonts;
		}
		set
		{
			this.m_Fonts = value;
		}
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06003687 RID: 13959 RVA: 0x001F96F8 File Offset: 0x001F7AF8
	// (set) Token: 0x06003688 RID: 13960 RVA: 0x001F9700 File Offset: 0x001F7B00
	[SerializeField]
	public List<TranslationElement> translationElements
	{
		get
		{
			return this.m_TranslationElements;
		}
		set
		{
			this.m_TranslationElements = value;
		}
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x001F970C File Offset: 0x001F7B0C
	public TranslationElement AddKey()
	{
		int num = -1;
		for (int i = 0; i < this.m_TranslationElements.Count; i++)
		{
			if (this.m_TranslationElements[i].id > num)
			{
				num = this.m_TranslationElements[i].id;
			}
		}
		num++;
		TranslationElement translationElement = new TranslationElement("Key" + num, Localization.Categories.NoCategory, string.Empty, string.Empty, string.Empty, 0, num);
		this.m_TranslationElements.Add(translationElement);
		return translationElement;
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x001F979C File Offset: 0x001F7B9C
	private void Awake()
	{
		if (this.m_TranslationElements.Count == 0)
		{
			this.m_TranslationElements = new List<TranslationElement>(1);
			TranslationElement item = new TranslationElement("Root", -1, 0);
			this.m_TranslationElements.Add(item);
		}
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x001F97DE File Offset: 0x001F7BDE
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x001F97E0 File Offset: 0x001F7BE0
	public void OnAfterDeserialize()
	{
		bool flag = false;
		int num = Enum.GetNames(typeof(Localization.Languages)).Length;
		if (this.fonts.Length < num)
		{
			flag = true;
		}
		int num2 = Enum.GetNames(typeof(Localization.Categories)).Length;
		if (this.fonts[0].fonts.Length < num2)
		{
			flag = true;
		}
		if (flag)
		{
			this.fonts = this.GrowFonts(this.fonts, num, num2);
		}
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x001F9858 File Offset: 0x001F7C58
	private Localization.CategoryLanguageFonts[] GrowFonts(Localization.CategoryLanguageFonts[] oldFonts, int newLanguagesLength, int newCategoriesLength)
	{
		Localization.CategoryLanguageFonts[] array = new Localization.CategoryLanguageFonts[newLanguagesLength];
		for (int i = 0; i < newLanguagesLength; i++)
		{
			array[i].fonts = new Localization.CategoryLanguageFont[newCategoriesLength];
		}
		for (int j = 0; j < oldFonts.Length; j++)
		{
			for (int k = 0; k < oldFonts[j].fonts.Length; k++)
			{
				array[j][k] = oldFonts[j][k];
			}
		}
		return array;
	}

	// Token: 0x04003E77 RID: 15991
	public const int LanguagesEnumSize = 12;

	// Token: 0x04003E78 RID: 15992
	public const string PATH = "LocalizationAsset";

	// Token: 0x04003E79 RID: 15993
	private static string[] csvKeys = new string[]
	{
		"id",
		"key",
		"category",
		"description",
		"|lang|_text",
		"|lang|_cuphead_text",
		"|lang|_mugman_text",
		"|lang|_image",
		"|lang|_spriteAtlasName",
		"|lang|_spriteAtlasImageName",
		"|lang|_cuphead_image",
		"|lang|_mugman_image",
		"|lang|_font",
		"|lang|_fontSize",
		"|lang|_fontAsset",
		"|lang|_fontAssetSize"
	};

	// Token: 0x04003E7A RID: 15994
	private static Localization _instance;

	// Token: 0x04003E7C RID: 15996
	public static Localization.Languages language1 = Localization.Languages.English;

	// Token: 0x04003E7D RID: 15997
	public static Localization.Languages language2 = Localization.Languages.French;

	// Token: 0x04003E7E RID: 15998
	[SerializeField]
	private List<TranslationElement> m_TranslationElements = new List<TranslationElement>();

	// Token: 0x04003E7F RID: 15999
	[SerializeField]
	public Localization.CategoryLanguageFonts[] m_Fonts;

	// Token: 0x02000919 RID: 2329
	[SerializeField]
	public enum Languages
	{
		// Token: 0x04003E81 RID: 16001
		English,
		// Token: 0x04003E82 RID: 16002
		French,
		// Token: 0x04003E83 RID: 16003
		Italian,
		// Token: 0x04003E84 RID: 16004
		German,
		// Token: 0x04003E85 RID: 16005
		SpanishSpain,
		// Token: 0x04003E86 RID: 16006
		SpanishAmerica,
		// Token: 0x04003E87 RID: 16007
		Korean,
		// Token: 0x04003E88 RID: 16008
		Russian,
		// Token: 0x04003E89 RID: 16009
		Polish,
		// Token: 0x04003E8A RID: 16010
		PortugueseBrazil,
		// Token: 0x04003E8B RID: 16011
		Japanese,
		// Token: 0x04003E8C RID: 16012
		SimplifiedChinese
	}

	// Token: 0x0200091A RID: 2330
	[SerializeField]
	public enum Categories
	{
		// Token: 0x04003E8E RID: 16014
		NoCategory,
		// Token: 0x04003E8F RID: 16015
		LevelSelectionName,
		// Token: 0x04003E90 RID: 16016
		LevelSelectionIn,
		// Token: 0x04003E91 RID: 16017
		LevelSelectionStage,
		// Token: 0x04003E92 RID: 16018
		LevelSelectionDifficultyHeader,
		// Token: 0x04003E93 RID: 16019
		LevelSelectionDifficultys,
		// Token: 0x04003E94 RID: 16020
		EquipCategoryNames,
		// Token: 0x04003E95 RID: 16021
		EquipWeaponNames,
		// Token: 0x04003E96 RID: 16022
		EquipCategoryBackName,
		// Token: 0x04003E97 RID: 16023
		EquipCategoryBackTitle,
		// Token: 0x04003E98 RID: 16024
		EquipCategoryBackSubtitle,
		// Token: 0x04003E99 RID: 16025
		EquipCategoryBackDescription,
		// Token: 0x04003E9A RID: 16026
		ChecklistTitle,
		// Token: 0x04003E9B RID: 16027
		ChecklistWorldNames,
		// Token: 0x04003E9C RID: 16028
		ChecklistContractHeaders,
		// Token: 0x04003E9D RID: 16029
		ChecklistContracts,
		// Token: 0x04003E9E RID: 16030
		PauseMenuItems,
		// Token: 0x04003E9F RID: 16031
		DeathMenuQuote,
		// Token: 0x04003EA0 RID: 16032
		DeathMenuItems,
		// Token: 0x04003EA1 RID: 16033
		ResultsMenuTitle,
		// Token: 0x04003EA2 RID: 16034
		ResultsMenuCategories,
		// Token: 0x04003EA3 RID: 16035
		ResultsMenuGrade,
		// Token: 0x04003EA4 RID: 16036
		ResultsMenuNewRecord,
		// Token: 0x04003EA5 RID: 16037
		ResultsMenuTryNormal,
		// Token: 0x04003EA6 RID: 16038
		IntroEndingText,
		// Token: 0x04003EA7 RID: 16039
		IntroEndingAction,
		// Token: 0x04003EA8 RID: 16040
		CutScenesText,
		// Token: 0x04003EA9 RID: 16041
		SpeechBalloons,
		// Token: 0x04003EAA RID: 16042
		WorldMapTitles,
		// Token: 0x04003EAB RID: 16043
		Glyphs,
		// Token: 0x04003EAC RID: 16044
		TitleScreenSelection,
		// Token: 0x04003EAD RID: 16045
		Notifications,
		// Token: 0x04003EAE RID: 16046
		Tutorials,
		// Token: 0x04003EAF RID: 16047
		OptionMenu,
		// Token: 0x04003EB0 RID: 16048
		RemappingMenu,
		// Token: 0x04003EB1 RID: 16049
		RemappingButton,
		// Token: 0x04003EB2 RID: 16050
		XboxNotification,
		// Token: 0x04003EB3 RID: 16051
		AttractScreen,
		// Token: 0x04003EB4 RID: 16052
		JoinPrompt,
		// Token: 0x04003EB5 RID: 16053
		ConfirmMenu,
		// Token: 0x04003EB6 RID: 16054
		DifficultyMenu,
		// Token: 0x04003EB7 RID: 16055
		ShopElement,
		// Token: 0x04003EB8 RID: 16056
		StageTitles,
		// Token: 0x04003EB9 RID: 16057
		NintendoSwitchNotification,
		// Token: 0x04003EBA RID: 16058
		Achievements
	}

	// Token: 0x0200091B RID: 2331
	[Serializable]
	public struct Translation
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x001F998C File Offset: 0x001F7D8C
		public bool hasSpriteAtlasImage
		{
			get
			{
				return this.spriteAtlasName != null && this.spriteAtlasName.Length > 0 && this.spriteAtlasImageName != null && this.spriteAtlasImageName.Length > 0;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x001F99C6 File Offset: 0x001F7DC6
		public bool hasCustomFont
		{
			get
			{
				return this.fonts.fontType != FontLoader.FontType.None;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x001F99D9 File Offset: 0x001F7DD9
		public bool hasCustomFontAsset
		{
			get
			{
				return this.fonts.tmpFontType != FontLoader.TMPFontType.None;
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x001F99EC File Offset: 0x001F7DEC
		public string SanitizedText()
		{
			return this.text.Replace("\\n", "\n");
		}

		// Token: 0x04003EBB RID: 16059
		[SerializeField]
		public bool hasImage;

		// Token: 0x04003EBC RID: 16060
		[SerializeField]
		public string text;

		// Token: 0x04003EBD RID: 16061
		[SerializeField]
		public Localization.CategoryLanguageFont fonts;

		// Token: 0x04003EBE RID: 16062
		[SerializeField]
		public Sprite image;

		// Token: 0x04003EBF RID: 16063
		[SerializeField]
		public string spriteAtlasName;

		// Token: 0x04003EC0 RID: 16064
		[SerializeField]
		public string spriteAtlasImageName;
	}

	// Token: 0x0200091C RID: 2332
	[Serializable]
	public class CategoryLanguageFont
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x001F9A0B File Offset: 0x001F7E0B
		public Font font
		{
			get
			{
				return FontLoader.GetFont(this.fontType);
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06003695 RID: 13973 RVA: 0x001F9A18 File Offset: 0x001F7E18
		public TMP_FontAsset fontAsset
		{
			get
			{
				return FontLoader.GetTMPFont(this.tmpFontType);
			}
		}

		// Token: 0x04003EC1 RID: 16065
		public int fontSize;

		// Token: 0x04003EC2 RID: 16066
		public FontLoader.FontType fontType;

		// Token: 0x04003EC3 RID: 16067
		public float fontAssetSize;

		// Token: 0x04003EC4 RID: 16068
		public FontLoader.TMPFontType tmpFontType;

		// Token: 0x04003EC5 RID: 16069
		public float charSpacing;
	}

	// Token: 0x0200091D RID: 2333
	[Serializable]
	public struct CategoryLanguageFonts
	{
		// Token: 0x17000470 RID: 1136
		public Localization.CategoryLanguageFont this[int index]
		{
			get
			{
				return this.fonts[index];
			}
			set
			{
				this.fonts[index] = value;
			}
		}

		// Token: 0x04003EC6 RID: 16070
		[SerializeField]
		public Localization.CategoryLanguageFont[] fonts;
	}

	// Token: 0x0200091E RID: 2334
	// (Invoke) Token: 0x06003699 RID: 13977
	public delegate void LanguageChanged();
}
