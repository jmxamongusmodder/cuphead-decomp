using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000B90 RID: 2960
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x0025DB74 File Offset: 0x0025BF74
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04004D7B RID: 19835
		public readonly string name;

		// Token: 0x04004D7C RID: 19836
		public bool dirty;
	}
}
