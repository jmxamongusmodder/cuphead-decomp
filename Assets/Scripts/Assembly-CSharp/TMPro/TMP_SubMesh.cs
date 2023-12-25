using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C7C RID: 3196
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class TMP_SubMesh : MonoBehaviour
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06005019 RID: 20505 RVA: 0x0029573A File Offset: 0x00293B3A
		// (set) Token: 0x0600501A RID: 20506 RVA: 0x00295742 File Offset: 0x00293B42
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

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x0600501B RID: 20507 RVA: 0x0029574B File Offset: 0x00293B4B
		// (set) Token: 0x0600501C RID: 20508 RVA: 0x00295753 File Offset: 0x00293B53
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

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x0029575C File Offset: 0x00293B5C
		// (set) Token: 0x0600501E RID: 20510 RVA: 0x0029576A File Offset: 0x00293B6A
		public Material material
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

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600501F RID: 20511 RVA: 0x002957A2 File Offset: 0x00293BA2
		// (set) Token: 0x06005020 RID: 20512 RVA: 0x002957AA File Offset: 0x00293BAA
		public Material sharedMaterial
		{
			get
			{
				return this.GetSharedMaterial();
			}
			set
			{
				this.SetSharedMaterial(value);
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x002957B3 File Offset: 0x00293BB3
		// (set) Token: 0x06005022 RID: 20514 RVA: 0x002957BB File Offset: 0x00293BBB
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

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06005023 RID: 20515 RVA: 0x002957C4 File Offset: 0x00293BC4
		// (set) Token: 0x06005024 RID: 20516 RVA: 0x002957CC File Offset: 0x00293BCC
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

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x002957D5 File Offset: 0x00293BD5
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

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x002957FA File Offset: 0x00293BFA
		public MeshFilter meshFilter
		{
			get
			{
				if (this.m_meshFilter == null)
				{
					this.m_meshFilter = base.GetComponent<MeshFilter>();
				}
				return this.m_meshFilter;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x00295820 File Offset: 0x00293C20
		// (set) Token: 0x06005028 RID: 20520 RVA: 0x0029586D File Offset: 0x00293C6D
		public Mesh mesh
		{
			get
			{
				if (this.m_mesh == null)
				{
					this.m_mesh = new Mesh();
					this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
					this.meshFilter.mesh = this.m_mesh;
				}
				return this.m_mesh;
			}
			set
			{
				this.m_mesh = value;
			}
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x00295878 File Offset: 0x00293C78
		private void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			if (this.m_sharedMaterial != null)
			{
				this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, new Vector4(-10000f, -10000f, 10000f, 10000f));
			}
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x002958D1 File Offset: 0x00293CD1
		private void OnDestroy()
		{
			if (this.m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_mesh);
			}
			this.m_isRegisteredForEvents = false;
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x002958F8 File Offset: 0x00293CF8
		public static TMP_SubMesh AddSubTextObject(TextMeshPro textComponent, MaterialReference materialReference)
		{
			GameObject gameObject = new GameObject("TMP SubMesh [" + materialReference.material.name + "]");
			TMP_SubMesh tmp_SubMesh = gameObject.AddComponent<TMP_SubMesh>();
			gameObject.transform.SetParent(textComponent.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.layer = textComponent.gameObject.layer;
			tmp_SubMesh.m_meshFilter = gameObject.GetComponent<MeshFilter>();
			tmp_SubMesh.m_TextComponent = textComponent;
			tmp_SubMesh.m_fontAsset = materialReference.fontAsset;
			tmp_SubMesh.m_spriteAsset = materialReference.spriteAsset;
			tmp_SubMesh.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tmp_SubMesh.SetSharedMaterial(materialReference.material);
			tmp_SubMesh.renderer.sortingLayerID = textComponent.renderer.sortingLayerID;
			tmp_SubMesh.renderer.sortingOrder = textComponent.renderer.sortingOrder;
			return tmp_SubMesh;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x002959F4 File Offset: 0x00293DF4
		public void DestroySelf()
		{
			UnityEngine.Object.Destroy(base.gameObject, 1f);
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x00295A08 File Offset: 0x00293E08
		private Material GetMaterial(Material mat)
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = base.GetComponent<Renderer>();
			}
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

		// Token: 0x0600502E RID: 20526 RVA: 0x00295A90 File Offset: 0x00293E90
		private Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x00295AC7 File Offset: 0x00293EC7
		private Material GetSharedMaterial()
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = base.GetComponent<Renderer>();
			}
			return this.m_renderer.sharedMaterial;
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x00295AF1 File Offset: 0x00293EF1
		private void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x00295B0C File Offset: 0x00293F0C
		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x00295B3C File Offset: 0x00293F3C
		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x00295B51 File Offset: 0x00293F51
		public void SetVerticesDirty()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.havePropertiesChanged = true;
				this.m_TextComponent.SetVerticesDirty();
			}
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x00295B87 File Offset: 0x00293F87
		public void SetMaterialDirty()
		{
			this.UpdateMaterial();
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x00295B8F File Offset: 0x00293F8F
		protected void UpdateMaterial()
		{
			if (this.m_renderer == null)
			{
				this.m_renderer = this.renderer;
			}
			this.m_renderer.sharedMaterial = this.m_sharedMaterial;
		}

		// Token: 0x040052BD RID: 21181
		[SerializeField]
		private TMP_FontAsset m_fontAsset;

		// Token: 0x040052BE RID: 21182
		[SerializeField]
		private TMP_SpriteAsset m_spriteAsset;

		// Token: 0x040052BF RID: 21183
		[SerializeField]
		private Material m_material;

		// Token: 0x040052C0 RID: 21184
		[SerializeField]
		private Material m_sharedMaterial;

		// Token: 0x040052C1 RID: 21185
		[SerializeField]
		private bool m_isDefaultMaterial;

		// Token: 0x040052C2 RID: 21186
		[SerializeField]
		private float m_padding;

		// Token: 0x040052C3 RID: 21187
		[SerializeField]
		private Renderer m_renderer;

		// Token: 0x040052C4 RID: 21188
		[SerializeField]
		private MeshFilter m_meshFilter;

		// Token: 0x040052C5 RID: 21189
		private Mesh m_mesh;

		// Token: 0x040052C6 RID: 21190
		[SerializeField]
		private TextMeshPro m_TextComponent;

		// Token: 0x040052C7 RID: 21191
		[NonSerialized]
		private bool m_isRegisteredForEvents;
	}
}
