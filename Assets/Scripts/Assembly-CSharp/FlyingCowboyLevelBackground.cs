using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public class FlyingCowboyLevelBackground : AbstractPausableComponent
{
	// Token: 0x06002101 RID: 8449 RVA: 0x00130D40 File Offset: 0x0012F140
	private void Start()
	{
		this.sunsetInitialY = this.skyLoopTransform.position.y;
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x00130D68 File Offset: 0x0012F168
	private void Update()
	{
		if (this.sunsetTimeElapsed < this.sunsetDuration)
		{
			this.sunsetTimeElapsed += CupheadTime.Delta;
			Vector3 position = this.skyLoopTransform.position;
			position.y = Mathf.Lerp(this.sunsetInitialY, this.sunsetTargetY, this.sunsetTimeElapsed / this.sunsetDuration);
			this.skyLoopTransform.position = position;
		}
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x00130DDC File Offset: 0x0012F1DC
	public void BeginTransition()
	{
		if (this.transitionStarted)
		{
			return;
		}
		this.transitionStarted = true;
		this.initialScrollingMidLayer.looping = false;
		SpriteRenderer spriteRenderer = null;
		foreach (SpriteRenderer spriteRenderer2 in this.initialScrollingMidLayer.copyRenderers)
		{
			if (spriteRenderer == null || spriteRenderer.transform.position.x < spriteRenderer2.transform.position.x)
			{
				spriteRenderer = spriteRenderer2;
			}
		}
		float x = spriteRenderer.sprite.bounds.size.x;
		float x2 = this.transitionBackground.GetComponent<SpriteRenderer>().bounds.size.x;
		Vector3 position = this.transitionBackground.transform.position;
		position.x = spriteRenderer.transform.position.x + x * 0.5f + x2 * 0.5f - this.initialScrollingMidLayer.speed * CupheadTime.Delta;
		this.transitionBackground.transform.position = position;
		this.transitionBackground.SetActive(true);
		Vector3 position2 = this.phase3Background.transform.position;
		position2.x = position.x + x2 - this.phase3Scrolling.offset;
		this.phase3Background.transform.position = position2;
		this.phase3Background.SetActive(true);
		base.StartCoroutine(this.transitionScroll_cr(this.initialScrollingMidLayer.speed, x));
		position2 = this.phase3Foreground.transform.position;
		position2.x = this.transitionBackground.transform.position.x;
		this.phase3Foreground.transform.position = position2;
		this.phase3Foreground.SetActive(true);
		base.StartCoroutine(this.foregroundTransitionScroll_cr(this.initialScrollingMidLayer.speed));
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x00131018 File Offset: 0x0012F418
	private IEnumerator transitionScroll_cr(float speed, float size)
	{
		float displacement;
		for (float totalDisplacement = 0f; totalDisplacement < 3f * size; totalDisplacement += displacement)
		{
			yield return null;
			displacement = speed * CupheadTime.Delta;
			Vector3 position = this.transitionBackground.transform.position;
			position.x -= displacement;
			this.transitionBackground.transform.position = position;
			if (!this.phase3Scrolling.enabled)
			{
				position = this.phase3Background.transform.position;
				position.x -= displacement;
				this.phase3Background.transform.position = position;
				if (this.phase3Background.transform.position.x < 0f)
				{
					this.phase3Scrolling.enabled = true;
					foreach (ScrollingSpriteSpawner scrollingSpriteSpawner in this.phase3MidSpawners)
					{
						scrollingSpriteSpawner.StartLoop(true);
					}
				}
			}
		}
		this.initialScrollingMidLayer.gameObject.SetActive(false);
		this.transitionBackground.SetActive(false);
		yield break;
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x00131044 File Offset: 0x0012F444
	private IEnumerator foregroundTransitionScroll_cr(float speed)
	{
		Transform transform = this.phase3Foreground.transform;
		transform.AddPosition(400f, 0f, 0f);
		Transform startTransform = this.phase3ForegroundStart.transform;
		bool initialPropsDisabled = false;
		while (transform.position.x > 0f)
		{
			yield return null;
			float positionX = startTransform.position.x;
			if (!initialPropsDisabled && positionX <= 2480f)
			{
				initialPropsDisabled = true;
				foreach (ScrollingSpriteSpawner scrollingSpriteSpawner in this.initialFGSpawners)
				{
					scrollingSpriteSpawner.StopAllCoroutines();
					scrollingSpriteSpawner.enabled = false;
				}
			}
			if (speed != this.phase3ForegroundScrolling.speed && positionX <= 2080f)
			{
				speed = this.phase3ForegroundScrolling.speed;
			}
			Vector3 position = transform.position;
			position.x -= speed * CupheadTime.Delta;
			transform.position = position;
		}
		this.phase3ForegroundScrolling.enabled = true;
		foreach (ScrollingSpriteSpawner scrollingSpriteSpawner2 in this.phase3FGSpawners)
		{
			scrollingSpriteSpawner2.StartLoop(true);
		}
		yield break;
	}

	// Token: 0x0400299A RID: 10650
	[SerializeField]
	private float sunsetDuration;

	// Token: 0x0400299B RID: 10651
	[SerializeField]
	private float sunsetTargetY;

	// Token: 0x0400299C RID: 10652
	[SerializeField]
	private Transform skyLoopTransform;

	// Token: 0x0400299D RID: 10653
	[SerializeField]
	private FlyingCowboyLevelOverlayScrollingSprite initialScrollingMidLayer;

	// Token: 0x0400299E RID: 10654
	[SerializeField]
	private ScrollingSpriteSpawner[] initialFGSpawners;

	// Token: 0x0400299F RID: 10655
	[SerializeField]
	private GameObject transitionBackground;

	// Token: 0x040029A0 RID: 10656
	[SerializeField]
	private GameObject phase3Background;

	// Token: 0x040029A1 RID: 10657
	[SerializeField]
	private ScrollingSprite phase3Scrolling;

	// Token: 0x040029A2 RID: 10658
	[SerializeField]
	private ScrollingSpriteSpawner[] phase3MidSpawners;

	// Token: 0x040029A3 RID: 10659
	[SerializeField]
	private GameObject phase3Foreground;

	// Token: 0x040029A4 RID: 10660
	[SerializeField]
	private GameObject phase3ForegroundStart;

	// Token: 0x040029A5 RID: 10661
	[SerializeField]
	private ScrollingSprite phase3ForegroundScrolling;

	// Token: 0x040029A6 RID: 10662
	[SerializeField]
	private ScrollingSpriteSpawner[] phase3FGSpawners;

	// Token: 0x040029A7 RID: 10663
	private float sunsetTimeElapsed;

	// Token: 0x040029A8 RID: 10664
	private float sunsetInitialY;

	// Token: 0x040029A9 RID: 10665
	private bool transitionStarted;
}
