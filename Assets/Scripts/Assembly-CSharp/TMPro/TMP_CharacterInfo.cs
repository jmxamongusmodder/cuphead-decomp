using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000CA4 RID: 3236
	public struct TMP_CharacterInfo
	{
		// Token: 0x04005452 RID: 21586
		public char character;

		// Token: 0x04005453 RID: 21587
		public short index;

		// Token: 0x04005454 RID: 21588
		public TMP_TextElementType elementType;

		// Token: 0x04005455 RID: 21589
		public TMP_TextElement textElement;

		// Token: 0x04005456 RID: 21590
		public TMP_FontAsset fontAsset;

		// Token: 0x04005457 RID: 21591
		public TMP_SpriteAsset spriteAsset;

		// Token: 0x04005458 RID: 21592
		public int spriteIndex;

		// Token: 0x04005459 RID: 21593
		public Material material;

		// Token: 0x0400545A RID: 21594
		public int materialReferenceIndex;

		// Token: 0x0400545B RID: 21595
		public float pointSize;

		// Token: 0x0400545C RID: 21596
		public short lineNumber;

		// Token: 0x0400545D RID: 21597
		public short pageNumber;

		// Token: 0x0400545E RID: 21598
		public short vertexIndex;

		// Token: 0x0400545F RID: 21599
		public TMP_Vertex vertex_TL;

		// Token: 0x04005460 RID: 21600
		public TMP_Vertex vertex_BL;

		// Token: 0x04005461 RID: 21601
		public TMP_Vertex vertex_TR;

		// Token: 0x04005462 RID: 21602
		public TMP_Vertex vertex_BR;

		// Token: 0x04005463 RID: 21603
		public Vector3 topLeft;

		// Token: 0x04005464 RID: 21604
		public Vector3 bottomLeft;

		// Token: 0x04005465 RID: 21605
		public Vector3 topRight;

		// Token: 0x04005466 RID: 21606
		public Vector3 bottomRight;

		// Token: 0x04005467 RID: 21607
		public float origin;

		// Token: 0x04005468 RID: 21608
		public float ascender;

		// Token: 0x04005469 RID: 21609
		public float baseLine;

		// Token: 0x0400546A RID: 21610
		public float descender;

		// Token: 0x0400546B RID: 21611
		public float xAdvance;

		// Token: 0x0400546C RID: 21612
		public float aspectRatio;

		// Token: 0x0400546D RID: 21613
		public float scale;

		// Token: 0x0400546E RID: 21614
		public Color32 color;

		// Token: 0x0400546F RID: 21615
		public FontStyles style;

		// Token: 0x04005470 RID: 21616
		public bool isVisible;
	}
}
