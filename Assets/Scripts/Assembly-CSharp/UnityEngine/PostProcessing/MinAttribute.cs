using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000B91 RID: 2961
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x06004817 RID: 18455 RVA: 0x0025DB83 File Offset: 0x0025BF83
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04004D7D RID: 19837
		public readonly float min;
	}
}
