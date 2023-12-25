using System;
using UnityEngine;

// Token: 0x02000ABC RID: 2748
public class PlaneWeaponChalice3WayExProjectile : AbstractProjectile
{
	// Token: 0x06004203 RID: 16899 RVA: 0x0023A728 File Offset: 0x00238B28
	protected override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		if (this.state == PlaneWeaponChalice3WayExProjectile.State.Idle)
		{
			this.Freeze();
			this.partner.Freeze();
		}
		AudioManager.Play("player_plane_weapon_ex_chomp");
		this.emitAudioFromObject.Add("player_plane_weapon_ex_chomp");
	}

	// Token: 0x06004204 RID: 16900 RVA: 0x0023A774 File Offset: 0x00238B74
	public void Freeze()
	{
		this.state = PlaneWeaponChalice3WayExProjectile.State.Frozen;
		this.timeSinceFrozen = 0f;
		this.deathSpark.transform.localScale = new Vector3(0.5f, 0.5f);
		this.deathSpark.flipX = Rand.Bool();
		this.deathSpark.flipY = Rand.Bool();
		this.deathSpark.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.animator.Play("Spark", 1, 0f);
	}

	// Token: 0x06004205 RID: 16901 RVA: 0x0023A814 File Offset: 0x00238C14
	public void SetArcPosition()
	{
		base.transform.localPosition = new Vector3(Mathf.Sin(EaseUtils.Linear(0.15f, 1f, this.arcTimer) * 3.1415927f) * this.arcX, 10f + EaseUtils.Linear(0f, 1f, this.arcTimer) * 3.1415927f * this.vDirection * this.arcY);
	}

	// Token: 0x06004206 RID: 16902 RVA: 0x0023A888 File Offset: 0x00238C88
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		this.smokeTimer += CupheadTime.FixedDelta;
		if (this.smokeTimer > this.firstSmokeDelay)
		{
			this.smokeFX.Create(base.transform.position);
			this.smokeTimer -= this.smokeDelay;
		}
		this.sparkleTimer += CupheadTime.FixedDelta;
		if (this.sparkleTimer > this.sparkleDelay)
		{
			this.sparkleFX.Create(base.transform.position + MathUtils.AngleToDirection((float)UnityEngine.Random.Range(0, 360)) * UnityEngine.Random.Range(0f, this.sparkleRadius));
			this.sparkleTimer -= this.sparkleDelay;
		}
		switch (this.state)
		{
		case PlaneWeaponChalice3WayExProjectile.State.Idle:
			this.SetArcPosition();
			this.arcTimer += this.arcSpeed / 3.1415927f * CupheadTime.FixedDelta;
			base.transform.localScale = new Vector3(Mathf.Lerp(0.5f, 1f, this.arcTimer), Mathf.Lerp(0.5f, 1f, this.arcTimer));
			if (this.arcTimer > 1f)
			{
				this.state = PlaneWeaponChalice3WayExProjectile.State.Paused;
				this.damageDealer.SetDamage(this.damageAfterLaunch);
				this.CollisionDeath.Enemies = true;
				base.transform.localScale = new Vector3(1f, 1f);
			}
			break;
		case PlaneWeaponChalice3WayExProjectile.State.Frozen:
			this.timeSinceFrozen += CupheadTime.FixedDelta;
			if (this.timeSinceFrozen > this.FreezeTime)
			{
				this.state = PlaneWeaponChalice3WayExProjectile.State.Idle;
			}
			break;
		case PlaneWeaponChalice3WayExProjectile.State.Paused:
			this.pauseTime -= CupheadTime.FixedDelta;
			if (this.pauseTime <= 0f)
			{
				this.FindTarget();
				this.state = PlaneWeaponChalice3WayExProjectile.State.Launched;
				Vector3 a = base.transform.parent.position + Vector3.right * this.xDistanceNoTarget;
				base.transform.parent = null;
				if (this.target != null && this.target.gameObject.activeInHierarchy && this.target.isActiveAndEnabled)
				{
					a = this.target.transform.position;
					a.x = Mathf.Clamp(a.x, base.transform.position.x + this.minXDistance, a.x);
				}
				this.velocityAfterLaunch = (a - base.transform.position).normalized;
				this.accelVectorAfterLaunch = this.velocityAfterLaunch * this.accelAfterLaunch;
				this.velocityAfterLaunch *= this.speedAfterLaunch;
			}
			break;
		case PlaneWeaponChalice3WayExProjectile.State.Launched:
			base.transform.position += this.velocityAfterLaunch * CupheadTime.FixedDelta;
			this.velocityAfterLaunch += this.accelVectorAfterLaunch * CupheadTime.FixedDelta;
			if (this.velocityAfterLaunch.x > 0f)
			{
				if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
				{
					base.animator.Play("Shoot");
					this.shootFX.Create(base.transform.position + Vector3.left * 20f);
				}
				this.magnet.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(this.velocityAfterLaunch));
			}
			break;
		}
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x0023AC91 File Offset: 0x00239091
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06004208 RID: 16904 RVA: 0x0023ACA2 File Offset: 0x002390A2
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06004209 RID: 16905 RVA: 0x0023ACC2 File Offset: 0x002390C2
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x0600420A RID: 16906 RVA: 0x0023ACD4 File Offset: 0x002390D4
	protected override void Die()
	{
		base.Die();
		this.magnet.transform.eulerAngles = Vector3.zero;
		this.magnet.flipX = Rand.Bool();
		this.deathSpark.transform.localScale = new Vector3(1f, 1f);
		this.deathSpark.flipX = Rand.Bool();
		this.deathSpark.flipY = Rand.Bool();
		this.deathSpark.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.animator.Play("Spark", 1, 0f);
		base.animator.Play((this.ID != 0) ? "DieB" : "DieA");
	}

	// Token: 0x0600420B RID: 16907 RVA: 0x0023ADB4 File Offset: 0x002391B4
	public void FindTarget()
	{
		if (this.partner != null && this.ID == 1)
		{
			return;
		}
		float num = float.MaxValue;
		Collider2D collider2D = null;
		Vector2 b = base.transform.parent.position;
		foreach (DamageReceiver damageReceiver in UnityEngine.Object.FindObjectsOfType<DamageReceiver>())
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.type == DamageReceiver.Type.Enemy && damageReceiver.transform.position.x >= base.transform.position.x)
			{
				foreach (Collider2D collider2D2 in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
					{
						float num2 = Mathf.Abs(MathUtils.DirectionToAngle(collider2D2.bounds.center - b));
						if (num2 < num)
						{
							num = num2;
							collider2D = collider2D2;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D3 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D3.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2f))
						{
							float num3 = Mathf.Abs(MathUtils.DirectionToAngle(collider2D3.bounds.center - b));
							if (num3 < num)
							{
								num = num3;
								collider2D = collider2D3;
							}
						}
					}
				}
			}
		}
		this.target = collider2D;
		if (this.partner != null)
		{
			this.partner.target = collider2D;
		}
	}

	// Token: 0x0600420C RID: 16908 RVA: 0x0023B015 File Offset: 0x00239415
	public override void OnLevelEnd()
	{
	}

	// Token: 0x0400486A RID: 18538
	private const float Y_OFFSET = 10f;

	// Token: 0x0400486B RID: 18539
	public float FreezeTime;

	// Token: 0x0400486C RID: 18540
	private PlaneWeaponChalice3WayExProjectile.State state;

	// Token: 0x0400486D RID: 18541
	private float timeSinceFrozen;

	// Token: 0x0400486E RID: 18542
	private float arcTimer;

	// Token: 0x0400486F RID: 18543
	public float arcSpeed = 5f;

	// Token: 0x04004870 RID: 18544
	public float arcX = 500f;

	// Token: 0x04004871 RID: 18545
	public float arcY = 500f;

	// Token: 0x04004872 RID: 18546
	public float damageAfterLaunch = 20f;

	// Token: 0x04004873 RID: 18547
	public float speedAfterLaunch = 3000f;

	// Token: 0x04004874 RID: 18548
	public float accelAfterLaunch = 100f;

	// Token: 0x04004875 RID: 18549
	public float minXDistance = 500f;

	// Token: 0x04004876 RID: 18550
	public float xDistanceNoTarget = 500f;

	// Token: 0x04004877 RID: 18551
	public int ID;

	// Token: 0x04004878 RID: 18552
	public PlaneWeaponChalice3WayExProjectile partner;

	// Token: 0x04004879 RID: 18553
	private Vector3 accelVectorAfterLaunch;

	// Token: 0x0400487A RID: 18554
	private Vector3 velocityAfterLaunch;

	// Token: 0x0400487B RID: 18555
	private Collider2D target;

	// Token: 0x0400487C RID: 18556
	public float pauseTime = 0.5f;

	// Token: 0x0400487D RID: 18557
	public float vDirection = 1f;

	// Token: 0x0400487E RID: 18558
	[SerializeField]
	private SpriteRenderer magnet;

	// Token: 0x0400487F RID: 18559
	[SerializeField]
	private SpriteRenderer deathSpark;

	// Token: 0x04004880 RID: 18560
	[SerializeField]
	private Effect shootFX;

	// Token: 0x04004881 RID: 18561
	[SerializeField]
	private Effect smokeFX;

	// Token: 0x04004882 RID: 18562
	[SerializeField]
	private Effect sparkleFX;

	// Token: 0x04004883 RID: 18563
	[SerializeField]
	private float firstSmokeDelay = 0.7f;

	// Token: 0x04004884 RID: 18564
	[SerializeField]
	private float smokeDelay = 0.09f;

	// Token: 0x04004885 RID: 18565
	[SerializeField]
	private float sparkleDelay = 0.15f;

	// Token: 0x04004886 RID: 18566
	[SerializeField]
	private float sparkleRadius = 20f;

	// Token: 0x04004887 RID: 18567
	private float smokeTimer;

	// Token: 0x04004888 RID: 18568
	private float sparkleTimer;

	// Token: 0x02000ABD RID: 2749
	public enum State
	{
		// Token: 0x0400488A RID: 18570
		Idle,
		// Token: 0x0400488B RID: 18571
		Frozen,
		// Token: 0x0400488C RID: 18572
		Paused,
		// Token: 0x0400488D RID: 18573
		Launched
	}
}
