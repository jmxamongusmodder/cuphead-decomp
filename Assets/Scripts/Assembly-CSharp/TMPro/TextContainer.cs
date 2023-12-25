using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x02000C69 RID: 3177
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Text Container")]
	public class TextContainer : UIBehaviour
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06004F18 RID: 20248 RVA: 0x0027B83A File Offset: 0x00279C3A
		// (set) Token: 0x06004F19 RID: 20249 RVA: 0x0027B842 File Offset: 0x00279C42
		public bool hasChanged
		{
			get
			{
				return this.m_hasChanged;
			}
			set
			{
				this.m_hasChanged = value;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06004F1A RID: 20250 RVA: 0x0027B84B File Offset: 0x00279C4B
		// (set) Token: 0x06004F1B RID: 20251 RVA: 0x0027B853 File Offset: 0x00279C53
		public Vector2 pivot
		{
			get
			{
				return this.m_pivot;
			}
			set
			{
				if (this.m_pivot != value)
				{
					this.m_pivot = value;
					this.m_anchorPosition = this.GetAnchorPosition(this.m_pivot);
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06004F1C RID: 20252 RVA: 0x0027B88C File Offset: 0x00279C8C
		// (set) Token: 0x06004F1D RID: 20253 RVA: 0x0027B894 File Offset: 0x00279C94
		public TextContainerAnchors anchorPosition
		{
			get
			{
				return this.m_anchorPosition;
			}
			set
			{
				if (this.m_anchorPosition != value)
				{
					this.m_anchorPosition = value;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06004F1E RID: 20254 RVA: 0x0027B8C8 File Offset: 0x00279CC8
		// (set) Token: 0x06004F1F RID: 20255 RVA: 0x0027B8D0 File Offset: 0x00279CD0
		public Rect rect
		{
			get
			{
				return this.m_rect;
			}
			set
			{
				if (this.m_rect != value)
				{
					this.m_rect = value;
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06004F20 RID: 20256 RVA: 0x0027B8F7 File Offset: 0x00279CF7
		// (set) Token: 0x06004F21 RID: 20257 RVA: 0x0027B914 File Offset: 0x00279D14
		public Vector2 size
		{
			get
			{
				return new Vector2(this.m_rect.width, this.m_rect.height);
			}
			set
			{
				if (new Vector2(this.m_rect.width, this.m_rect.height) != value)
				{
					this.SetRect(value);
					this.m_hasChanged = true;
					this.m_isDefaultWidth = false;
					this.m_isDefaultHeight = false;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06004F22 RID: 20258 RVA: 0x0027B969 File Offset: 0x00279D69
		// (set) Token: 0x06004F23 RID: 20259 RVA: 0x0027B976 File Offset: 0x00279D76
		public float width
		{
			get
			{
				return this.m_rect.width;
			}
			set
			{
				this.SetRect(new Vector2(value, this.m_rect.height));
				this.m_hasChanged = true;
				this.m_isDefaultWidth = false;
				this.OnContainerChanged();
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06004F24 RID: 20260 RVA: 0x0027B9A3 File Offset: 0x00279DA3
		// (set) Token: 0x06004F25 RID: 20261 RVA: 0x0027B9B0 File Offset: 0x00279DB0
		public float height
		{
			get
			{
				return this.m_rect.height;
			}
			set
			{
				this.SetRect(new Vector2(this.m_rect.width, value));
				this.m_hasChanged = true;
				this.m_isDefaultHeight = false;
				this.OnContainerChanged();
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06004F26 RID: 20262 RVA: 0x0027B9DD File Offset: 0x00279DDD
		public bool isDefaultWidth
		{
			get
			{
				return this.m_isDefaultWidth;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06004F27 RID: 20263 RVA: 0x0027B9E5 File Offset: 0x00279DE5
		public bool isDefaultHeight
		{
			get
			{
				return this.m_isDefaultHeight;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06004F28 RID: 20264 RVA: 0x0027B9ED File Offset: 0x00279DED
		// (set) Token: 0x06004F29 RID: 20265 RVA: 0x0027B9F5 File Offset: 0x00279DF5
		public bool isAutoFitting
		{
			get
			{
				return this.m_isAutoFitting;
			}
			set
			{
				this.m_isAutoFitting = value;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06004F2A RID: 20266 RVA: 0x0027B9FE File Offset: 0x00279DFE
		public Vector3[] corners
		{
			get
			{
				return this.m_corners;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06004F2B RID: 20267 RVA: 0x0027BA06 File Offset: 0x00279E06
		public Vector3[] worldCorners
		{
			get
			{
				return this.m_worldCorners;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06004F2C RID: 20268 RVA: 0x0027BA0E File Offset: 0x00279E0E
		// (set) Token: 0x06004F2D RID: 20269 RVA: 0x0027BA16 File Offset: 0x00279E16
		public Vector4 margins
		{
			get
			{
				return this.m_margins;
			}
			set
			{
				if (this.m_margins != value)
				{
					this.m_margins = value;
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06004F2E RID: 20270 RVA: 0x0027BA3D File Offset: 0x00279E3D
		public RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06004F2F RID: 20271 RVA: 0x0027BA62 File Offset: 0x00279E62
		public TextMeshPro textMeshPro
		{
			get
			{
				if (this.m_textMeshPro == null)
				{
					this.m_textMeshPro = base.GetComponent<TextMeshPro>();
				}
				return this.m_textMeshPro;
			}
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x0027BA88 File Offset: 0x00279E88
		protected override void Awake()
		{
			this.m_rectTransform = this.rectTransform;
			if (this.m_rectTransform == null)
			{
				Vector2 pivot = this.m_pivot;
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
				this.m_pivot = pivot;
			}
			this.m_textMeshPro = (base.GetComponent(typeof(TextMeshPro)) as TextMeshPro);
			if (this.m_rect.width == 0f || this.m_rect.height == 0f)
			{
				if (this.m_textMeshPro != null && this.m_textMeshPro.anchor != TMP_Compatibility.AnchorPositions.None)
				{
					this.m_isDefaultHeight = true;
					int num = (int)this.m_textMeshPro.anchor;
					this.m_textMeshPro.anchor = TMP_Compatibility.AnchorPositions.None;
					if (num == 9)
					{
						switch (this.m_textMeshPro.alignment)
						{
						case TextAlignmentOptions.TopLeft:
							this.m_textMeshPro.alignment = TextAlignmentOptions.BaselineLeft;
							break;
						case TextAlignmentOptions.Top:
							this.m_textMeshPro.alignment = TextAlignmentOptions.Baseline;
							break;
						case TextAlignmentOptions.TopRight:
							this.m_textMeshPro.alignment = TextAlignmentOptions.BaselineRight;
							break;
						case TextAlignmentOptions.TopJustified:
							this.m_textMeshPro.alignment = TextAlignmentOptions.BaselineJustified;
							break;
						}
						num = 3;
					}
					this.m_anchorPosition = (TextContainerAnchors)num;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					if (this.m_textMeshPro.lineLength == 72f)
					{
						this.m_rect.size = this.m_textMeshPro.GetPreferredValues(this.m_textMeshPro.text);
					}
					else
					{
						this.m_rect.width = this.m_textMeshPro.lineLength;
						this.m_rect.height = this.m_textMeshPro.GetPreferredValues(this.m_rect.width, float.PositiveInfinity).y;
					}
				}
				else
				{
					this.m_isDefaultWidth = true;
					this.m_isDefaultHeight = true;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					this.m_rect.width = 20f;
					this.m_rect.height = 5f;
					this.m_rectTransform.sizeDelta = this.size;
				}
				this.m_margins = new Vector4(0f, 0f, 0f, 0f);
				this.UpdateCorners();
			}
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x0027BCE5 File Offset: 0x0027A0E5
		protected override void OnEnable()
		{
			this.OnContainerChanged();
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x0027BCED File Offset: 0x0027A0ED
		protected override void OnDisable()
		{
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x0027BCF0 File Offset: 0x0027A0F0
		private void OnContainerChanged()
		{
			this.UpdateCorners();
			if (this.m_rectTransform != null)
			{
				this.m_rectTransform.sizeDelta = this.size;
				this.m_rectTransform.hasChanged = true;
			}
			if (this.textMeshPro != null)
			{
				this.m_textMeshPro.SetVerticesDirty();
				this.m_textMeshPro.margin = this.m_margins;
			}
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x0027BD60 File Offset: 0x0027A160
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.rectTransform == null)
			{
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			if (this.m_rectTransform.sizeDelta != TextContainer.k_defaultSize)
			{
				this.size = this.m_rectTransform.sizeDelta;
			}
			this.pivot = this.m_rectTransform.pivot;
			this.m_hasChanged = true;
			this.OnContainerChanged();
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x0027BDD8 File Offset: 0x0027A1D8
		private void SetRect(Vector2 size)
		{
			this.m_rect = new Rect(this.m_rect.x, this.m_rect.y, size.x, size.y);
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x0027BE0C File Offset: 0x0027A20C
		private void UpdateCorners()
		{
			this.m_corners[0] = new Vector3(-this.m_pivot.x * this.m_rect.width, -this.m_pivot.y * this.m_rect.height);
			this.m_corners[1] = new Vector3(-this.m_pivot.x * this.m_rect.width, (1f - this.m_pivot.y) * this.m_rect.height);
			this.m_corners[2] = new Vector3((1f - this.m_pivot.x) * this.m_rect.width, (1f - this.m_pivot.y) * this.m_rect.height);
			this.m_corners[3] = new Vector3((1f - this.m_pivot.x) * this.m_rect.width, -this.m_pivot.y * this.m_rect.height);
			if (this.m_rectTransform != null)
			{
				this.m_rectTransform.pivot = this.m_pivot;
			}
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x0027BF68 File Offset: 0x0027A368
		private Vector2 GetPivot(TextContainerAnchors anchor)
		{
			Vector2 zero = Vector2.zero;
			switch (anchor)
			{
			case TextContainerAnchors.TopLeft:
				zero = new Vector2(0f, 1f);
				break;
			case TextContainerAnchors.Top:
				zero = new Vector2(0.5f, 1f);
				break;
			case TextContainerAnchors.TopRight:
				zero = new Vector2(1f, 1f);
				break;
			case TextContainerAnchors.Left:
				zero = new Vector2(0f, 0.5f);
				break;
			case TextContainerAnchors.Middle:
				zero = new Vector2(0.5f, 0.5f);
				break;
			case TextContainerAnchors.Right:
				zero = new Vector2(1f, 0.5f);
				break;
			case TextContainerAnchors.BottomLeft:
				zero = new Vector2(0f, 0f);
				break;
			case TextContainerAnchors.Bottom:
				zero = new Vector2(0.5f, 0f);
				break;
			case TextContainerAnchors.BottomRight:
				zero = new Vector2(1f, 0f);
				break;
			}
			return zero;
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x0027C074 File Offset: 0x0027A474
		private TextContainerAnchors GetAnchorPosition(Vector2 pivot)
		{
			if (pivot == new Vector2(0f, 1f))
			{
				return TextContainerAnchors.TopLeft;
			}
			if (pivot == new Vector2(0.5f, 1f))
			{
				return TextContainerAnchors.Top;
			}
			if (pivot == new Vector2(1f, 1f))
			{
				return TextContainerAnchors.TopRight;
			}
			if (pivot == new Vector2(0f, 0.5f))
			{
				return TextContainerAnchors.Left;
			}
			if (pivot == new Vector2(0.5f, 0.5f))
			{
				return TextContainerAnchors.Middle;
			}
			if (pivot == new Vector2(1f, 0.5f))
			{
				return TextContainerAnchors.Right;
			}
			if (pivot == new Vector2(0f, 0f))
			{
				return TextContainerAnchors.BottomLeft;
			}
			if (pivot == new Vector2(0.5f, 0f))
			{
				return TextContainerAnchors.Bottom;
			}
			if (pivot == new Vector2(1f, 0f))
			{
				return TextContainerAnchors.BottomRight;
			}
			return TextContainerAnchors.Custom;
		}

		// Token: 0x04005228 RID: 21032
		private bool m_hasChanged;

		// Token: 0x04005229 RID: 21033
		[SerializeField]
		private Vector2 m_pivot;

		// Token: 0x0400522A RID: 21034
		[SerializeField]
		private TextContainerAnchors m_anchorPosition = TextContainerAnchors.Middle;

		// Token: 0x0400522B RID: 21035
		[SerializeField]
		private Rect m_rect;

		// Token: 0x0400522C RID: 21036
		private bool m_isDefaultWidth;

		// Token: 0x0400522D RID: 21037
		private bool m_isDefaultHeight;

		// Token: 0x0400522E RID: 21038
		private bool m_isAutoFitting;

		// Token: 0x0400522F RID: 21039
		private Vector3[] m_corners = new Vector3[4];

		// Token: 0x04005230 RID: 21040
		private Vector3[] m_worldCorners = new Vector3[4];

		// Token: 0x04005231 RID: 21041
		[SerializeField]
		private Vector4 m_margins;

		// Token: 0x04005232 RID: 21042
		private RectTransform m_rectTransform;

		// Token: 0x04005233 RID: 21043
		private static Vector2 k_defaultSize = new Vector2(100f, 100f);

		// Token: 0x04005234 RID: 21044
		private TextMeshPro m_textMeshPro;
	}
}
