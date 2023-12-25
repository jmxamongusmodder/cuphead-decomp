using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C65 RID: 3173
	[ExecuteInEditMode]
	public class InlineGraphicManager : MonoBehaviour
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06004EED RID: 20205 RVA: 0x0027AFFA File Offset: 0x002793FA
		// (set) Token: 0x06004EEE RID: 20206 RVA: 0x0027B002 File Offset: 0x00279402
		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.LoadSpriteAsset(value);
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06004EEF RID: 20207 RVA: 0x0027B00B File Offset: 0x0027940B
		// (set) Token: 0x06004EF0 RID: 20208 RVA: 0x0027B013 File Offset: 0x00279413
		public InlineGraphic inlineGraphic
		{
			get
			{
				return this.m_inlineGraphic;
			}
			set
			{
				if (this.m_inlineGraphic != value)
				{
					this.m_inlineGraphic = value;
				}
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x0027B02D File Offset: 0x0027942D
		public CanvasRenderer canvasRenderer
		{
			get
			{
				return this.m_inlineGraphicCanvasRenderer;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x0027B035 File Offset: 0x00279435
		public UIVertex[] uiVertex
		{
			get
			{
				return this.m_uiVertex;
			}
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x0027B03D File Offset: 0x0027943D
		private void Awake()
		{
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x0027B03F File Offset: 0x0027943F
		private void OnEnable()
		{
			base.enabled = false;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x0027B048 File Offset: 0x00279448
		private void OnDisable()
		{
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x0027B04A File Offset: 0x0027944A
		private void OnDestroy()
		{
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x0027B04C File Offset: 0x0027944C
		private void LoadSpriteAsset(TMP_SpriteAsset spriteAsset)
		{
			if (spriteAsset == null)
			{
				if (TMP_Settings.defaultSpriteAsset != null)
				{
					spriteAsset = TMP_Settings.defaultSpriteAsset;
				}
				else
				{
					spriteAsset = (Resources.Load("Sprite Assets/Default Sprite Asset") as TMP_SpriteAsset);
				}
			}
			this.m_spriteAsset = spriteAsset;
			this.m_inlineGraphic.texture = this.m_spriteAsset.spriteSheet;
			if (this.m_textComponent != null && this.m_isInitialized)
			{
				this.m_textComponent.havePropertiesChanged = true;
				this.m_textComponent.SetVerticesDirty();
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x0027B0E4 File Offset: 0x002794E4
		public void AddInlineGraphicsChild()
		{
			if (this.m_inlineGraphic != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Inline Graphic");
			this.m_inlineGraphic = gameObject.AddComponent<InlineGraphic>();
			this.m_inlineGraphicRectTransform = gameObject.GetComponent<RectTransform>();
			this.m_inlineGraphicCanvasRenderer = gameObject.GetComponent<CanvasRenderer>();
			this.m_inlineGraphicRectTransform.SetParent(base.transform, false);
			this.m_inlineGraphicRectTransform.localPosition = Vector3.zero;
			this.m_inlineGraphicRectTransform.anchoredPosition3D = Vector3.zero;
			this.m_inlineGraphicRectTransform.sizeDelta = Vector2.zero;
			this.m_inlineGraphicRectTransform.anchorMin = Vector2.zero;
			this.m_inlineGraphicRectTransform.anchorMax = Vector2.one;
			this.m_textComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x0027B1A0 File Offset: 0x002795A0
		public void AllocatedVertexBuffers(int size)
		{
			if (this.m_inlineGraphic == null)
			{
				this.AddInlineGraphicsChild();
				this.LoadSpriteAsset(this.m_spriteAsset);
			}
			if (this.m_uiVertex == null)
			{
				this.m_uiVertex = new UIVertex[4];
			}
			int num = size * 4;
			if (num > this.m_uiVertex.Length)
			{
				this.m_uiVertex = new UIVertex[Mathf.NextPowerOfTwo(num)];
			}
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x0027B20A File Offset: 0x0027960A
		public void UpdatePivot(Vector2 pivot)
		{
			if (this.m_inlineGraphicRectTransform == null)
			{
				this.m_inlineGraphicRectTransform = this.m_inlineGraphic.GetComponent<RectTransform>();
			}
			this.m_inlineGraphicRectTransform.pivot = pivot;
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x0027B23A File Offset: 0x0027963A
		public void ClearUIVertex()
		{
			if (this.uiVertex != null && this.uiVertex.Length > 0)
			{
				Array.Clear(this.uiVertex, 0, this.uiVertex.Length);
				this.m_inlineGraphicCanvasRenderer.Clear();
			}
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x0027B274 File Offset: 0x00279674
		public void DrawSprite(UIVertex[] uiVertices, int spriteCount)
		{
			if (this.m_inlineGraphicCanvasRenderer == null)
			{
				this.m_inlineGraphicCanvasRenderer = this.m_inlineGraphic.GetComponent<CanvasRenderer>();
			}
			this.m_inlineGraphicCanvasRenderer.SetVertices(uiVertices, spriteCount * 4);
			this.m_inlineGraphic.UpdateMaterial();
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x0027B2B4 File Offset: 0x002796B4
		public TMP_Sprite GetSprite(int index)
		{
			if (this.m_spriteAsset == null)
			{
				return null;
			}
			if (this.m_spriteAsset.spriteInfoList == null || index > this.m_spriteAsset.spriteInfoList.Count - 1)
			{
				return null;
			}
			return this.m_spriteAsset.spriteInfoList[index];
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x0027B310 File Offset: 0x00279710
		public int GetSpriteIndexByHashCode(int hashCode)
		{
			if (this.m_spriteAsset == null || this.m_spriteAsset.spriteInfoList == null)
			{
				return -1;
			}
			return this.m_spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => item.hashCode == hashCode);
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x0027B36C File Offset: 0x0027976C
		public int GetSpriteIndexByIndex(int index)
		{
			if (this.m_spriteAsset == null || this.m_spriteAsset.spriteInfoList == null)
			{
				return -1;
			}
			return this.m_spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => item.id == index);
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x0027B3C7 File Offset: 0x002797C7
		public void SetUIVertex(UIVertex[] uiVertex)
		{
			this.m_uiVertex = uiVertex;
		}

		// Token: 0x0400520A RID: 21002
		[SerializeField]
		private TMP_SpriteAsset m_spriteAsset;

		// Token: 0x0400520B RID: 21003
		[SerializeField]
		[HideInInspector]
		private InlineGraphic m_inlineGraphic;

		// Token: 0x0400520C RID: 21004
		[SerializeField]
		[HideInInspector]
		private CanvasRenderer m_inlineGraphicCanvasRenderer;

		// Token: 0x0400520D RID: 21005
		private UIVertex[] m_uiVertex;

		// Token: 0x0400520E RID: 21006
		private RectTransform m_inlineGraphicRectTransform;

		// Token: 0x0400520F RID: 21007
		private TMP_Text m_textComponent;

		// Token: 0x04005210 RID: 21008
		private bool m_isInitialized;
	}
}
