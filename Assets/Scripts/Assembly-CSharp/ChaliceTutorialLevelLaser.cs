using System;
using UnityEngine;

// Token: 0x02000526 RID: 1318
public class ChaliceTutorialLevelLaser : AbstractCollidableObject
{
	// Token: 0x060017B9 RID: 6073 RVA: 0x000D5CC8 File Offset: 0x000D40C8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.level.resetParryables = true;
		AudioManager.Play("sfx_rip_fail");
		this.hitAnimator.transform.position = new Vector3(base.transform.position.x + this.coll.bounds.size.x / 2f, hit.transform.position.y + 100f);
		this.hitAnimator.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.hitAnimator.Play("Hit");
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x000D5D91 File Offset: 0x000D4191
	private void Enabled()
	{
		base.animator.SetBool("On", true);
		this.coll.enabled = true;
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000D5DB0 File Offset: 0x000D41B0
	private void Disabled()
	{
		base.animator.SetBool("On", false);
		this.coll.enabled = false;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x000D5DCF File Offset: 0x000D41CF
	private void Update()
	{
		if (!this.parryable.isDeactivated)
		{
			this.Enabled();
		}
		else
		{
			this.Disabled();
		}
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000D5DF2 File Offset: 0x000D41F2
	private void AniEvent_SFX_Open()
	{
		AudioManager.Play("sfx_rip_open");
	}

	// Token: 0x040020E7 RID: 8423
	[SerializeField]
	private ChaliceTutorialLevel level;

	// Token: 0x040020E8 RID: 8424
	[SerializeField]
	private ChaliceTutorialLevelParryable parryable;

	// Token: 0x040020E9 RID: 8425
	[SerializeField]
	private Collider2D coll;

	// Token: 0x040020EA RID: 8426
	[SerializeField]
	private Animator hitAnimator;
}
