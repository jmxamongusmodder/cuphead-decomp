using System;
using UnityEngine;

// Token: 0x02000AE6 RID: 2790
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageReceiver))]
public class BasicDamagableProjectile : BasicProjectile
{
	// Token: 0x06004391 RID: 17297 RVA: 0x0023FF08 File Offset: 0x0023E308
	public virtual BasicDamagableProjectile Create(Vector2 position, float rotation, float speed, float health)
	{
		BasicDamagableProjectile basicDamagableProjectile = this.Create(position, rotation, speed) as BasicDamagableProjectile;
		basicDamagableProjectile.health = health;
		return basicDamagableProjectile;
	}

	// Token: 0x06004392 RID: 17298 RVA: 0x0023FF30 File Offset: 0x0023E330
	public virtual BasicDamagableProjectile Create(Vector2 position, float rotation, Vector2 scale, float speed, float health)
	{
		BasicDamagableProjectile basicDamagableProjectile = this.Create(position, rotation, scale, speed) as BasicDamagableProjectile;
		basicDamagableProjectile.health = health;
		return basicDamagableProjectile;
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x0023FF57 File Offset: 0x0023E357
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x0023FF82 File Offset: 0x0023E382
	protected override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x0023FFA1 File Offset: 0x0023E3A1
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06004396 RID: 17302 RVA: 0x0023FFBF File Offset: 0x0023E3BF
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x04004959 RID: 18777
	public float health = 10f;

	// Token: 0x0400495A RID: 18778
	private DamageReceiver damageReceiver;
}
