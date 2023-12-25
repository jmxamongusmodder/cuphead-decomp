using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A00 RID: 2560
public abstract class AbstractArcadeWeapon : AbstractPausableComponent
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06003C7E RID: 15486
	protected abstract bool rapidFire { get; }

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06003C7F RID: 15487
	protected abstract float rapidFireRate { get; }

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06003C80 RID: 15488 RVA: 0x002199D1 File Offset: 0x00217DD1
	// (set) Token: 0x06003C81 RID: 15489 RVA: 0x002199D9 File Offset: 0x00217DD9
	public Weapon id { get; private set; }

	// Token: 0x06003C82 RID: 15490 RVA: 0x002199E4 File Offset: 0x00217DE4
	public virtual void Initialize(ArcadePlayerWeaponManager weaponManager, Weapon id)
	{
		this.weaponManager = weaponManager;
		this.player = weaponManager.GetComponent<ArcadePlayerController>();
		this.id = id;
		this.firing = new AbstractArcadeWeapon.FiringSwitches();
		this.StartCoroutines();
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x00219A34 File Offset: 0x00217E34
	private void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		if (this.player == null || this.player.IsDead || this.player.stats == null || !receiver.enabled)
		{
			return;
		}
		this.player.stats.OnDealDamage(damage, dealer);
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x00219A96 File Offset: 0x00217E96
	private void OnRevive(Vector3 pos)
	{
		this.StartCoroutines();
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x00219A9E File Offset: 0x00217E9E
	private void StartCoroutines()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.fireWeapon_cr(AbstractArcadeWeapon.Mode.Basic));
		base.StartCoroutine(this.fireWeapon_cr(AbstractArcadeWeapon.Mode.Ex));
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x00219AC2 File Offset: 0x00217EC2
	private void OnEnable()
	{
		this.StartCoroutines();
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x00219ACA File Offset: 0x00217ECA
	public virtual void BeginBasic()
	{
		this.beginFiring(AbstractArcadeWeapon.Mode.Basic);
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x00219AD3 File Offset: 0x00217ED3
	public virtual void EndBasic()
	{
		this.endFiring(AbstractArcadeWeapon.Mode.Basic);
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x00219ADC File Offset: 0x00217EDC
	protected virtual AbstractProjectile fireBasic()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractArcadeWeapon.Mode.Basic);
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x00219B04 File Offset: 0x00217F04
	public virtual void BeginEx()
	{
		this.beginFiring(AbstractArcadeWeapon.Mode.Ex);
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00219B0D File Offset: 0x00217F0D
	public virtual void EndEx()
	{
		this.endFiring(AbstractArcadeWeapon.Mode.Ex);
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00219B18 File Offset: 0x00217F18
	protected virtual AbstractProjectile fireEx()
	{
		return this.fireProjectile(AbstractArcadeWeapon.Mode.Ex);
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00219B2E File Offset: 0x00217F2E
	protected virtual void beginFiring(AbstractArcadeWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = true;
		this.firing.Set(mode, true);
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00219B4C File Offset: 0x00217F4C
	protected virtual AbstractProjectile fireProjectile(AbstractArcadeWeapon.Mode mode)
	{
		Vector2 position = this.weaponManager.GetBulletPosition();
		if (mode == AbstractArcadeWeapon.Mode.Ex)
		{
			position = this.weaponManager.ExPosition;
		}
		if (mode == AbstractArcadeWeapon.Mode.Basic)
		{
			this.weaponManager.UpdateAim();
		}
		if (this.GetProjectile(mode) == null)
		{
			return null;
		}
		if (this.GetEffect(mode) != null)
		{
			if (mode != AbstractArcadeWeapon.Mode.Basic && mode == AbstractArcadeWeapon.Mode.Ex)
			{
				this.weaponManager.CreateExDust(this.GetEffect(mode));
			}
		}
		this.weaponManager.UpdateAim();
		return this.GetProjectile(mode).Create(position, this.weaponManager.GetBulletRotation(), this.weaponManager.GetBulletScale());
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00219C12 File Offset: 0x00218012
	protected virtual void endFiring(AbstractArcadeWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = false;
		this.firing.Set(mode, false);
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00219C2D File Offset: 0x0021802D
	private AbstractProjectile GetProjectile(AbstractArcadeWeapon.Mode mode)
	{
		if (mode == AbstractArcadeWeapon.Mode.Basic || mode != AbstractArcadeWeapon.Mode.Ex)
		{
			return this.basicPrefab;
		}
		return this.exPrefab;
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00219C4E File Offset: 0x0021804E
	private Effect GetEffect(AbstractArcadeWeapon.Mode mode)
	{
		if (mode == AbstractArcadeWeapon.Mode.Basic || mode != AbstractArcadeWeapon.Mode.Ex)
		{
			return this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00219C6F File Offset: 0x0021806F
	private AbstractArcadeWeapon.FireProjectileDelegate getFiringMethod(AbstractArcadeWeapon.Mode mode)
	{
		if (mode != AbstractArcadeWeapon.Mode.Ex)
		{
			if (mode != AbstractArcadeWeapon.Mode.Basic)
			{
			}
			return new AbstractArcadeWeapon.FireProjectileDelegate(this.fireBasic);
		}
		return new AbstractArcadeWeapon.FireProjectileDelegate(this.fireEx);
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x00219CA0 File Offset: 0x002180A0
	protected virtual IEnumerator fireWeapon_cr(AbstractArcadeWeapon.Mode mode)
	{
		float t = 0f;
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (!this.player.motor.Dashing)
			{
				if (t < this.rapidFireRate)
				{
					t += CupheadTime.FixedDelta;
				}
				else if (this.firing.Get(mode) && this.weaponManager.IsShooting)
				{
					this.weaponManager.TriggerWeaponFire();
					this.getFiringMethod(mode)();
					if (mode == AbstractArcadeWeapon.Mode.Ex || !this.rapidFire)
					{
						this.firing.Set(mode, false);
						this.weaponManager.IsShooting = false;
					}
					t = 0f;
				}
			}
		}
		yield break;
	}

	// Token: 0x040043E1 RID: 17377
	[Header("Ex")]
	[SerializeField]
	protected AbstractProjectile exPrefab;

	// Token: 0x040043E2 RID: 17378
	[SerializeField]
	protected Effect exEffectPrefab;

	// Token: 0x040043E3 RID: 17379
	[Header("Basic")]
	[SerializeField]
	protected AbstractProjectile basicPrefab;

	// Token: 0x040043E4 RID: 17380
	[SerializeField]
	protected Effect basicEffectPrefab;

	// Token: 0x040043E6 RID: 17382
	protected AbstractArcadeWeapon.FiringSwitches firing;

	// Token: 0x040043E7 RID: 17383
	protected ArcadePlayerController player;

	// Token: 0x040043E8 RID: 17384
	protected ArcadePlayerWeaponManager weaponManager;

	// Token: 0x02000A01 RID: 2561
	public enum Mode
	{
		// Token: 0x040043EA RID: 17386
		Basic,
		// Token: 0x040043EB RID: 17387
		Ex
	}

	// Token: 0x02000A02 RID: 2562
	// (Invoke) Token: 0x06003C95 RID: 15509
	private delegate AbstractProjectile FireProjectileDelegate();

	// Token: 0x02000A03 RID: 2563
	[Serializable]
	public class Prefabs
	{
		// Token: 0x06003C99 RID: 15513 RVA: 0x00219CCA File Offset: 0x002180CA
		public AbstractProjectile Get(AbstractArcadeWeapon.Mode mode)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x040043EC RID: 17388
		public AbstractProjectile basic;

		// Token: 0x040043ED RID: 17389
		public AbstractProjectile ex;
	}

	// Token: 0x02000A04 RID: 2564
	[Serializable]
	public class MuzzleEffects
	{
		// Token: 0x06003C9B RID: 15515 RVA: 0x00219CF3 File Offset: 0x002180F3
		public Effect Get(AbstractArcadeWeapon.Mode mode)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x040043EE RID: 17390
		public Effect basic;

		// Token: 0x040043EF RID: 17391
		public Effect ex;
	}

	// Token: 0x02000A05 RID: 2565
	public class FiringSwitches
	{
		// Token: 0x06003C9D RID: 15517 RVA: 0x00219D1C File Offset: 0x0021811C
		public bool Get(AbstractArcadeWeapon.Mode mode)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x00219D3D File Offset: 0x0021813D
		public void Set(AbstractArcadeWeapon.Mode mode, bool val)
		{
			if (mode != AbstractArcadeWeapon.Mode.Ex)
			{
				if (mode != AbstractArcadeWeapon.Mode.Basic)
				{
				}
				this.basic = val;
			}
			else
			{
				this.ex = val;
			}
		}

		// Token: 0x040043F0 RID: 17392
		public bool basic;

		// Token: 0x040043F1 RID: 17393
		public bool ex;
	}
}
