using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C48 RID: 3144
	[AddComponentMenu("")]
	public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06004D4E RID: 19790 RVA: 0x00265CD0 File Offset: 0x002640D0
		// (remove) Token: 0x06004D4F RID: 19791 RVA: 0x00265D08 File Offset: 0x00264108
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<GameObject> OnSelectedEvent;

		// Token: 0x06004D50 RID: 19792 RVA: 0x00265D3E File Offset: 0x0026413E
		public void OnSelect(BaseEventData eventData)
		{
			if (this.OnSelectedEvent != null)
			{
				this.OnSelectedEvent(base.gameObject);
			}
		}

		// Token: 0x0400518B RID: 20875
		public string identifier;

		// Token: 0x0400518C RID: 20876
		public int intData;

		// Token: 0x0400518D RID: 20877
		public Text text;
	}
}
