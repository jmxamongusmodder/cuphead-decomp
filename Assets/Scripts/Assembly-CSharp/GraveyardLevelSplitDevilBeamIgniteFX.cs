using System;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
public class GraveyardLevelSplitDevilBeamIgniteFX : Effect
{
	// Token: 0x060024FC RID: 9468 RVA: 0x0015B064 File Offset: 0x00159464
	public Effect Create(Vector3 position, Animator fireBeamAnimator)
	{
		GraveyardLevelSplitDevilBeamIgniteFX graveyardLevelSplitDevilBeamIgniteFX = base.Create(position) as GraveyardLevelSplitDevilBeamIgniteFX;
		graveyardLevelSplitDevilBeamIgniteFX.fireBeamAnimator = fireBeamAnimator;
		graveyardLevelSplitDevilBeamIgniteFX.UpdateFade(1f);
		return graveyardLevelSplitDevilBeamIgniteFX;
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x0015B094 File Offset: 0x00159494
	private void Update()
	{
		this.frameTimer += CupheadTime.Delta;
		while (this.frameTimer > 0.041666668f)
		{
			this.frameTimer -= 0.041666668f;
			this.UpdateFade(0.25f);
		}
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x0015B0EC File Offset: 0x001594EC
	private void UpdateFade(float amount)
	{
		bool @bool = this.fireBeamAnimator.GetBool("Smoke");
		foreach (SpriteRenderer spriteRenderer in this.groundRends)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + ((!@bool) ? (-amount) : amount), 0f, 1f));
		}
		foreach (SpriteRenderer spriteRenderer2 in this.noGroundRends)
		{
			spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + ((!@bool) ? amount : (-amount)), 0f, 1f));
		}
	}

	// Token: 0x04002DAB RID: 11691
	[SerializeField]
	private Animator fireBeamAnimator;

	// Token: 0x04002DAC RID: 11692
	[SerializeField]
	private SpriteRenderer[] groundRends;

	// Token: 0x04002DAD RID: 11693
	[SerializeField]
	private SpriteRenderer[] noGroundRends;

	// Token: 0x04002DAE RID: 11694
	private float frameTimer;
}
