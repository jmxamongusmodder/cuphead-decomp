using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020003E2 RID: 994
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/Chromatic Aberration Film Grain")]
public class ChromaticAberrationFilmGrain : PostEffectsBase
{
	// Token: 0x06000D54 RID: 3412 RVA: 0x0008D254 File Offset: 0x0008B654
	public void Initialize(Texture2D[] filmGrain)
	{
		base.enabled = true;
		this.rStart = this.r;
		this.gStart = this.g;
		this.bStart = this.b;
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		this.textures = filmGrain;
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0008D2B4 File Offset: 0x0008B6B4
	private IEnumerator animate_cr()
	{
		float t = 0f;
		int loopsUntilFullLoop = UnityEngine.Random.Range(7, 15);
		for (;;)
		{
			t += Time.deltaTime;
			while (t > 0.025f)
			{
				t -= 0.025f;
				if (this.animated)
				{
					this.currentTexture++;
					if (loopsUntilFullLoop > 0)
					{
						if (this.currentTexture >= this.earlyLoopPoint)
						{
							this.currentTexture = 0;
							loopsUntilFullLoop--;
							this.UV_Transform = new Vector4((float)MathUtils.PlusOrMinus(), 0f, 0f, (float)MathUtils.PlusOrMinus());
						}
					}
					else if (this.currentTexture >= this.textures.Length)
					{
						this.currentTexture = 0;
						loopsUntilFullLoop = UnityEngine.Random.Range(7, 15);
						this.UV_Transform = new Vector4((float)MathUtils.PlusOrMinus(), 0f, 0f, (float)MathUtils.PlusOrMinus());
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0008D2CF File Offset: 0x0008B6CF
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x0008D308 File Offset: 0x0008B708
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.material.SetVector("_UV_Transform", this.UV_Transform);
		this.material.SetFloat("_Intensity", this.intensity);
		if (this.textures != null && this.textures.Length > this.currentTexture && this.textures[this.currentTexture] != null)
		{
			this.material.SetTexture("_Overlay", this.textures[this.currentTexture]);
		}
		float num = (float)source.width / (float)source.height;
		float num2 = (num >= 1.7777778f) ? 1f : (num / 1.7777778f);
		num2 *= 1f - 0.1f * SettingsData.Data.overscan;
		float d = SettingsData.Data.chromaticAberration * num2 * (float)source.height / 1080f;
		Vector2 v = this.r * d;
		Vector2 vector = this.g * d;
		Vector2 vector2 = this.b * d;
		if (SettingsData.Data.filter == BlurGamma.Filter.TwoStrip)
		{
			Vector2 vector3 = vector2 * 0.4f + vector * 0.6f;
			vector = vector3;
		}
		this.material.SetVector("_Screen", new Vector2((float)source.width, (float)source.height));
		this.material.SetVector("_Red", v);
		this.material.SetVector("_Green", vector);
		this.material.SetVector("_Blue", vector2);
		int num3 = 0;
		BlurGamma.Filter filter = SettingsData.Data.filter;
		if (filter != BlurGamma.Filter.TwoStrip)
		{
			if (filter == BlurGamma.Filter.BW)
			{
				num3 += 2;
			}
		}
		else
		{
			num3++;
		}
		Graphics.Blit(source, destination, this.material, num3);
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0008D519 File Offset: 0x0008B919
	protected virtual void OnDisable()
	{
		if (this.material)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
		}
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x0008D536 File Offset: 0x0008B936
	public void PsychedelicEffect(float amount, float speed, float time)
	{
		base.StartCoroutine(this.psychedelic_effect(amount, speed, time));
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0008D548 File Offset: 0x0008B948
	private IEnumerator psychedelic_effect(float amount, float speed, float time)
	{
		float t = 0f;
		float slowdownTime = 0.5f;
		while (amount > 0f)
		{
			t += Time.deltaTime;
			float angle = speed * t;
			float phase = Mathf.Sin(angle) * amount;
			this.r = Vector2.up * phase;
			this.g = Vector2.up * phase / 2f;
			this.b = Vector2.down * phase;
			if (t >= time)
			{
				amount -= slowdownTime;
			}
			yield return null;
		}
		this.r = this.rStart;
		this.g = this.gStart;
		this.b = this.bStart;
		yield return null;
		yield break;
	}

	// Token: 0x040016CC RID: 5836
	public Shader shader;

	// Token: 0x040016CD RID: 5837
	private Material material;

	// Token: 0x040016CE RID: 5838
	private const float FRAME_TIME = 0.025f;

	// Token: 0x040016CF RID: 5839
	private Vector4 UV_Transform = new Vector4(1f, 0f, 0f, 1f);

	// Token: 0x040016D0 RID: 5840
	public float intensity = 1f;

	// Token: 0x040016D1 RID: 5841
	public bool animated = true;

	// Token: 0x040016D2 RID: 5842
	public int earlyLoopPoint = 102;

	// Token: 0x040016D3 RID: 5843
	private int currentTexture;

	// Token: 0x040016D4 RID: 5844
	public Vector2 r;

	// Token: 0x040016D5 RID: 5845
	public Vector2 g;

	// Token: 0x040016D6 RID: 5846
	public Vector2 b;

	// Token: 0x040016D7 RID: 5847
	private Texture2D[] textures;

	// Token: 0x040016D8 RID: 5848
	private Vector2 rStart;

	// Token: 0x040016D9 RID: 5849
	private Vector2 gStart;

	// Token: 0x040016DA RID: 5850
	private Vector2 bStart;
}
