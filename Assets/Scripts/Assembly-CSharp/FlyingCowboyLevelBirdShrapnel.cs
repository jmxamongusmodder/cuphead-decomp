using System;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class FlyingCowboyLevelBirdShrapnel : BasicProjectile
{
	// Token: 0x06002137 RID: 8503 RVA: 0x00133410 File Offset: 0x00131810
	public override AbstractProjectile Create()
	{
		AbstractProjectile abstractProjectile = base.Create();
		abstractProjectile.animator.Update(0f);
		abstractProjectile.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
		abstractProjectile.animator.Update(0f);
		abstractProjectile.animator.RoundFrame(0);
		abstractProjectile.GetComponent<SpriteRenderer>().flipY = Rand.Bool();
		return abstractProjectile;
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x0013347D File Offset: 0x0013187D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		AudioManager.Play("sfx_dlc_cowgirl_p1_dynamitehitplayer");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_dynamitehitplayer");
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x001334A1 File Offset: 0x001318A1
	private void animationEvent_LoopMiddleReached()
	{
		this.trailBRenderer.enabled = true;
		this.trailBRenderer.flipY = Rand.Bool();
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x001334BF File Offset: 0x001318BF
	private void animationEvent_LoopEndReached()
	{
		this.trailARenderer.flipY = Rand.Bool();
	}

	// Token: 0x040029CF RID: 10703
	[SerializeField]
	private SpriteRenderer trailARenderer;

	// Token: 0x040029D0 RID: 10704
	[SerializeField]
	private SpriteRenderer trailBRenderer;
}
