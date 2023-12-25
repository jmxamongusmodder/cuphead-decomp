using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
public class VeggiesLevelBeetBabyBullet : AbstractProjectile
{
	// Token: 0x060030E7 RID: 12519 RVA: 0x001CBED0 File Offset: 0x001CA2D0
	public VeggiesLevelBeetBabyBullet Create(float speed, Vector2 pos, float rot)
	{
		VeggiesLevelBeetBabyBullet veggiesLevelBeetBabyBullet = this.Create(pos, rot) as VeggiesLevelBeetBabyBullet;
		veggiesLevelBeetBabyBullet.CollisionDeath.OnlyPlayer();
		veggiesLevelBeetBabyBullet.DamagesType.OnlyPlayer();
		veggiesLevelBeetBabyBullet.speed = speed;
		return veggiesLevelBeetBabyBullet;
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x001CBF0A File Offset: 0x001CA30A
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x001CBF14 File Offset: 0x001CA314
	protected override void Update()
	{
		base.Update();
		if (this.state == VeggiesLevelBeetBabyBullet.State.Dead)
		{
			return;
		}
		base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
		if (base.transform.position.y < (float)Level.Current.Ground)
		{
			this.Die();
		}
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x001CBF98 File Offset: 0x001CA398
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x001CBFAF File Offset: 0x001CA3AF
	protected override void Die()
	{
		base.Die();
		this.state = VeggiesLevelBeetBabyBullet.State.Dead;
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04003991 RID: 14737
	private VeggiesLevelBeetBabyBullet.State state;

	// Token: 0x04003992 RID: 14738
	private float speed;

	// Token: 0x02000841 RID: 2113
	public enum State
	{
		// Token: 0x04003994 RID: 14740
		Go,
		// Token: 0x04003995 RID: 14741
		Dead
	}
}
