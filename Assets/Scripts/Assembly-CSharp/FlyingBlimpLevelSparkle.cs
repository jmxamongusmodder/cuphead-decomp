using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class FlyingBlimpLevelSparkle : ScrollingSprite
{
	// Token: 0x060020DA RID: 8410 RVA: 0x0012F722 File Offset: 0x0012DB22
	protected override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.DisableStars), 1);
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x0012F740 File Offset: 0x0012DB40
	private void DisableStars()
	{
		this.twinkleSprite = base.GetComponent<SpriteRenderer>();
		this.starClones = this.starSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.twinkleClones = base.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.starSprite.enabled = false;
		this.twinkleSprite.enabled = false;
		this.change = true;
		this.fadeTime = 0.8f;
		for (int i = 0; i < this.starClones.Length; i++)
		{
			this.starClones[i].enabled = false;
		}
		for (int j = 0; j < this.twinkleClones.Length; j++)
		{
			this.twinkleClones[j].enabled = false;
		}
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x0012F802 File Offset: 0x0012DC02
	protected override void Update()
	{
		base.Update();
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && this.change)
		{
			base.StartCoroutine(this.fadein_cr());
			this.change = false;
		}
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x0012F83C File Offset: 0x0012DC3C
	private IEnumerator fadein_cr()
	{
		float t = 0f;
		this.starSprite.enabled = true;
		while (t < this.fadeTime)
		{
			this.starSprite.color = new Color(1f, 1f, 1f, t / this.fadeTime);
			for (int i = 0; i < this.starClones.Length; i++)
			{
				this.starClones[i].enabled = true;
				this.starClones[i].color = new Color(1f, 1f, 1f, t / this.fadeTime);
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		this.starSprite.color = new Color(1f, 1f, 1f, 1f);
		for (int j = 0; j < this.starClones.Length; j++)
		{
			this.starClones[j].color = new Color(1f, 1f, 1f, 1f);
		}
		for (int k = 0; k < this.twinkleClones.Length; k++)
		{
			this.twinkleClones[k].color = new Color(1f, 1f, 1f, 1f);
		}
		base.StartCoroutine(this.twinkle_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x0012F858 File Offset: 0x0012DC58
	private IEnumerator twinkle_cr()
	{
		this.twinkleSprite.enabled = true;
		for (int i = 0; i < this.twinkleClones.Length; i++)
		{
			this.twinkleClones[i].enabled = true;
		}
		for (;;)
		{
			this.getSecond = UnityEngine.Random.Range(this.minSecond, this.maxSecond);
			if (this.fadeIn)
			{
				float t = 0f;
				while (t < this.fadeTime)
				{
					this.twinkleSprite.color = new Color(1f, 1f, 1f, t / this.fadeTime);
					for (int j = 0; j < this.twinkleClones.Length; j++)
					{
						this.twinkleClones[j].color = new Color(1f, 1f, 1f, t / this.fadeTime);
					}
					t += CupheadTime.Delta;
					yield return null;
				}
				this.twinkleSprite.color = new Color(1f, 1f, 1f, 1f);
				for (int k = 0; k < this.twinkleClones.Length; k++)
				{
					this.twinkleClones[k].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			else
			{
				float t2 = 0f;
				while (t2 < this.fadeTime)
				{
					this.twinkleSprite.color = new Color(1f, 1f, 1f, 1f - t2 / this.fadeTime);
					for (int l = 0; l < this.twinkleClones.Length; l++)
					{
						this.twinkleClones[l].color = new Color(1f, 1f, 1f, 1f - t2 / this.fadeTime);
					}
					t2 += CupheadTime.Delta;
					yield return null;
				}
				this.twinkleSprite.color = new Color(1f, 1f, 1f, 0f);
				for (int m = 0; m < this.twinkleClones.Length; m++)
				{
					this.twinkleClones[m].color = new Color(1f, 1f, 1f, 0f);
				}
				yield return CupheadTime.WaitForSeconds(this, this.getSecond);
			}
			this.fadeIn = !this.fadeIn;
		}
		yield break;
	}

	// Token: 0x0400296E RID: 10606
	[SerializeField]
	private float minSecond;

	// Token: 0x0400296F RID: 10607
	[SerializeField]
	private float maxSecond;

	// Token: 0x04002970 RID: 10608
	private float getSecond;

	// Token: 0x04002971 RID: 10609
	private float fadeTime;

	// Token: 0x04002972 RID: 10610
	private float setSpeed;

	// Token: 0x04002973 RID: 10611
	private bool fadeIn;

	// Token: 0x04002974 RID: 10612
	private bool change;

	// Token: 0x04002975 RID: 10613
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04002976 RID: 10614
	private SpriteRenderer twinkleSprite;

	// Token: 0x04002977 RID: 10615
	[SerializeField]
	private SpriteRenderer starSprite;

	// Token: 0x04002978 RID: 10616
	private SpriteRenderer[] twinkleClones;

	// Token: 0x04002979 RID: 10617
	private SpriteRenderer[] starClones;
}
