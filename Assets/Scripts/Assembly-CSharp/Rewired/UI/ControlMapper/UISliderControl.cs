using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C4D RID: 3149
	[AddComponentMenu("")]
	public class UISliderControl : UIControl
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06004D5F RID: 19807 RVA: 0x00275F17 File Offset: 0x00274317
		// (set) Token: 0x06004D60 RID: 19808 RVA: 0x00275F1F File Offset: 0x0027431F
		public bool showIcon
		{
			get
			{
				return this._showIcon;
			}
			set
			{
				if (this.iconImage == null)
				{
					return;
				}
				this.iconImage.gameObject.SetActive(value);
				this._showIcon = value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06004D61 RID: 19809 RVA: 0x00275F4B File Offset: 0x0027434B
		// (set) Token: 0x06004D62 RID: 19810 RVA: 0x00275F53 File Offset: 0x00274353
		public bool showSlider
		{
			get
			{
				return this._showSlider;
			}
			set
			{
				if (this.slider == null)
				{
					return;
				}
				this.slider.gameObject.SetActive(value);
				this._showSlider = value;
			}
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x00275F80 File Offset: 0x00274380
		public override void SetCancelCallback(Action cancelCallback)
		{
			base.SetCancelCallback(cancelCallback);
			if (cancelCallback == null || this.slider == null)
			{
				return;
			}
			if (this.slider is ICustomSelectable)
			{
				(this.slider as ICustomSelectable).CancelEvent += delegate()
				{
					cancelCallback();
				};
			}
			else
			{
				EventTrigger eventTrigger = this.slider.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = this.slider.gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.callback = new EventTrigger.TriggerEvent();
				entry.eventID = EventTriggerType.Cancel;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					cancelCallback();
				});
				if (eventTrigger.triggers == null)
				{
					eventTrigger.triggers = new List<EventTrigger.Entry>();
				}
				eventTrigger.triggers.Add(entry);
			}
		}

		// Token: 0x04005195 RID: 20885
		public Image iconImage;

		// Token: 0x04005196 RID: 20886
		public Slider slider;

		// Token: 0x04005197 RID: 20887
		private bool _showIcon;

		// Token: 0x04005198 RID: 20888
		private bool _showSlider;
	}
}
