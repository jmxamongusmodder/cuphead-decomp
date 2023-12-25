using System;
using UnityEngine;

// Token: 0x02000657 RID: 1623
public class FlyingCowboyLevelOverlayScrollingSprite : ScrollingSprite
{
	// Token: 0x060021D5 RID: 8661 RVA: 0x0013B5C8 File Offset: 0x001399C8
	protected override void Start()
	{
		base.Start();
		this.leftRenderer = base.GetComponent<SpriteRenderer>();
		this.rightRenderer = base.copyRenderers.Find((SpriteRenderer renderer) => renderer.transform.position.x > this.leftRenderer.transform.position.x);
		this.rightOverlayRenderers = new SpriteRenderer[this.overlayRenderers.Length];
		for (int i = 0; i < this.overlayRenderers.Length; i++)
		{
			SpriteRenderer spriteRenderer = this.overlayRenderers[i];
			GameObject gameObject = new GameObject(spriteRenderer.gameObject.name);
			SpriteRenderer spriteRenderer2 = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer2.sprite = spriteRenderer.sprite;
			spriteRenderer2.sortingLayerID = spriteRenderer.sortingLayerID;
			spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder;
			spriteRenderer2.enabled = false;
			gameObject.transform.SetParent(this.rightRenderer.transform, false);
			this.rightOverlayRenderers[i] = spriteRenderer2;
		}
		this.leftOverlaysEnabled = new bool[this.overlayRenderers.Length];
		this.rightOverlaysEnabled = new bool[this.overlayRenderers.Length];
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x0013B6C4 File Offset: 0x00139AC4
	protected override void onLoop()
	{
		base.onLoop();
		bool[] array = this.leftOverlaysEnabled;
		this.leftOverlaysEnabled = this.rightOverlaysEnabled;
		this.rightOverlaysEnabled = array;
		for (int i = 0; i < this.rightOverlaysEnabled.Length; i++)
		{
			this.rightOverlaysEnabled[i] = (UnityEngine.Random.value < this.overlayProbability);
		}
		FlyingCowboyLevelOverlayScrollingSprite.toggleOverlays(this.overlayRenderers, this.leftOverlaysEnabled);
		FlyingCowboyLevelOverlayScrollingSprite.toggleOverlays(this.rightOverlayRenderers, this.rightOverlaysEnabled);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x0013B744 File Offset: 0x00139B44
	private static void toggleOverlays(SpriteRenderer[] overlayRenderers, bool[] activeStatus)
	{
		for (int i = 0; i < overlayRenderers.Length; i++)
		{
			overlayRenderers[i].enabled = activeStatus[i];
		}
	}

	// Token: 0x04002A8B RID: 10891
	[SerializeField]
	[Range(0f, 1f)]
	private float overlayProbability;

	// Token: 0x04002A8C RID: 10892
	[SerializeField]
	private SpriteRenderer[] overlayRenderers;

	// Token: 0x04002A8D RID: 10893
	private SpriteRenderer leftRenderer;

	// Token: 0x04002A8E RID: 10894
	private SpriteRenderer rightRenderer;

	// Token: 0x04002A8F RID: 10895
	private SpriteRenderer[] rightOverlayRenderers;

	// Token: 0x04002A90 RID: 10896
	private bool[] leftOverlaysEnabled;

	// Token: 0x04002A91 RID: 10897
	private bool[] rightOverlaysEnabled;
}
