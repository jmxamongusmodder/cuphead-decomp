using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C49 RID: 3145
	[AddComponentMenu("")]
	public class UIGroup : MonoBehaviour
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06004D52 RID: 19794 RVA: 0x00275B7C File Offset: 0x00273F7C
		// (set) Token: 0x06004D53 RID: 19795 RVA: 0x00275BA4 File Offset: 0x00273FA4
		public string labelText
		{
			get
			{
				return (!(this._label != null)) ? string.Empty : this._label.text;
			}
			set
			{
				if (this._label == null)
				{
					return;
				}
				this._label.text = value;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06004D54 RID: 19796 RVA: 0x00275BC4 File Offset: 0x00273FC4
		public Transform content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x00275BCC File Offset: 0x00273FCC
		public void SetLabelActive(bool state)
		{
			if (this._label == null)
			{
				return;
			}
			this._label.gameObject.SetActive(state);
		}

		// Token: 0x0400518F RID: 20879
		[SerializeField]
		private Text _label;

		// Token: 0x04005190 RID: 20880
		[SerializeField]
		private Transform _content;
	}
}
