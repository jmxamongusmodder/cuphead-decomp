using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A73 RID: 2675
public class WeaponCrackshotExProjectile : BasicProjectile
{
	// Token: 0x06003FE1 RID: 16353 RVA: 0x0022D809 File Offset: 0x0022BC09
	protected override void OnDieDistance()
	{
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x0022D80B File Offset: 0x0022BC0B
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0022D80D File Offset: 0x0022BC0D
	public override float ParryMeterMultiplier
	{
		get
		{
			return (this.parryTimeOut != 0f) ? 0.1f : 1f;
		}
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x0022D830 File Offset: 0x0022BC30
	protected override void Start()
	{
		base.Start();
		this._countParryTowardsScore = false;
		this.move = false;
		this.shotNumber = WeaponProperties.LevelWeaponCrackshot.Ex.shotNumber;
		base.transform.position += base.transform.right * 120f;
		this.angle = base.transform.eulerAngles.z;
		base.transform.eulerAngles = Vector3.zero;
		base.transform.localScale = new Vector3(Mathf.Sign(MathUtils.AngleToDirection(this.angle).x), 1f);
		this.basePos = base.transform.position;
		this.startPos = base.transform.position;
		this.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Ex.collideDamage);
		this.damageDealer.isDLCWeapon = true;
		this.SetParryable(WeaponProperties.LevelWeaponCrackshot.Ex.isPink);
		AudioManager.FadeSFXVolume("player_weapon_crackshot_turret_loop", 0.0001f, 0.0001f);
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x0022D93A File Offset: 0x0022BD3A
	public void GetReplaced()
	{
		this.LaunchAtTarget();
	}

	// Token: 0x06003FE6 RID: 16358 RVA: 0x0022D944 File Offset: 0x0022BD44
	private void AniEvent_StartSpinSFX()
	{
		AudioManager.Play("player_weapon_crackshot_turret_loop_start");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_loop_start");
		AudioManager.PlayLoop("player_weapon_crackshot_turret_loop");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_loop");
		AudioManager.FadeSFXVolumeLinear("player_weapon_crackshot_turret_loop", 0.3f, 1f);
	}

	// Token: 0x06003FE7 RID: 16359 RVA: 0x0022D999 File Offset: 0x0022BD99
	public override void OnParry(AbstractPlayerController player)
	{
		if (!this.parried)
		{
			this.LaunchAtTarget();
		}
		else
		{
			this.Die();
		}
	}

	// Token: 0x06003FE8 RID: 16360 RVA: 0x0022D9B8 File Offset: 0x0022BDB8
	private void LaunchAtTarget()
	{
		base.animator.Play("Launch");
		AudioManager.Stop("player_weapon_crackshot_turret_loop");
		AudioManager.Play("player_weapon_crackshot_turret_parry");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_parry");
		Collider2D collider2D = this.FindTarget();
		if (collider2D)
		{
			this.angle = MathUtils.DirectionToAngle(collider2D.bounds.center - base.transform.position);
		}
		Effect effect = this.launchFXPrefab.Create(base.transform.position);
		effect.transform.eulerAngles = new Vector3(0f, 0f, this.angle);
		effect.transform.localScale = new Vector3(-1f, (float)MathUtils.PlusOrMinus());
		this.parried = true;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.angle));
		base.transform.localScale = new Vector3(-1f, 1f);
		this.Speed = WeaponProperties.LevelWeaponCrackshot.Ex.parryBulletSpeed;
		this.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Ex.parryBulletDamage);
		this.SetParryable(false);
		this.parryTimeOut = WeaponProperties.LevelWeaponCrackshot.Ex.parryTimeOut;
		this.move = true;
	}

	// Token: 0x06003FE9 RID: 16361 RVA: 0x0022DB0C File Offset: 0x0022BF0C
	protected override void Die()
	{
		base.animator.Play("Explode");
		AudioManager.Stop("player_weapon_crackshot_turret_parry");
		AudioManager.Play("player_weapon_crackshot_turret_parryexplode");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_parryexplode");
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus());
		this.move = false;
		this.coll.enabled = false;
	}

	// Token: 0x06003FEA RID: 16362 RVA: 0x0022DBA2 File Offset: 0x0022BFA2
	private void _OnDieAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x0022DBAF File Offset: 0x0022BFAF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		AudioManager.FadeSFXVolume("player_weapon_crackshot_turret_loop", 0.0001f, 0.25f);
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x0022DBCC File Offset: 0x0022BFCC
	private void HandleShot()
	{
		this.shootTimer -= CupheadTime.FixedDelta;
		if (this.shootTimer <= 0f)
		{
			Collider2D collider2D = this.FindTarget();
			if (collider2D)
			{
				MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
				BasicProjectile projectile = this.childPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(collider2D.bounds.center - base.transform.position), WeaponProperties.LevelWeaponCrackshot.Ex.bulletSpeed);
				this.childPrefab.Damage = WeaponProperties.LevelWeaponCrackshot.Ex.bulletDamage;
				this.childPrefab.Speed = WeaponProperties.LevelWeaponCrackshot.Ex.bulletSpeed;
				this.childPrefab.PlayerId = this.PlayerId;
				meterScoreTracker.Add(projectile);
				base.StartCoroutine(this.shoot_stretch_squash_cr());
				this.shootFXPrefab.Create(base.transform.position + (collider2D.bounds.center - base.transform.position).normalized * 25f);
				AudioManager.Play("player_weapon_crackshot_turret_shoot");
				this.emitAudioFromObject.Add("player_weapon_crackshot_turret_shoot");
			}
			this.shotNumber--;
			if (this.shotNumber == 0)
			{
				base.animator.SetTrigger("Disappear");
				this.coll.enabled = false;
			}
			else
			{
				this.shootTimer += WeaponProperties.LevelWeaponCrackshot.Ex.shootDelay;
			}
		}
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x0022DD58 File Offset: 0x0022C158
	private IEnumerator shoot_stretch_squash_cr()
	{
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.2f, Mathf.Sign(base.transform.localScale.y) * 1.2f);
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.25f, Mathf.Sign(base.transform.localScale.y) * 1.25f);
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.22f, Mathf.Sign(base.transform.localScale.y) * 1.22f);
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.16f, Mathf.Sign(base.transform.localScale.y) * 1.16f);
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x), Mathf.Sign(base.transform.localScale.y));
		yield break;
	}

	// Token: 0x06003FEE RID: 16366 RVA: 0x0022DD74 File Offset: 0x0022C174
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.parried)
		{
			if (this.parryTimeOut > 0f)
			{
				this.parryTimeOut -= CupheadTime.FixedDelta;
				if (this.parryTimeOut <= 0f)
				{
					this.parryTimeOut = 0f;
					this.SetParryable(true);
				}
			}
			return;
		}
		if (base.lifetime < WeaponProperties.LevelWeaponCrackshot.Ex.timeToHoverPoint)
		{
			this.basePos = Vector3.Lerp(this.startPos, this.startPos + MathUtils.AngleToDirection(this.angle) * WeaponProperties.LevelWeaponCrackshot.Ex.launchDistance, EaseUtils.EaseOutSine(0f, 1f, base.lifetime / WeaponProperties.LevelWeaponCrackshot.Ex.timeToHoverPoint));
		}
		else
		{
			if (!this.timerSet)
			{
				this.timerSet = true;
				this.basePos = this.startPos + MathUtils.AngleToDirection(this.angle) * WeaponProperties.LevelWeaponCrackshot.Ex.launchDistance;
				this.shootTimer = WeaponProperties.LevelWeaponCrackshot.Ex.shootDelay;
			}
			this.basePos += Vector3.up * WeaponProperties.LevelWeaponCrackshot.Ex.riseSpeed * CupheadTime.FixedDelta;
			this.HandleShot();
		}
		if (this.shotNumber > 0)
		{
			float num = base.lifetime * WeaponProperties.LevelWeaponCrackshot.Ex.hoverSpeed;
			base.transform.position = this.basePos + new Vector3(Mathf.Cos(num + 1.5707964f) * WeaponProperties.LevelWeaponCrackshot.Ex.hoverWidth * -Mathf.Sign(MathUtils.AngleToDirection(this.angle).x), Mathf.Sin(num * 2f) * WeaponProperties.LevelWeaponCrackshot.Ex.hoverHeight);
		}
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x0022DF27 File Offset: 0x0022C327
	public Collider2D FindTarget()
	{
		return this.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers());
	}

	// Token: 0x06003FF0 RID: 16368 RVA: 0x0022DF34 File Offset: 0x0022C334
	private Collider2D findBestTarget(IEnumerable<DamageReceiver> damageReceivers)
	{
		float num = float.MaxValue;
		Collider2D result = null;
		Vector2 a = base.transform.position;
		foreach (DamageReceiver damageReceiver in damageReceivers)
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.enabled && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D.bounds.center, collider2D.bounds.size / 2f))
					{
						float sqrMagnitude = (a - collider2D.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							result = collider2D;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D2 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
						{
							float sqrMagnitude2 = (a - collider2D2.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num)
							{
								num = sqrMagnitude2;
								result = collider2D2;
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x040046BA RID: 18106
	[SerializeField]
	private WeaponCrackshotExProjectileChild childPrefab;

	// Token: 0x040046BB RID: 18107
	[SerializeField]
	private Effect shootFXPrefab;

	// Token: 0x040046BC RID: 18108
	[SerializeField]
	private Effect launchFXPrefab;

	// Token: 0x040046BD RID: 18109
	[SerializeField]
	private Collider2D coll;

	// Token: 0x040046BE RID: 18110
	private Vector3 basePos;

	// Token: 0x040046BF RID: 18111
	private Vector3 startPos;

	// Token: 0x040046C0 RID: 18112
	private int shotNumber = 5;

	// Token: 0x040046C1 RID: 18113
	private float shootTimer;

	// Token: 0x040046C2 RID: 18114
	private bool timerSet;

	// Token: 0x040046C3 RID: 18115
	private bool parried;

	// Token: 0x040046C4 RID: 18116
	private float parryTimeOut;

	// Token: 0x040046C5 RID: 18117
	private float angle;
}
