using System;

namespace TMPro
{
	// Token: 0x02000C8F RID: 3215
	public struct CaretInfo
	{
		// Token: 0x06005129 RID: 20777 RVA: 0x00296539 File Offset: 0x00294939
		public CaretInfo(int index, CaretPosition position)
		{
			this.index = index;
			this.position = position;
		}

		// Token: 0x040053E7 RID: 21479
		public int index;

		// Token: 0x040053E8 RID: 21480
		public CaretPosition position;
	}
}
