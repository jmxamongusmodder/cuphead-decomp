using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C09 RID: 3081
	[AddComponentMenu("")]
	public class CanvasScalerExt : CanvasScaler
	{
		// Token: 0x06004992 RID: 18834 RVA: 0x0026748E File Offset: 0x0026588E
		public void ForceRefresh()
		{
			this.Handle();
		}
	}
}
