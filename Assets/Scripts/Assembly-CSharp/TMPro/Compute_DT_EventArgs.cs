using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C97 RID: 3223
	public class Compute_DT_EventArgs
	{
		// Token: 0x0600516C RID: 20844 RVA: 0x002990CF File Offset: 0x002974CF
		public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, float progress)
		{
			this.EventType = type;
			this.ProgressPercentage = progress;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x002990E5 File Offset: 0x002974E5
		public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, Color[] colors)
		{
			this.EventType = type;
			this.Colors = colors;
		}

		// Token: 0x0400540A RID: 21514
		public Compute_DistanceTransform_EventTypes EventType;

		// Token: 0x0400540B RID: 21515
		public float ProgressPercentage;

		// Token: 0x0400540C RID: 21516
		public Color[] Colors;
	}
}
