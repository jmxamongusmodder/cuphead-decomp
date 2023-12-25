using System;
using UnityEngine;

// Token: 0x02000A74 RID: 2676
public class WeaponCrackshotExProjectileChild : BasicProjectile
{
	// Token: 0x06003FF2 RID: 16370 RVA: 0x0022E440 File Offset: 0x0022C840
	protected override void Start()
	{
		base.Start();
		base.animator.SetBool("IsB", Rand.Bool());
		base.animator.Play((!Rand.Bool()) ? "CometStartA" : "CometStartB");
		this.damageDealer.isDLCWeapon = true;
	}

	// Token: 0x06003FF3 RID: 16371 RVA: 0x0022E498 File Offset: 0x0022C898
	protected override void Die()
	{
		base.Die();
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Comet"))
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactCometB" : "ImpactCometA");
		}
		else
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactSmallB" : "ImpactSmallA");
		}
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x0022E516 File Offset: 0x0022C916
	private void OnEffectComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
