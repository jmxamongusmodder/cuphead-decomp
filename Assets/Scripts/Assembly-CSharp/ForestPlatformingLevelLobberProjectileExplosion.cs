using System;
using UnityEngine;

// Token: 0x02000882 RID: 2178
public class ForestPlatformingLevelLobberProjectileExplosion : Effect
{
	// Token: 0x06003292 RID: 12946 RVA: 0x001D685F File Offset: 0x001D4C5F
	protected override void Awake()
	{
		base.Awake();
		AudioManager.Play("level_lobber_projectile_explosion");
		this.emitAudioFromObject.Add("level_lobber_projectile_explosion");
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x001D688C File Offset: 0x001D4C8C
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x001D68A4 File Offset: 0x001D4CA4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04003ADF RID: 15071
	private DamageDealer damageDealer;
}
