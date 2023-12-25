using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000CAE RID: 3246
	public struct WordWrapState
	{
		// Token: 0x040054A4 RID: 21668
		public int previous_WordBreak;

		// Token: 0x040054A5 RID: 21669
		public int total_CharacterCount;

		// Token: 0x040054A6 RID: 21670
		public int visible_CharacterCount;

		// Token: 0x040054A7 RID: 21671
		public int visible_SpriteCount;

		// Token: 0x040054A8 RID: 21672
		public int visible_LinkCount;

		// Token: 0x040054A9 RID: 21673
		public int firstCharacterIndex;

		// Token: 0x040054AA RID: 21674
		public int firstVisibleCharacterIndex;

		// Token: 0x040054AB RID: 21675
		public int lastCharacterIndex;

		// Token: 0x040054AC RID: 21676
		public int lastVisibleCharIndex;

		// Token: 0x040054AD RID: 21677
		public int lineNumber;

		// Token: 0x040054AE RID: 21678
		public float maxAscender;

		// Token: 0x040054AF RID: 21679
		public float maxDescender;

		// Token: 0x040054B0 RID: 21680
		public float maxLineAscender;

		// Token: 0x040054B1 RID: 21681
		public float maxLineDescender;

		// Token: 0x040054B2 RID: 21682
		public float previousLineAscender;

		// Token: 0x040054B3 RID: 21683
		public float xAdvance;

		// Token: 0x040054B4 RID: 21684
		public float preferredWidth;

		// Token: 0x040054B5 RID: 21685
		public float preferredHeight;

		// Token: 0x040054B6 RID: 21686
		public float previousLineScale;

		// Token: 0x040054B7 RID: 21687
		public int wordCount;

		// Token: 0x040054B8 RID: 21688
		public FontStyles fontStyle;

		// Token: 0x040054B9 RID: 21689
		public float fontScale;

		// Token: 0x040054BA RID: 21690
		public float fontScaleMultiplier;

		// Token: 0x040054BB RID: 21691
		public float currentFontSize;

		// Token: 0x040054BC RID: 21692
		public float baselineOffset;

		// Token: 0x040054BD RID: 21693
		public float lineOffset;

		// Token: 0x040054BE RID: 21694
		public TMP_TextInfo textInfo;

		// Token: 0x040054BF RID: 21695
		public TMP_LineInfo lineInfo;

		// Token: 0x040054C0 RID: 21696
		public Color32 vertexColor;

		// Token: 0x040054C1 RID: 21697
		public TMP_XmlTagStack<Color32> colorStack;

		// Token: 0x040054C2 RID: 21698
		public TMP_XmlTagStack<float> sizeStack;

		// Token: 0x040054C3 RID: 21699
		public TMP_XmlTagStack<int> fontWeightStack;

		// Token: 0x040054C4 RID: 21700
		public TMP_XmlTagStack<int> styleStack;

		// Token: 0x040054C5 RID: 21701
		public TMP_XmlTagStack<int> actionStack;

		// Token: 0x040054C6 RID: 21702
		public TMP_XmlTagStack<MaterialReference> materialReferenceStack;

		// Token: 0x040054C7 RID: 21703
		public TMP_FontAsset currentFontAsset;

		// Token: 0x040054C8 RID: 21704
		public TMP_SpriteAsset currentSpriteAsset;

		// Token: 0x040054C9 RID: 21705
		public Material currentMaterial;

		// Token: 0x040054CA RID: 21706
		public int currentMaterialIndex;

		// Token: 0x040054CB RID: 21707
		public Extents meshExtents;

		// Token: 0x040054CC RID: 21708
		public bool tagNoParsing;
	}
}
