using System;
using UnityEngine;

// Token: 0x02000A8B RID: 2699
public class WeaponUpshotProjectile : AbstractProjectile
{
	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06004086 RID: 16518 RVA: 0x00232043 File Offset: 0x00230443
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x00232048 File Offset: 0x00230448
	protected override void Start()
	{
		base.Start();
		this.damageDealer.isDLCWeapon = true;
		AbstractPlayerController player = PlayerManager.GetPlayer(this.PlayerId);
		this.onLeft = (player.transform.localScale.x < 0f);
		this.startAngle = base.transform.eulerAngles.z;
		this.SetAngle();
	}

	// Token: 0x06004088 RID: 16520 RVA: 0x002320B2 File Offset: 0x002304B2
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.UpdateSpeed();
		this.Move();
	}

	// Token: 0x06004089 RID: 16521 RVA: 0x002320C8 File Offset: 0x002304C8
	private void UpdateSpeed()
	{
		if (this.time < this.timeToArc)
		{
			this.time += CupheadTime.FixedDelta;
			this.ySpeed = this.ySpeedMinMax.GetFloatAt(this.time / this.timeToArc);
			this.ySpeed = ((!this.onLeft) ? this.ySpeed : (-this.ySpeed));
			this.SetAngle();
		}
	}

	// Token: 0x0600408A RID: 16522 RVA: 0x00232140 File Offset: 0x00230540
	private void Move()
	{
		if (base.dead)
		{
			return;
		}
		Vector3 vector = new Vector3(this.xSpeed, this.ySpeed);
		Quaternion rotation = Quaternion.Euler(0f, 0f, this.startAngle);
		vector = rotation * vector;
		base.transform.position += vector * CupheadTime.FixedDelta;
	}

	// Token: 0x0600408B RID: 16523 RVA: 0x002321AC File Offset: 0x002305AC
	private void SetAngle()
	{
		int num = Mathf.RoundToInt(this.startAngle);
		if (num != 0)
		{
			if (num == 45)
			{
				base.transform.SetEulerAngles(null, null, new float?(this.time * 2f / this.timeToArc * 45f));
				return;
			}
			if (num == 90)
			{
				base.transform.SetEulerAngles(null, null, new float?((float)((!this.onLeft) ? 45 : -225) + this.time * 2f / this.timeToArc * (float)((!this.onLeft) ? 45 : -45)));
				return;
			}
			if (num == 135)
			{
				base.transform.SetEulerAngles(null, null, new float?(180f + this.time * 2f / this.timeToArc * -45f));
				return;
			}
			if (num != 180)
			{
				if (num == 225)
				{
					base.transform.SetEulerAngles(null, null, new float?(270f + this.time * 2f / this.timeToArc * -45f));
					return;
				}
				if (num == 270)
				{
					base.transform.SetEulerAngles(null, null, new float?((float)((!this.onLeft) ? 225 : -45) + this.time * 2f / this.timeToArc * (float)((!this.onLeft) ? 45 : -45)));
					return;
				}
				if (num != 315)
				{
					return;
				}
				base.transform.SetEulerAngles(null, null, new float?(270f + this.time * 2f / this.timeToArc * 45f));
				return;
			}
		}
		base.transform.SetEulerAngles(null, null, new float?((float)((!this.onLeft) ? -45 : 225) + this.time * 2f / this.timeToArc * (float)((!this.onLeft) ? 45 : -45)));
	}

	// Token: 0x0600408C RID: 16524 RVA: 0x00232464 File Offset: 0x00230864
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600408D RID: 16525 RVA: 0x00232484 File Offset: 0x00230884
	protected override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if (hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled)
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play("player_weapon_peashot_miss");
			}
		}
	}

	// Token: 0x0600408E RID: 16526 RVA: 0x002324ED File Offset: 0x002308ED
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x0400474A RID: 18250
	public MinMax ySpeedMinMax;

	// Token: 0x0400474B RID: 18251
	public float timeToArc;

	// Token: 0x0400474C RID: 18252
	public float xSpeed;

	// Token: 0x0400474D RID: 18253
	private float ySpeed;

	// Token: 0x0400474E RID: 18254
	private float time;

	// Token: 0x0400474F RID: 18255
	private bool onLeft;

	// Token: 0x04004750 RID: 18256
	private float startAngle;
}
