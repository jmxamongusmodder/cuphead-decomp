using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C64 RID: 3172
	public class InlineGraphic : MaskableGraphic
	{
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06004EE4 RID: 20196 RVA: 0x0027AEB5 File Offset: 0x002792B5
		public override Texture mainTexture
		{
			get
			{
				if (this.texture == null)
				{
					return Graphic.s_WhiteTexture;
				}
				return this.texture;
			}
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x0027AED4 File Offset: 0x002792D4
		protected override void Awake()
		{
			this.m_manager = base.GetComponentInParent<InlineGraphicManager>();
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x0027AEE4 File Offset: 0x002792E4
		protected override void OnEnable()
		{
			if (this.m_RectTransform == null)
			{
				this.m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (this.m_manager != null && this.m_manager.spriteAsset != null)
			{
				this.texture = this.m_manager.spriteAsset.spriteSheet;
			}
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x0027AF50 File Offset: 0x00279350
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x0027AF58 File Offset: 0x00279358
		protected override void OnTransformParentChanged()
		{
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x0027AF5C File Offset: 0x0027935C
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.m_RectTransform == null)
			{
				this.m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (this.m_ParentRectTransform == null)
			{
				this.m_ParentRectTransform = this.m_RectTransform.parent.GetComponent<RectTransform>();
			}
			if (this.m_RectTransform.pivot != this.m_ParentRectTransform.pivot)
			{
				this.m_RectTransform.pivot = this.m_ParentRectTransform.pivot;
			}
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x0027AFE8 File Offset: 0x002793E8
		public new void UpdateMaterial()
		{
			base.UpdateMaterial();
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x0027AFF0 File Offset: 0x002793F0
		protected override void UpdateGeometry()
		{
		}

		// Token: 0x04005206 RID: 20998
		public Texture texture;

		// Token: 0x04005207 RID: 20999
		private InlineGraphicManager m_manager;

		// Token: 0x04005208 RID: 21000
		private RectTransform m_RectTransform;

		// Token: 0x04005209 RID: 21001
		private RectTransform m_ParentRectTransform;
	}
}
