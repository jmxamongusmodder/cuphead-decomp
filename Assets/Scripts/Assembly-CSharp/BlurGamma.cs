using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020003DF RID: 991
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlurGamma : PostEffectsBase
{
	// Token: 0x06000D4B RID: 3403 RVA: 0x0008CEDA File Offset: 0x0008B2DA
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x0008CF13 File Offset: 0x0008B313
	public void OnDisable()
	{
		if (this.blurMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.blurMaterial);
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x0008CF30 File Offset: 0x0008B330
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		float num = (float)source.width / (float)source.height;
		float num2 = (num >= 1.7777778f) ? 1f : (num / 1.7777778f);
		num2 *= 1f - 0.1f * SettingsData.Data.overscan;
		float num3 = (float)source.height / 1080f;
		num3 *= num2;
		if (SettingsData.Data.filter == BlurGamma.Filter.BW)
		{
			num3 *= 1.35f;
		}
		this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num3, -this.blurSize * num3, Mathf.Pow(1.4f, -SettingsData.Data.Brightness), 0f));
		source.filterMode = FilterMode.Bilinear;
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
		Graphics.Blit(source, temporary, this.blurMaterial, 0);
		int num4 = 1;
		BlurGamma.Filter filter = SettingsData.Data.filter;
		if (filter != BlurGamma.Filter.TwoStrip)
		{
			if (filter == BlurGamma.Filter.BW)
			{
				num4 += 2;
			}
		}
		else
		{
			num4++;
		}
		Graphics.Blit(temporary, destination, this.blurMaterial, num4);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x040016BE RID: 5822
	[Range(0f, 10f)]
	public float blurSize = 3f;

	// Token: 0x040016BF RID: 5823
	[Range(1f, 4f)]
	public int blurIterations = 2;

	// Token: 0x040016C0 RID: 5824
	public Shader blurShader;

	// Token: 0x040016C1 RID: 5825
	private Material blurMaterial;

	// Token: 0x020003E0 RID: 992
	public enum Filter
	{
		// Token: 0x040016C3 RID: 5827
		None,
		// Token: 0x040016C4 RID: 5828
		TwoStrip,
		// Token: 0x040016C5 RID: 5829
		BW,
		// Token: 0x040016C6 RID: 5830
		Chalice
	}
}
