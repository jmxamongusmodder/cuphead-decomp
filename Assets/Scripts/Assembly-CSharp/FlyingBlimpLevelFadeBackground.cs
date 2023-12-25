using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class FlyingBlimpLevelFadeBackground : ScrollingSprite
{
	// Token: 0x0600209F RID: 8351 RVA: 0x0012CA56 File Offset: 0x0012AE56
	protected override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.DisableSprites), 1);
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x0012CA74 File Offset: 0x0012AE74
	private void DisableSprites()
	{
		this.fadeTime = 10f;
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

	// Token: 0x060020A1 RID: 8353 RVA: 0x0012CB50 File Offset: 0x0012AF50
	protected override void Update()
	{
		base.Update();
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && !this.startedChange)
		{
			this.startedChange = true;
			this.StartChange();
		}
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x0012CB81 File Offset: 0x0012AF81
	private void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x0012CB90 File Offset: 0x0012AF90
	private IEnumerator change_cr()
	{
		float t = 0f;
		float alphaValue = 1f;
		float startSpeed = this.speed;
		float endSpeed = this.speed + this.speed * 0.3f;
		while (t < this.fadeTime)
		{
			for (int j = 0; j < this.replacementClones.Length; j++)
			{
				if (this.replacementClones[j].transform != null)
				{
					this.replacementClones[j].enabled = true;
					this.replacementClones[j].color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			if (this.fadeOriginal)
			{
				for (int i = 0; i < this.currentClones.Length; i++)
				{
					if (this.currentClones[i].transform != null)
					{
						this.currentClones[i].color = new Color(1f, 1f, 1f, alphaValue - t / this.fadeTime);
						if (alphaValue <= 0f)
						{
							this.currentClones[i].color = new Color(1f, 1f, 1f, 0f);
							yield return null;
						}
					}
				}
			}
			this.speed = Mathf.Lerp(startSpeed, endSpeed, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int k = 0; k < this.replacementClones.Length; k++)
		{
			this.replacementClones[k].color = new Color(1f, 1f, 1f, 1f);
		}
		if (this.fadeOriginal)
		{
			for (int l = 0; l < this.currentClones.Length; l++)
			{
				this.currentClones[l].enabled = false;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x04002921 RID: 10529
	public bool fadeOriginal;

	// Token: 0x04002922 RID: 10530
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04002923 RID: 10531
	[SerializeField]
	private Transform replacementSprite;

	// Token: 0x04002924 RID: 10532
	private SpriteRenderer[] replacementClones;

	// Token: 0x04002925 RID: 10533
	private SpriteRenderer current;

	// Token: 0x04002926 RID: 10534
	private SpriteRenderer[] currentClones;

	// Token: 0x04002927 RID: 10535
	private float fadeTime;

	// Token: 0x04002928 RID: 10536
	private bool startedChange;
}
