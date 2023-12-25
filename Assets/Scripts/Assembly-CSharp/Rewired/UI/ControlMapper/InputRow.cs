using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C33 RID: 3123
	[AddComponentMenu("")]
	public class InputRow : MonoBehaviour
	{
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x00273B72 File Offset: 0x00271F72
		// (set) Token: 0x06004C97 RID: 19607 RVA: 0x00273B7A File Offset: 0x00271F7A
		public ButtonInfo[] buttons { get; private set; }

		// Token: 0x06004C98 RID: 19608 RVA: 0x00273B83 File Offset: 0x00271F83
		public void Initialize(int rowIndex, string label, Action<int, ButtonInfo> inputFieldActivatedCallback)
		{
			this.rowIndex = rowIndex;
			this.label.text = label;
			this.inputFieldActivatedCallback = inputFieldActivatedCallback;
			this.buttons = base.transform.GetComponentsInChildren<ButtonInfo>(true);
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x00273BB1 File Offset: 0x00271FB1
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (this.inputFieldActivatedCallback == null)
			{
				return;
			}
			this.inputFieldActivatedCallback(this.rowIndex, buttonInfo);
		}

		// Token: 0x040050F0 RID: 20720
		public Text label;

		// Token: 0x040050F2 RID: 20722
		private int rowIndex;

		// Token: 0x040050F3 RID: 20723
		private Action<int, ButtonInfo> inputFieldActivatedCallback;
	}
}
