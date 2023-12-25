using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200063F RID: 1599
public class FlyingBlimpLevelShootProjectile : AbstractProjectile
{
	// Token: 0x060020D3 RID: 8403 RVA: 0x0012F520 File Offset: 0x0012D920
	public FlyingBlimpLevelShootProjectile Create(Vector2 pos, float rotation, LevelProperties.FlyingBlimp.Shoot properties)
	{
		FlyingBlimpLevelShootProjectile flyingBlimpLevelShootProjectile = base.Create() as FlyingBlimpLevelShootProjectile;
		flyingBlimpLevelShootProjectile.properties = properties;
		flyingBlimpLevelShootProjectile.velocity = properties.speedMin;
		flyingBlimpLevelShootProjectile.transform.position = pos;
		return flyingBlimpLevelShootProjectile;
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x0012F55E File Offset: 0x0012D95E
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x0012F573 File Offset: 0x0012D973
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x0012F591 File Offset: 0x0012D991
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x0012F5B0 File Offset: 0x0012D9B0
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.velocity < this.properties.speedMax)
		{
			this.velocity += this.properties.accelerationTime * CupheadTime.FixedDelta;
			yield return wait;
			base.transform.AddPosition(-this.velocity * CupheadTime.FixedDelta, 0f, 0f);
		}
		this.Die();
		yield return wait;
		yield break;
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x0012F5CB File Offset: 0x0012D9CB
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400296C RID: 10604
	private LevelProperties.FlyingBlimp.Shoot properties;

	// Token: 0x0400296D RID: 10605
	private float velocity;
}
