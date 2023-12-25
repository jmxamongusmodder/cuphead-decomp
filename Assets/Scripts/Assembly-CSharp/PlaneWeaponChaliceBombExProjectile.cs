using System;
using UnityEngine;

// Token: 0x02000AB9 RID: 2745
public class PlaneWeaponChaliceBombExProjectile : AbstractProjectile
{
	// Token: 0x060041F4 RID: 16884 RVA: 0x0023A314 File Offset: 0x00238714
	protected override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.DamageRate += this.DamageRateIncrease;
		damageDealer.SetRate(this.DamageRate);
		this.chompFxPrefab.Create(this.chompFxRoot.position);
		this.state = PlaneWeaponChaliceBombExProjectile.State.Frozen;
		this.speed = 0f;
		this.timeSinceFrozen = 0f;
		AudioManager.Play("player_plane_weapon_ex_chomp");
		this.emitAudioFromObject.Add("player_plane_weapon_ex_chomp");
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x0023A398 File Offset: 0x00238798
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		PlaneWeaponChaliceBombExProjectile.State state = this.state;
		if (state != PlaneWeaponChaliceBombExProjectile.State.Idle)
		{
			if (state == PlaneWeaponChaliceBombExProjectile.State.Frozen)
			{
				this.timeSinceFrozen += CupheadTime.FixedDelta;
				if (this.timeSinceFrozen > this.FreezeTime)
				{
					this.state = PlaneWeaponChaliceBombExProjectile.State.Idle;
				}
			}
		}
		else
		{
			this.Velocity.y = this.Velocity.y - this.Gravity * CupheadTime.FixedDelta;
			base.transform.position += this.Velocity * CupheadTime.FixedDelta;
			base.transform.rotation = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.Velocity));
		}
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x0023A472 File Offset: 0x00238872
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0023A483 File Offset: 0x00238883
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x0023A4A3 File Offset: 0x002388A3
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x0023A4B2 File Offset: 0x002388B2
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04004855 RID: 18517
	public float MaxSpeed;

	// Token: 0x04004856 RID: 18518
	public float Acceleration;

	// Token: 0x04004857 RID: 18519
	public float FreezeTime;

	// Token: 0x04004858 RID: 18520
	[SerializeField]
	private Effect chompFxPrefab;

	// Token: 0x04004859 RID: 18521
	[SerializeField]
	private Transform chompFxRoot;

	// Token: 0x0400485A RID: 18522
	public Vector3 Velocity;

	// Token: 0x0400485B RID: 18523
	public float Gravity;

	// Token: 0x0400485C RID: 18524
	public float DamageRateIncrease;

	// Token: 0x0400485D RID: 18525
	private PlaneWeaponChaliceBombExProjectile.State state;

	// Token: 0x0400485E RID: 18526
	private float timeSinceFrozen;

	// Token: 0x0400485F RID: 18527
	public float speed;

	// Token: 0x02000ABA RID: 2746
	public enum State
	{
		// Token: 0x04004861 RID: 18529
		Idle,
		// Token: 0x04004862 RID: 18530
		Frozen
	}
}
