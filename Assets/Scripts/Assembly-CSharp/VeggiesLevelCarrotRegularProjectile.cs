using System;
using UnityEngine;

// Token: 0x0200084A RID: 2122
public class VeggiesLevelCarrotRegularProjectile : AbstractProjectile
{
	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06003120 RID: 12576 RVA: 0x001CCF07 File Offset: 0x001CB307
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x001CCF10 File Offset: 0x001CB310
	public VeggiesLevelCarrotRegularProjectile Create(VeggiesLevelCarrot parent, Vector2 pos, float speed, float rotation)
	{
		VeggiesLevelCarrotRegularProjectile veggiesLevelCarrotRegularProjectile = this.Create() as VeggiesLevelCarrotRegularProjectile;
		veggiesLevelCarrotRegularProjectile.CollisionDeath.None();
		veggiesLevelCarrotRegularProjectile.DamagesType.OnlyPlayer();
		veggiesLevelCarrotRegularProjectile.Init(parent, pos, speed, rotation);
		return veggiesLevelCarrotRegularProjectile;
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x001CCF4C File Offset: 0x001CB34C
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		base.transform.position += base.transform.right * (this.speed * CupheadTime.FixedDelta);
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x001CCF9D File Offset: 0x001CB39D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.Die;
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x001CCFBD File Offset: 0x001CB3BD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x001CCFDC File Offset: 0x001CB3DC
	private void Init(VeggiesLevelCarrot parent, Vector2 pos, float speed, float rotation)
	{
		this.parent = parent;
		this.speed = speed;
		parent.OnDeathEvent += this.Die;
		base.transform.position = pos;
		base.transform.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x001CD041 File Offset: 0x001CB441
	protected override void Die()
	{
		AudioManager.Play("level_veggies_carrot_projectile_death");
		base.Die();
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
	}

	// Token: 0x040039BC RID: 14780
	private float speed;

	// Token: 0x040039BD RID: 14781
	private VeggiesLevelCarrot parent;
}
