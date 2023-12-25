using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020003E3 RID: 995
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Screen Overlay Animated")]
public class ScreenOverlayAnimated : PostEffectsBase
{
	// Token: 0x06000D5C RID: 3420 RVA: 0x0008D94A File Offset: 0x0008BD4A
	protected override void Start()
	{
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x0008D95C File Offset: 0x0008BD5C
	private IEnumerator animate_cr()
	{
		for (;;)
		{
			yield return new WaitForSeconds(0.025f);
			if (this.animated)
			{
				this.currentTexture++;
				if (this.currentTexture >= this.textures.Length)
				{
					this.currentTexture = 0;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x0008D977 File Offset: 0x0008BD77
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0008D9B0 File Offset: 0x0008BDB0
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.overlayMaterial.SetVector("_UV_Transform", this.UV_Transform);
		this.overlayMaterial.SetFloat("_Intensity", this.intensity);
		if (this.textures != null && this.textures.Length > this.currentTexture && this.textures[this.currentTexture] != null)
		{
			this.overlayMaterial.SetTexture("_Overlay", this.textures[this.currentTexture]);
		}
		Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
	}

	// Token: 0x040016DB RID: 5851
	private const float FRAME_TIME = 0.025f;

	// Token: 0x040016DC RID: 5852
	private Vector4 UV_Transform = new Vector4(1f, 0f, 0f, 1f);

	// Token: 0x040016DD RID: 5853
	public ScreenOverlayAnimated.OverlayBlendMode blendMode = ScreenOverlayAnimated.OverlayBlendMode.Overlay;

	// Token: 0x040016DE RID: 5854
	public float intensity = 1f;

	// Token: 0x040016DF RID: 5855
	public bool animated = true;

	// Token: 0x040016E0 RID: 5856
	public Texture2D[] textures;

	// Token: 0x040016E1 RID: 5857
	public Shader overlayShader;

	// Token: 0x040016E2 RID: 5858
	private int currentTexture;

	// Token: 0x040016E3 RID: 5859
	private Material overlayMaterial;

	// Token: 0x020003E4 RID: 996
	public enum OverlayBlendMode
	{
		// Token: 0x040016E5 RID: 5861
		Additive,
		// Token: 0x040016E6 RID: 5862
		ScreenBlend,
		// Token: 0x040016E7 RID: 5863
		Multiply,
		// Token: 0x040016E8 RID: 5864
		Overlay,
		// Token: 0x040016E9 RID: 5865
		AlphaBlend
	}
}
