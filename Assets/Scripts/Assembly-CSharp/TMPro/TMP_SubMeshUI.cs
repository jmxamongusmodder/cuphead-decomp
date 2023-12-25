using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C7D RID: 3197
	[ExecuteInEditMode]
	public class TMP_SubMeshUI : MaskableGraphic, ITextElement, IClippable, IMaskable, IMaterialModifier
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06005037 RID: 20535 RVA: 0x00295BC7 File Offset: 0x00293FC7
		// (set) Token: 0x06005038 RID: 20536 RVA: 0x00295BCF File Offset: 0x00293FCF
		public TMP_FontAsset fontAsset
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				this.m_fontAsset = value;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06005039 RID: 20537 RVA: 0x00295BD8 File Offset: 0x00293FD8
		// (set) Token: 0x0600503A RID: 20538 RVA: 0x00295BE0 File Offset: 0x00293FE0
		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.m_spriteAsset = value;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600503B RID: 20539 RVA: 0x00295BE9 File Offset: 0x00293FE9
		public override Texture mainTexture
		{
			get
			{
				if (this.sharedMaterial != null)
				{
					return this.sharedMaterial.mainTexture;
				}
				return null;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600503C RID: 20540 RVA: 0x00295C09 File Offset: 0x00294009
		// (set) Token: 0x0600503D RID: 20541 RVA: 0x00295C17 File Offset: 0x00294017
		public override Material material
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
				{
					return;
				}
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x00295C4F File Offset: 0x0029404F
		// (set) Token: 0x0600503F RID: 20543 RVA: 0x00295C57 File Offset: 0x00294057
		public Material sharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				this.SetSharedMaterial(value);
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06005040 RID: 20544 RVA: 0x00295C60 File Offset: 0x00294060
		public override Material materialForRendering
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return null;
				}
				return this.GetModifiedMaterial(this.m_sharedMaterial);
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x00295C81 File Offset: 0x00294081
		// (set) Token: 0x06005042 RID: 20546 RVA: 0x00295C89 File Offset: 0x00294089
		public bool isDefaultMaterial
		{
			get
			{
				return this.m_isDefaultMaterial;
			}
			set
			{
				this.m_isDefaultMaterial = value;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x00295C92 File Offset: 0x00294092
		// (set) Token: 0x06005044 RID: 20548 RVA: 0x00295C9A File Offset: 0x0029409A
		public float padding
		{
			get
			{
				return this.m_padding;
			}
			set
			{
				this.m_padding = value;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06005045 RID: 20549 RVA: 0x00295CA3 File Offset: 0x002940A3
		public new CanvasRenderer canvasRenderer
		{
			get
			{
				if (this.m_canvasRenderer == null)
				{
					this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
				}
				return this.m_canvasRenderer;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06005046 RID: 20550 RVA: 0x00295CC8 File Offset: 0x002940C8
		// (set) Token: 0x06005047 RID: 20551 RVA: 0x00295CF9 File Offset: 0x002940F9
		public Mesh mesh
		{
			get
			{
				if (this.m_mesh == null)
				{
					this.m_mesh = new Mesh();
					this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_mesh;
			}
			set
			{
				this.m_mesh = value;
			}
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x00295D04 File Offset: 0x00294104
		public static TMP_SubMeshUI AddSubTextObject(TextMeshProUGUI textComponent, MaterialReference materialReference)
		{
			GameObject gameObject = new GameObject("TMP UI SubObject [" + materialReference.material.name + "]");
			gameObject.layer = textComponent.gameObject.layer;
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.pivot = textComponent.rectTransform.pivot;
			TMP_SubMeshUI tmp_SubMeshUI = gameObject.AddComponent<TMP_SubMeshUI>();
			tmp_SubMeshUI.m_canvasRenderer = tmp_SubMeshUI.canvasRenderer;
			tmp_SubMeshUI.m_TextComponent = textComponent;
			tmp_SubMeshUI.m_materialReferenceIndex = materialReference.index;
			tmp_SubMeshUI.m_fontAsset = materialReference.fontAsset;
			tmp_SubMeshUI.m_spriteAsset = materialReference.spriteAsset;
			tmp_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tmp_SubMeshUI.SetSharedMaterial(materialReference.material);
			gameObject.transform.SetParent(textComponent.transform, false);
			return tmp_SubMeshUI;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x00295DEA File Offset: 0x002941EA
		protected override void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			this.m_ShouldRecalculateStencil = true;
			this.RecalculateClipping();
			this.RecalculateMasking();
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x00295E11 File Offset: 0x00294211
		protected override void OnDisable()
		{
			TMP_UpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			base.OnDisable();
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x00295E44 File Offset: 0x00294244
		protected override void OnDestroy()
		{
			if (this.m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_mesh);
			}
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
			}
			this.m_isRegisteredForEvents = false;
			this.RecalculateClipping();
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x00295E96 File Offset: 0x00294296
		protected override void OnTransformParentChanged()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_ShouldRecalculateStencil = true;
			this.RecalculateClipping();
			this.RecalculateMasking();
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x00295EB8 File Offset: 0x002942B8
		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
			{
				this.m_StencilValue = TMP_MaterialManager.GetStencilID(base.gameObject);
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_StencilValue > 0)
			{
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, this.m_StencilValue);
				if (this.m_MaskMaterial != null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				}
				this.m_MaskMaterial = material;
			}
			return material;
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x00295F28 File Offset: 0x00294328
		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x00295F58 File Offset: 0x00294358
		public float GetPaddingForMaterial(Material mat)
		{
			return ShaderUtilities.GetPadding(mat, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x00295F83 File Offset: 0x00294383
		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x00295F98 File Offset: 0x00294398
		public override void SetAllDirty()
		{
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x00295F9A File Offset: 0x0029439A
		public override void SetVerticesDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.havePropertiesChanged = true;
				this.m_TextComponent.SetVerticesDirty();
			}
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x00295FD0 File Offset: 0x002943D0
		public override void SetLayoutDirty()
		{
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x00295FD2 File Offset: 0x002943D2
		public override void SetMaterialDirty()
		{
			this.m_materialDirty = true;
			this.UpdateMaterial();
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x00295FE1 File Offset: 0x002943E1
		public void SetPivotDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			base.rectTransform.pivot = this.m_TextComponent.rectTransform.pivot;
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0029600A File Offset: 0x0029440A
		protected override void UpdateGeometry()
		{
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x0029600C File Offset: 0x0029440C
		public override void Rebuild(CanvasUpdate update)
		{
			if (update == CanvasUpdate.PreRender)
			{
				if (!this.m_materialDirty)
				{
					return;
				}
				this.UpdateMaterial();
				this.m_materialDirty = false;
			}
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0029602E File Offset: 0x0029442E
		public void RefreshMaterial()
		{
			this.UpdateMaterial();
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x00296038 File Offset: 0x00294438
		protected override void UpdateMaterial()
		{
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = this.canvasRenderer;
			}
			this.m_canvasRenderer.materialCount = 1;
			this.m_canvasRenderer.SetMaterial(this.materialForRendering, 0);
			this.m_canvasRenderer.SetTexture(this.mainTexture);
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x00296091 File Offset: 0x00294491
		public override void RecalculateClipping()
		{
			base.RecalculateClipping();
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x00296099 File Offset: 0x00294499
		public override void RecalculateMasking()
		{
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x002960A8 File Offset: 0x002944A8
		private Material GetMaterial()
		{
			return this.m_sharedMaterial;
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x002960B0 File Offset: 0x002944B0
		private Material GetMaterial(Material mat)
		{
			if (this.m_material == null || this.m_material.GetInstanceID() != mat.GetInstanceID())
			{
				this.m_material = this.CreateMaterialInstance(mat);
			}
			this.m_sharedMaterial = this.m_material;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0029611C File Offset: 0x0029451C
		private Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x00296153 File Offset: 0x00294553
		private Material GetSharedMaterial()
		{
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
			}
			return this.m_canvasRenderer.GetMaterial();
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x0029617D File Offset: 0x0029457D
		private void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_Material = this.m_sharedMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x002961A4 File Offset: 0x002945A4
		int ITextElement.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x040052C8 RID: 21192
		[SerializeField]
		private TMP_FontAsset m_fontAsset;

		// Token: 0x040052C9 RID: 21193
		[SerializeField]
		private TMP_SpriteAsset m_spriteAsset;

		// Token: 0x040052CA RID: 21194
		[SerializeField]
		private Material m_material;

		// Token: 0x040052CB RID: 21195
		[SerializeField]
		private Material m_sharedMaterial;

		// Token: 0x040052CC RID: 21196
		[SerializeField]
		private bool m_isDefaultMaterial;

		// Token: 0x040052CD RID: 21197
		[SerializeField]
		private float m_padding;

		// Token: 0x040052CE RID: 21198
		[SerializeField]
		private CanvasRenderer m_canvasRenderer;

		// Token: 0x040052CF RID: 21199
		private Mesh m_mesh;

		// Token: 0x040052D0 RID: 21200
		[SerializeField]
		private TextMeshProUGUI m_TextComponent;

		// Token: 0x040052D1 RID: 21201
		[NonSerialized]
		private bool m_isRegisteredForEvents;

		// Token: 0x040052D2 RID: 21202
		private bool m_materialDirty;

		// Token: 0x040052D3 RID: 21203
		[SerializeField]
		private int m_materialReferenceIndex;
	}
}
