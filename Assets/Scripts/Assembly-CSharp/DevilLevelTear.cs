using System;
using UnityEngine;

// Token: 0x02000599 RID: 1433
public class DevilLevelTear : AbstractProjectile
{
	// Token: 0x06001B78 RID: 7032 RVA: 0x000FB1D8 File Offset: 0x000F95D8
	public DevilLevelTear CreateTear(Vector2 position, float speed)
	{
		DevilLevelTear devilLevelTear = this.InstantiatePrefab<DevilLevelTear>();
		devilLevelTear.transform.position = position;
		devilLevelTear.speed = speed;
		devilLevelTear.animator.Play("Drop_" + UnityEngine.Random.Range(1, 7).ToStringInvariant());
		return devilLevelTear;
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x000FB226 File Offset: 0x000F9626
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000FB244 File Offset: 0x000F9644
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		base.transform.AddPosition(0f, -this.speed * CupheadTime.FixedDelta, 0f);
	}

	// Token: 0x040024A7 RID: 9383
	private float speed;
}
