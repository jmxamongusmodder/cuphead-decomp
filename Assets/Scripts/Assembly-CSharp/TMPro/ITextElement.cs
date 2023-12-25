using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C7E RID: 3198
	public interface ITextElement
	{
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06005062 RID: 20578
		Material sharedMaterial { get; }

		// Token: 0x06005063 RID: 20579
		void Rebuild(CanvasUpdate update);

		// Token: 0x06005064 RID: 20580
		int GetInstanceID();
	}
}
