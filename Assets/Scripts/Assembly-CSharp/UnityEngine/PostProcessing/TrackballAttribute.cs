using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000B92 RID: 2962
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x06004818 RID: 18456 RVA: 0x0025DB92 File Offset: 0x0025BF92
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04004D7E RID: 19838
		public readonly string method;
	}
}
