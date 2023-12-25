using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AAE RID: 2734
public abstract class AbstractPlaneWeapon : AbstractPausableComponent
{
	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x060041AE RID: 16814
	protected abstract bool rapidFire { get; }

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x060041AF RID: 16815
	protected abstract float rapidFireRate { get; }

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x060041B0 RID: 16816 RVA: 0x00238E1F File Offset: 0x0023721F
	// (set) Token: 0x060041B1 RID: 16817 RVA: 0x00238E27 File Offset: 0x00237227
	public int index { get; private set; }

	// Token: 0x060041B2 RID: 16818 RVA: 0x00238E30 File Offset: 0x00237230
	public virtual void Initialize(PlanePlayerWeaponManager weaponManager, int index)
	{
		this.weaponManager = weaponManager;
		this.player = weaponManager.GetComponent<PlanePlayerController>();
		this.index = index;
		this.firing = new AbstractPlaneWeapon.FiringSwitches();
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Basic));
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Ex));
		base.StartCoroutine(this.endFiringAnimation_cr());
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x00238EA2 File Offset: 0x002372A2
	private void OnRevive(Vector3 pos)
	{
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Basic));
		base.StartCoroutine(this.fireWeapon_cr(AbstractPlaneWeapon.Mode.Ex));
		base.StartCoroutine(this.endFiringAnimation_cr());
	}

	// Token: 0x060041B4 RID: 16820 RVA: 0x00238ED0 File Offset: 0x002372D0
	private void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		if (this.player == null || this.player.IsDead || this.player.stats == null || !receiver.enabled)
		{
			return;
		}
		this.player.stats.OnDealDamage(damage, dealer);
	}

	// Token: 0x060041B5 RID: 16821 RVA: 0x00238F32 File Offset: 0x00237332
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.exPrefab = null;
		this.exEffectPrefab = null;
		this.basicPrefab = null;
		this.basicEffectPrefab = null;
		this.shrunkPrefab = null;
	}

	// Token: 0x060041B6 RID: 16822 RVA: 0x00238F5D File Offset: 0x0023735D
	public virtual void BeginBasic()
	{
		this.beginFiring(AbstractPlaneWeapon.Mode.Basic);
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x00238F66 File Offset: 0x00237366
	public virtual void EndBasic()
	{
		this.endFiring(AbstractPlaneWeapon.Mode.Basic);
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x00238F70 File Offset: 0x00237370
	protected virtual AbstractProjectile fireBasic()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractPlaneWeapon.Mode.Basic);
		abstractProjectile.PlayerId = this.player.id;
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x00238FA9 File Offset: 0x002373A9
	public virtual void BeginEx()
	{
		this.beginFiring(AbstractPlaneWeapon.Mode.Ex);
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x00238FB2 File Offset: 0x002373B2
	public virtual void EndEx()
	{
		this.endFiring(AbstractPlaneWeapon.Mode.Ex);
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x00238FBC File Offset: 0x002373BC
	protected virtual AbstractProjectile fireEx()
	{
		return this.fireProjectile(AbstractPlaneWeapon.Mode.Ex);
	}

	// Token: 0x060041BC RID: 16828 RVA: 0x00238FD2 File Offset: 0x002373D2
	protected virtual void beginFiring(AbstractPlaneWeapon.Mode mode)
	{
		base.StopCoroutine("endFiringAnimation_cr");
		this.weaponManager.IsShooting = true;
		this.firing.Set(mode, true);
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x00238FF8 File Offset: 0x002373F8
	protected virtual AbstractProjectile fireProjectile(AbstractPlaneWeapon.Mode mode)
	{
		Vector2 vector = this.weaponManager.GetBulletPosition() + new Vector2(-10f, 0f) + new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f));
		if (this.GetProjectile(mode) == null)
		{
			return null;
		}
		if (this.GetEffect(mode) != null)
		{
			if (mode == AbstractPlaneWeapon.Mode.Basic || mode != AbstractPlaneWeapon.Mode.Ex)
			{
				this.basicEffectPrefab.Create(vector, base.transform.localScale).transform.SetParent(base.transform);
			}
		}
		AbstractProjectile abstractProjectile = this.GetProjectile(mode).Create(vector);
		if (mode == AbstractPlaneWeapon.Mode.Ex)
		{
			abstractProjectile.DamageSource = DamageDealer.DamageSource.Ex;
			CupheadLevelCamera.Current.Shake(5f, 0.5f, false);
		}
		abstractProjectile.PlayerId = this.player.id;
		return abstractProjectile;
	}

	// Token: 0x060041BE RID: 16830 RVA: 0x002390FE File Offset: 0x002374FE
	protected virtual void endFiring(AbstractPlaneWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = false;
		this.firing.Set(mode, false);
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x00239119 File Offset: 0x00237519
	private AbstractProjectile GetProjectile(AbstractPlaneWeapon.Mode mode)
	{
		if (mode != AbstractPlaneWeapon.Mode.Basic && mode == AbstractPlaneWeapon.Mode.Ex)
		{
			return this.exPrefab;
		}
		if (this.player.Shrunk)
		{
			return this.shrunkPrefab;
		}
		return this.basicPrefab;
	}

	// Token: 0x060041C0 RID: 16832 RVA: 0x00239151 File Offset: 0x00237551
	protected virtual Effect GetEffect(AbstractPlaneWeapon.Mode mode)
	{
		if (mode == AbstractPlaneWeapon.Mode.Basic || mode != AbstractPlaneWeapon.Mode.Ex)
		{
			return this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x060041C1 RID: 16833 RVA: 0x00239172 File Offset: 0x00237572
	private AbstractPlaneWeapon.FireProjectileDelegate getFiringMethod(AbstractPlaneWeapon.Mode mode)
	{
		if (mode != AbstractPlaneWeapon.Mode.Ex)
		{
			if (mode != AbstractPlaneWeapon.Mode.Basic)
			{
			}
			return new AbstractPlaneWeapon.FireProjectileDelegate(this.fireBasic);
		}
		return new AbstractPlaneWeapon.FireProjectileDelegate(this.fireEx);
	}

	// Token: 0x060041C2 RID: 16834 RVA: 0x002391A4 File Offset: 0x002375A4
	private IEnumerator fireWeapon_cr(AbstractPlaneWeapon.Mode mode)
	{
		float time = this.rapidFireRate;
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (mode == AbstractPlaneWeapon.Mode.Basic && this.t < time)
			{
				if (this.weaponManager.CurrentWeapon == this)
				{
					this.t += CupheadTime.FixedDelta;
				}
			}
			else if (this.firing.Get(mode))
			{
				this.getFiringMethod(mode)();
				if (mode == AbstractPlaneWeapon.Mode.Ex || !this.rapidFire)
				{
					this.firing.Set(mode, false);
					base.StartCoroutine(this.endFiringAnimation_cr());
				}
				this.t = 0f;
			}
		}
		yield break;
	}

	// Token: 0x060041C3 RID: 16835 RVA: 0x002391C8 File Offset: 0x002375C8
	private IEnumerator endFiringAnimation_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.16666667f);
		this.weaponManager.IsShooting = false;
		yield break;
	}

	// Token: 0x04004826 RID: 18470
	private const int ANIMATION_FRAMES = 10;

	// Token: 0x04004827 RID: 18471
	[Header("Ex")]
	[SerializeField]
	protected AbstractProjectile exPrefab;

	// Token: 0x04004828 RID: 18472
	[SerializeField]
	protected Effect exEffectPrefab;

	// Token: 0x04004829 RID: 18473
	[Header("Basic")]
	[SerializeField]
	protected AbstractProjectile basicPrefab;

	// Token: 0x0400482A RID: 18474
	[SerializeField]
	protected Effect basicEffectPrefab;

	// Token: 0x0400482B RID: 18475
	[Header("Shrunk")]
	[SerializeField]
	protected AbstractProjectile shrunkPrefab;

	// Token: 0x0400482C RID: 18476
	[SerializeField]
	protected float shrunkDamageMultiplier = 0.5f;

	// Token: 0x0400482E RID: 18478
	protected AbstractPlaneWeapon.FiringSwitches firing;

	// Token: 0x0400482F RID: 18479
	protected PlanePlayerController player;

	// Token: 0x04004830 RID: 18480
	protected PlanePlayerWeaponManager weaponManager;

	// Token: 0x04004831 RID: 18481
	private float t = 1000f;

	// Token: 0x02000AAF RID: 2735
	public enum Mode
	{
		// Token: 0x04004833 RID: 18483
		Basic,
		// Token: 0x04004834 RID: 18484
		Ex
	}

	// Token: 0x02000AB0 RID: 2736
	// (Invoke) Token: 0x060041C5 RID: 16837
	private delegate AbstractProjectile FireProjectileDelegate();

	// Token: 0x02000AB1 RID: 2737
	[Serializable]
	public class Prefabs
	{
		// Token: 0x060041C9 RID: 16841 RVA: 0x002391EB File Offset: 0x002375EB
		public AbstractProjectile Get(AbstractPlaneWeapon.Mode mode)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04004835 RID: 18485
		public AbstractProjectile basic;

		// Token: 0x04004836 RID: 18486
		public AbstractProjectile ex;
	}

	// Token: 0x02000AB2 RID: 2738
	[Serializable]
	public class MuzzleEffects
	{
		// Token: 0x060041CB RID: 16843 RVA: 0x00239214 File Offset: 0x00237614
		public Effect Get(AbstractPlaneWeapon.Mode mode)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04004837 RID: 18487
		public Effect basic;

		// Token: 0x04004838 RID: 18488
		public Effect ex;
	}

	// Token: 0x02000AB3 RID: 2739
	public class FiringSwitches
	{
		// Token: 0x060041CD RID: 16845 RVA: 0x0023923D File Offset: 0x0023763D
		public bool Get(AbstractPlaneWeapon.Mode mode)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x0023925E File Offset: 0x0023765E
		public void Set(AbstractPlaneWeapon.Mode mode, bool val)
		{
			if (mode != AbstractPlaneWeapon.Mode.Ex)
			{
				if (mode != AbstractPlaneWeapon.Mode.Basic)
				{
				}
				this.basic = val;
			}
			else
			{
				this.ex = val;
			}
		}

		// Token: 0x04004839 RID: 18489
		public bool basic;

		// Token: 0x0400483A RID: 18490
		public bool ex;
	}
}
