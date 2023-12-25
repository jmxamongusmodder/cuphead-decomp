using System;
using UnityEngine;

// Token: 0x020003E1 RID: 993
[ExecuteInEditMode]
public class ChromaticAberration : MonoBehaviour
{
	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0008D085 File Offset: 0x0008B485
	private Material material
	{
		get
		{
			if (this.curMaterial == null)
			{
				this.curMaterial = new Material(this.shader);
				this.curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.curMaterial;
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0008D0BC File Offset: 0x0008B4BC
	protected virtual void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0008D0D0 File Offset: 0x0008B4D0
	protected virtual void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.shader != null)
		{
			float num = (float)destTexture.width / (float)destTexture.height;
			float num2 = (num >= 1.7777778f) ? 1f : (num / 1.7777778f);
			num2 *= 1f - 0.1f * SettingsData.Data.overscan;
			float d = num2 * (float)destTexture.height / 1080f;
			this.material.SetVector("_Screen", new Vector2((float)destTexture.width, (float)destTexture.height));
			this.material.SetVector("_Red", this.r * d);
			this.material.SetVector("_Green", this.g * d);
			this.material.SetVector("_Blue", this.b * d);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0008D1E8 File Offset: 0x0008B5E8
	protected virtual void OnDisable()
	{
		if (this.curMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.curMaterial);
		}
	}

	// Token: 0x040016C7 RID: 5831
	public Shader shader;

	// Token: 0x040016C8 RID: 5832
	public Vector2 r;

	// Token: 0x040016C9 RID: 5833
	public Vector2 g;

	// Token: 0x040016CA RID: 5834
	public Vector2 b;

	// Token: 0x040016CB RID: 5835
	private Material curMaterial;
}
