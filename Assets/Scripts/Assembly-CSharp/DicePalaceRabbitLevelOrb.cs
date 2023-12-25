using System;
using UnityEngine;

// Token: 0x020005DF RID: 1503
public class DicePalaceRabbitLevelOrb : AbstractProjectile
{
	// Token: 0x06001DB7 RID: 7607 RVA: 0x00111318 File Offset: 0x0010F718
	protected override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x00111336 File Offset: 0x0010F736
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x00111354 File Offset: 0x0010F754
	public void SetAsGold(bool isGold)
	{
		if (isGold)
		{
			base.animator.SetTrigger("Gold");
		}
		else
		{
			base.animator.SetTrigger("Blue");
		}
	}
}
