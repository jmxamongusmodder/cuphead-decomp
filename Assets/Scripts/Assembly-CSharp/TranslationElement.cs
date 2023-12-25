using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000924 RID: 2340
[Serializable]
public class TranslationElement : ISerializationCallbackReceiver
{
	// Token: 0x060036AC RID: 13996 RVA: 0x001FA137 File Offset: 0x001F8537
	public TranslationElement()
	{
	}

	// Token: 0x060036AD RID: 13997 RVA: 0x001FA162 File Offset: 0x001F8562
	public TranslationElement(string key, int depth, int id)
	{
		this.key = key;
		this.m_ID = id;
		this.m_Depth = depth;
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x001FA1A4 File Offset: 0x001F85A4
	public TranslationElement(string key, Localization.Categories category, string description, string translation1, string translation2, int depth, int id)
	{
		this.m_ID = id;
		this.m_Depth = depth;
		this.key = key;
		this.category = category;
		this.description = description;
		this.translations[(int)Localization.language1].text = translation1;
		this.translations[(int)Localization.language2].text = translation2;
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x060036AF RID: 13999 RVA: 0x001FA230 File Offset: 0x001F8630
	// (set) Token: 0x060036B0 RID: 14000 RVA: 0x001FA2CC File Offset: 0x001F86CC
	public Localization.Translation translation
	{
		get
		{
			if (!PlayerManager.Multiplayer && this.translationsCuphead != null && (Localization.Languages)this.translationsCuphead.Length > Localization.language)
			{
				return this.translationsCuphead[(int)Localization.language];
			}
			if (!PlayerManager.Multiplayer && this.translationsMugman != null && (Localization.Languages)this.translationsMugman.Length > Localization.language)
			{
				return this.translationsMugman[(int)Localization.language];
			}
			return this.translations[(int)Localization.language];
		}
		set
		{
			if (!PlayerManager.Multiplayer && this.translationsCuphead != null && (Localization.Languages)this.translationsCuphead.Length > Localization.language)
			{
				this.translationsCuphead[(int)Localization.language] = value;
			}
			if (!PlayerManager.Multiplayer && this.translationsMugman != null && (Localization.Languages)this.translationsMugman.Length > Localization.language)
			{
				this.translationsMugman[(int)Localization.language] = value;
			}
			this.translations[(int)Localization.language] = value;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x060036B1 RID: 14001 RVA: 0x001FA369 File Offset: 0x001F8769
	// (set) Token: 0x060036B2 RID: 14002 RVA: 0x001FA371 File Offset: 0x001F8771
	public int depth
	{
		get
		{
			return this.m_Depth;
		}
		set
		{
			this.m_Depth = value;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x060036B3 RID: 14003 RVA: 0x001FA37A File Offset: 0x001F877A
	// (set) Token: 0x060036B4 RID: 14004 RVA: 0x001FA382 File Offset: 0x001F8782
	public TranslationElement parent
	{
		get
		{
			return this.m_Parent;
		}
		set
		{
			this.m_Parent = value;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x060036B5 RID: 14005 RVA: 0x001FA38B File Offset: 0x001F878B
	// (set) Token: 0x060036B6 RID: 14006 RVA: 0x001FA393 File Offset: 0x001F8793
	public List<TranslationElement> children
	{
		get
		{
			return this.m_Children;
		}
		set
		{
			this.m_Children = value;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x060036B7 RID: 14007 RVA: 0x001FA39C File Offset: 0x001F879C
	public bool hasChildren
	{
		get
		{
			return this.children != null && this.children.Count > 0;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x060036B8 RID: 14008 RVA: 0x001FA3BA File Offset: 0x001F87BA
	// (set) Token: 0x060036B9 RID: 14009 RVA: 0x001FA3C2 File Offset: 0x001F87C2
	public int id
	{
		get
		{
			return this.m_ID;
		}
		set
		{
			this.m_ID = value;
		}
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x001FA3CC File Offset: 0x001F87CC
	private Localization.Translation[] Grow(Localization.Translation[] oldTranslations, int newLength)
	{
		Localization.Translation[] array = new Localization.Translation[newLength];
		for (int i = 0; i < oldTranslations.Length; i++)
		{
			array[i].fonts = oldTranslations[i].fonts;
			array[i].image = oldTranslations[i].image;
			array[i].spriteAtlasName = oldTranslations[i].spriteAtlasName;
			array[i].spriteAtlasImageName = oldTranslations[i].spriteAtlasImageName;
			array[i].hasImage = oldTranslations[i].hasImage;
			array[i].text = oldTranslations[i].text;
		}
		for (int j = oldTranslations.Length; j < array.Length; j++)
		{
			array[j].fonts = null;
			array[j].image = null;
			array[j].hasImage = false;
			array[j].text = string.Empty;
			array[j].spriteAtlasName = string.Empty;
			array[j].spriteAtlasImageName = string.Empty;
		}
		return array;
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x001FA4F5 File Offset: 0x001F88F5
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x001FA4F8 File Offset: 0x001F88F8
	public void OnAfterDeserialize()
	{
		int num = Enum.GetNames(typeof(Localization.Languages)).Length;
		if (this.translations.Length < num)
		{
			this.translations = this.Grow(this.translations, num);
			if (this.translationsCuphead != null)
			{
				this.translationsCuphead = this.Grow(this.translationsCuphead, num);
			}
			if (this.translationsMugman != null)
			{
				this.translationsMugman = this.Grow(this.translationsMugman, num);
			}
		}
	}

	// Token: 0x04003EDC RID: 16092
	[SerializeField]
	private int m_ID;

	// Token: 0x04003EDD RID: 16093
	[SerializeField]
	private int m_Depth;

	// Token: 0x04003EDE RID: 16094
	[NonSerialized]
	private TranslationElement m_Parent;

	// Token: 0x04003EDF RID: 16095
	[NonSerialized]
	private List<TranslationElement> m_Children;

	// Token: 0x04003EE0 RID: 16096
	[SerializeField]
	public string key = string.Empty;

	// Token: 0x04003EE1 RID: 16097
	[SerializeField]
	public Localization.Categories category;

	// Token: 0x04003EE2 RID: 16098
	[SerializeField]
	public string description = string.Empty;

	// Token: 0x04003EE3 RID: 16099
	[SerializeField]
	public Localization.Translation[] translations = new Localization.Translation[12];

	// Token: 0x04003EE4 RID: 16100
	[SerializeField]
	public Localization.Translation[] translationsCuphead;

	// Token: 0x04003EE5 RID: 16101
	[SerializeField]
	public Localization.Translation[] translationsMugman;

	// Token: 0x04003EE6 RID: 16102
	public bool enabled;

	// Token: 0x04003EE7 RID: 16103
	[NonSerialized]
	public bool multiplayerLock;
}
