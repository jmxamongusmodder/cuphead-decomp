using System;
using UnityEngine;

// Token: 0x02000A70 RID: 2672
public class WeaponChargeExBurst : AbstractProjectile
{
	// Token: 0x06003FD4 RID: 16340 RVA: 0x0022D52E File Offset: 0x0022B92E
	protected override void Start()
	{
		base.Start();
		base.GetComponent<SpriteRenderer>().flipX = Rand.Bool();
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x0022D546 File Offset: 0x0022B946
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x0022D56E File Offset: 0x0022B96E
	private void OnEffectComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040046B1 RID: 18097
	public const float Offset = 125f;
}
