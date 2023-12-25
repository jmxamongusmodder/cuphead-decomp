using System;
using UnityEngine;

// Token: 0x020006CA RID: 1738
public class GraveyardLevelSplitDevilBeamTrailFX : Effect
{
	// Token: 0x06002500 RID: 9472 RVA: 0x0015B228 File Offset: 0x00159628
	public Effect Create(Vector3 position, Vector3 scale, GraveyardLevelSplitDevilBeam main, int anim)
	{
		GraveyardLevelSplitDevilBeamTrailFX graveyardLevelSplitDevilBeamTrailFX = base.Create(position) as GraveyardLevelSplitDevilBeamTrailFX;
		graveyardLevelSplitDevilBeamTrailFX.transform.localScale = scale;
		graveyardLevelSplitDevilBeamTrailFX.main = main;
		graveyardLevelSplitDevilBeamTrailFX.animator.Play(anim.ToString());
		graveyardLevelSplitDevilBeamTrailFX.animator.Update(0f);
		graveyardLevelSplitDevilBeamTrailFX.rend.sortingOrder = -5 + anim;
		graveyardLevelSplitDevilBeamTrailFX.UpdateFade(1f);
		return graveyardLevelSplitDevilBeamTrailFX;
	}

	// Token: 0x06002501 RID: 9473 RVA: 0x0015B29C File Offset: 0x0015969C
	private void Update()
	{
		this.frameTimer += CupheadTime.Delta;
		while (this.frameTimer > 0.041666668f)
		{
			this.frameTimer -= 0.041666668f;
			this.UpdateFade(0.25f);
		}
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x0015B2F4 File Offset: 0x001596F4
	private void UpdateFade(float amount)
	{
		this.rend.color = new Color(this.rend.color.r, this.rend.color.g, this.rend.color.b, Mathf.Clamp(this.rend.color.a + (this.main.devil.isAngel ? (-amount) : amount), 0f, 1f));
	}

	// Token: 0x04002DAF RID: 11695
	[SerializeField]
	private GraveyardLevelSplitDevilBeam main;

	// Token: 0x04002DB0 RID: 11696
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04002DB1 RID: 11697
	private float frameTimer;
}
