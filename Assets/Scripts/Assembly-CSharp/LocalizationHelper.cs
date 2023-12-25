using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x0200091F RID: 2335
public class LocalizationHelper : MonoBehaviour
{
	// Token: 0x0600369D RID: 13981 RVA: 0x001F9A50 File Offset: 0x001F7E50
	private void Init()
	{
		if (this.textComponent != null)
		{
			this.initialFontSize = this.textComponent.fontSize;
		}
		if (this.textMeshProComponent != null)
		{
			this.initialFontAssetSize = this.textMeshProComponent.fontSize;
		}
		this.isInit = true;
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x001F9AA8 File Offset: 0x001F7EA8
	private void Awake()
	{
		this.platformOverride = base.GetComponent<LocalizationHelperPlatformOverride>();
		this.hasOverride = (this.platformOverride != null);
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x001F9AC8 File Offset: 0x001F7EC8
	private void Start()
	{
		Localization.OnLanguageChangedEvent += this.ApplyTranslation;
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x001F9ADB File Offset: 0x001F7EDB
	private void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ApplyTranslation;
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x001F9AEE File Offset: 0x001F7EEE
	private void OnEnable()
	{
		this.ApplyTranslation();
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x001F9AF8 File Offset: 0x001F7EF8
	public void ApplyTranslation()
	{
		int id = this.currentID;
		int num;
		if (this.hasOverride && this.platformOverride.HasOverrideForCurrentPlatform(out num))
		{
			id = num;
		}
		this.ApplyTranslation(Localization.Find(id));
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x001F9B37 File Offset: 0x001F7F37
	public void ApplyTranslation(TranslationElement translationElement, LocalizationHelper.LocalizationSubtext[] subTranslations = null)
	{
		this.subTranslations = subTranslations;
		this.ApplyTranslation(translationElement);
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x001F9B48 File Offset: 0x001F7F48
	private void ApplyTranslation(TranslationElement translationElement)
	{
		if (!this.isInit)
		{
			this.Init();
		}
		this.currentLanguage = Localization.language;
		if (this.currentLanguage == (Localization.Languages)(-1) || translationElement == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(translationElement.key))
		{
			return;
		}
		Localization.Translation translation = translationElement.translation;
		if (string.IsNullOrEmpty(translation.text))
		{
			translation = Localization.Translate(translationElement.key);
		}
		string text = translation.text;
		if (text != null)
		{
			text = text.Replace("\\n", "\n");
		}
		if (text != null && text.Contains("{") && text.Contains("}"))
		{
			if (this.subTranslations != null)
			{
				bool flag = true;
				while (flag)
				{
					flag = false;
					for (int i = 0; i < this.subTranslations.Length; i++)
					{
						if (text.Contains("{" + this.subTranslations[i].key + "}"))
						{
							flag = true;
							if (this.subTranslations[i].dontTranslate)
							{
								text = text.Replace("{" + this.subTranslations[i].key + "}", this.subTranslations[i].value);
							}
							else
							{
								Localization.Translation translation2 = Localization.Translate(this.subTranslations[i].value);
								if (string.IsNullOrEmpty(translation2.text))
								{
									text = text.Replace("{" + this.subTranslations[i].key + "}", this.subTranslations[i].value);
								}
								else
								{
									text = text.Replace("{" + this.subTranslations[i].key + "}", translation2.text);
								}
							}
						}
					}
				}
			}
			string[] array = text.Split(new char[]
			{
				'{'
			});
			if (array.Length > 1)
			{
				string[] array2 = array[1].Split(new char[]
				{
					'}'
				});
				if (array2.Length > 1)
				{
					string text2 = array2[0];
					Localization.Translation translation3 = Localization.Translate(text2);
					if (!string.IsNullOrEmpty(translation3.text))
					{
						text = text.Replace("{" + text2 + "}", translation3.text);
					}
				}
			}
		}
		if (this.textComponent != null)
		{
			this.textComponent.text = text;
			this.textComponent.enabled = !string.IsNullOrEmpty(text);
			if (translation.hasCustomFont)
			{
				this.textComponent.font = translation.fonts.font;
			}
			else if (Localization.Instance.fonts[(int)this.currentLanguage][(int)translationElement.category].fontType != FontLoader.FontType.None)
			{
				this.textComponent.font = Localization.Instance.fonts[(int)this.currentLanguage][(int)translationElement.category].font;
			}
			this.textComponent.fontSize = ((translation.fonts.fontSize <= 0) ? this.initialFontSize : translation.fonts.fontSize);
		}
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.text = text;
			this.textMeshProComponent.enabled = !string.IsNullOrEmpty(text);
			this.textMeshProComponent.characterSpacing = translation.fonts.charSpacing;
			if (translation.hasCustomFontAsset)
			{
				this.textMeshProComponent.font = translation.fonts.fontAsset;
			}
			else
			{
				this.textMeshProComponent.font = Localization.Instance.fonts[(int)this.currentLanguage][(int)translationElement.category].fontAsset;
			}
			this.textMeshProComponent.fontSize = ((translation.fonts.fontAssetSize <= 0f) ? this.initialFontAssetSize : translation.fonts.fontAssetSize);
		}
		if (this.spriteRendererComponent != null)
		{
			Sprite sprite;
			if (translation.hasSpriteAtlasImage)
			{
				SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset(translation.spriteAtlasName);
				sprite = cachedAsset.GetSprite(translation.spriteAtlasImageName);
			}
			else
			{
				sprite = translation.image;
			}
			this.spriteRendererComponent.sprite = sprite;
			this.spriteRendererComponent.enabled = false;
			this.spriteRendererComponent.enabled = (sprite != null);
		}
		if (this.imageComponent != null)
		{
			Sprite sprite2;
			if (translation.hasSpriteAtlasImage)
			{
				SpriteAtlas cachedAsset2 = AssetLoader<SpriteAtlas>.GetCachedAsset(translation.spriteAtlasName);
				sprite2 = cachedAsset2.GetSprite(translation.spriteAtlasImageName);
			}
			else
			{
				sprite2 = translation.image;
			}
			this.imageComponent.sprite = sprite2;
			this.imageComponent.enabled = false;
			this.imageComponent.enabled = (sprite2 != null);
		}
	}

	// Token: 0x04003EC7 RID: 16071
	public bool existingKey;

	// Token: 0x04003EC8 RID: 16072
	public int currentID = -1;

	// Token: 0x04003EC9 RID: 16073
	public Localization.Languages currentLanguage = (Localization.Languages)(-1);

	// Token: 0x04003ECA RID: 16074
	public Localization.Categories currentCategory;

	// Token: 0x04003ECB RID: 16075
	public bool currentCustomFont;

	// Token: 0x04003ECC RID: 16076
	public Text textComponent;

	// Token: 0x04003ECD RID: 16077
	public Image imageComponent;

	// Token: 0x04003ECE RID: 16078
	public SpriteRenderer spriteRendererComponent;

	// Token: 0x04003ECF RID: 16079
	public TMP_Text textMeshProComponent;

	// Token: 0x04003ED0 RID: 16080
	private int initialFontSize;

	// Token: 0x04003ED1 RID: 16081
	private float initialFontAssetSize;

	// Token: 0x04003ED2 RID: 16082
	private bool isInit;

	// Token: 0x04003ED3 RID: 16083
	private LocalizationHelper.LocalizationSubtext[] subTranslations;

	// Token: 0x04003ED4 RID: 16084
	private bool hasOverride;

	// Token: 0x04003ED5 RID: 16085
	private LocalizationHelperPlatformOverride platformOverride;

	// Token: 0x02000920 RID: 2336
	public struct LocalizationSubtext
	{
		// Token: 0x060036A5 RID: 13989 RVA: 0x001FA075 File Offset: 0x001F8475
		public LocalizationSubtext(string key, string value, bool dontTranslate = false)
		{
			this.key = key;
			this.value = value;
			this.dontTranslate = dontTranslate;
		}

		// Token: 0x04003ED6 RID: 16086
		public string key;

		// Token: 0x04003ED7 RID: 16087
		public string value;

		// Token: 0x04003ED8 RID: 16088
		public bool dontTranslate;
	}
}
