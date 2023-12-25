using System;
using System.Globalization;

// Token: 0x02000916 RID: 2326
public static class DetectLanguage
{
	// Token: 0x06003674 RID: 13940 RVA: 0x001F8690 File Offset: 0x001F6A90
	public static Localization.Languages GetDefaultLanguage()
	{
		Localization.Languages result = Localization.Languages.English;
		DetectLanguage.getDefaultLanguage(ref result);
		return result;
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x001F86A8 File Offset: 0x001F6AA8
	private static void getDefaultLanguage(ref Localization.Languages defaultLanguage)
	{
		CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
		string twoLetterISOLanguageName = currentUICulture.TwoLetterISOLanguageName;
		if (twoLetterISOLanguageName == "fr")
		{
			defaultLanguage = Localization.Languages.French;
		}
		else if (twoLetterISOLanguageName == "de")
		{
			defaultLanguage = Localization.Languages.German;
		}
		else if (twoLetterISOLanguageName == "it")
		{
			defaultLanguage = Localization.Languages.Italian;
		}
		else if (twoLetterISOLanguageName == "ja")
		{
			defaultLanguage = Localization.Languages.Japanese;
		}
		else if (twoLetterISOLanguageName == "zh")
		{
			defaultLanguage = Localization.Languages.SimplifiedChinese;
		}
		else if (twoLetterISOLanguageName == "ru")
		{
			defaultLanguage = Localization.Languages.Russian;
		}
		else if (twoLetterISOLanguageName == "es")
		{
			if (currentUICulture.Name == "es-ES" || currentUICulture.Name == "es")
			{
				defaultLanguage = Localization.Languages.SpanishSpain;
			}
			else
			{
				defaultLanguage = Localization.Languages.SpanishAmerica;
			}
		}
		else if (twoLetterISOLanguageName == "ko")
		{
			defaultLanguage = Localization.Languages.Korean;
		}
		else if (twoLetterISOLanguageName == "po")
		{
			defaultLanguage = Localization.Languages.Polish;
		}
		else if (currentUICulture.Name == "pt-BR")
		{
			defaultLanguage = Localization.Languages.PortugueseBrazil;
		}
		else
		{
			defaultLanguage = Localization.Languages.English;
		}
	}
}
