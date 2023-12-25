using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009A4 RID: 2468
public class MapUIVignetteDialogue : AbstractMonoBehaviour
{
	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060039E8 RID: 14824 RVA: 0x0020EFCB File Offset: 0x0020D3CB
	// (set) Token: 0x060039E9 RID: 14825 RVA: 0x0020EFD2 File Offset: 0x0020D3D2
	public static MapUIVignetteDialogue Current { get; private set; }

	// Token: 0x060039EA RID: 14826 RVA: 0x0020EFDA File Offset: 0x0020D3DA
	protected override void Awake()
	{
		base.Awake();
		MapUIVignetteDialogue.Current = this;
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x0020EFF8 File Offset: 0x0020D3F8
	private void LateUpdate()
	{
		base.transform.position = CupheadMapCamera.Current.transform.position;
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x0020F014 File Offset: 0x0020D414
	public void FadeIn()
	{
		this.Fade(1f);
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x0020F021 File Offset: 0x0020D421
	public void FadeOut()
	{
		this.Fade(0f);
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x0020F02E File Offset: 0x0020D42E
	public void Fade(float target)
	{
		base.StartCoroutine(this.fade_cr(this.canvasGroup.alpha, target));
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x0020F04C File Offset: 0x0020D44C
	private IEnumerator fade_cr(float startOpacity, float endOpacity)
	{
		float t = 0f;
		while (t < MapUIVignetteDialogue.fadeTime)
		{
			yield return null;
			t += CupheadTime.Delta;
			this.canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, t / MapUIVignetteDialogue.fadeTime);
		}
		this.canvasGroup.alpha = endOpacity;
		yield break;
	}

	// Token: 0x040041D8 RID: 16856
	public static float fadeTime = 0.5f;

	// Token: 0x040041D9 RID: 16857
	[SerializeField]
	private CanvasGroup canvasGroup;
}
