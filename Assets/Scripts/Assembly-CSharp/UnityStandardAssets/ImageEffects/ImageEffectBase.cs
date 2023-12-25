using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CDC RID: 3292
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		// Token: 0x06005222 RID: 21026 RVA: 0x0029EE05 File Offset: 0x0029D205
		protected virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.shader || !this.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x0029EE40 File Offset: 0x0029D240
		protected Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.shader);
					this.m_Material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material;
			}
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x0029EE77 File Offset: 0x0029D277
		protected virtual void OnDisable()
		{
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
		}

		// Token: 0x04005684 RID: 22148
		public Shader shader;

		// Token: 0x04005685 RID: 22149
		private Material m_Material;
	}
}
