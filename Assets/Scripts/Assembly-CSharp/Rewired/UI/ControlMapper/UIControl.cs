using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C46 RID: 3142
	[AddComponentMenu("")]
	public class UIControl : MonoBehaviour
	{
		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x00275905 File Offset: 0x00273D05
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0027590D File Offset: 0x00273D0D
		private void Awake()
		{
			this._id = UIControl.GetNextUid();
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06004D44 RID: 19780 RVA: 0x0027591A File Offset: 0x00273D1A
		// (set) Token: 0x06004D45 RID: 19781 RVA: 0x00275922 File Offset: 0x00273D22
		public bool showTitle
		{
			get
			{
				return this._showTitle;
			}
			set
			{
				if (this.title == null)
				{
					return;
				}
				this.title.gameObject.SetActive(value);
				this._showTitle = value;
			}
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0027594E File Offset: 0x00273D4E
		public virtual void SetCancelCallback(Action cancelCallback)
		{
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x00275950 File Offset: 0x00273D50
		private static int GetNextUid()
		{
			if (UIControl._uidCounter == 2147483647)
			{
				UIControl._uidCounter = 0;
			}
			int uidCounter = UIControl._uidCounter;
			UIControl._uidCounter++;
			return uidCounter;
		}

		// Token: 0x04005185 RID: 20869
		public Text title;

		// Token: 0x04005186 RID: 20870
		private int _id;

		// Token: 0x04005187 RID: 20871
		private bool _showTitle;

		// Token: 0x04005188 RID: 20872
		private static int _uidCounter;
	}
}
