using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class DragonLevelBackgroundChange : DragonLevelScrollingSprite
{
	// Token: 0x06001E07 RID: 7687 RVA: 0x001147AE File Offset: 0x00112BAE
	protected override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.DisableSprites), 1);
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x001147CC File Offset: 0x00112BCC
	private void DisableSprites()
	{
		this.fadeTime = 6f;
		this.current = base.GetComponent<SpriteRenderer>();
		this.replacementClones = this.replacementSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.currentClones = this.current.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.replacementSprite.transform.position = new Vector2(base.transform.position.x, this.replacementSprite.transform.position.y);
		this.replacementSprite.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		for (int i = 0; i < this.replacementClones.Length; i++)
		{
			this.replacementClones[i].enabled = false;
		}
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x001148A8 File Offset: 0x00112CA8
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x001148B8 File Offset: 0x00112CB8
	private IEnumerator change_cr()
	{
		float t = 0f;
		while (t < this.fadeTime)
		{
			for (int i = 0; i < this.replacementClones.Length; i++)
			{
				if (this.replacementClones[i].transform != null)
				{
					this.replacementClones[i].enabled = true;
					this.replacementClones[i].color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			for (int j = 0; j < this.currentClones.Length; j++)
			{
				this.currentClones[j].color = new Color(1f, 1f, 1f, 1f - t / this.fadeTime);
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int k = 0; k < this.replacementClones.Length; k++)
		{
			this.replacementClones[k].color = new Color(1f, 1f, 1f, 1f);
		}
		for (int l = 0; l < this.currentClones.Length; l++)
		{
			this.currentClones[l].enabled = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x001148D3 File Offset: 0x00112CD3
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.replacementClones = null;
		this.currentClones = null;
	}

	// Token: 0x040026D2 RID: 9938
	[SerializeField]
	private Transform replacementSprite;

	// Token: 0x040026D3 RID: 9939
	private SpriteRenderer[] replacementClones;

	// Token: 0x040026D4 RID: 9940
	private SpriteRenderer current;

	// Token: 0x040026D5 RID: 9941
	private SpriteRenderer[] currentClones;

	// Token: 0x040026D6 RID: 9942
	private bool changeStart;

	// Token: 0x040026D7 RID: 9943
	private float fadeTime;
}
