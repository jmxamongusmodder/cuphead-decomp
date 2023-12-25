using System;

namespace TMPro
{
	// Token: 0x02000C9F RID: 3231
	[Serializable]
	public class KerningPair
	{
		// Token: 0x0600517F RID: 20863 RVA: 0x00299674 File Offset: 0x00297A74
		public KerningPair(int left, int right, float offset)
		{
			this.AscII_Left = left;
			this.AscII_Right = right;
			this.XadvanceOffset = offset;
		}

		// Token: 0x04005436 RID: 21558
		public int AscII_Left;

		// Token: 0x04005437 RID: 21559
		public int AscII_Right;

		// Token: 0x04005438 RID: 21560
		public float XadvanceOffset;
	}
}
