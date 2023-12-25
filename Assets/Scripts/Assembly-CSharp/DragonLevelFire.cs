using System;
using UnityEngine;

// Token: 0x020005EF RID: 1519
public class DragonLevelFire : AbstractCollidableObject
{
	// Token: 0x06001E32 RID: 7730 RVA: 0x00115F24 File Offset: 0x00114324
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x00115F37 File Offset: 0x00114337
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x00115F4F File Offset: 0x0011434F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x00115F78 File Offset: 0x00114378
	public void SetColliderEnabled(bool enabled)
	{
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = enabled;
		}
	}

	// Token: 0x040026F7 RID: 9975
	private DamageDealer damageDealer;

	// Token: 0x040026F8 RID: 9976
	private Vector3 localPosition;

	// Token: 0x040026F9 RID: 9977
	private Vector3 localScale;
}
