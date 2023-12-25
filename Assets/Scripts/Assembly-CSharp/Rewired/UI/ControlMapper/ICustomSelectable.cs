using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C2D RID: 3117
	public interface ICustomSelectable : ICancelHandler, IEventSystemHandler
	{
		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06004C62 RID: 19554
		// (set) Token: 0x06004C63 RID: 19555
		Sprite disabledHighlightedSprite { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06004C64 RID: 19556
		// (set) Token: 0x06004C65 RID: 19557
		Color disabledHighlightedColor { get; set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06004C66 RID: 19558
		// (set) Token: 0x06004C67 RID: 19559
		string disabledHighlightedTrigger { get; set; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06004C68 RID: 19560
		// (set) Token: 0x06004C69 RID: 19561
		bool autoNavUp { get; set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06004C6A RID: 19562
		// (set) Token: 0x06004C6B RID: 19563
		bool autoNavDown { get; set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06004C6C RID: 19564
		// (set) Token: 0x06004C6D RID: 19565
		bool autoNavLeft { get; set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06004C6E RID: 19566
		// (set) Token: 0x06004C6F RID: 19567
		bool autoNavRight { get; set; }

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x06004C70 RID: 19568
		// (remove) Token: 0x06004C71 RID: 19569
		event UnityAction CancelEvent;
	}
}
