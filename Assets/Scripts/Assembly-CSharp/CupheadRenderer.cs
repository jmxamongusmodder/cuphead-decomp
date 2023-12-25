using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF9 RID: 2809
public class CupheadRenderer : AbstractMonoBehaviour
{
	// Token: 0x06004411 RID: 17425 RVA: 0x00240BC1 File Offset: 0x0023EFC1
	protected override void Awake()
	{
		base.Awake();
		if (CupheadRenderer.Instance == null)
		{
			CupheadRenderer.Instance = this;
			this.Setup();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x00240BF6 File Offset: 0x0023EFF6
	private void OnDestroy()
	{
		if (CupheadRenderer.Instance == this)
		{
			CupheadRenderer.Instance = null;
		}
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x00240C0E File Offset: 0x0023F00E
	private void Setup()
	{
		this.rendererCamera = UnityEngine.Object.Instantiate<CupheadRendererCamera>(this.cameraPrefab);
		this.rendererCamera.transform.SetParent(base.transform);
		this.rendererCamera.transform.ResetLocalTransforms();
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x00240C47 File Offset: 0x0023F047
	public void TouchFuzzy(float amount, float speed, float time)
	{
		this.rendererCamera.GetComponent<ChromaticAberrationFilmGrain>().PsychedelicEffect(amount, speed, time);
		base.StartCoroutine(this.change_blur_cr(time));
	}

	// Token: 0x06004415 RID: 17429 RVA: 0x00240C6C File Offset: 0x0023F06C
	private IEnumerator change_blur_cr(float time)
	{
		float t = 0f;
		float incrementTime = 1f;
		float blurStart = this.rendererCamera.GetComponent<BlurGamma>().blurSize;
		this.rendererCamera.GetComponent<BlurGamma>().blurSize += incrementTime;
		while (this.rendererCamera.GetComponent<BlurGamma>().blurSize > blurStart)
		{
			t += Time.deltaTime;
			if (t >= time / 2f)
			{
				this.rendererCamera.GetComponent<BlurGamma>().blurSize -= incrementTime * Time.deltaTime;
			}
			else
			{
				this.rendererCamera.GetComponent<BlurGamma>().blurSize += incrementTime * Time.deltaTime;
			}
			yield return null;
		}
		this.rendererCamera.GetComponent<BlurGamma>().blurSize = blurStart;
		yield break;
	}

	// Token: 0x040049B8 RID: 18872
	public static CupheadRenderer Instance;

	// Token: 0x040049B9 RID: 18873
	[SerializeField]
	private CupheadRendererCamera cameraPrefab;

	// Token: 0x040049BA RID: 18874
	private CupheadRendererCamera rendererCamera;

	// Token: 0x040049BB RID: 18875
	private Camera bgCamera;

	// Token: 0x040049BC RID: 18876
	private Canvas canvas;

	// Token: 0x040049BD RID: 18877
	private Dictionary<CupheadRenderer.RenderLayer, RectTransform> rendererParents;

	// Token: 0x040049BE RID: 18878
	private Image background;

	// Token: 0x040049BF RID: 18879
	private Image fader;

	// Token: 0x040049C0 RID: 18880
	public bool fuzzyEffectPlaying;

	// Token: 0x02000AFA RID: 2810
	public enum RenderLayer
	{
		// Token: 0x040049C2 RID: 18882
		None,
		// Token: 0x040049C3 RID: 18883
		Game,
		// Token: 0x040049C4 RID: 18884
		UI,
		// Token: 0x040049C5 RID: 18885
		Loader
	}
}
