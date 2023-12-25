using System;
using UnityEngine;

// Token: 0x02000586 RID: 1414
[RequireComponent(typeof(Collider2D))]
public class DevilLevelSplitDevilVisual : LevelProperties.Devil.Entity
{
	// Token: 0x06001AFB RID: 6907 RVA: 0x000F7F8A File Offset: 0x000F638A
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += base.GetComponentInParent<DevilLevelSplitDevil>().OnDamageTaken;
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x000F7FC5 File Offset: 0x000F63C5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x000F7FDD File Offset: 0x000F63DD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0400243A RID: 9274
	private DamageDealer damageDealer;

	// Token: 0x0400243B RID: 9275
	private DamageReceiver damageReceiver;
}
