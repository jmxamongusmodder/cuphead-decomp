using System;

namespace TMPro
{
	// Token: 0x02000C9B RID: 3227
	[Serializable]
	public class TMP_Glyph : TMP_TextElement
	{
		// Token: 0x0600517C RID: 20860 RVA: 0x002995DC File Offset: 0x002979DC
		public static TMP_Glyph Clone(TMP_Glyph source)
		{
			return new TMP_Glyph
			{
				id = source.id,
				x = source.x,
				y = source.y,
				width = source.width,
				height = source.height,
				xOffset = source.xOffset,
				yOffset = source.yOffset,
				xAdvance = source.xAdvance
			};
		}
	}
}
