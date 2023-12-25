using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C6A RID: 3178
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(TextContainer))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[AddComponentMenu("Mesh/TextMesh Pro")]
	[SelectionBase]
	public class TextMeshPro : TMP_Text, ILayoutElement
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06004F3B RID: 20283 RVA: 0x002842C3 File Offset: 0x002826C3
		// (set) Token: 0x06004F3C RID: 20284 RVA: 0x002842CB File Offset: 0x002826CB
		[Obsolete("The length of the line is now controlled by the size of the text container and margins.")]
		public float lineLength
		{
			get
			{
				return this.m_lineLength;
			}
			set
			{
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06004F3D RID: 20285 RVA: 0x002842CD File Offset: 0x002826CD
		// (set) Token: 0x06004F3E RID: 20286 RVA: 0x002842D5 File Offset: 0x002826D5
		[Obsolete("The length of the line is now controlled by the size of the text container and margins.")]
		public TMP_Compatibility.AnchorPositions anchor
		{
			get
			{
				return this.m_anchor;
			}
			set
			{
				this.m_anchor = value;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06004F3F RID: 20287 RVA: 0x002842DE File Offset: 0x002826DE
		// (set) Token: 0x06004F40 RID: 20288 RVA: 0x002842E6 File Offset: 0x002826E6
		public override Vector4 margin
		{
			get
			{
				return this.m_margin;
			}
			set
			{
				if (this.m_margin == value)
				{
					return;
				}
				this.m_margin = value;
				this.textContainer.margins = this.m_margin;
				this.ComputeMarginSize();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06004F41 RID: 20289 RVA: 0x00284325 File Offset: 0x00282725
		// (set) Token: 0x06004F42 RID: 20290 RVA: 0x00284332 File Offset: 0x00282732
		public int sortingLayerID
		{
			get
			{
				return this.m_renderer.sortingLayerID;
			}
			set
			{
				this.m_renderer.sortingLayerID = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06004F43 RID: 20291 RVA: 0x00284340 File Offset: 0x00282740
		// (set) Token: 0x06004F44 RID: 20292 RVA: 0x0028434D File Offset: 0x0028274D
		public int sortingOrder
		{
			get
			{
				return this.m_renderer.sortingOrder;
			}
			set
			{
				this.m_renderer.sortingOrder = value;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06004F45 RID: 20293 RVA: 0x0028435B File Offset: 0x0028275B
		// (set) Token: 0x06004F46 RID: 20294 RVA: 0x00284363 File Offset: 0x00282763
		public override bool autoSizeTextContainer
		{
			get
			{
				return this.m_autoSizeTextContainer;
			}
			set
			{
				if (this.m_autoSizeTextContainer == value)
				{
					return;
				}
				this.m_autoSizeTextContainer = value;
				if (this.m_autoSizeTextContainer)
				{
					TMP_UpdateManager.RegisterTextElementForLayoutRebuild(this);
					this.SetLayoutDirty();
				}
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06004F47 RID: 20295 RVA: 0x00284390 File Offset: 0x00282790
		public TextContainer textContainer
		{
			get
			{
				if (this.m_textContainer == null)
				{
					this.m_textContainer = base.GetComponent<TextContainer>();
				}
				return this.m_textContainer;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06004F48 RID: 20296 RVA: 0x002843B5 File Offset: 0x002827B5
		public new Transform transform
		{
			get
			{
				if (this.m_transform == null)
				{
					this.m_transform = base.GetComponent<Transform>();
				}
				return this.m_transform;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06004F49 RID: 20297 RVA: 0x002843DA File Offset: 0x002827DA
		public Renderer renderer
		{
			get
			{
				if (this.m_renderer == null)
				{
					this.m_renderer = base.GetComponent<Renderer>();
				}
				return this.m_renderer;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06004F4A RID: 20298 RVA: 0x002843FF File Offset: 0x002827FF
		public override Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06004F4B RID: 20299 RVA: 0x00284408 File Offset: 0x00282808
		public override Bounds bounds
		{
			get
			{
				if (this.m_mesh != null)
				{
					return this.m_mesh.bounds;
				}
				return default(Bounds);
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06004F4C RID: 20300 RVA: 0x0028443B File Offset: 0x0028283B
		// (set) Token: 0x06004F4D RID: 20301 RVA: 0x00284443 File Offset: 0x00282843
		public MaskingTypes maskType
		{
			get
			{
				return this.m_maskType;
			}
			set
			{
				this.m_maskType = value;
				this.SetMask(this.m_maskType);
			}
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00284458 File Offset: 0x00282858
		public void SetMask(MaskingTypes type, Vector4 maskCoords)
		{
			this.SetMask(type);
			this.SetMaskCoordinates(maskCoords);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00284468 File Offset: 0x00282868
		public void SetMask(MaskingTypes type, Vector4 maskCoords, float softnessX, float softnessY)
		{
			this.SetMask(type);
			this.SetMaskCoordinates(maskCoords, softnessX, softnessY);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x0028447B File Offset: 0x0028287B
		public override void SetVerticesDirty()
		{
			if (this.m_verticesAlreadyDirty || !this.IsActive())
			{
				return;
			}
			TMP_UpdateManager.RegisterTextElementForGraphicRebuild(this);
			this.m_verticesAlreadyDirty = true;
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x002844A1 File Offset: 0x002828A1
		public override void SetLayoutDirty()
		{
			if (this.m_layoutAlreadyDirty || !this.IsActive())
			{
				return;
			}
			this.m_layoutAlreadyDirty = true;
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x002844C1 File Offset: 0x002828C1
		public override void SetMaterialDirty()
		{
			this.UpdateMaterial();
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x002844CC File Offset: 0x002828CC
		public override void Rebuild(CanvasUpdate update)
		{
			if (update == CanvasUpdate.Prelayout && this.m_autoSizeTextContainer)
			{
				this.CalculateLayoutInputHorizontal();
				if (this.m_textContainer.isDefaultWidth)
				{
					this.m_textContainer.width = this.m_preferredWidth;
				}
				this.CalculateLayoutInputVertical();
				if (this.m_textContainer.isDefaultHeight)
				{
					this.m_textContainer.height = this.m_preferredHeight;
				}
			}
			if (update == CanvasUpdate.PreRender)
			{
				this.OnPreRenderObject();
				this.m_verticesAlreadyDirty = false;
				this.m_layoutAlreadyDirty = false;
				if (!this.m_isMaterialDirty)
				{
					return;
				}
				this.UpdateMaterial();
				this.m_isMaterialDirty = false;
			}
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x0028456C File Offset: 0x0028296C
		protected override void UpdateMaterial()
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = this.renderer;
			}
			this.m_renderer.sharedMaterial = this.m_sharedMaterial;
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x0028459C File Offset: 0x0028299C
		public override void UpdateMeshPadding()
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_havePropertiesChanged = true;
			this.checkPaddingRequired = false;
			for (int i = 1; i < this.m_textInfo.materialCount; i++)
			{
				this.m_subTextObjects[i].UpdateMeshPadding(this.m_enableExtraPadding, this.m_isUsingBold);
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x0028461A File Offset: 0x00282A1A
		public override void ForceMeshUpdate()
		{
			this.m_havePropertiesChanged = true;
			this.OnPreRenderObject();
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x00284629 File Offset: 0x00282A29
		public override TMP_TextInfo GetTextInfo(string text)
		{
			base.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			this.m_renderMode = TextRenderFlags.DontRender;
			this.ComputeMarginSize();
			this.GenerateTextMesh();
			this.m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x00284669 File Offset: 0x00282A69
		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x00284674 File Offset: 0x00282A74
		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
				{
					mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
				{
					mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
				{
					mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
				{
					mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				}
				mesh.RecalculateBounds();
			}
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x0028475C File Offset: 0x00282B5C
		public override void UpdateVertexData()
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				mesh.RecalculateBounds();
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x0028481D File Offset: 0x00282C1D
		public void UpdateFontAsset()
		{
			this.LoadFontAsset();
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x00284828 File Offset: 0x00282C28
		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.IsRectTransformDriven = true;
			this.m_currentAutoSizeMode = this.m_enableAutoSizing;
			if (this.m_isCalculateSizeRequired || this.m_rectTransform.hasChanged)
			{
				this.m_minWidth = 0f;
				this.m_flexibleWidth = 0f;
				if (this.m_enableAutoSizing)
				{
					this.m_fontSize = this.m_fontSizeMax;
				}
				this.m_marginWidth = float.PositiveInfinity;
				this.m_marginHeight = float.PositiveInfinity;
				if (this.m_isInputParsingRequired || this.m_isTextTruncated)
				{
					base.ParseInputText();
				}
				this.GenerateTextMesh();
				this.m_renderMode = TextRenderFlags.Render;
				this.ComputeMarginSize();
				this.m_isLayoutDirty = true;
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x002848F4 File Offset: 0x00282CF4
		public void CalculateLayoutInputVertical()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.IsRectTransformDriven = true;
			if (this.m_isCalculateSizeRequired || this.m_rectTransform.hasChanged)
			{
				this.m_minHeight = 0f;
				this.m_flexibleHeight = 0f;
				if (this.m_enableAutoSizing)
				{
					this.m_currentAutoSizeMode = true;
					this.m_enableAutoSizing = false;
				}
				this.m_marginHeight = float.PositiveInfinity;
				this.GenerateTextMesh();
				this.m_enableAutoSizing = this.m_currentAutoSizeMode;
				this.m_renderMode = TextRenderFlags.Render;
				this.ComputeMarginSize();
				this.m_isLayoutDirty = true;
			}
			this.m_isCalculateSizeRequired = false;
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x002849A0 File Offset: 0x00282DA0
		protected override void Awake()
		{
			if (this.m_fontColor == Color.white && this.m_fontColor32 != Color.white)
			{
				this.m_fontColor = this.m_fontColor32;
			}
			this.m_textContainer = base.GetComponent<TextContainer>();
			if (this.m_textContainer == null)
			{
				this.m_textContainer = base.gameObject.AddComponent<TextContainer>();
			}
			this.m_renderer = base.GetComponent<Renderer>();
			if (this.m_renderer == null)
			{
				this.m_renderer = base.gameObject.AddComponent<Renderer>();
			}
			if (base.canvasRenderer == null)
			{
				CanvasRenderer canvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
				canvasRenderer.hideFlags = HideFlags.HideInInspector;
			}
			this.m_rectTransform = base.rectTransform;
			this.m_transform = this.transform;
			this.m_meshFilter = base.GetComponent<MeshFilter>();
			if (this.m_meshFilter == null)
			{
				this.m_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (this.m_mesh == null)
			{
				this.m_mesh = new Mesh();
				this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
				this.m_meshFilter.mesh = this.m_mesh;
			}
			this.m_meshFilter.hideFlags = HideFlags.HideInInspector;
			if (this.m_text == null)
			{
				this.m_enableWordWrapping = TMP_Settings.enableWordWrapping;
				this.m_enableKerning = TMP_Settings.enableKerning;
				this.m_enableExtraPadding = TMP_Settings.enableExtraPadding;
				this.m_tintAllSprites = TMP_Settings.enableTintAllSprites;
			}
			this.LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			this.m_char_buffer = new int[this.m_max_characters];
			this.m_cached_TextElement = new TMP_Glyph();
			this.m_isFirstAllocation = true;
			this.m_textInfo = new TMP_TextInfo(this);
			if (this.m_fontAsset == null)
			{
				return;
			}
			if (this.m_fontSizeMin == 0f)
			{
				this.m_fontSizeMin = this.m_fontSize / 2f;
			}
			if (this.m_fontSizeMax == 0f)
			{
				this.m_fontSizeMax = this.m_fontSize * 2f;
			}
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00284BD8 File Offset: 0x00282FD8
		protected override void OnEnable()
		{
			if (this.m_meshFilter.sharedMesh == null)
			{
				this.m_meshFilter.mesh = this.m_mesh;
			}
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			this.ComputeMarginSize();
			this.m_verticesAlreadyDirty = false;
			this.SetVerticesDirty();
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x00284C31 File Offset: 0x00283031
		protected override void OnDisable()
		{
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x00284C39 File Offset: 0x00283039
		protected override void OnDestroy()
		{
			if (this.m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_mesh);
			}
			this.m_isRegisteredForEvents = false;
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x00284C64 File Offset: 0x00283064
		protected override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontAsset == null)
			{
				if (TMP_Settings.defaultFontAsset != null)
				{
					this.m_fontAsset = TMP_Settings.defaultFontAsset;
				}
				else
				{
					this.m_fontAsset = (Resources.Load("Fonts & Materials/ARIAL SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
				}
				if (this.m_fontAsset == null)
				{
					return;
				}
				if (this.m_fontAsset.characterDictionary == null)
				{
				}
				this.m_renderer.sharedMaterial = this.m_fontAsset.material;
				this.m_sharedMaterial = this.m_fontAsset.material;
				this.m_sharedMaterial.SetFloat("_CullMode", 0f);
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				this.m_renderer.receiveShadows = false;
				this.m_renderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			else
			{
				if (this.m_fontAsset.characterDictionary == null)
				{
					this.m_fontAsset.ReadFontDefinition();
				}
				if (this.m_renderer.sharedMaterial == null || this.m_renderer.sharedMaterial.mainTexture == null || this.m_fontAsset.atlas.GetInstanceID() != this.m_renderer.sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
				{
					this.m_renderer.sharedMaterial = this.m_fontAsset.material;
					this.m_sharedMaterial = this.m_fontAsset.material;
				}
				else
				{
					this.m_sharedMaterial = this.m_renderer.sharedMaterial;
				}
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				if (this.m_sharedMaterial.passCount > 1)
				{
					this.m_renderer.receiveShadows = true;
					this.m_renderer.shadowCastingMode = ShadowCastingMode.On;
				}
				else
				{
					this.m_renderer.receiveShadows = false;
					this.m_renderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
			this.m_padding = this.GetPaddingForMaterial();
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			base.GetSpecialCharacters(this.m_fontAsset);
			this.m_sharedMaterials.Add(this.m_sharedMaterial);
			this.m_sharedMaterialHashCode = TMP_TextUtilities.GetSimpleHashCode(this.m_sharedMaterial.name);
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x00284EBC File Offset: 0x002832BC
		private void UpdateEnvMapMatrix()
		{
			if (!this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) || this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null)
			{
				return;
			}
			Vector3 euler = this.m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
			this.m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
			this.m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, this.m_EnvMapMatrix);
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00284F44 File Offset: 0x00283344
		private void SetMask(MaskingTypes maskType)
		{
			if (maskType != MaskingTypes.MaskOff)
			{
				if (maskType != MaskingTypes.MaskSoft)
				{
					if (maskType == MaskingTypes.MaskHard)
					{
						this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_HARD);
						this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
						this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
					}
				}
				else
				{
					this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
					this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
					this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				}
			}
			else
			{
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			}
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x00285009 File Offset: 0x00283409
		private void SetMaskCoordinates(Vector4 coords)
		{
			this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x0028501C File Offset: 0x0028341C
		private void SetMaskCoordinates(Vector4 coords, float softX, float softY)
		{
			this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessX, softX);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessY, softY);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x00285054 File Offset: 0x00283454
		private void EnableMasking()
		{
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.m_isMaskingEnabled = true;
				this.UpdateMask();
			}
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x002850B4 File Offset: 0x002834B4
		private void DisableMasking()
		{
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.m_isMaskingEnabled = false;
				this.UpdateMask();
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00285114 File Offset: 0x00283514
		private void UpdateMask()
		{
			if (!this.m_isMaskingEnabled)
			{
				return;
			}
			if (this.m_isMaskingEnabled && this.m_fontMaterial == null)
			{
				this.CreateMaterialInstance();
			}
			float num = Mathf.Min(Mathf.Min(this.m_textContainer.margins.x, this.m_textContainer.margins.z), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX));
			float num2 = Mathf.Min(Mathf.Min(this.m_textContainer.margins.y, this.m_textContainer.margins.w), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY));
			num = ((num <= 0f) ? 0f : num);
			num2 = ((num2 <= 0f) ? 0f : num2);
			float z = (this.m_textContainer.width - Mathf.Max(this.m_textContainer.margins.x, 0f) - Mathf.Max(this.m_textContainer.margins.z, 0f)) / 2f + num;
			float w = (this.m_textContainer.height - Mathf.Max(this.m_textContainer.margins.y, 0f) - Mathf.Max(this.m_textContainer.margins.w, 0f)) / 2f + num2;
			Vector2 vector = new Vector2((0.5f - this.m_textContainer.pivot.x) * this.m_textContainer.width + (Mathf.Max(this.m_textContainer.margins.x, 0f) - Mathf.Max(this.m_textContainer.margins.z, 0f)) / 2f, (0.5f - this.m_textContainer.pivot.y) * this.m_textContainer.height + (-Mathf.Max(this.m_textContainer.margins.y, 0f) + Mathf.Max(this.m_textContainer.margins.w, 0f)) / 2f);
			Vector4 value = new Vector4(vector.x, vector.y, z, w);
			this.m_fontMaterial.SetVector(ShaderUtilities.ID_ClipRect, value);
			this.m_fontMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessX, num);
			this.m_fontMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessY, num2);
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x002853D0 File Offset: 0x002837D0
		protected override Material GetMaterial(Material mat)
		{
			if (this.m_fontMaterial == null || this.m_fontMaterial.GetInstanceID() != mat.GetInstanceID())
			{
				this.m_fontMaterial = this.CreateMaterialInstance(mat);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x0028543C File Offset: 0x0028383C
		protected override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontMaterials == null)
			{
				this.m_fontMaterials = new Material[materialCount];
			}
			else if (this.m_fontMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontMaterials[i] = base.fontMaterial;
				}
				else
				{
					this.m_fontMaterials[i] = this.m_subTextObjects[i].material;
				}
			}
			this.m_fontSharedMaterials = this.m_fontMaterials;
			return this.m_fontMaterials;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x002854DE File Offset: 0x002838DE
		protected override void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x002854FC File Offset: 0x002838FC
		protected override Material[] GetSharedMaterials()
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontSharedMaterials[i] = this.m_sharedMaterial;
				}
				else
				{
					this.m_fontSharedMaterials[i] = this.m_subTextObjects[i].sharedMaterial;
				}
			}
			return this.m_fontSharedMaterials;
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x00285594 File Offset: 0x00283994
		protected override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					if (!(materials[i].mainTexture == null) && materials[i].mainTexture.GetInstanceID() == this.m_sharedMaterial.mainTexture.GetInstanceID())
					{
						this.m_sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
						this.m_padding = this.GetPaddingForMaterial(this.m_sharedMaterial);
					}
				}
				else if (!(materials[i].mainTexture == null) && materials[i].mainTexture.GetInstanceID() == this.m_subTextObjects[i].sharedMaterial.mainTexture.GetInstanceID())
				{
					if (this.m_subTextObjects[i].isDefaultMaterial)
					{
						this.m_subTextObjects[i].sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
					}
				}
			}
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x002856D0 File Offset: 0x00283AD0
		protected override void SetOutlineThickness(float thickness)
		{
			thickness = Mathf.Clamp01(thickness);
			this.m_renderer.material.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.m_renderer.material;
			}
			this.m_fontMaterial = this.m_renderer.material;
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x00285748 File Offset: 0x00283B48
		protected override void SetFaceColor(Color32 color)
		{
			this.m_renderer.material.SetColor(ShaderUtilities.ID_FaceColor, color);
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.m_renderer.material;
			}
			this.m_sharedMaterial = this.m_fontMaterial;
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x002857A0 File Offset: 0x00283BA0
		protected override void SetOutlineColor(Color32 color)
		{
			this.m_renderer.material.SetColor(ShaderUtilities.ID_OutlineColor, color);
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.m_renderer.material;
			}
			this.m_sharedMaterial = this.m_fontMaterial;
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x002857F8 File Offset: 0x00283BF8
		private void CreateMaterialInstance()
		{
			Material material = new Material(this.m_sharedMaterial);
			material.shaderKeywords = this.m_sharedMaterial.shaderKeywords;
			Material material2 = material;
			material2.name += " Instance";
			this.m_fontMaterial = material;
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x00285840 File Offset: 0x00283C40
		protected override void SetShaderDepth()
		{
			if (this.m_isOverlay)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
				this.m_renderer.material.renderQueue = 4000;
				this.m_sharedMaterial = this.m_renderer.material;
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				this.m_renderer.material.renderQueue = -1;
				this.m_sharedMaterial = this.m_renderer.material;
			}
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x002858D0 File Offset: 0x00283CD0
		protected override void SetCulling()
		{
			if (this.m_isCullingEnabled)
			{
				this.m_renderer.material.SetFloat("_CullMode", 2f);
			}
			else
			{
				this.m_renderer.material.SetFloat("_CullMode", 0f);
			}
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x00285921 File Offset: 0x00283D21
		private void SetPerspectiveCorrection()
		{
			if (this.m_isOrthographic)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
			}
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x00285960 File Offset: 0x00283D60
		protected override float GetPaddingForMaterial(Material mat)
		{
			this.m_padding = ShaderUtilities.GetPadding(mat, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x002859B0 File Offset: 0x00283DB0
		protected override float GetPaddingForMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00285A0C File Offset: 0x00283E0C
		private void SetMeshArrays(int size)
		{
			this.m_textInfo.meshInfo[0].ResizeMeshInfo(size);
			this.m_mesh.bounds = this.m_default_bounds;
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x00285A38 File Offset: 0x00283E38
		protected override int SetArraySizes(int[] chars)
		{
			int num = 0;
			int num2 = 0;
			this.m_totalCharacterCount = 0;
			this.m_isUsingBold = false;
			this.m_isParsingText = false;
			this.tag_NoParsing = false;
			this.m_style = this.m_fontStyle;
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			if (this.m_textInfo == null)
			{
				this.m_textInfo = new TMP_TextInfo();
			}
			this.m_textElementType = TMP_TextElementType.Character;
			int num3 = 0;
			while (chars[num3] != 0)
			{
				if (this.m_textInfo.characterInfo == null || this.m_totalCharacterCount >= this.m_textInfo.characterInfo.Length)
				{
					TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, this.m_totalCharacterCount + 1, true);
				}
				int num4 = chars[num3];
				if (!this.m_isRichText || num4 != 60)
				{
					goto IL_1FE;
				}
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!base.ValidateHtmlTag(chars, num3 + 1, out num))
				{
					goto IL_1FE;
				}
				num3 = num;
				if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
				{
					this.m_isUsingBold = true;
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					MaterialReference[] materialReferences = this.m_materialReferences;
					int currentMaterialIndex2 = this.m_currentMaterialIndex;
					materialReferences[currentMaterialIndex2].referenceCount = materialReferences[currentMaterialIndex2].referenceCount + 1;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)(57344 + this.m_spriteIndex);
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_textElementType = TMP_TextElementType.Character;
					this.m_currentMaterialIndex = currentMaterialIndex;
					num2++;
					this.m_totalCharacterCount++;
				}
				IL_7D5:
				num3++;
				continue;
				IL_1FE:
				bool flag = false;
				bool flag2 = false;
				TMP_FontAsset currentFontAsset = this.m_currentFontAsset;
				Material currentMaterial = this.m_currentMaterial;
				int currentMaterialIndex3 = this.m_currentMaterialIndex;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num4))
						{
							num4 = (int)char.ToUpper((char)num4);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num4))
						{
							num4 = (int)char.ToLower((char)num4);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num4))
					{
						num4 = (int)char.ToUpper((char)num4);
					}
				}
				TMP_Glyph tmp_Glyph;
				if (!this.m_currentFontAsset.characterDictionary.TryGetValue(num4, out tmp_Glyph))
				{
					if (this.m_currentFontAsset.fallbackFontAssets != null && this.m_currentFontAsset.fallbackFontAssets.Count > 0)
					{
						for (int i = 0; i < this.m_currentFontAsset.fallbackFontAssets.Count; i++)
						{
							TMP_FontAsset tmp_FontAsset = this.m_currentFontAsset.fallbackFontAssets[i];
							if (!(tmp_FontAsset == null))
							{
								if (tmp_FontAsset.characterDictionary.TryGetValue(num4, out tmp_Glyph))
								{
									flag = true;
									this.m_currentFontAsset = tmp_FontAsset;
									this.m_currentMaterial = tmp_FontAsset.material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									this.m_materialReferences[this.m_currentMaterialIndex].isFallbackFont = true;
									break;
								}
							}
						}
					}
					if (tmp_Glyph == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
					{
						for (int j = 0; j < TMP_Settings.fallbackFontAssets.Count; j++)
						{
							TMP_FontAsset tmp_FontAsset = TMP_Settings.fallbackFontAssets[j];
							if (!(tmp_FontAsset == null))
							{
								if (tmp_FontAsset.characterDictionary.TryGetValue(num4, out tmp_Glyph))
								{
									flag = true;
									this.m_currentFontAsset = tmp_FontAsset;
									this.m_currentMaterial = tmp_FontAsset.material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									break;
								}
							}
						}
					}
					if (tmp_Glyph == null)
					{
						if (char.IsLower((char)num4))
						{
							if (this.m_currentFontAsset.characterDictionary.TryGetValue((int)char.ToUpper((char)num4), out tmp_Glyph))
							{
								num4 = (chars[num3] = (int)char.ToUpper((char)num4));
							}
						}
						else if (char.IsUpper((char)num4) && this.m_currentFontAsset.characterDictionary.TryGetValue((int)char.ToLower((char)num4), out tmp_Glyph))
						{
							num4 = (chars[num3] = (int)char.ToLower((char)num4));
						}
					}
					if (tmp_Glyph == null)
					{
						if (this.m_currentFontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
						{
							if (!TMP_Settings.warningsDisabled)
							{
							}
							num4 = (chars[num3] = 9633);
						}
						else
						{
							if (TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
							{
								for (int k = 0; k < TMP_Settings.fallbackFontAssets.Count; k++)
								{
									TMP_FontAsset tmp_FontAsset = TMP_Settings.fallbackFontAssets[k];
									if (!(tmp_FontAsset == null))
									{
										if (tmp_FontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
										{
											if (!TMP_Settings.warningsDisabled)
											{
											}
											num4 = (chars[num3] = 9633);
											flag = true;
											this.m_currentFontAsset = tmp_FontAsset;
											this.m_currentMaterial = tmp_FontAsset.material;
											this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
											break;
										}
									}
								}
							}
							if (tmp_Glyph == null)
							{
								TMP_FontAsset tmp_FontAsset = TMP_Settings.GetFontAsset();
								if (tmp_FontAsset != null && tmp_FontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
								{
									if (!TMP_Settings.warningsDisabled)
									{
									}
									num4 = (chars[num3] = 9633);
									flag = true;
									this.m_currentFontAsset = tmp_FontAsset;
									this.m_currentMaterial = tmp_FontAsset.material;
									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
								}
								else
								{
									tmp_FontAsset = TMP_FontAsset.defaultFontAsset;
									if (tmp_FontAsset != null && tmp_FontAsset.characterDictionary.TryGetValue(9633, out tmp_Glyph))
									{
										if (!TMP_Settings.warningsDisabled)
										{
										}
										num4 = (chars[num3] = 9633);
										flag = true;
										this.m_currentFontAsset = tmp_FontAsset;
										this.m_currentMaterial = tmp_FontAsset.material;
										this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
									}
								}
							}
						}
					}
				}
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].textElement = tmp_Glyph;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].material = this.m_currentMaterial;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
				if (!char.IsWhiteSpace((char)num4))
				{
					MaterialReference[] materialReferences2 = this.m_materialReferences;
					int currentMaterialIndex4 = this.m_currentMaterialIndex;
					materialReferences2[currentMaterialIndex4].referenceCount = materialReferences2[currentMaterialIndex4].referenceCount + 1;
					if (flag2)
					{
						this.m_currentMaterialIndex = currentMaterialIndex3;
					}
					if (flag)
					{
						this.m_currentFontAsset = currentFontAsset;
						this.m_currentMaterial = currentMaterial;
						this.m_currentMaterialIndex = currentMaterialIndex3;
					}
				}
				this.m_totalCharacterCount++;
				goto IL_7D5;
			}
			this.m_textInfo.spriteCount = num2;
			int num5 = this.m_textInfo.materialCount = this.m_materialReferenceIndexLookup.Count;
			if (num5 > this.m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize<TMP_MeshInfo>(ref this.m_textInfo.meshInfo, num5, false);
			}
			for (int l = 0; l < num5; l++)
			{
				if (l > 0)
				{
					if (this.m_subTextObjects[l] == null)
					{
						this.m_subTextObjects[l] = TMP_SubMesh.AddSubTextObject(this, this.m_materialReferences[l]);
						this.m_textInfo.meshInfo[l].vertices = null;
					}
					if (this.m_materialReferences[l].isFallbackFont)
					{
						Material fallbackMaterial = TMP_MaterialManager.GetFallbackMaterial((l != 1) ? this.m_subTextObjects[l - 1].sharedMaterial : this.m_sharedMaterial, this.m_materialReferences[l].material.mainTexture);
						this.m_materialReferences[l].material = fallbackMaterial;
						this.m_subTextObjects[l].sharedMaterial = fallbackMaterial;
					}
					if (this.m_subTextObjects[l].sharedMaterial == null || this.m_subTextObjects[l].sharedMaterial.GetInstanceID() != this.m_materialReferences[l].material.GetInstanceID())
					{
						bool isDefaultMaterial = this.m_materialReferences[l].isDefaultMaterial;
						this.m_subTextObjects[l].isDefaultMaterial = isDefaultMaterial;
						if (!isDefaultMaterial || this.m_subTextObjects[l].sharedMaterial == null || this.m_subTextObjects[l].sharedMaterial.mainTexture.GetInstanceID() != this.m_materialReferences[l].material.mainTexture.GetInstanceID())
						{
							this.m_subTextObjects[l].sharedMaterial = this.m_materialReferences[l].material;
							this.m_subTextObjects[l].fontAsset = this.m_materialReferences[l].fontAsset;
							this.m_subTextObjects[l].spriteAsset = this.m_materialReferences[l].spriteAsset;
						}
					}
				}
				int referenceCount = this.m_materialReferences[l].referenceCount;
				if (this.m_textInfo.meshInfo[l].vertices == null || this.m_textInfo.meshInfo[l].vertices.Length < referenceCount * 4)
				{
					if (this.m_textInfo.meshInfo[l].vertices == null)
					{
						if (l == 0)
						{
							this.m_textInfo.meshInfo[l] = new TMP_MeshInfo(this.m_mesh, referenceCount + 1);
						}
						else
						{
							this.m_textInfo.meshInfo[l] = new TMP_MeshInfo(this.m_subTextObjects[l].mesh, referenceCount + 1);
						}
					}
					else
					{
						this.m_textInfo.meshInfo[l].ResizeMeshInfo((referenceCount <= 1024) ? Mathf.NextPowerOfTwo(referenceCount) : (referenceCount + 256));
					}
				}
			}
			int num6 = num5;
			while (num6 < this.m_subTextObjects.Length + 1 && this.m_subTextObjects[num6] != null)
			{
				if (num6 < this.m_textInfo.meshInfo.Length)
				{
					this.m_textInfo.meshInfo[num6].ClearUnusedVertices(0, true);
				}
				num6++;
			}
			return this.m_totalCharacterCount;
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x002865F4 File Offset: 0x002849F4
		protected override void ComputeMarginSize()
		{
			if (this.m_textContainer != null)
			{
				Vector4 margins = this.m_textContainer.margins;
				this.m_marginWidth = this.m_textContainer.rect.width - margins.z - margins.x;
				this.m_marginHeight = this.m_textContainer.rect.height - margins.y - margins.w;
			}
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x00286670 File Offset: 0x00284A70
		protected override void OnDidApplyAnimationProperties()
		{
			this.m_havePropertiesChanged = true;
			this.isMaskUpdateRequired = true;
			this.SetVerticesDirty();
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00286686 File Offset: 0x00284A86
		protected override void OnTransformParentChanged()
		{
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x00286694 File Offset: 0x00284A94
		protected override void OnRectTransformDimensionsChange()
		{
			this.ComputeMarginSize();
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x002866A8 File Offset: 0x00284AA8
		private void LateUpdate()
		{
			if (this.m_rectTransform.hasChanged)
			{
				Vector3 lossyScale = this.m_rectTransform.lossyScale;
				if (!this.m_havePropertiesChanged && lossyScale.y != this.m_previousLossyScale.y && this.m_text != string.Empty)
				{
					this.UpdateSDFScale(lossyScale.y);
					this.m_previousLossyScale = lossyScale;
				}
			}
			if (this.m_isUsingLegacyAnimationComponent)
			{
				this.m_havePropertiesChanged = true;
				this.OnPreRenderObject();
			}
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x00286734 File Offset: 0x00284B34
		private void OnPreRenderObject()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.loopCountA = 0;
			if (this.m_transform.hasChanged)
			{
				this.m_transform.hasChanged = false;
				if (this.m_textContainer != null && this.m_textContainer.hasChanged)
				{
					this.ComputeMarginSize();
					this.isMaskUpdateRequired = true;
					this.m_textContainer.hasChanged = false;
					this.m_havePropertiesChanged = true;
				}
			}
			if (this.m_havePropertiesChanged || this.m_isLayoutDirty)
			{
				if (this.isMaskUpdateRequired)
				{
					this.UpdateMask();
					this.isMaskUpdateRequired = false;
				}
				if (this.checkPaddingRequired)
				{
					this.UpdateMeshPadding();
				}
				if (this.m_isInputParsingRequired || this.m_isTextTruncated)
				{
					base.ParseInputText();
				}
				if (this.m_enableAutoSizing)
				{
					this.m_fontSize = Mathf.Clamp(this.m_fontSize, this.m_fontSizeMin, this.m_fontSizeMax);
				}
				this.m_maxFontSize = this.m_fontSizeMax;
				this.m_minFontSize = this.m_fontSizeMin;
				this.m_lineSpacingDelta = 0f;
				this.m_charWidthAdjDelta = 0f;
				this.m_recursiveCount = 0;
				this.m_isCharacterWrappingEnabled = false;
				this.m_isTextTruncated = false;
				this.m_havePropertiesChanged = false;
				this.m_isLayoutDirty = false;
				this.GenerateTextMesh();
			}
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x0028688C File Offset: 0x00284C8C
		protected override void GenerateTextMesh()
		{
			if (this.m_fontAsset == null || this.m_fontAsset.characterDictionary == null)
			{
				return;
			}
			if (this.m_textInfo != null)
			{
				this.m_textInfo.Clear();
			}
			if (this.m_char_buffer == null || this.m_char_buffer.Length == 0 || this.m_char_buffer[0] == 0)
			{
				this.ClearMesh(true);
				this.m_preferredWidth = 0f;
				this.m_preferredHeight = 0f;
				return;
			}
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_currentSpriteAsset = this.m_spriteAsset;
			int totalCharacterCount = this.m_totalCharacterCount;
			this.m_fontScale = this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize * ((!this.m_isOrthographic) ? 0.1f : 1f);
			float num = this.m_fontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
			float num2 = this.m_fontScale;
			this.m_fontScaleMultiplier = 1f;
			this.m_currentFontSize = this.m_fontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_lineJustification = this.m_textAlignment;
			float num3 = 0f;
			float num4 = 1f;
			this.m_baselineOffset = 0f;
			bool flag = false;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			bool flag2 = false;
			Vector3 zero3 = Vector3.zero;
			Vector3 zero4 = Vector3.zero;
			this.m_fontColor32 = this.m_fontColor;
			this.m_htmlColor = this.m_fontColor32;
			this.m_colorStack.SetDefault(this.m_htmlColor);
			this.m_styleStack.Clear();
			this.m_actionStack.Clear();
			this.m_lineOffset = 0f;
			this.m_lineHeight = 0f;
			float num5 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
			this.m_cSpacing = 0f;
			this.m_monoSpacing = 0f;
			this.m_xAdvance = 0f;
			this.tag_LineIndent = 0f;
			this.tag_Indent = 0f;
			this.m_indentStack.SetDefault(0f);
			this.tag_NoParsing = false;
			this.m_characterCount = 0;
			this.m_visibleCharacterCount = 0;
			this.m_firstCharacterOfLine = 0;
			this.m_lastCharacterOfLine = 0;
			this.m_firstVisibleCharacterOfLine = 0;
			this.m_lastVisibleCharacterOfLine = 0;
			this.m_maxLineAscender = float.NegativeInfinity;
			this.m_maxLineDescender = float.PositiveInfinity;
			this.m_lineNumber = 0;
			bool flag3 = true;
			this.m_pageNumber = 0;
			int num6 = Mathf.Clamp(this.m_pageToDisplay - 1, 0, this.m_textInfo.pageInfo.Length - 1);
			int num7 = 0;
			Vector4 margin = this.m_margin;
			float marginWidth = this.m_marginWidth;
			float marginHeight = this.m_marginHeight;
			this.m_marginLeft = 0f;
			this.m_marginRight = 0f;
			this.m_width = -1f;
			this.m_meshExtents.min = TMP_Text.k_InfinityVectorPositive;
			this.m_meshExtents.max = TMP_Text.k_InfinityVectorNegative;
			this.m_textInfo.ClearLineInfo();
			this.m_maxAscender = 0f;
			this.m_maxDescender = 0f;
			float num8 = 0f;
			float num9 = 0f;
			bool flag4 = false;
			this.m_isNewPage = false;
			bool flag5 = true;
			bool flag6 = false;
			int num10 = 0;
			this.loopCountA++;
			int num11 = 0;
			int num12 = 0;
			while (this.m_char_buffer[num12] != 0)
			{
				int num13 = this.m_char_buffer[num12];
				this.m_textElementType = TMP_TextElementType.Character;
				this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
				this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!this.m_isRichText || num13 != 60)
				{
					goto IL_486;
				}
				this.m_isParsingText = true;
				if (!base.ValidateHtmlTag(this.m_char_buffer, num12 + 1, out num11))
				{
					goto IL_486;
				}
				num12 = num11;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_486;
				}
				IL_2882:
				num12++;
				continue;
				IL_486:
				this.m_isParsingText = false;
				bool flag7 = false;
				float num14 = 1f;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num13))
						{
							num13 = (int)char.ToUpper((char)num13);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num13))
						{
							num13 = (int)char.ToLower((char)num13);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num13))
					{
						num14 = 0.8f;
						num13 = (int)char.ToUpper((char)num13);
					}
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
					num13 = 57344 + this.m_spriteIndex;
					this.m_currentFontAsset = this.m_fontAsset;
					float num15 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
					num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num15;
					this.m_cached_TextElement = tmp_Sprite;
					this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
					this.m_textInfo.characterInfo[this.m_characterCount].scale = num15;
					this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset = this.m_currentSpriteAsset;
					this.m_textInfo.characterInfo[this.m_characterCount].fontAsset = this.m_currentFontAsset;
					this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_currentMaterialIndex = currentMaterialIndex;
					num3 = 0f;
				}
				else if (this.m_textElementType == TMP_TextElementType.Character)
				{
					this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
					if (this.m_cached_TextElement == null)
					{
						goto IL_2882;
					}
					this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
					this.m_currentMaterial = this.m_textInfo.characterInfo[this.m_characterCount].material;
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num14 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
					num2 = this.m_fontScale * this.m_fontScaleMultiplier;
					this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
					this.m_textInfo.characterInfo[this.m_characterCount].scale = num2;
					num3 = ((this.m_currentMaterialIndex != 0) ? this.m_subTextObjects[this.m_currentMaterialIndex].padding : this.m_padding);
				}
				if (this.m_isRightToLeft)
				{
					this.m_xAdvance -= ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].character = (char)num13;
				this.m_textInfo.characterInfo[this.m_characterCount].pointSize = this.m_currentFontSize;
				this.m_textInfo.characterInfo[this.m_characterCount].color = this.m_htmlColor;
				this.m_textInfo.characterInfo[this.m_characterCount].style = this.m_style;
				this.m_textInfo.characterInfo[this.m_characterCount].index = (short)num12;
				if (this.m_enableKerning && this.m_characterCount >= 1)
				{
					int character = (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num13);
					KerningPair kerningPair;
					this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
					if (kerningPair != null)
					{
						this.m_xAdvance += kerningPair.XadvanceOffset * num2;
					}
				}
				float num16 = 0f;
				if (this.m_monoSpacing != 0f)
				{
					num16 = (this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2) * (1f - this.m_charWidthAdjDelta);
					this.m_xAdvance += num16;
				}
				float num17;
				if (this.m_textElementType == TMP_TextElementType.Character && ((this.m_style & FontStyles.Bold) == FontStyles.Bold || (this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold))
				{
					num17 = this.m_currentFontAsset.boldStyle * 2f;
					num4 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					num17 = this.m_currentFontAsset.normalStyle * 2f;
					num4 = 1f;
				}
				float baseline = this.m_currentFontAsset.fontInfo.Baseline;
				Vector3 vector = new Vector3(this.m_xAdvance + (this.m_cached_TextElement.xOffset - num3 - num17) * num2 * (1f - this.m_charWidthAdjDelta), (baseline + this.m_cached_TextElement.yOffset + num3) * num2 - this.m_lineOffset + this.m_baselineOffset, 0f);
				Vector3 vector2 = new Vector3(vector.x, vector.y - (this.m_cached_TextElement.height + num3 * 2f) * num2, 0f);
				Vector3 vector3 = new Vector3(vector2.x + (this.m_cached_TextElement.width + num3 * 2f + num17 * 2f) * num2 * (1f - this.m_charWidthAdjDelta), vector.y, 0f);
				Vector3 vector4 = new Vector3(vector3.x, vector2.y, 0f);
				if (this.m_textElementType == TMP_TextElementType.Character && ((this.m_style & FontStyles.Italic) == FontStyles.Italic || (this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic))
				{
					float num18 = (float)this.m_currentFontAsset.italicStyle * 0.01f;
					Vector3 b = new Vector3(num18 * ((this.m_cached_TextElement.yOffset + num3 + num17) * num2), 0f, 0f);
					Vector3 b2 = new Vector3(num18 * ((this.m_cached_TextElement.yOffset - this.m_cached_TextElement.height - num3 - num17) * num2), 0f, 0f);
					vector += b;
					vector2 += b2;
					vector3 += b;
					vector4 += b2;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft = vector2;
				this.m_textInfo.characterInfo[this.m_characterCount].topLeft = vector;
				this.m_textInfo.characterInfo[this.m_characterCount].topRight = vector3;
				this.m_textInfo.characterInfo[this.m_characterCount].bottomRight = vector4;
				this.m_textInfo.characterInfo[this.m_characterCount].origin = this.m_xAdvance;
				this.m_textInfo.characterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
				this.m_textInfo.characterInfo[this.m_characterCount].aspectRatio = (vector3.x - vector2.x) / (vector.y - vector2.y);
				float num19 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : num2) + this.m_baselineOffset;
				this.m_textInfo.characterInfo[this.m_characterCount].ascender = num19 - this.m_lineOffset;
				this.m_maxLineAscender = ((num19 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num19);
				float num20 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : num2) + this.m_baselineOffset;
				float num21 = this.m_textInfo.characterInfo[this.m_characterCount].descender = num20 - this.m_lineOffset;
				this.m_maxLineDescender = ((num20 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num20);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num22 = (num19 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num19 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num22 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num22);
					float num23 = (num20 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num20 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num23 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num23);
				}
				if (this.m_lineNumber == 0)
				{
					this.m_maxAscender = ((this.m_maxAscender <= num19) ? num19 : this.m_maxAscender);
				}
				if (this.m_lineOffset == 0f)
				{
					num8 = ((num8 <= num19) ? num19 : num8);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
				if (num13 == 9 || !char.IsWhiteSpace((char)num13) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					this.m_textInfo.characterInfo[this.m_characterCount].isVisible = true;
					float num24 = (this.m_width == -1f) ? (marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
					this.m_textInfo.lineInfo[this.m_lineNumber].width = num24;
					this.m_textInfo.lineInfo[this.m_lineNumber].marginLeft = this.m_marginLeft;
					if (Mathf.Abs(this.m_xAdvance) + (this.m_isRightToLeft ? 0f : this.m_cached_TextElement.xAdvance) * (1f - this.m_charWidthAdjDelta) * num2 > num24)
					{
						num7 = this.m_characterCount - 1;
						if (base.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
						{
							if (num10 == this.m_SavedWordWrapState.previous_WordBreak || flag5)
							{
								if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
								{
									if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
									{
										this.loopCountA = 0;
										this.m_charWidthAdjDelta += 0.01f;
										this.GenerateTextMesh();
										return;
									}
									this.m_maxFontSize = this.m_fontSize;
									this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
									this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
									if (this.loopCountA > 20)
									{
										return;
									}
									this.GenerateTextMesh();
									return;
								}
								else
								{
									if (!this.m_isCharacterWrappingEnabled)
									{
										this.m_isCharacterWrappingEnabled = true;
									}
									else
									{
										flag6 = true;
									}
									this.m_recursiveCount++;
									if (this.m_recursiveCount > 20)
									{
										goto IL_2882;
									}
								}
							}
							num12 = base.RestoreWordWrappingState(ref this.m_SavedWordWrapState);
							num10 = num12;
							if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
							{
								float num25 = this.m_maxLineAscender - this.m_startOfLineAscender;
								this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num25);
								this.m_lineOffset += num25;
								this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
								this.m_SavedWordWrapState.previousLineAscender = this.m_maxLineAscender;
							}
							this.m_isNewPage = false;
							float num26 = this.m_maxLineAscender - this.m_lineOffset;
							float num27 = this.m_maxLineDescender - this.m_lineOffset;
							this.m_maxDescender = ((this.m_maxDescender >= num27) ? num27 : this.m_maxDescender);
							if (!flag4)
							{
								num9 = this.m_maxDescender;
							}
							if (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines)
							{
								flag4 = true;
							}
							this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = ((this.m_characterCount - 1 <= 0) ? 0 : (this.m_characterCount - 1));
							this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = this.m_lastVisibleCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
							this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num27);
							this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num26);
							this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x;
							this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2;
							this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
							this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num26;
							this.m_textInfo.lineInfo[this.m_lineNumber].descender = num27;
							this.m_firstCharacterOfLine = this.m_characterCount;
							base.SaveWordWrappingState(ref this.m_SavedLineState, num12, this.m_characterCount - 1);
							this.m_lineNumber++;
							flag3 = true;
							if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
							{
								base.ResizeLineExtents(this.m_lineNumber);
							}
							if (this.m_lineHeight == 0f)
							{
								float num28 = this.m_textInfo.characterInfo[this.m_characterCount].ascender - this.m_textInfo.characterInfo[this.m_characterCount].baseLine;
								float num29 = 0f - this.m_maxLineDescender + num28 + (num5 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
								this.m_lineOffset += num29;
								this.m_startOfLineAscender = num28;
							}
							else
							{
								this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
							}
							this.m_maxLineAscender = float.NegativeInfinity;
							this.m_maxLineDescender = float.PositiveInfinity;
							this.m_xAdvance = this.tag_Indent;
							goto IL_2882;
						}
						if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
						{
							if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
							{
								this.loopCountA = 0;
								this.m_charWidthAdjDelta += 0.01f;
								this.GenerateTextMesh();
								return;
							}
							this.m_maxFontSize = this.m_fontSize;
							this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
							this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
							this.m_recursiveCount = 0;
							if (this.loopCountA > 20)
							{
								return;
							}
							this.GenerateTextMesh();
							return;
						}
						else
						{
							switch (this.m_overflowMode)
							{
							case TextOverflowModes.Overflow:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								break;
							case TextOverflowModes.Ellipsis:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								this.m_isTextTruncated = true;
								if (this.m_characterCount >= 1)
								{
									this.m_char_buffer[num12 - 1] = 8230;
									this.m_char_buffer[num12] = 0;
									if (this.m_cached_Ellipsis_GlyphInfo != null)
									{
										this.m_textInfo.characterInfo[num7].character = '…';
										this.m_textInfo.characterInfo[num7].textElement = this.m_cached_Ellipsis_GlyphInfo;
										this.m_textInfo.characterInfo[num7].fontAsset = this.m_materialReferences[0].fontAsset;
										this.m_textInfo.characterInfo[num7].material = this.m_materialReferences[0].material;
										this.m_textInfo.characterInfo[num7].materialReferenceIndex = 0;
									}
									this.m_totalCharacterCount = num7 + 1;
									this.GenerateTextMesh();
									return;
								}
								this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
								this.m_visibleCharacterCount = 0;
								break;
							case TextOverflowModes.Masking:
								if (!this.m_isMaskingEnabled)
								{
									this.EnableMasking();
								}
								break;
							case TextOverflowModes.Truncate:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
								break;
							case TextOverflowModes.ScrollRect:
								if (!this.m_isMaskingEnabled)
								{
									this.EnableMasking();
								}
								break;
							}
						}
					}
					if (num13 != 9)
					{
						Color32 vertexColor;
						if (flag7)
						{
							vertexColor = Color.red;
						}
						else if (this.m_overrideHtmlColors)
						{
							vertexColor = this.m_fontColor32;
						}
						else
						{
							vertexColor = this.m_htmlColor;
						}
						if (this.m_textElementType == TMP_TextElementType.Character)
						{
							this.SaveGlyphVertexInfo(num3, num17, vertexColor);
						}
						else if (this.m_textElementType == TMP_TextElementType.Sprite)
						{
							this.SaveSpriteVertexInfo(vertexColor);
						}
					}
					else
					{
						this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
						this.m_lastVisibleCharacterOfLine = this.m_characterCount;
						TMP_LineInfo[] lineInfo = this.m_textInfo.lineInfo;
						int lineNumber = this.m_lineNumber;
						lineInfo[lineNumber].spaceCount = lineInfo[lineNumber].spaceCount + 1;
						this.m_textInfo.spaceCount++;
					}
					if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
					{
						if (flag3)
						{
							flag3 = false;
							this.m_firstVisibleCharacterOfLine = this.m_characterCount;
						}
						this.m_visibleCharacterCount++;
						this.m_lastVisibleCharacterOfLine = this.m_characterCount;
					}
				}
				else if (num13 == 10 || char.IsSeparator((char)num13))
				{
					TMP_LineInfo[] lineInfo2 = this.m_textInfo.lineInfo;
					int lineNumber2 = this.m_lineNumber;
					lineInfo2[lineNumber2].spaceCount = lineInfo2[lineNumber2].spaceCount + 1;
					this.m_textInfo.spaceCount++;
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
				{
					float num30 = this.m_maxLineAscender - this.m_startOfLineAscender;
					this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num30);
					num21 -= num30;
					this.m_lineOffset += num30;
					this.m_startOfLineAscender += num30;
					this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
					this.m_SavedWordWrapState.previousLineAscender = this.m_startOfLineAscender;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].lineNumber = (short)this.m_lineNumber;
				this.m_textInfo.characterInfo[this.m_characterCount].pageNumber = (short)this.m_pageNumber;
				if ((num13 != 10 && num13 != 13 && num13 != 8230) || this.m_textInfo.lineInfo[this.m_lineNumber].characterCount == 1)
				{
					this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
				}
				if (this.m_maxAscender - num21 > marginHeight + 0.0001f)
				{
					if (this.m_enableAutoSizing && this.m_lineSpacingDelta > this.m_lineSpacingMax && this.m_lineNumber > 0)
					{
						this.m_lineSpacingDelta -= 1f;
						this.GenerateTextMesh();
						return;
					}
					if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
					{
						this.m_maxFontSize = this.m_fontSize;
						this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
						this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
						this.m_recursiveCount = 0;
						if (this.loopCountA > 20)
						{
							return;
						}
						this.GenerateTextMesh();
						return;
					}
					else
					{
						switch (this.m_overflowMode)
						{
						case TextOverflowModes.Overflow:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							break;
						case TextOverflowModes.Ellipsis:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[(int)this.m_textInfo.characterInfo[num7].index] = 8230;
								this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num7].index + 1)] = 0;
								if (this.m_cached_Ellipsis_GlyphInfo != null)
								{
									this.m_textInfo.characterInfo[num7].character = '…';
									this.m_textInfo.characterInfo[num7].textElement = this.m_cached_Ellipsis_GlyphInfo;
									this.m_textInfo.characterInfo[num7].fontAsset = this.m_materialReferences[0].fontAsset;
									this.m_textInfo.characterInfo[num7].material = this.m_materialReferences[0].material;
									this.m_textInfo.characterInfo[num7].materialReferenceIndex = 0;
								}
								this.m_totalCharacterCount = num7 + 1;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh(false);
							return;
						case TextOverflowModes.Masking:
							if (!this.m_isMaskingEnabled)
							{
								this.EnableMasking();
							}
							break;
						case TextOverflowModes.Truncate:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num7].index + 1)] = 0;
								this.m_totalCharacterCount = num7 + 1;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh(false);
							return;
						case TextOverflowModes.ScrollRect:
							if (!this.m_isMaskingEnabled)
							{
								this.EnableMasking();
							}
							break;
						case TextOverflowModes.Page:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (num13 != 13 && num13 != 10)
							{
								num12 = base.RestoreWordWrappingState(ref this.m_SavedLineState);
								if (num12 == 0)
								{
									this.ClearMesh(false);
									return;
								}
								this.m_isNewPage = true;
								this.m_xAdvance = this.tag_Indent;
								this.m_lineOffset = 0f;
								this.m_lineNumber++;
								this.m_pageNumber++;
								goto IL_2882;
							}
							break;
						}
					}
				}
				if (num13 == 9)
				{
					this.m_xAdvance += this.m_currentFontAsset.fontInfo.TabWidth * num2;
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += (this.m_monoSpacing - num16 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
				}
				else if (!this.m_isRightToLeft)
				{
					this.m_xAdvance += ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].xAdvance = this.m_xAdvance;
				if (num13 == 13)
				{
					this.m_xAdvance = this.tag_Indent;
				}
				if (num13 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
					{
						float num31 = this.m_maxLineAscender - this.m_startOfLineAscender;
						this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num31);
						num21 -= num31;
						this.m_lineOffset += num31;
					}
					this.m_isNewPage = false;
					float num32 = this.m_maxLineAscender - this.m_lineOffset;
					float num33 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender >= num33) ? num33 : this.m_maxDescender);
					if (!flag4)
					{
						num9 = this.m_maxDescender;
					}
					if (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines)
					{
						flag4 = true;
					}
					this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
					this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
					this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = this.m_characterCount;
					this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = ((this.m_lastVisibleCharacterOfLine < this.m_firstVisibleCharacterOfLine) ? this.m_firstVisibleCharacterOfLine : this.m_lastVisibleCharacterOfLine);
					this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
					this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num33);
					this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num32);
					this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x - num3 * num2;
					this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2;
					this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
					this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num32;
					this.m_textInfo.lineInfo[this.m_lineNumber].descender = num33;
					this.m_firstCharacterOfLine = this.m_characterCount + 1;
					if (num13 == 10)
					{
						base.SaveWordWrappingState(ref this.m_SavedLineState, num12, this.m_characterCount);
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
						this.m_lineNumber++;
						flag3 = true;
						if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
						{
							base.ResizeLineExtents(this.m_lineNumber);
						}
						if (this.m_lineHeight == 0f)
						{
							float num29 = 0f - this.m_maxLineDescender + num19 + (num5 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num29;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
						}
						this.m_maxLineAscender = float.NegativeInfinity;
						this.m_maxLineDescender = float.PositiveInfinity;
						this.m_startOfLineAscender = num19;
						this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
						num7 = this.m_characterCount - 1;
						this.m_characterCount++;
						goto IL_2882;
					}
				}
				if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
				{
					this.m_meshExtents.min.x = Mathf.Min(this.m_meshExtents.min.x, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.x);
					this.m_meshExtents.min.y = Mathf.Min(this.m_meshExtents.min.y, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.y);
					this.m_meshExtents.max.x = Mathf.Max(this.m_meshExtents.max.x, this.m_textInfo.characterInfo[this.m_characterCount].topRight.x);
					this.m_meshExtents.max.y = Mathf.Max(this.m_meshExtents.max.y, this.m_textInfo.characterInfo[this.m_characterCount].topRight.y);
				}
				if (this.m_overflowMode == TextOverflowModes.Page && num13 != 13 && num13 != 10 && this.m_pageNumber < 16)
				{
					this.m_textInfo.pageInfo[this.m_pageNumber].ascender = num8;
					this.m_textInfo.pageInfo[this.m_pageNumber].descender = ((num20 >= this.m_textInfo.pageInfo[this.m_pageNumber].descender) ? this.m_textInfo.pageInfo[this.m_pageNumber].descender : num20);
					if (this.m_pageNumber == 0 && this.m_characterCount == 0)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
					}
					else if (this.m_characterCount > 0 && this.m_pageNumber != (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].pageNumber)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber - 1].lastCharacterIndex = this.m_characterCount - 1;
						this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
					}
					else if (this.m_characterCount == totalCharacterCount - 1)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber].lastCharacterIndex = this.m_characterCount;
					}
				}
				if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate || this.m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if (char.IsWhiteSpace((char)num13) && !this.m_isNonBreakingSpace)
					{
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
						this.m_isCharacterWrappingEnabled = false;
						flag5 = false;
					}
					else if (num13 > 11904 && num13 < 40959 && !this.m_isNonBreakingSpace)
					{
						if (!this.m_currentFontAsset.lineBreakingInfo.leadingCharacters.ContainsKey(num13) && this.m_characterCount < totalCharacterCount - 1 && !this.m_currentFontAsset.lineBreakingInfo.followingCharacters.ContainsKey((int)this.m_textInfo.characterInfo[this.m_characterCount + 1].character))
						{
							base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
							this.m_isCharacterWrappingEnabled = false;
							flag5 = false;
						}
					}
					else if (flag5 || this.m_isCharacterWrappingEnabled || flag6)
					{
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num12, this.m_characterCount);
					}
				}
				this.m_characterCount++;
				goto IL_2882;
			}
			float num34 = this.m_maxFontSize - this.m_minFontSize;
			if ((!this.m_textContainer.isDefaultWidth || !this.m_textContainer.isDefaultHeight) && !this.m_isCharacterWrappingEnabled && this.m_enableAutoSizing && num34 > 0.051f && this.m_fontSize < this.m_fontSizeMax)
			{
				this.m_minFontSize = this.m_fontSize;
				this.m_fontSize += Mathf.Max((this.m_maxFontSize - this.m_fontSize) / 2f, 0.05f);
				this.m_fontSize = (float)((int)(Mathf.Min(this.m_fontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
				if (this.loopCountA > 20)
				{
					return;
				}
				this.GenerateTextMesh();
				return;
			}
			else
			{
				this.m_isCharacterWrappingEnabled = false;
				if (this.m_visibleCharacterCount == 0 && this.m_visibleSpriteCount == 0)
				{
					this.ClearMesh(true);
					return;
				}
				int num35 = this.m_materialReferences[0].referenceCount * 4;
				this.m_textInfo.meshInfo[0].Clear(false);
				Vector3 a = Vector3.zero;
				Vector3[] textContainerLocalCorners = this.GetTextContainerLocalCorners();
				switch (this.m_textAlignment)
				{
				case TextAlignmentOptions.TopLeft:
				case TextAlignmentOptions.Top:
				case TextAlignmentOptions.TopRight:
				case TextAlignmentOptions.TopJustified:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						a = textContainerLocalCorners[1] + new Vector3(margin.x, 0f - this.m_maxAscender - margin.y, 0f);
					}
					else
					{
						a = textContainerLocalCorners[1] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].ascender - margin.y, 0f);
					}
					break;
				case TextAlignmentOptions.Left:
				case TextAlignmentOptions.Center:
				case TextAlignmentOptions.Right:
				case TextAlignmentOptions.Justified:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						a = (textContainerLocalCorners[0] + textContainerLocalCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxAscender + margin.y + num9 - margin.w) / 2f, 0f);
					}
					else
					{
						a = (textContainerLocalCorners[0] + textContainerLocalCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_textInfo.pageInfo[num6].ascender + margin.y + this.m_textInfo.pageInfo[num6].descender - margin.w) / 2f, 0f);
					}
					break;
				case TextAlignmentOptions.BottomLeft:
				case TextAlignmentOptions.Bottom:
				case TextAlignmentOptions.BottomRight:
				case TextAlignmentOptions.BottomJustified:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						a = textContainerLocalCorners[0] + new Vector3(margin.x, 0f - num9 + margin.w, 0f);
					}
					else
					{
						a = textContainerLocalCorners[0] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].descender + margin.w, 0f);
					}
					break;
				case TextAlignmentOptions.BaselineLeft:
				case TextAlignmentOptions.Baseline:
				case TextAlignmentOptions.BaselineRight:
				case TextAlignmentOptions.BaselineJustified:
					a = (textContainerLocalCorners[0] + textContainerLocalCorners[1]) / 2f + new Vector3(margin.x, 0f, 0f);
					break;
				case TextAlignmentOptions.MidlineLeft:
				case TextAlignmentOptions.Midline:
				case TextAlignmentOptions.MidlineRight:
				case TextAlignmentOptions.MidlineJustified:
					a = (textContainerLocalCorners[0] + textContainerLocalCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_meshExtents.max.y + margin.y + this.m_meshExtents.min.y - margin.w) / 2f, 0f);
					break;
				}
				Vector3 vector5 = Vector3.zero;
				Vector3 b3 = Vector3.zero;
				int index_X = 0;
				int index_X2 = 0;
				int num36 = 0;
				int num37 = 0;
				int num38 = 0;
				bool flag8 = false;
				int num39 = 0;
				Color32 underlineColor = Color.white;
				Color32 underlineColor2 = Color.white;
				float num40 = 0f;
				float num41 = 0f;
				float num42 = float.PositiveInfinity;
				int num43 = 0;
				float num44 = 0f;
				float num45 = 0f;
				float b4 = 0f;
				float y = this.m_transform.lossyScale.y;
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				for (int i = 0; i < this.m_characterCount; i++)
				{
					char character2 = characterInfo[i].character;
					int lineNumber3 = (int)characterInfo[i].lineNumber;
					TMP_LineInfo tmp_LineInfo = this.m_textInfo.lineInfo[lineNumber3];
					num37 = lineNumber3 + 1;
					switch (tmp_LineInfo.alignment)
					{
					case TextAlignmentOptions.TopLeft:
					case TextAlignmentOptions.Left:
					case TextAlignmentOptions.BottomLeft:
					case TextAlignmentOptions.BaselineLeft:
					case TextAlignmentOptions.MidlineLeft:
						if (!this.m_isRightToLeft)
						{
							vector5 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
						}
						else
						{
							vector5 = new Vector3(0f - tmp_LineInfo.maxAdvance, 0f, 0f);
						}
						break;
					case TextAlignmentOptions.Top:
					case TextAlignmentOptions.Center:
					case TextAlignmentOptions.Bottom:
					case TextAlignmentOptions.Baseline:
					case TextAlignmentOptions.Midline:
						vector5 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - tmp_LineInfo.maxAdvance / 2f, 0f, 0f);
						break;
					case TextAlignmentOptions.TopRight:
					case TextAlignmentOptions.Right:
					case TextAlignmentOptions.BottomRight:
					case TextAlignmentOptions.BaselineRight:
					case TextAlignmentOptions.MidlineRight:
						if (!this.m_isRightToLeft)
						{
							vector5 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width - tmp_LineInfo.maxAdvance, 0f, 0f);
						}
						else
						{
							vector5 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
						}
						break;
					case TextAlignmentOptions.TopJustified:
					case TextAlignmentOptions.Justified:
					case TextAlignmentOptions.BottomJustified:
					case TextAlignmentOptions.BaselineJustified:
					case TextAlignmentOptions.MidlineJustified:
					{
						char character3 = characterInfo[tmp_LineInfo.lastCharacterIndex].character;
						if (!char.IsControl(character3) && lineNumber3 < this.m_lineNumber)
						{
							float num46 = tmp_LineInfo.width - tmp_LineInfo.maxAdvance;
							float num47 = (tmp_LineInfo.spaceCount <= 2) ? 1f : this.m_wordWrappingRatios;
							if (lineNumber3 != num38 || i == 0)
							{
								vector5 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
							}
							else if (character2 == '\t' || char.IsSeparator(character2))
							{
								int num48 = (tmp_LineInfo.spaceCount - 1 <= 0) ? 1 : (tmp_LineInfo.spaceCount - 1);
								vector5 += new Vector3(num46 * (1f - num47) / (float)num48, 0f, 0f);
							}
							else
							{
								vector5 += new Vector3(num46 * num47 / (float)(tmp_LineInfo.characterCount - tmp_LineInfo.spaceCount - 1), 0f, 0f);
							}
						}
						else
						{
							vector5 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
						}
						break;
					}
					}
					b3 = a + vector5;
					bool isVisible = characterInfo[i].isVisible;
					if (isVisible)
					{
						TMP_TextElementType elementType = characterInfo[i].elementType;
						if (elementType != TMP_TextElementType.Character)
						{
							if (elementType != TMP_TextElementType.Sprite)
							{
							}
						}
						else
						{
							Extents lineExtents = tmp_LineInfo.lineExtents;
							float num49 = this.m_uvLineOffset * (float)lineNumber3 % 1f + this.m_uvOffset.x;
							switch (this.m_horizontalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[i].vertex_BL.uv2.x = this.m_uvOffset.x;
								characterInfo[i].vertex_TL.uv2.x = this.m_uvOffset.x;
								characterInfo[i].vertex_TR.uv2.x = 1f + this.m_uvOffset.x;
								characterInfo[i].vertex_BR.uv2.x = 1f + this.m_uvOffset.x;
								break;
							case TextureMappingOptions.Line:
								if (this.m_textAlignment != TextAlignmentOptions.Justified)
								{
									characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num49;
									characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num49;
									characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num49;
									characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num49;
								}
								else
								{
									characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
									characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
									characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
									characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
								}
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
								characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
								characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
								characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x + vector5.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num49;
								break;
							case TextureMappingOptions.MatchAspect:
							{
								switch (this.m_verticalMapping)
								{
								case TextureMappingOptions.Character:
									characterInfo[i].vertex_BL.uv2.y = this.m_uvOffset.y;
									characterInfo[i].vertex_TL.uv2.y = 1f + this.m_uvOffset.y;
									characterInfo[i].vertex_TR.uv2.y = this.m_uvOffset.y;
									characterInfo[i].vertex_BR.uv2.y = 1f + this.m_uvOffset.y;
									break;
								case TextureMappingOptions.Line:
									characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num49;
									characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num49;
									characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
									characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
									break;
								case TextureMappingOptions.Paragraph:
									characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num49;
									characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num49;
									characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
									characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
									break;
								}
								float num50 = (1f - (characterInfo[i].vertex_BL.uv2.y + characterInfo[i].vertex_TL.uv2.y) * characterInfo[i].aspectRatio) / 2f;
								characterInfo[i].vertex_BL.uv2.x = characterInfo[i].vertex_BL.uv2.y * characterInfo[i].aspectRatio + num50 + num49;
								characterInfo[i].vertex_TL.uv2.x = characterInfo[i].vertex_BL.uv2.x;
								characterInfo[i].vertex_TR.uv2.x = characterInfo[i].vertex_TL.uv2.y * characterInfo[i].aspectRatio + num50 + num49;
								characterInfo[i].vertex_BR.uv2.x = characterInfo[i].vertex_TR.uv2.x;
								break;
							}
							}
							switch (this.m_verticalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[i].vertex_BL.uv2.y = this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = 1f + this.m_uvOffset.y;
								characterInfo[i].vertex_TR.uv2.y = 1f + this.m_uvOffset.y;
								characterInfo[i].vertex_BR.uv2.y = this.m_uvOffset.y;
								break;
							case TextureMappingOptions.Line:
								characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + this.m_uvOffset.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + this.m_uvOffset.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								break;
							case TextureMappingOptions.MatchAspect:
							{
								float num51 = (1f - (characterInfo[i].vertex_BL.uv2.x + characterInfo[i].vertex_TR.uv2.x) / characterInfo[i].aspectRatio) / 2f;
								characterInfo[i].vertex_BL.uv2.y = num51 + characterInfo[i].vertex_BL.uv2.x / characterInfo[i].aspectRatio + this.m_uvOffset.y;
								characterInfo[i].vertex_TL.uv2.y = num51 + characterInfo[i].vertex_TR.uv2.x / characterInfo[i].aspectRatio + this.m_uvOffset.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								break;
							}
							}
							float num52 = this.m_textInfo.characterInfo[i].scale * y * (1f - this.m_charWidthAdjDelta);
							if ((this.m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
							{
								num52 *= -1f;
							}
							float num53 = characterInfo[i].vertex_BL.uv2.x;
							float num54 = characterInfo[i].vertex_BL.uv2.y;
							float num55 = characterInfo[i].vertex_TR.uv2.x;
							float num56 = characterInfo[i].vertex_TR.uv2.y;
							float num57 = Mathf.Floor(num53);
							float num58 = Mathf.Floor(num54);
							num53 -= num57;
							num55 -= num57;
							num54 -= num58;
							num56 -= num58;
							characterInfo[i].vertex_BL.uv2.x = base.PackUV(num53, num54);
							characterInfo[i].vertex_BL.uv2.y = num52;
							characterInfo[i].vertex_TL.uv2.x = base.PackUV(num53, num56);
							characterInfo[i].vertex_TL.uv2.y = num52;
							characterInfo[i].vertex_TR.uv2.x = base.PackUV(num55, num56);
							characterInfo[i].vertex_TR.uv2.y = num52;
							characterInfo[i].vertex_BR.uv2.x = base.PackUV(num55, num54);
							characterInfo[i].vertex_BR.uv2.y = num52;
						}
						if (i < this.m_maxVisibleCharacters && lineNumber3 < this.m_maxVisibleLines && this.m_overflowMode != TextOverflowModes.Page)
						{
							TMP_CharacterInfo[] array = characterInfo;
							int num59 = i;
							array[num59].vertex_BL.position = array[num59].vertex_BL.position + b3;
							TMP_CharacterInfo[] array2 = characterInfo;
							int num60 = i;
							array2[num60].vertex_TL.position = array2[num60].vertex_TL.position + b3;
							TMP_CharacterInfo[] array3 = characterInfo;
							int num61 = i;
							array3[num61].vertex_TR.position = array3[num61].vertex_TR.position + b3;
							TMP_CharacterInfo[] array4 = characterInfo;
							int num62 = i;
							array4[num62].vertex_BR.position = array4[num62].vertex_BR.position + b3;
						}
						else if (i < this.m_maxVisibleCharacters && lineNumber3 < this.m_maxVisibleLines && this.m_overflowMode == TextOverflowModes.Page && (int)characterInfo[i].pageNumber == num6)
						{
							TMP_CharacterInfo[] array5 = characterInfo;
							int num63 = i;
							array5[num63].vertex_BL.position = array5[num63].vertex_BL.position + b3;
							TMP_CharacterInfo[] array6 = characterInfo;
							int num64 = i;
							array6[num64].vertex_TL.position = array6[num64].vertex_TL.position + b3;
							TMP_CharacterInfo[] array7 = characterInfo;
							int num65 = i;
							array7[num65].vertex_TR.position = array7[num65].vertex_TR.position + b3;
							TMP_CharacterInfo[] array8 = characterInfo;
							int num66 = i;
							array8[num66].vertex_BR.position = array8[num66].vertex_BR.position + b3;
						}
						else
						{
							characterInfo[i].vertex_BL.position = Vector3.zero;
							characterInfo[i].vertex_TL.position = Vector3.zero;
							characterInfo[i].vertex_TR.position = Vector3.zero;
							characterInfo[i].vertex_BR.position = Vector3.zero;
						}
						if (elementType == TMP_TextElementType.Character)
						{
							this.FillCharacterVertexBuffers(i, index_X);
						}
						else if (elementType == TMP_TextElementType.Sprite)
						{
							this.FillSpriteVertexBuffers(i, index_X2);
						}
					}
					TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
					int num67 = i;
					characterInfo2[num67].bottomLeft = characterInfo2[num67].bottomLeft + b3;
					TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
					int num68 = i;
					characterInfo3[num68].topLeft = characterInfo3[num68].topLeft + b3;
					TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
					int num69 = i;
					characterInfo4[num69].topRight = characterInfo4[num69].topRight + b3;
					TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
					int num70 = i;
					characterInfo5[num70].bottomRight = characterInfo5[num70].bottomRight + b3;
					TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
					int num71 = i;
					characterInfo6[num71].origin = characterInfo6[num71].origin + b3.x;
					TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
					int num72 = i;
					characterInfo7[num72].xAdvance = characterInfo7[num72].xAdvance + b3.x;
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num73 = i;
					characterInfo8[num73].ascender = characterInfo8[num73].ascender + b3.y;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num74 = i;
					characterInfo9[num74].descender = characterInfo9[num74].descender + b3.y;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num75 = i;
					characterInfo10[num75].baseLine = characterInfo10[num75].baseLine + b3.y;
					if (isVisible)
					{
					}
					if (lineNumber3 != num38 || i == this.m_characterCount - 1)
					{
						if (lineNumber3 != num38)
						{
							this.m_textInfo.lineInfo[num38].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num38].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[num38].descender);
							this.m_textInfo.lineInfo[num38].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num38].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[num38].ascender);
							TMP_LineInfo[] lineInfo3 = this.m_textInfo.lineInfo;
							int num76 = num38;
							lineInfo3[num76].baseline = lineInfo3[num76].baseline + b3.y;
							TMP_LineInfo[] lineInfo4 = this.m_textInfo.lineInfo;
							int num77 = num38;
							lineInfo4[num77].ascender = lineInfo4[num77].ascender + b3.y;
							TMP_LineInfo[] lineInfo5 = this.m_textInfo.lineInfo;
							int num78 = num38;
							lineInfo5[num78].descender = lineInfo5[num78].descender + b3.y;
						}
						if (i == this.m_characterCount - 1)
						{
							this.m_textInfo.lineInfo[lineNumber3].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber3].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[lineNumber3].descender);
							this.m_textInfo.lineInfo[lineNumber3].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber3].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[lineNumber3].ascender);
							TMP_LineInfo[] lineInfo6 = this.m_textInfo.lineInfo;
							int num79 = lineNumber3;
							lineInfo6[num79].baseline = lineInfo6[num79].baseline + b3.y;
							TMP_LineInfo[] lineInfo7 = this.m_textInfo.lineInfo;
							int num80 = lineNumber3;
							lineInfo7[num80].ascender = lineInfo7[num80].ascender + b3.y;
							TMP_LineInfo[] lineInfo8 = this.m_textInfo.lineInfo;
							int num81 = lineNumber3;
							lineInfo8[num81].descender = lineInfo8[num81].descender + b3.y;
						}
					}
					if (char.IsLetterOrDigit(character2) || character2 == '-' || character2 == '’')
					{
						if (!flag8)
						{
							flag8 = true;
							num39 = i;
						}
						if (flag8 && i == this.m_characterCount - 1)
						{
							int num82 = this.m_textInfo.wordInfo.Length;
							int wordCount = this.m_textInfo.wordCount;
							if (this.m_textInfo.wordCount + 1 > num82)
							{
								TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num82 + 1);
							}
							int num83 = i;
							this.m_textInfo.wordInfo[wordCount].firstCharacterIndex = num39;
							this.m_textInfo.wordInfo[wordCount].lastCharacterIndex = num83;
							this.m_textInfo.wordInfo[wordCount].characterCount = num83 - num39 + 1;
							this.m_textInfo.wordInfo[wordCount].textComponent = this;
							num36++;
							this.m_textInfo.wordCount++;
							TMP_LineInfo[] lineInfo9 = this.m_textInfo.lineInfo;
							int num84 = lineNumber3;
							lineInfo9[num84].wordCount = lineInfo9[num84].wordCount + 1;
						}
					}
					else if (flag8 || (i == 0 && (!char.IsPunctuation(character2) || char.IsWhiteSpace(character2) || i == this.m_characterCount - 1)))
					{
						if (i <= 0 || i >= this.m_characterCount || character2 != '\'' || !char.IsLetterOrDigit(characterInfo[i - 1].character) || !char.IsLetterOrDigit(characterInfo[i + 1].character))
						{
							int num83 = (i != this.m_characterCount - 1 || !char.IsLetterOrDigit(character2)) ? (i - 1) : i;
							flag8 = false;
							int num85 = this.m_textInfo.wordInfo.Length;
							int wordCount2 = this.m_textInfo.wordCount;
							if (this.m_textInfo.wordCount + 1 > num85)
							{
								TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num85 + 1);
							}
							this.m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num39;
							this.m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num83;
							this.m_textInfo.wordInfo[wordCount2].characterCount = num83 - num39 + 1;
							this.m_textInfo.wordInfo[wordCount2].textComponent = this;
							num36++;
							this.m_textInfo.wordCount++;
							TMP_LineInfo[] lineInfo10 = this.m_textInfo.lineInfo;
							int num86 = lineNumber3;
							lineInfo10[num86].wordCount = lineInfo10[num86].wordCount + 1;
						}
					}
					bool flag9 = (this.m_textInfo.characterInfo[i].style & FontStyles.Underline) == FontStyles.Underline;
					if (flag9)
					{
						bool flag10 = true;
						int pageNumber = (int)this.m_textInfo.characterInfo[i].pageNumber;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && pageNumber + 1 != this.m_pageToDisplay))
						{
							flag10 = false;
						}
						if (!char.IsWhiteSpace(character2))
						{
							num41 = Mathf.Max(num41, this.m_textInfo.characterInfo[i].scale);
							num42 = Mathf.Min((pageNumber != num43) ? float.PositiveInfinity : num42, this.m_textInfo.characterInfo[i].baseLine + base.font.fontInfo.Underline * num41);
							num43 = pageNumber;
						}
						if (!flag && flag10 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
							{
								flag = true;
								num40 = this.m_textInfo.characterInfo[i].scale;
								if (num41 == 0f)
								{
									num41 = num40;
								}
								zero = new Vector3(this.m_textInfo.characterInfo[i].bottomLeft.x, num42, 0f);
								underlineColor = this.m_textInfo.characterInfo[i].color;
							}
						}
						if (flag && this.m_characterCount == 1)
						{
							flag = false;
							zero2 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, num42, 0f);
							float scale = this.m_textInfo.characterInfo[i].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num35, num40, scale, num41, underlineColor);
							num41 = 0f;
							num42 = float.PositiveInfinity;
						}
						else if (flag && (i == tmp_LineInfo.lastCharacterIndex || i >= tmp_LineInfo.lastVisibleCharacterIndex))
						{
							float scale;
							if (char.IsWhiteSpace(character2))
							{
								int lastVisibleCharacterIndex = tmp_LineInfo.lastVisibleCharacterIndex;
								zero2 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num42, 0f);
								scale = this.m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
							}
							else
							{
								zero2 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, num42, 0f);
								scale = this.m_textInfo.characterInfo[i].scale;
							}
							flag = false;
							this.DrawUnderlineMesh(zero, zero2, ref num35, num40, scale, num41, underlineColor);
							num41 = 0f;
							num42 = float.PositiveInfinity;
						}
						else if (flag && !flag10)
						{
							flag = false;
							zero2 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, num42, 0f);
							float scale = this.m_textInfo.characterInfo[i - 1].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num35, num40, scale, num41, underlineColor);
							num41 = 0f;
							num42 = float.PositiveInfinity;
						}
					}
					else if (flag)
					{
						flag = false;
						zero2 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, num42, 0f);
						float scale = this.m_textInfo.characterInfo[i - 1].scale;
						this.DrawUnderlineMesh(zero, zero2, ref num35, num40, scale, num41, underlineColor);
						num41 = 0f;
						num42 = float.PositiveInfinity;
					}
					bool flag11 = (this.m_textInfo.characterInfo[i].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
					if (flag11)
					{
						bool flag12 = true;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && (int)(this.m_textInfo.characterInfo[i].pageNumber + 1) != this.m_pageToDisplay))
						{
							flag12 = false;
						}
						if (!flag2 && flag12 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
							{
								flag2 = true;
								num44 = this.m_textInfo.characterInfo[i].pointSize;
								num45 = this.m_textInfo.characterInfo[i].scale;
								zero3 = new Vector3(this.m_textInfo.characterInfo[i].bottomLeft.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2.75f * num45, 0f);
								underlineColor2 = this.m_textInfo.characterInfo[i].color;
								b4 = this.m_textInfo.characterInfo[i].baseLine;
							}
						}
						if (flag2 && this.m_characterCount == 1)
						{
							flag2 = false;
							zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num45, num45, num45, underlineColor2);
						}
						else if (flag2 && i == tmp_LineInfo.lastCharacterIndex)
						{
							if (char.IsWhiteSpace(character2))
							{
								int lastVisibleCharacterIndex2 = tmp_LineInfo.lastVisibleCharacterIndex;
								zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
							}
							else
							{
								zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
							}
							flag2 = false;
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num45, num45, num45, underlineColor2);
						}
						else if (flag2 && i < this.m_characterCount && (this.m_textInfo.characterInfo[i + 1].pointSize != num44 || !TMP_Math.Approximately(this.m_textInfo.characterInfo[i + 1].baseLine + b3.y, b4)))
						{
							flag2 = false;
							int lastVisibleCharacterIndex3 = tmp_LineInfo.lastVisibleCharacterIndex;
							if (i > lastVisibleCharacterIndex3)
							{
								zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
							}
							else
							{
								zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
							}
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num45, num45, num45, underlineColor2);
						}
						else if (flag2 && !flag12)
						{
							flag2 = false;
							zero4 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, this.m_textInfo.characterInfo[i - 1].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num35, num45, num45, num45, underlineColor2);
						}
					}
					else if (flag2)
					{
						flag2 = false;
						zero4 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, this.m_textInfo.characterInfo[i - 1].baseLine + (base.font.fontInfo.Ascender + base.font.fontInfo.Descender) / 2f * num45, 0f);
						this.DrawUnderlineMesh(zero3, zero4, ref num35, num45, num45, num45, underlineColor2);
					}
					num38 = lineNumber3;
				}
				this.m_textInfo.characterCount = (int)((short)this.m_characterCount);
				this.m_textInfo.spriteCount = this.m_spriteCount;
				this.m_textInfo.lineCount = (int)((short)num37);
				this.m_textInfo.wordCount = (int)((num36 == 0 || this.m_characterCount <= 0) ? 1 : ((short)num36));
				this.m_textInfo.pageCount = this.m_pageNumber + 1;
				if (this.m_renderMode == TextRenderFlags.Render)
				{
					this.m_mesh.MarkDynamic();
					this.m_mesh.vertices = this.m_textInfo.meshInfo[0].vertices;
					this.m_mesh.uv = this.m_textInfo.meshInfo[0].uvs0;
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
					this.m_mesh.colors32 = this.m_textInfo.meshInfo[0].colors32;
					this.m_mesh.RecalculateBounds();
					for (int j = 1; j < this.m_textInfo.materialCount; j++)
					{
						this.m_textInfo.meshInfo[j].ClearUnusedVertices();
						this.m_subTextObjects[j].mesh.vertices = this.m_textInfo.meshInfo[j].vertices;
						this.m_subTextObjects[j].mesh.uv = this.m_textInfo.meshInfo[j].uvs0;
						this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
						this.m_subTextObjects[j].mesh.colors32 = this.m_textInfo.meshInfo[j].colors32;
						this.m_subTextObjects[j].mesh.RecalculateBounds();
					}
				}
				TMPro_EventManager.ON_TEXT_CHANGED(this);
				return;
			}
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x0028BEC5 File Offset: 0x0028A2C5
		protected override Vector3[] GetTextContainerLocalCorners()
		{
			return this.textContainer.corners;
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x0028BED4 File Offset: 0x0028A2D4
		private void ClearMesh(bool updateMesh)
		{
			if (this.m_textInfo.meshInfo[0].mesh == null)
			{
				this.m_textInfo.meshInfo[0].mesh = this.m_mesh;
			}
			this.m_textInfo.ClearMeshInfo(updateMesh);
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x0028BF2C File Offset: 0x0028A32C
		private void UpdateSDFScale(float lossyScale)
		{
			for (int i = 0; i < this.m_textInfo.characterCount; i++)
			{
				if (this.m_textInfo.characterInfo[i].isVisible && this.m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
				{
					float num = lossyScale * this.m_textInfo.characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
					if ((this.m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num *= -1f;
					}
					int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = (int)this.m_textInfo.characterInfo[i].vertexIndex;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num;
				}
			}
			for (int j = 0; j < this.m_textInfo.meshInfo.Length; j++)
			{
				if (j == 0)
				{
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
				}
				else
				{
					this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
				}
			}
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x0028C10C File Offset: 0x0028A50C
		protected override void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
			Vector3 b = new Vector3(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				int num = i;
				characterInfo[num].bottomLeft = characterInfo[num].bottomLeft - b;
				TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
				int num2 = i;
				characterInfo2[num2].topLeft = characterInfo2[num2].topLeft - b;
				TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
				int num3 = i;
				characterInfo3[num3].topRight = characterInfo3[num3].topRight - b;
				TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
				int num4 = i;
				characterInfo4[num4].bottomRight = characterInfo4[num4].bottomRight - b;
				TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
				int num5 = i;
				characterInfo5[num5].descender = characterInfo5[num5].descender - b.y;
				TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
				int num6 = i;
				characterInfo6[num6].baseLine = characterInfo6[num6].baseLine - b.y;
				TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
				int num7 = i;
				characterInfo7[num7].ascender = characterInfo7[num7].ascender - b.y;
				if (this.m_textInfo.characterInfo[i].isVisible)
				{
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num8 = i;
					characterInfo8[num8].vertex_BL.position = characterInfo8[num8].vertex_BL.position - b;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num9 = i;
					characterInfo9[num9].vertex_TL.position = characterInfo9[num9].vertex_TL.position - b;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num10 = i;
					characterInfo10[num10].vertex_TR.position = characterInfo10[num10].vertex_TR.position - b;
					TMP_CharacterInfo[] characterInfo11 = this.m_textInfo.characterInfo;
					int num11 = i;
					characterInfo11[num11].vertex_BR.position = characterInfo11[num11].vertex_BR.position - b;
				}
			}
		}

		// Token: 0x04005235 RID: 21045
		[SerializeField]
		private float m_lineLength;

		// Token: 0x04005236 RID: 21046
		[SerializeField]
		private TMP_Compatibility.AnchorPositions m_anchor = TMP_Compatibility.AnchorPositions.None;

		// Token: 0x04005237 RID: 21047
		private bool m_autoSizeTextContainer;

		// Token: 0x04005238 RID: 21048
		private bool m_currentAutoSizeMode;

		// Token: 0x04005239 RID: 21049
		[SerializeField]
		private Vector2 m_uvOffset = Vector2.zero;

		// Token: 0x0400523A RID: 21050
		[SerializeField]
		private float m_uvLineOffset;

		// Token: 0x0400523B RID: 21051
		[SerializeField]
		private bool m_hasFontAssetChanged;

		// Token: 0x0400523C RID: 21052
		private Vector3 m_previousLossyScale;

		// Token: 0x0400523D RID: 21053
		[SerializeField]
		private Renderer m_renderer;

		// Token: 0x0400523E RID: 21054
		private MeshFilter m_meshFilter;

		// Token: 0x0400523F RID: 21055
		private bool m_isFirstAllocation;

		// Token: 0x04005240 RID: 21056
		private int m_max_characters = 8;

		// Token: 0x04005241 RID: 21057
		private int m_max_numberOfLines = 4;

		// Token: 0x04005242 RID: 21058
		private WordWrapState m_SavedWordWrapState = default(WordWrapState);

		// Token: 0x04005243 RID: 21059
		private WordWrapState m_SavedLineState = default(WordWrapState);

		// Token: 0x04005244 RID: 21060
		private Bounds m_default_bounds = new Bounds(Vector3.zero, new Vector3(1000f, 1000f, 0f));

		// Token: 0x04005245 RID: 21061
		[SerializeField]
		protected TMP_SubMesh[] m_subTextObjects = new TMP_SubMesh[16];

		// Token: 0x04005246 RID: 21062
		private List<Material> m_sharedMaterials = new List<Material>(16);

		// Token: 0x04005247 RID: 21063
		private bool m_isMaskingEnabled;

		// Token: 0x04005248 RID: 21064
		private bool isMaskUpdateRequired;

		// Token: 0x04005249 RID: 21065
		[SerializeField]
		private MaskingTypes m_maskType;

		// Token: 0x0400524A RID: 21066
		private Matrix4x4 m_EnvMapMatrix = default(Matrix4x4);

		// Token: 0x0400524B RID: 21067
		private TextContainer m_textContainer;

		// Token: 0x0400524C RID: 21068
		[NonSerialized]
		private bool m_isRegisteredForEvents;

		// Token: 0x0400524D RID: 21069
		private int m_recursiveCount;

		// Token: 0x0400524E RID: 21070
		private int loopCountA;
	}
}
