using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C37 RID: 3127
	[AddComponentMenu("")]
	[RequireComponent(typeof(Selectable))]
	public class ScrollRectSelectableChild : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x0027499B File Offset: 0x00272D9B
		private RectTransform parentScrollRectContentTransform
		{
			get
			{
				return this.parentScrollRect.content;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06004CE2 RID: 19682 RVA: 0x002749A8 File Offset: 0x00272DA8
		private Selectable selectable
		{
			get
			{
				Selectable result;
				if ((result = this._selectable) == null)
				{
					result = (this._selectable = base.GetComponent<Selectable>());
				}
				return result;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x002749D1 File Offset: 0x00272DD1
		private RectTransform rectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x002749DE File Offset: 0x00272DDE
		private void Start()
		{
			this.parentScrollRect = base.transform.GetComponentInParent<ScrollRect>();
			if (this.parentScrollRect == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!");
				return;
			}
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x00274A10 File Offset: 0x00272E10
		public void OnSelect(BaseEventData eventData)
		{
			if (this.parentScrollRect == null)
			{
				return;
			}
			if (!(eventData is AxisEventData))
			{
				return;
			}
			RectTransform rectTransform = this.parentScrollRect.transform as RectTransform;
			Rect child = MathTools.TransformRect(this.rectTransform.rect, this.rectTransform, rectTransform);
			Rect rect = rectTransform.rect;
			Rect rect2 = rectTransform.rect;
			float height;
			if (this.useCustomEdgePadding)
			{
				height = this.customEdgePadding;
			}
			else
			{
				height = child.height;
			}
			rect2.yMax -= height;
			rect2.yMin += height;
			if (MathTools.RectContains(rect2, child))
			{
				return;
			}
			Vector2 vector;
			if (!MathTools.GetOffsetToContainRect(rect2, child, out vector))
			{
				return;
			}
			Vector2 anchoredPosition = this.parentScrollRectContentTransform.anchoredPosition;
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x + vector.x, 0f, Mathf.Abs(rect.width - this.parentScrollRectContentTransform.sizeDelta.x));
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + vector.y, 0f, Mathf.Abs(rect.height - this.parentScrollRectContentTransform.sizeDelta.y));
			this.parentScrollRectContentTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04005133 RID: 20787
		public bool useCustomEdgePadding;

		// Token: 0x04005134 RID: 20788
		public float customEdgePadding = 50f;

		// Token: 0x04005135 RID: 20789
		private ScrollRect parentScrollRect;

		// Token: 0x04005136 RID: 20790
		private Selectable _selectable;
	}
}
