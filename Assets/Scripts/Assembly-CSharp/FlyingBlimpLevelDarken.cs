using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class FlyingBlimpLevelDarken : AbstractPausableComponent
{
	// Token: 0x0600207A RID: 8314 RVA: 0x0012B65B File Offset: 0x00129A5B
	private void Update()
	{
		this.children = base.transform.GetComponentsInChildren<SpriteRenderer>();
		if (this.blimpLady.fading && !this.startedFade)
		{
			this.startedFade = true;
			this.StartFade();
		}
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x0012B696 File Offset: 0x00129A96
	private void StartFade()
	{
		base.StartCoroutine(this.fade_cr());
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x0012B6A8 File Offset: 0x00129AA8
	private IEnumerator fade_cr()
	{
		float t = 0f;
		float fadeTime = 0.005f;
		float fadeVal = 1f;
		while (t < fadeTime && fadeVal > this.fadeMax)
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i].transform != null)
				{
					this.children[i].color = new Color(fadeVal - t / fadeTime, fadeVal - t / fadeTime, fadeVal - t / fadeTime, 1f);
				}
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.dark_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x0012B6C4 File Offset: 0x00129AC4
	private IEnumerator dark_cr()
	{
		for (;;)
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i].transform != null)
				{
					this.children[i].color = new Color(this.fadeMax, this.fadeMax, this.fadeMax, 1f);
				}
			}
			if (!this.blimpLady.fading)
			{
				break;
			}
			yield return null;
		}
		base.StartCoroutine(this.light_cr());
		yield break;
		yield break;
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x0012B6E0 File Offset: 0x00129AE0
	private IEnumerator light_cr()
	{
		float fadeMid = 0.87f;
		while (this.startedFade)
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i] != null && this.children[i].transform != null)
				{
					this.children[i].color = new Color(fadeMid, fadeMid, fadeMid, 1f);
				}
			}
			if (this.blimpLady.state == FlyingBlimpLevelBlimpLady.State.Idle)
			{
				for (int j = 0; j < this.children.Length; j++)
				{
					if (this.children[j].transform != null)
					{
						this.children[j].color = new Color(1f, 1f, 1f, 1f);
					}
				}
				this.startedFade = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040028FB RID: 10491
	[SerializeField]
	private FlyingBlimpLevelBlimpLady blimpLady;

	// Token: 0x040028FC RID: 10492
	private SpriteRenderer[] children;

	// Token: 0x040028FD RID: 10493
	private float fadeMax = 0.75f;

	// Token: 0x040028FE RID: 10494
	private bool startedFade;
}
