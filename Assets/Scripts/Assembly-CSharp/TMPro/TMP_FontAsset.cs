using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C70 RID: 3184
	[Serializable]
	public class TMP_FontAsset : TMP_Asset
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06004FC7 RID: 20423 RVA: 0x002942B3 File Offset: 0x002926B3
		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				if (TMP_FontAsset.s_defaultFontAsset == null)
				{
					TMP_FontAsset.s_defaultFontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/ARIAL SDF");
				}
				return TMP_FontAsset.s_defaultFontAsset;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06004FC8 RID: 20424 RVA: 0x002942D9 File Offset: 0x002926D9
		public FaceInfo fontInfo
		{
			get
			{
				return this.m_fontInfo;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x002942E1 File Offset: 0x002926E1
		public Dictionary<int, TMP_Glyph> characterDictionary
		{
			get
			{
				return this.m_characterDictionary;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06004FCA RID: 20426 RVA: 0x002942E9 File Offset: 0x002926E9
		public Dictionary<int, KerningPair> kerningDictionary
		{
			get
			{
				return this.m_kerningDictionary;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x002942F1 File Offset: 0x002926F1
		public KerningTable kerningInfo
		{
			get
			{
				return this.m_kerningInfo;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06004FCC RID: 20428 RVA: 0x002942F9 File Offset: 0x002926F9
		public LineBreakingTable lineBreakingInfo
		{
			get
			{
				return this.m_lineBreakingInfo;
			}
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x00294301 File Offset: 0x00292701
		private void OnEnable()
		{
			if (this.m_characterDictionary == null)
			{
				this.ReadFontDefinition();
			}
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x00294319 File Offset: 0x00292719
		private void OnDisable()
		{
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x0029431B File Offset: 0x0029271B
		public void AddFaceInfo(FaceInfo faceInfo)
		{
			this.m_fontInfo = faceInfo;
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x00294324 File Offset: 0x00292724
		public void AddGlyphInfo(TMP_Glyph[] glyphInfo)
		{
			this.m_glyphInfoList = new List<TMP_Glyph>();
			int num = glyphInfo.Length;
			this.m_fontInfo.CharacterCount = num;
			this.m_characterSet = new int[num];
			for (int i = 0; i < num; i++)
			{
				TMP_Glyph tmp_Glyph = new TMP_Glyph();
				tmp_Glyph.id = glyphInfo[i].id;
				tmp_Glyph.x = glyphInfo[i].x;
				tmp_Glyph.y = glyphInfo[i].y;
				tmp_Glyph.width = glyphInfo[i].width;
				tmp_Glyph.height = glyphInfo[i].height;
				tmp_Glyph.xOffset = glyphInfo[i].xOffset;
				tmp_Glyph.yOffset = glyphInfo[i].yOffset + this.m_fontInfo.Padding;
				tmp_Glyph.xAdvance = glyphInfo[i].xAdvance;
				this.m_glyphInfoList.Add(tmp_Glyph);
				this.m_characterSet[i] = tmp_Glyph.id;
			}
			this.m_glyphInfoList = (from s in this.m_glyphInfoList
			orderby s.id
			select s).ToList<TMP_Glyph>();
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x00294439 File Offset: 0x00292839
		public void AddKerningInfo(KerningTable kerningTable)
		{
			this.m_kerningInfo = kerningTable;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00294444 File Offset: 0x00292844
		public void ReadFontDefinition()
		{
			if (this.m_fontInfo == null)
			{
				return;
			}
			this.m_characterDictionary = new Dictionary<int, TMP_Glyph>();
			foreach (TMP_Glyph tmp_Glyph in this.m_glyphInfoList)
			{
				if (!this.m_characterDictionary.ContainsKey(tmp_Glyph.id))
				{
					this.m_characterDictionary.Add(tmp_Glyph.id, tmp_Glyph);
				}
			}
			TMP_Glyph tmp_Glyph2 = new TMP_Glyph();
			if (this.m_characterDictionary.ContainsKey(32))
			{
				this.m_characterDictionary[32].width = this.m_characterDictionary[32].xAdvance;
				this.m_characterDictionary[32].height = this.m_fontInfo.Ascender - this.m_fontInfo.Descender;
				this.m_characterDictionary[32].yOffset = this.m_fontInfo.Ascender;
			}
			else
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 32;
				tmp_Glyph2.x = 0f;
				tmp_Glyph2.y = 0f;
				tmp_Glyph2.width = this.m_fontInfo.Ascender / 5f;
				tmp_Glyph2.height = this.m_fontInfo.Ascender - this.m_fontInfo.Descender;
				tmp_Glyph2.xOffset = 0f;
				tmp_Glyph2.yOffset = this.m_fontInfo.Ascender;
				tmp_Glyph2.xAdvance = this.m_fontInfo.PointSize / 4f;
				this.m_characterDictionary.Add(32, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(160))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[32]);
				this.m_characterDictionary.Add(160, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(8203))
			{
				tmp_Glyph2 = TMP_Glyph.Clone(this.m_characterDictionary[32]);
				tmp_Glyph2.width = 0f;
				tmp_Glyph2.xAdvance = 0f;
				this.m_characterDictionary.Add(8203, tmp_Glyph2);
			}
			if (!this.m_characterDictionary.ContainsKey(10))
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 10;
				tmp_Glyph2.x = 0f;
				tmp_Glyph2.y = 0f;
				tmp_Glyph2.width = 10f;
				tmp_Glyph2.height = this.m_characterDictionary[32].height;
				tmp_Glyph2.xOffset = 0f;
				tmp_Glyph2.yOffset = this.m_characterDictionary[32].yOffset;
				tmp_Glyph2.xAdvance = 0f;
				this.m_characterDictionary.Add(10, tmp_Glyph2);
				if (!this.m_characterDictionary.ContainsKey(13))
				{
					this.m_characterDictionary.Add(13, tmp_Glyph2);
				}
			}
			if (!this.m_characterDictionary.ContainsKey(9))
			{
				tmp_Glyph2 = new TMP_Glyph();
				tmp_Glyph2.id = 9;
				tmp_Glyph2.x = this.m_characterDictionary[32].x;
				tmp_Glyph2.y = this.m_characterDictionary[32].y;
				tmp_Glyph2.width = this.m_characterDictionary[32].width * (float)this.tabSize + (this.m_characterDictionary[32].xAdvance - this.m_characterDictionary[32].width) * (float)(this.tabSize - 1);
				tmp_Glyph2.height = this.m_characterDictionary[32].height;
				tmp_Glyph2.xOffset = this.m_characterDictionary[32].xOffset;
				tmp_Glyph2.yOffset = this.m_characterDictionary[32].yOffset;
				tmp_Glyph2.xAdvance = this.m_characterDictionary[32].xAdvance * (float)this.tabSize;
				this.m_characterDictionary.Add(9, tmp_Glyph2);
			}
			this.m_fontInfo.TabWidth = this.m_characterDictionary[9].xAdvance;
			if (this.m_fontInfo.Scale == 0f)
			{
				this.m_fontInfo.Scale = 1f;
			}
			this.m_kerningDictionary = new Dictionary<int, KerningPair>();
			List<KerningPair> kerningPairs = this.m_kerningInfo.kerningPairs;
			for (int i = 0; i < kerningPairs.Count; i++)
			{
				KerningPair kerningPair = kerningPairs[i];
				KerningPairKey kerningPairKey = new KerningPairKey(kerningPair.AscII_Left, kerningPair.AscII_Right);
				if (!this.m_kerningDictionary.ContainsKey(kerningPairKey.key))
				{
					this.m_kerningDictionary.Add(kerningPairKey.key, kerningPair);
				}
				else if (!TMP_Settings.warningsDisabled)
				{
				}
			}
			this.m_lineBreakingInfo = new LineBreakingTable();
			TextAsset textAsset = Resources.Load("LineBreaking Leading Characters", typeof(TextAsset)) as TextAsset;
			if (textAsset != null)
			{
				this.m_lineBreakingInfo.leadingCharacters = this.GetCharacters(textAsset);
			}
			TextAsset textAsset2 = Resources.Load("LineBreaking Following Characters", typeof(TextAsset)) as TextAsset;
			if (textAsset2 != null)
			{
				this.m_lineBreakingInfo.followingCharacters = this.GetCharacters(textAsset2);
			}
			this.hashCode = TMP_TextUtilities.GetSimpleHashCode(base.name);
			this.materialHashCode = TMP_TextUtilities.GetSimpleHashCode(this.material.name);
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x002949C0 File Offset: 0x00292DC0
		private Dictionary<int, char> GetCharacters(TextAsset file)
		{
			Dictionary<int, char> dictionary = new Dictionary<int, char>();
			foreach (char c in file.text)
			{
				if (!dictionary.ContainsKey((int)c))
				{
					dictionary.Add((int)c, c);
				}
			}
			return dictionary;
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x00294A0E File Offset: 0x00292E0E
		public bool HasCharacter(int character)
		{
			return this.m_characterDictionary != null && this.m_characterDictionary.ContainsKey(character);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x00294A31 File Offset: 0x00292E31
		public bool HasCharacter(char character)
		{
			return this.m_characterDictionary != null && this.m_characterDictionary.ContainsKey((int)character);
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x00294A54 File Offset: 0x00292E54
		public bool HasCharacters(string text, out List<char> missingCharacters)
		{
			if (this.m_characterDictionary == null)
			{
				missingCharacters = null;
				return false;
			}
			missingCharacters = new List<char>();
			for (int i = 0; i < text.Length; i++)
			{
				if (!this.m_characterDictionary.ContainsKey((int)text[i]))
				{
					missingCharacters.Add(text[i]);
				}
			}
			return missingCharacters.Count == 0;
		}

		// Token: 0x04005278 RID: 21112
		private static TMP_FontAsset s_defaultFontAsset;

		// Token: 0x04005279 RID: 21113
		public TMP_FontAsset.FontAssetTypes fontAssetType;

		// Token: 0x0400527A RID: 21114
		[SerializeField]
		private FaceInfo m_fontInfo;

		// Token: 0x0400527B RID: 21115
		[SerializeField]
		public Texture2D atlas;

		// Token: 0x0400527C RID: 21116
		[SerializeField]
		private List<TMP_Glyph> m_glyphInfoList;

		// Token: 0x0400527D RID: 21117
		private Dictionary<int, TMP_Glyph> m_characterDictionary;

		// Token: 0x0400527E RID: 21118
		private Dictionary<int, KerningPair> m_kerningDictionary;

		// Token: 0x0400527F RID: 21119
		[SerializeField]
		private KerningTable m_kerningInfo;

		// Token: 0x04005280 RID: 21120
		[SerializeField]
		private KerningPair m_kerningPair;

		// Token: 0x04005281 RID: 21121
		[SerializeField]
		private LineBreakingTable m_lineBreakingInfo;

		// Token: 0x04005282 RID: 21122
		[SerializeField]
		public List<TMP_FontAsset> fallbackFontAssets;

		// Token: 0x04005283 RID: 21123
		[SerializeField]
		public FontCreationSetting fontCreationSettings;

		// Token: 0x04005284 RID: 21124
		public TMP_FontWeights[] fontWeights = new TMP_FontWeights[10];

		// Token: 0x04005285 RID: 21125
		private int[] m_characterSet;

		// Token: 0x04005286 RID: 21126
		public float normalStyle;

		// Token: 0x04005287 RID: 21127
		public float normalSpacingOffset;

		// Token: 0x04005288 RID: 21128
		public float boldStyle = 0.75f;

		// Token: 0x04005289 RID: 21129
		public float boldSpacing = 7f;

		// Token: 0x0400528A RID: 21130
		public byte italicStyle = 35;

		// Token: 0x0400528B RID: 21131
		public byte tabSize = 10;

		// Token: 0x0400528C RID: 21132
		private byte m_oldTabSize;

		// Token: 0x02000C71 RID: 3185
		public enum FontAssetTypes
		{
			// Token: 0x0400528F RID: 21135
			None,
			// Token: 0x04005290 RID: 21136
			SDF,
			// Token: 0x04005291 RID: 21137
			Bitmap
		}
	}
}
