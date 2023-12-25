using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C36 RID: 3126
	[AddComponentMenu("")]
	public class ScrollbarVisibilityHelper : MonoBehaviour
	{
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06004CD8 RID: 19672 RVA: 0x002746E0 File Offset: 0x00272AE0
		private Scrollbar hScrollBar
		{
			get
			{
				return (!(this.scrollRect != null)) ? null : this.scrollRect.horizontalScrollbar;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06004CD9 RID: 19673 RVA: 0x00274704 File Offset: 0x00272B04
		private Scrollbar vScrollBar
		{
			get
			{
				return (!(this.scrollRect != null)) ? null : this.scrollRect.verticalScrollbar;
			}
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00274728 File Offset: 0x00272B28
		private void Awake()
		{
			if (this.scrollRect != null)
			{
				this.target = this.scrollRect.gameObject.AddComponent<ScrollbarVisibilityHelper>();
				this.target.onlySendMessage = true;
				this.target.target = this;
			}
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x00274774 File Offset: 0x00272B74
		private void OnRectTransformDimensionsChange()
		{
			if (this.onlySendMessage)
			{
				if (this.target != null)
				{
					this.target.ScrollRectTransformDimensionsChanged();
				}
			}
			else
			{
				this.EvaluateScrollbar();
			}
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x002747A8 File Offset: 0x00272BA8
		private void ScrollRectTransformDimensionsChanged()
		{
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x002747B0 File Offset: 0x00272BB0
		private void EvaluateScrollbar()
		{
			if (this.scrollRect == null)
			{
				return;
			}
			if (this.vScrollBar == null && this.hScrollBar == null)
			{
				return;
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Rect rect = this.scrollRect.content.rect;
			Rect rect2 = (this.scrollRect.transform as RectTransform).rect;
			if (this.vScrollBar != null)
			{
				bool value = rect.height > rect2.height;
				this.SetActiveDeferred(this.vScrollBar.gameObject, value);
			}
			if (this.hScrollBar != null)
			{
				bool value2 = rect.width > rect2.width;
				this.SetActiveDeferred(this.hScrollBar.gameObject, value2);
			}
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x002748A6 File Offset: 0x00272CA6
		private void SetActiveDeferred(GameObject obj, bool value)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.SetActiveCoroutine(obj, value));
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x002748C0 File Offset: 0x00272CC0
		private IEnumerator SetActiveCoroutine(GameObject obj, bool value)
		{
			yield return null;
			if (obj != null)
			{
				obj.SetActive(value);
			}
			yield break;
		}

		// Token: 0x04005130 RID: 20784
		public ScrollRect scrollRect;

		// Token: 0x04005131 RID: 20785
		private bool onlySendMessage;

		// Token: 0x04005132 RID: 20786
		private ScrollbarVisibilityHelper target;
	}
}
