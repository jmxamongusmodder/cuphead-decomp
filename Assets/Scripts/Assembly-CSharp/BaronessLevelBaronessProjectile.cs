using System;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class BaronessLevelBaronessProjectile : AbstractProjectile
{
	// Token: 0x0600155A RID: 5466 RVA: 0x000BF354 File Offset: 0x000BD754
	protected override void Start()
	{
		base.Start();
		this.health = (float)base.GetComponentInParent<BaronessLevelBaronessProjectileBunch>().properties.projectileHP;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x000BF3A1 File Offset: 0x000BD7A1
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x000BF3BF File Offset: 0x000BD7BF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x000BF3DD File Offset: 0x000BD7DD
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x000BF408 File Offset: 0x000BD808
	protected override void Die()
	{
		this.deathFX.Create(base.transform.position);
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.FX.SetActive(false);
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x04001EB3 RID: 7859
	[SerializeField]
	private Effect deathFX;

	// Token: 0x04001EB4 RID: 7860
	[SerializeField]
	private GameObject FX;

	// Token: 0x04001EB5 RID: 7861
	private DamageReceiver damageReceiver;

	// Token: 0x04001EB6 RID: 7862
	private float health;
}
