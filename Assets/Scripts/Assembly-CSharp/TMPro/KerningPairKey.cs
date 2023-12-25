using System;

namespace TMPro
{
	// Token: 0x02000C9E RID: 3230
	public struct KerningPairKey
	{
		// Token: 0x0600517E RID: 20862 RVA: 0x00299658 File Offset: 0x00297A58
		public KerningPairKey(int ascii_left, int ascii_right)
		{
			this.ascii_Left = ascii_left;
			this.ascii_Right = ascii_right;
			this.key = (ascii_right << 16) + ascii_left;
		}

		// Token: 0x04005433 RID: 21555
		public int ascii_Left;

		// Token: 0x04005434 RID: 21556
		public int ascii_Right;

		// Token: 0x04005435 RID: 21557
		public int key;
	}
}
