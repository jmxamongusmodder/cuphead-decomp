using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A68 RID: 2664
public class WeaponArcProjectile : AbstractProjectile
{
	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06003F8D RID: 16269 RVA: 0x0022B43C File Offset: 0x0022983C
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x0022B443 File Offset: 0x00229843
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06003F8F RID: 16271 RVA: 0x0022B44C File Offset: 0x0022984C
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		WeaponArcProjectile.State state = this._state;
		if (state != WeaponArcProjectile.State.InAir)
		{
			if (state == WeaponArcProjectile.State.OnGround)
			{
				this.UpdateOnGround();
			}
		}
		else
		{
			this.UpdateInAir();
		}
		if (!this.isEx)
		{
			this.UpdateDamageState();
			if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 1000f)))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x0022B4E4 File Offset: 0x002298E4
	private void UpdateInAir()
	{
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		base.transform.position += this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x0022B53A File Offset: 0x0022993A
	private void UpdateOnGround()
	{
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x0022B53C File Offset: 0x0022993C
	private void UpdateDamageState()
	{
		if (base.lifetime < WeaponProperties.LevelWeaponArc.Basic.timeStateTwo)
		{
			this.Damage = WeaponProperties.LevelWeaponArc.Basic.baseDamage;
			base.transform.SetScale(new float?(1f), new float?(1f), null);
		}
		else if (base.lifetime < WeaponProperties.LevelWeaponArc.Basic.timeStateThree)
		{
			this.Damage = WeaponProperties.LevelWeaponArc.Basic.damageStateTwo;
			base.transform.SetScale(new float?(1.5f), new float?(1.5f), null);
		}
		else
		{
			this.Damage = WeaponProperties.LevelWeaponArc.Basic.damageStateThree;
			base.transform.SetScale(new float?(2.5f), new float?(2.5f), null);
		}
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x0022B60C File Offset: 0x00229A0C
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (this._state == WeaponArcProjectile.State.InAir && (component == null || (!component.canFallThrough && this.velocity.y < 0f)))
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x0022B668 File Offset: 0x00229A68
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (this._state == WeaponArcProjectile.State.InAir && component != null && !component.canFallThrough && this.velocity.y < 0f)
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x0022B6C2 File Offset: 0x00229AC2
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (!this.isEx)
		{
			this.damageDealer.SetDamage(this.Damage);
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x0022B6F8 File Offset: 0x00229AF8
	private void HitGround(GameObject hit)
	{
		this._state = WeaponArcProjectile.State.OnGround;
		if (!this.isEx)
		{
			this.weapon.projectilesOnGround.Add(this);
			if (this.weapon.projectilesOnGround.Count > WeaponProperties.LevelWeaponArc.Basic.maxNumMines)
			{
				WeaponArcProjectile weaponArcProjectile = this.weapon.projectilesOnGround[0];
				this.weapon.projectilesOnGround.RemoveAt(0);
				weaponArcProjectile.Die();
			}
		}
		else
		{
			base.StartCoroutine(this.timedExplode_cr());
		}
		base.transform.SetParent(hit.transform);
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x0022B790 File Offset: 0x00229B90
	private IEnumerator timedExplode_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelWeaponArc.Ex.explodeDelay);
		this.Die();
		yield break;
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x0022B7AC File Offset: 0x00229BAC
	protected override void Die()
	{
		base.Die();
		if (this.isEx)
		{
			this.exExplosion.Create(base.transform.position, this.Damage, base.DamageMultiplier, this.PlayerId);
			AudioManager.Play("player_weapon_arc_ex_explosion");
			this.emitAudioFromObject.Add("player_weapon_arc_ex_explosion");
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x0022B81D File Offset: 0x00229C1D
	protected override void OnDestroy()
	{
		if (this.weapon.projectilesOnGround.Contains(this))
		{
			this.weapon.projectilesOnGround.Remove(this);
		}
		base.OnDestroy();
	}

	// Token: 0x04004678 RID: 18040
	[SerializeField]
	private bool isEx;

	// Token: 0x04004679 RID: 18041
	[SerializeField]
	private WeaponArcProjectileExplosion exExplosion;

	// Token: 0x0400467A RID: 18042
	public float chargeTime;

	// Token: 0x0400467B RID: 18043
	public float gravity;

	// Token: 0x0400467C RID: 18044
	public Vector2 velocity;

	// Token: 0x0400467D RID: 18045
	public WeaponArc weapon;

	// Token: 0x0400467E RID: 18046
	private WeaponArcProjectile.State _state;

	// Token: 0x02000A69 RID: 2665
	public enum State
	{
		// Token: 0x04004680 RID: 18048
		InAir,
		// Token: 0x04004681 RID: 18049
		OnGround
	}
}
