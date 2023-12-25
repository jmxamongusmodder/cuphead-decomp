using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
public class DicePalaceFlyingMemoryLevelCard : ParrySwitch
{
	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06001CED RID: 7405 RVA: 0x00109218 File Offset: 0x00107618
	// (set) Token: 0x06001CEE RID: 7406 RVA: 0x00109220 File Offset: 0x00107620
	public bool flippedUp { get; private set; }

	// Token: 0x06001CEF RID: 7407 RVA: 0x00109229 File Offset: 0x00107629
	protected override void Awake()
	{
		base.Awake();
		this.flippedUp = false;
		this.flippedDownCard = base.GetComponent<SpriteRenderer>().sprite;
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x0010924C File Offset: 0x0010764C
	public void FlipUp()
	{
		base.StartCoroutine(this.rotate_cr(0f, 360f, 0.6f));
		this.flippedUpCard = this.flippedUpCards[(int)this.card];
		base.GetComponent<SpriteRenderer>().sprite = this.flippedUpCard;
		this.flippedUp = true;
		this.pinkDot.enabled = false;
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x001092AC File Offset: 0x001076AC
	public void EnableCards()
	{
		if (!this.permanentlyFlipped)
		{
			if (this.flippedUp)
			{
				base.StartCoroutine(this.rotate_cr(0f, 360f, 0.6f));
				base.GetComponent<SpriteRenderer>().sprite = this.flippedDownCard;
				this.flippedUp = false;
			}
			this.pinkDot.enabled = true;
			base.StartCoroutine(this.fade_pink_cr(false));
			base.GetComponent<Collider2D>().enabled = true;
		}
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x00109329 File Offset: 0x00107729
	public void DisableCard()
	{
		base.GetComponent<Collider2D>().enabled = false;
		if (!this.flippedUp || !this.permanentlyFlipped)
		{
			base.StartCoroutine(this.fade_pink_cr(true));
		}
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x0010935C File Offset: 0x0010775C
	private IEnumerator rotate_cr(float start, float end, float time)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetEulerAngles(new float?(0f), new float?(EaseUtils.Ease(this.ROTATION_EASE, start, end, val)), new float?(0f));
			t += Time.deltaTime;
			yield return null;
		}
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		yield return null;
		yield break;
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0010938C File Offset: 0x0010778C
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.FlipUp();
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0010939C File Offset: 0x0010779C
	private IEnumerator fade_pink_cr(bool fadingOut)
	{
		if (fadingOut)
		{
			float t = 0f;
			while (t < this.fadeTime)
			{
				this.pinkDot.color = new Color(1f, 1f, 1f, 1f - t / this.fadeTime);
				t += CupheadTime.Delta;
				yield return null;
			}
			this.pinkDot.color = new Color(1f, 1f, 1f, 0f);
		}
		else
		{
			float t2 = 0f;
			while (t2 < this.fadeTime)
			{
				this.pinkDot.color = new Color(1f, 1f, 1f, t2 / this.fadeTime);
				t2 += CupheadTime.Delta;
				yield return null;
			}
			this.pinkDot.color = new Color(1f, 1f, 1f, 1f);
		}
		yield break;
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x001093BE File Offset: 0x001077BE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.flippedUpCards = null;
		this.flippedUpCard = null;
		this.flippedDownCard = null;
	}

	// Token: 0x040025D7 RID: 9687
	public bool permanentlyFlipped;

	// Token: 0x040025D8 RID: 9688
	private const float ROTATION_TIME = 0.6f;

	// Token: 0x040025D9 RID: 9689
	private const float ROTATION_BACK = 360f;

	// Token: 0x040025DA RID: 9690
	private EaseUtils.EaseType ROTATION_EASE = EaseUtils.EaseType.easeOutBack;

	// Token: 0x040025DB RID: 9691
	[SerializeField]
	private Sprite[] flippedUpCards;

	// Token: 0x040025DC RID: 9692
	[SerializeField]
	private SpriteRenderer pinkDot;

	// Token: 0x040025DD RID: 9693
	private Sprite flippedUpCard;

	// Token: 0x040025DE RID: 9694
	private Sprite flippedDownCard;

	// Token: 0x040025DF RID: 9695
	private Coroutine rotationCoroutine;

	// Token: 0x040025E0 RID: 9696
	private float fadeTime = 0.7f;

	// Token: 0x040025E1 RID: 9697
	public DicePalaceFlyingMemoryLevelCard.Card card;

	// Token: 0x020005C8 RID: 1480
	public enum Card
	{
		// Token: 0x040025E3 RID: 9699
		Cuphead,
		// Token: 0x040025E4 RID: 9700
		Chips,
		// Token: 0x040025E5 RID: 9701
		Flowers,
		// Token: 0x040025E6 RID: 9702
		Shield,
		// Token: 0x040025E7 RID: 9703
		Spindle,
		// Token: 0x040025E8 RID: 9704
		Mugman
	}
}
