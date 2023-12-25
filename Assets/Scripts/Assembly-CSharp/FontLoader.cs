using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020003AC RID: 940
public static class FontLoader
{
	// Token: 0x06000B96 RID: 2966 RVA: 0x00084060 File Offset: 0x00082460
	public static Coroutine[] Initialize()
	{
		Array values = Enum.GetValues(typeof(FontLoader.FontType));
		Array values2 = Enum.GetValues(typeof(FontLoader.TMPFontType));
		Coroutine[] array = new Coroutine[values.Length + values2.Length - 2];
		for (int i = 1; i < values.Length; i++)
		{
			FontLoader.FontType fontType = (FontLoader.FontType)values.GetValue(i);
			string bundleName = fontType.ToString();
			Action<Font> completionHandler = delegate(Font font)
			{
				FontLoader.FontType fontType = fontType;
				FontLoader.FontCache.Add(fontType, font);
			};
			array[i - 1] = AssetBundleLoader.LoadFont(bundleName, FontLoader.GetFilename(fontType), completionHandler);
		}
		for (int j = 1; j < values2.Length; j++)
		{
			FontLoader.TMPFontType fontType = (FontLoader.TMPFontType)values2.GetValue(j);
			string bundleName2 = fontType.ToString();
			Action<UnityEngine.Object[]> completionHandler2 = delegate(UnityEngine.Object[] objects)
			{
				foreach (UnityEngine.Object @object in objects)
				{
					if (@object is TMP_FontAsset)
					{
						FontLoader.TMPFontType fontType = fontType;
						FontLoader.TMPFontCache.Add(fontType, (TMP_FontAsset)@object);
					}
					else
					{
						if (!(@object is Material))
						{
							throw new Exception("Unhandled object type: " + @object.GetType());
						}
						FontLoader.TMPMaterialCache.Add(@object.name, (Material)@object);
					}
				}
			};
			array[j - 1 + (values.Length - 1)] = AssetBundleLoader.LoadTMPFont(bundleName2, completionHandler2);
		}
		return array;
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00084182 File Offset: 0x00082582
	public static Font GetFont(FontLoader.FontType fontType)
	{
		if (fontType == FontLoader.FontType.None)
		{
			return null;
		}
		return FontLoader.FontCache[fontType];
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00084197 File Offset: 0x00082597
	public static TMP_FontAsset GetTMPFont(FontLoader.TMPFontType fontType)
	{
		if (fontType == FontLoader.TMPFontType.None)
		{
			return null;
		}
		return FontLoader.TMPFontCache[fontType];
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x000841AC File Offset: 0x000825AC
	public static Material GetTMPMaterial(string materialName)
	{
		return FontLoader.TMPMaterialCache[materialName];
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x000841BC File Offset: 0x000825BC
	public static string ConvertAssetNameToEnumName(string assetName)
	{
		assetName = assetName.Replace("-", "_");
		assetName = assetName.Replace(" ", "__");
		assetName = assetName.Replace("(", "66");
		assetName = assetName.Replace(")", "99");
		return assetName;
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00084212 File Offset: 0x00082612
	public static string GetFilename(FontLoader.FontType fontType)
	{
		return FontLoader.FontTypeMapping[fontType];
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x0008421F File Offset: 0x0008261F
	public static string GetFilename(FontLoader.TMPFontType fontType)
	{
		return FontLoader.TMPFontTypeMapping[fontType];
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0008422C File Offset: 0x0008262C
	public static FontLoader.FontType ConvertFontToFontType(Font font)
	{
		return FontLoader.ConvertAssetNameToFontType(font.name);
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0008423C File Offset: 0x0008263C
	private static FontLoader.FontType ConvertAssetNameToFontType(string assetName)
	{
		string value = FontLoader.ConvertAssetNameToEnumName(assetName);
		return (FontLoader.FontType)Enum.Parse(typeof(FontLoader.FontType), value);
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00084267 File Offset: 0x00082667
	public static FontLoader.TMPFontType ConvertTMPFontAssetToTMPFontType(TMP_FontAsset fontAsset)
	{
		return FontLoader.ConvertAssetNameToTMPFontType(fontAsset.name);
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00084274 File Offset: 0x00082674
	private static FontLoader.TMPFontType ConvertAssetNameToTMPFontType(string assetName)
	{
		string text = FontLoader.ConvertAssetNameToEnumName(assetName);
		if (text == "rounded_mgenplus_1c_medium__SDF")
		{
			text = "rounded_mgenplus_1c_meduim__SDF";
		}
		return (FontLoader.TMPFontType)Enum.Parse(typeof(FontLoader.TMPFontType), text);
	}

	// Token: 0x04001527 RID: 5415
	private static Dictionary<FontLoader.FontType, string> FontTypeMapping = new Dictionary<FontLoader.FontType, string>
	{
		{
			FontLoader.FontType.CupheadFelix_Regular_merged,
			"CupheadFelix-Regular-merged"
		},
		{
			FontLoader.FontType.CupheadHenriette_A_merged,
			"CupheadHenriette-A-merged"
		},
		{
			FontLoader.FontType.CupheadMemphis_Medium_merged,
			"CupheadMemphis-Medium-merged"
		},
		{
			FontLoader.FontType.CupheadPoster_Regular66Cyr_Lat_English99,
			"CupheadPoster-Regular(Cyr_Lat_English)"
		},
		{
			FontLoader.FontType.CupheadVogue_Bold_merged,
			"CupheadVogue-Bold-merged"
		},
		{
			FontLoader.FontType.CupheadVogue_ExtraBold_merged,
			"CupheadVogue-ExtraBold-merged"
		},
		{
			FontLoader.FontType.DFBrushRDStd_W7,
			"DFBrushRDStd-W7"
		},
		{
			FontLoader.FontType.DFBrushSQStd_W5,
			"DFBrushSQStd-W5"
		},
		{
			FontLoader.FontType.DSRefinedLetterB,
			"DSRefinedLetterB"
		},
		{
			FontLoader.FontType.FBBlue,
			"FBBlue"
		},
		{
			FontLoader.FontType.hyk2gjm,
			"hyk2gjm"
		},
		{
			FontLoader.FontType.jpchw00u,
			"jpchw00u"
		},
		{
			FontLoader.FontType.MComicPRC_Medium,
			"MComicPRC-Medium"
		},
		{
			FontLoader.FontType.YoonBackjaeM,
			"YoonBackjaeM"
		},
		{
			FontLoader.FontType.rounded_mgenplus_1c_medium,
			"rounded-mgenplus-1c-medium"
		},
		{
			FontLoader.FontType.hisikusa_A,
			"hisikusa-A"
		},
		{
			FontLoader.FontType.FGPotego__2,
			"FGPotego 2"
		},
		{
			FontLoader.FontType.FGPotegoBold__2,
			"FGPotegoBold 2"
		},
		{
			FontLoader.FontType.FGNewRetro,
			"FGNewRetro"
		},
		{
			FontLoader.FontType.ElegantHeiseiMinchoMono_9W,
			"ElegantHeiseiMinchoMono-9W"
		}
	};

	// Token: 0x04001528 RID: 5416
	private static Dictionary<FontLoader.TMPFontType, string> TMPFontTypeMapping = new Dictionary<FontLoader.TMPFontType, string>
	{
		{
			FontLoader.TMPFontType.CupheadFelix_Regular_merged__SDF,
			"CupheadFelix-Regular-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadHenriette_A_merged__SDF,
			"CupheadHenriette-A-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadMemphis_Medium_merged__SDF,
			"CupheadMemphis-Medium-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadPoster_Regular66Cyr_Lat_English99__SDF,
			"CupheadPoster-Regular(Cyr_Lat_English) SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_Bold_merged__SDF,
			"CupheadVogue-Bold-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_ExtraBold_merged__outline__SDF,
			"CupheadVogue-ExtraBold-merged outline SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_ExtraBold_merged__SDF,
			"CupheadVogue-ExtraBold-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_ExtraBold_merged__shadow__SDF,
			"CupheadVogue-ExtraBold-merged shadow SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushRDStd_W7__outline__SDF,
			"DFBrushRDStd-W7 outline SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushRDStd_W7__SDF,
			"DFBrushRDStd-W7 SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushRDStd_W7__shadow__SDF,
			"DFBrushRDStd-W7 shadow SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushSQStd_W5__SDF,
			"DFBrushSQStd-W5 SDF"
		},
		{
			FontLoader.TMPFontType.DSRefinedLetterB__SDF,
			"DSRefinedLetterB SDF"
		},
		{
			FontLoader.TMPFontType.FBBlue__SDF,
			"FBBlue SDF"
		},
		{
			FontLoader.TMPFontType.hyk2gjm__outline__SDF,
			"hyk2gjm outline SDF"
		},
		{
			FontLoader.TMPFontType.hyk2gjm__SDF,
			"hyk2gjm SDF"
		},
		{
			FontLoader.TMPFontType.hyk2gjm__shadow__SDF,
			"hyk2gjm shadow SDF"
		},
		{
			FontLoader.TMPFontType.jpchw00u__SDF,
			"jpchw00u SDF"
		},
		{
			FontLoader.TMPFontType.MComicPRC_Medium__SDF,
			"MComicPRC-Medium SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__outline__SDF,
			"YoonBackjaeM outline SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__SDF,
			"YoonBackjaeM SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__shadow__SDF,
			"YoonBackjaeM shadow SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__bold__SDF,
			"YoonBackjaeM bold SDF"
		},
		{
			FontLoader.TMPFontType.rounded_mgenplus_1c_meduim__SDF,
			"rounded-mgenplus-1c-meduim SDF"
		},
		{
			FontLoader.TMPFontType.hisikusa_A__SDF,
			"hisikusa-A SDF"
		},
		{
			FontLoader.TMPFontType.FGPotego__2__SDF,
			"FGPotego 2 SDF"
		},
		{
			FontLoader.TMPFontType.FGPotegoBold__2__SDF,
			"FGPotegoBold 2 SDF"
		},
		{
			FontLoader.TMPFontType.FGNewRetro__SDF,
			"FGNewRetro SDF"
		},
		{
			FontLoader.TMPFontType.ElegantHeiseiMinchoMono_9W__SDF,
			"ElegantHeiseiMinchoMono-9W SDF"
		},
		{
			FontLoader.TMPFontType.FGPotegoBold__2__outline__SDF,
			"FGPotegoBold 2 outline SDF"
		}
	};

	// Token: 0x04001529 RID: 5417
	private static Dictionary<FontLoader.FontType, Font> FontCache = new Dictionary<FontLoader.FontType, Font>();

	// Token: 0x0400152A RID: 5418
	private static Dictionary<FontLoader.TMPFontType, TMP_FontAsset> TMPFontCache = new Dictionary<FontLoader.TMPFontType, TMP_FontAsset>();

	// Token: 0x0400152B RID: 5419
	private static Dictionary<string, Material> TMPMaterialCache = new Dictionary<string, Material>();

	// Token: 0x020003AD RID: 941
	public enum FontType
	{
		// Token: 0x0400152D RID: 5421
		None,
		// Token: 0x0400152E RID: 5422
		CupheadFelix_Regular_merged,
		// Token: 0x0400152F RID: 5423
		CupheadHenriette_A_merged,
		// Token: 0x04001530 RID: 5424
		CupheadMemphis_Medium_merged,
		// Token: 0x04001531 RID: 5425
		CupheadPoster_Regular66Cyr_Lat_English99,
		// Token: 0x04001532 RID: 5426
		CupheadVogue_Bold_merged,
		// Token: 0x04001533 RID: 5427
		CupheadVogue_ExtraBold_merged,
		// Token: 0x04001534 RID: 5428
		DFBrushRDStd_W7,
		// Token: 0x04001535 RID: 5429
		DFBrushSQStd_W5,
		// Token: 0x04001536 RID: 5430
		DSRefinedLetterB,
		// Token: 0x04001537 RID: 5431
		FBBlue,
		// Token: 0x04001538 RID: 5432
		hyk2gjm,
		// Token: 0x04001539 RID: 5433
		jpchw00u,
		// Token: 0x0400153A RID: 5434
		MComicPRC_Medium,
		// Token: 0x0400153B RID: 5435
		YoonBackjaeM,
		// Token: 0x0400153C RID: 5436
		rounded_mgenplus_1c_medium,
		// Token: 0x0400153D RID: 5437
		hisikusa_A,
		// Token: 0x0400153E RID: 5438
		FGPotego__2,
		// Token: 0x0400153F RID: 5439
		FGPotegoBold__2,
		// Token: 0x04001540 RID: 5440
		FGNewRetro,
		// Token: 0x04001541 RID: 5441
		ElegantHeiseiMinchoMono_9W
	}

	// Token: 0x020003AE RID: 942
	public enum TMPFontType
	{
		// Token: 0x04001543 RID: 5443
		None,
		// Token: 0x04001544 RID: 5444
		CupheadFelix_Regular_merged__SDF,
		// Token: 0x04001545 RID: 5445
		CupheadHenriette_A_merged__SDF,
		// Token: 0x04001546 RID: 5446
		CupheadMemphis_Medium_merged__SDF,
		// Token: 0x04001547 RID: 5447
		CupheadPoster_Regular66Cyr_Lat_English99__SDF,
		// Token: 0x04001548 RID: 5448
		CupheadVogue_Bold_merged__SDF,
		// Token: 0x04001549 RID: 5449
		CupheadVogue_ExtraBold_merged__outline__SDF,
		// Token: 0x0400154A RID: 5450
		CupheadVogue_ExtraBold_merged__SDF,
		// Token: 0x0400154B RID: 5451
		CupheadVogue_ExtraBold_merged__shadow__SDF,
		// Token: 0x0400154C RID: 5452
		DFBrushRDStd_W7__outline__SDF,
		// Token: 0x0400154D RID: 5453
		DFBrushRDStd_W7__SDF,
		// Token: 0x0400154E RID: 5454
		DFBrushRDStd_W7__shadow__SDF,
		// Token: 0x0400154F RID: 5455
		DFBrushSQStd_W5__SDF,
		// Token: 0x04001550 RID: 5456
		DSRefinedLetterB__SDF,
		// Token: 0x04001551 RID: 5457
		FBBlue__SDF,
		// Token: 0x04001552 RID: 5458
		hyk2gjm__outline__SDF,
		// Token: 0x04001553 RID: 5459
		hyk2gjm__SDF,
		// Token: 0x04001554 RID: 5460
		hyk2gjm__shadow__SDF,
		// Token: 0x04001555 RID: 5461
		jpchw00u__SDF,
		// Token: 0x04001556 RID: 5462
		MComicPRC_Medium__SDF,
		// Token: 0x04001557 RID: 5463
		YoonBackjaeM__outline__SDF,
		// Token: 0x04001558 RID: 5464
		YoonBackjaeM__SDF,
		// Token: 0x04001559 RID: 5465
		YoonBackjaeM__shadow__SDF,
		// Token: 0x0400155A RID: 5466
		YoonBackjaeM__bold__SDF,
		// Token: 0x0400155B RID: 5467
		rounded_mgenplus_1c_meduim__SDF,
		// Token: 0x0400155C RID: 5468
		hisikusa_A__SDF,
		// Token: 0x0400155D RID: 5469
		FGPotego__2__SDF,
		// Token: 0x0400155E RID: 5470
		FGPotegoBold__2__SDF,
		// Token: 0x0400155F RID: 5471
		FGNewRetro__SDF,
		// Token: 0x04001560 RID: 5472
		ElegantHeiseiMinchoMono_9W__SDF,
		// Token: 0x04001561 RID: 5473
		FGPotegoBold__2__outline__SDF
	}
}
