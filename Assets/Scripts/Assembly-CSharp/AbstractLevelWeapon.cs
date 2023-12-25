using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A5D RID: 2653
public abstract class AbstractLevelWeapon : AbstractPausableComponent
{
	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06003F3D RID: 16189 RVA: 0x0022A0E3 File Offset: 0x002284E3
	// (set) Token: 0x06003F3E RID: 16190 RVA: 0x0022A0EA File Offset: 0x002284EA
	public static bool ONE_PLAYER_FIRING { get; private set; }

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06003F3F RID: 16191
	protected abstract bool rapidFire { get; }

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06003F40 RID: 16192
	protected abstract float rapidFireRate { get; }

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06003F41 RID: 16193 RVA: 0x0022A0F2 File Offset: 0x002284F2
	// (set) Token: 0x06003F42 RID: 16194 RVA: 0x0022A0FA File Offset: 0x002284FA
	public Weapon id { get; private set; }

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06003F43 RID: 16195 RVA: 0x0022A103 File Offset: 0x00228503
	protected virtual bool isChargeWeapon
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06003F44 RID: 16196 RVA: 0x0022A106 File Offset: 0x00228506
	protected override Transform emitTransform
	{
		get
		{
			return this.player.transform;
		}
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x0022A114 File Offset: 0x00228514
	public virtual void Initialize(LevelPlayerWeaponManager weaponManager, Weapon id)
	{
		this.weaponManager = weaponManager;
		this.player = weaponManager.GetComponent<LevelPlayerController>();
		this.id = id;
		this.firing = new AbstractLevelWeapon.FiringSwitches();
		this.StartCoroutines();
		this.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003F46 RID: 16198 RVA: 0x0022A164 File Offset: 0x00228564
	public void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		if (this.player == null || this.player.IsDead || this.player.stats == null || !receiver.enabled)
		{
			return;
		}
		this.player.stats.OnDealDamage(damage, dealer);
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x0022A1C6 File Offset: 0x002285C6
	private void OnRevive(Vector3 pos)
	{
		this.StartCoroutines();
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x0022A1D0 File Offset: 0x002285D0
	private void StartCoroutines()
	{
		this.StopAllCoroutines();
		if (this.isChargeWeapon)
		{
			base.StartCoroutine(this.chargeFireWeapon_cr(AbstractLevelWeapon.Mode.Basic));
		}
		else
		{
			base.StartCoroutine(this.fireWeapon_cr(AbstractLevelWeapon.Mode.Basic));
		}
		base.StartCoroutine(this.fireWeapon_cr(AbstractLevelWeapon.Mode.Ex));
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x0022A21D File Offset: 0x0022861D
	protected virtual void OnEnable()
	{
		this.StartCoroutines();
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x0022A228 File Offset: 0x00228628
	private void Update()
	{
		if (this.firing.Get(AbstractLevelWeapon.Mode.Basic) || this.firing.Get(AbstractLevelWeapon.Mode.Ex))
		{
			AbstractLevelWeapon.ONE_PLAYER_FIRING = true;
		}
		if (this.isUsingLoop && AudioManager.CheckIfPlaying(this.WeaponSound))
		{
			this.emitAudioFromObject.Add(this.WeaponSound);
		}
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0022A289 File Offset: 0x00228689
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.exPrefab = null;
		this.exEffectPrefab = null;
		this.exFiringHitboxPrefab = null;
		this.basicPrefab = null;
		this.basicEffectPrefab = null;
		this.basicFiringHitboxPrefab = null;
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0022A2BB File Offset: 0x002286BB
	protected virtual void BasicSoundOneShot(string soundP1, string soundP2)
	{
		if (this.player.id == PlayerId.PlayerOne)
		{
			AudioManager.Play(soundP1);
			this.emitAudioFromObject.Add(soundP1);
		}
		else
		{
			AudioManager.Play(soundP2);
			this.emitAudioFromObject.Add(soundP2);
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x0022A2F6 File Offset: 0x002286F6
	protected virtual void OneShotCooldown(string sound)
	{
		if (this.coolingDown)
		{
			return;
		}
		AudioManager.Play(sound);
		this.emitAudioFromObject.Add(sound);
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x0022A316 File Offset: 0x00228716
	protected virtual void ActivateCooldown()
	{
		if (this.coolingDown)
		{
			return;
		}
		this.coolingDown = true;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.shot_cooldown_cr());
		}
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x0022A348 File Offset: 0x00228748
	private IEnumerator shot_cooldown_cr()
	{
		float t = 0f;
		float cooldownTime = UnityEngine.Random.Range(4f, 7f);
		while (t < cooldownTime)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.coolingDown = false;
		yield break;
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x0022A364 File Offset: 0x00228764
	protected virtual void BeginBasicCheckAttenuation(string soundP1, string soundP2)
	{
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			if (this.player.id == PlayerId.PlayerOne)
			{
				AudioManager.Attenuation(soundP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
			}
			else
			{
				AudioManager.Attenuation(soundP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
			}
		}
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x0022A3B8 File Offset: 0x002287B8
	protected virtual void EndBasicCheckAttenuation(string soundP1, string soundP2)
	{
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			if (this.player.id == PlayerId.PlayerOne)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					AudioManager.Attenuation(soundP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
					AudioManager.Attenuation(soundP1, false, 0.1f);
				}
			}
			else
			{
				AudioManager.Attenuation(soundP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
				AudioManager.Attenuation(soundP2, false, 0.1f);
			}
		}
	}

	// Token: 0x06003F52 RID: 16210 RVA: 0x0022A434 File Offset: 0x00228834
	protected virtual void BasicSoundLoop(string loopP1, string loopP2)
	{
		if (this.player.id == PlayerId.PlayerOne)
		{
			this.WeaponSound = loopP1;
			AudioManager.PlayLoop(loopP1);
			AudioManager.Attenuation(loopP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
		}
		else
		{
			this.WeaponSound = loopP2;
			AudioManager.PlayLoop(loopP2);
			AudioManager.Attenuation(loopP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
		}
		this.isUsingLoop = true;
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x0022A498 File Offset: 0x00228898
	protected virtual void StopLoopSound(string loopP1, string loopP2)
	{
		if (this.player.id == PlayerId.PlayerOne)
		{
			AudioManager.Stop(loopP1);
			if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				AudioManager.Attenuation(loopP2, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
			}
		}
		else
		{
			AudioManager.Stop(loopP2);
			AudioManager.Attenuation(loopP1, AbstractLevelWeapon.ONE_PLAYER_FIRING, 0.1f);
		}
	}

	// Token: 0x06003F54 RID: 16212 RVA: 0x0022A4F7 File Offset: 0x002288F7
	public virtual void BeginBasic()
	{
		this.beginFiring(AbstractLevelWeapon.Mode.Basic);
	}

	// Token: 0x06003F55 RID: 16213 RVA: 0x0022A500 File Offset: 0x00228900
	public virtual void EndBasic()
	{
		this.endFiring(AbstractLevelWeapon.Mode.Basic);
		AbstractLevelWeapon.ONE_PLAYER_FIRING = false;
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x0022A510 File Offset: 0x00228910
	protected virtual AbstractProjectile fireBasic()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractLevelWeapon.Mode.Basic, true);
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x0022A53C File Offset: 0x0022893C
	protected AbstractProjectile fireBasicNoEffect()
	{
		AbstractProjectile abstractProjectile = this.fireProjectile(AbstractLevelWeapon.Mode.Basic, false);
		abstractProjectile.OnDealDamageEvent += this.OnDealDamage;
		return abstractProjectile;
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x0022A565 File Offset: 0x00228965
	public virtual void BeginEx()
	{
		this.beginFiring(AbstractLevelWeapon.Mode.Ex);
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x0022A56E File Offset: 0x0022896E
	public virtual void EndEx()
	{
		this.endFiring(AbstractLevelWeapon.Mode.Ex);
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x0022A578 File Offset: 0x00228978
	protected virtual AbstractProjectile fireEx()
	{
		return this.fireProjectile(AbstractLevelWeapon.Mode.Ex, true);
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x0022A58F File Offset: 0x0022898F
	protected virtual void beginFiring(AbstractLevelWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = true;
		this.firing.Set(mode, true);
	}

	// Token: 0x06003F5C RID: 16220 RVA: 0x0022A5AC File Offset: 0x002289AC
	protected virtual AbstractProjectile fireProjectile(AbstractLevelWeapon.Mode mode, bool createEffect = true)
	{
		Vector2 vector = this.weaponManager.GetBulletPosition();
		if (mode == AbstractLevelWeapon.Mode.Ex)
		{
			vector = this.weaponManager.ExPosition;
		}
		if (mode == AbstractLevelWeapon.Mode.Basic)
		{
			this.weaponManager.UpdateAim();
		}
		if (this.GetProjectile(mode) == null)
		{
			return null;
		}
		if (this.GetEffect(mode) != null && createEffect)
		{
			if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
			{
				Effect effect = this.basicEffectPrefab.Create(vector, base.transform.localScale);
				WeaponSparkEffect weaponSparkEffect = effect as WeaponSparkEffect;
				if (weaponSparkEffect != null)
				{
					LevelPlayerWeaponManager.Pose directionPose = this.weaponManager.GetDirectionPose();
					if (directionPose == LevelPlayerWeaponManager.Pose.Forward || directionPose == LevelPlayerWeaponManager.Pose.Forward_R || directionPose == LevelPlayerWeaponManager.Pose.Up_D || directionPose == LevelPlayerWeaponManager.Pose.Up_D_R)
					{
						weaponSparkEffect.SetPlayer(this.player);
					}
					if (directionPose == LevelPlayerWeaponManager.Pose.Down)
					{
						weaponSparkEffect.BringToFrontOfPlayer();
					}
				}
			}
			else
			{
				this.weaponManager.CreateExDust(this.GetEffect(mode));
			}
		}
		AbstractProjectile abstractProjectile = this.GetProjectile(mode).Create(vector, this.weaponManager.GetBulletRotation(), this.weaponManager.GetBulletScale());
		if (mode == AbstractLevelWeapon.Mode.Ex)
		{
			abstractProjectile.DamageSource = DamageDealer.DamageSource.Ex;
			CupheadLevelCamera.Current.Shake(5f, 0.5f, false);
		}
		if (this.GetFiringHitbox(mode) != null)
		{
			abstractProjectile.AddFiringHitbox(this.GetFiringHitbox(mode).Create(vector, this.weaponManager.GetBulletRotation()));
		}
		abstractProjectile.PlayerId = this.player.id;
		return abstractProjectile;
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x0022A746 File Offset: 0x00228B46
	protected virtual void endFiring(AbstractLevelWeapon.Mode mode)
	{
		this.weaponManager.IsShooting = false;
		this.firing.Set(mode, false);
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x0022A761 File Offset: 0x00228B61
	private AbstractProjectile GetProjectile(AbstractLevelWeapon.Mode mode)
	{
		if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
		{
			return this.basicPrefab;
		}
		return this.exPrefab;
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x0022A782 File Offset: 0x00228B82
	private Effect GetEffect(AbstractLevelWeapon.Mode mode)
	{
		if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
		{
			return this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x0022A7A3 File Offset: 0x00228BA3
	private LevelPlayerWeaponFiringHitbox GetFiringHitbox(AbstractLevelWeapon.Mode mode)
	{
		if (mode == AbstractLevelWeapon.Mode.Basic || mode != AbstractLevelWeapon.Mode.Ex)
		{
			return this.basicFiringHitboxPrefab;
		}
		return this.exFiringHitboxPrefab;
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x0022A7C4 File Offset: 0x00228BC4
	private AbstractLevelWeapon.FireProjectileDelegate getFiringMethod(AbstractLevelWeapon.Mode mode)
	{
		if (mode != AbstractLevelWeapon.Mode.Ex)
		{
			if (mode != AbstractLevelWeapon.Mode.Basic)
			{
			}
			return new AbstractLevelWeapon.FireProjectileDelegate(this.fireBasic);
		}
		return new AbstractLevelWeapon.FireProjectileDelegate(this.fireEx);
	}

	// Token: 0x06003F62 RID: 16226 RVA: 0x0022A7F4 File Offset: 0x00228BF4
	protected virtual IEnumerator fireWeapon_cr(AbstractLevelWeapon.Mode mode)
	{
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (!this.player.motor.Dashing)
			{
				if (mode == AbstractLevelWeapon.Mode.Basic && this.t < this.rapidFireRate)
				{
					if (this.weaponManager.CurrentWeapon == this)
					{
						this.t += CupheadTime.FixedDelta;
					}
				}
				else if (this.firing.Get(mode) && this.weaponManager.IsShooting)
				{
					this.weaponManager.TriggerWeaponFire();
					this.getFiringMethod(mode)();
					if (mode == AbstractLevelWeapon.Mode.Ex || !this.rapidFire)
					{
						this.firing.Set(mode, false);
						this.weaponManager.IsShooting = false;
					}
					this.t = 0f;
				}
			}
		}
		yield break;
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x0022A818 File Offset: 0x00228C18
	protected virtual IEnumerator chargeFireWeapon_cr(AbstractLevelWeapon.Mode mode)
	{
		WaitForFixedUpdate waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			if (mode == AbstractLevelWeapon.Mode.Basic && this.firing.Get(mode) && this.weaponManager.IsShooting)
			{
				this.alreadyHeld = true;
			}
			else if (mode == AbstractLevelWeapon.Mode.Basic && this.alreadyHeld)
			{
				this.alreadyReleased = true;
			}
			if (mode == AbstractLevelWeapon.Mode.Basic && this.t < this.rapidFireRate)
			{
				if (this.weaponManager.CurrentWeapon == this)
				{
					this.t += CupheadTime.FixedDelta;
					this.charging = false;
				}
			}
			else if (this.firing.Get(mode) && this.weaponManager.IsShooting && !this.player.motor.Dashing && !this.player.motor.IsHit && !this.player.motor.IsUsingSuperOrEx && !this.alreadyReleased)
			{
				if (!this.charging)
				{
					this.StartCharging();
				}
				this.charging = true;
			}
			else if (this.charging || this.alreadyReleased)
			{
				this.charging = false;
				this.alreadyReleased = false;
				this.alreadyHeld = false;
				this.weaponManager.TriggerWeaponFire();
				this.getFiringMethod(mode)();
				if (!this.weaponManager.IsShooting)
				{
					this.firing.Set(mode, false);
				}
				this.t = 0f;
			}
			else if (!this.charging)
			{
				this.StopCharging();
			}
		}
		yield break;
	}

	// Token: 0x06003F64 RID: 16228 RVA: 0x0022A83A File Offset: 0x00228C3A
	protected virtual void StartCharging()
	{
	}

	// Token: 0x06003F65 RID: 16229 RVA: 0x0022A83C File Offset: 0x00228C3C
	protected virtual void StopCharging()
	{
	}

	// Token: 0x0400464F RID: 17999
	[Header("Ex")]
	[SerializeField]
	protected AbstractProjectile exPrefab;

	// Token: 0x04004650 RID: 18000
	[SerializeField]
	protected Effect exEffectPrefab;

	// Token: 0x04004651 RID: 18001
	[SerializeField]
	protected LevelPlayerWeaponFiringHitbox exFiringHitboxPrefab;

	// Token: 0x04004652 RID: 18002
	[Header("Basic")]
	[SerializeField]
	protected AbstractProjectile basicPrefab;

	// Token: 0x04004653 RID: 18003
	[SerializeField]
	protected Effect basicEffectPrefab;

	// Token: 0x04004654 RID: 18004
	[SerializeField]
	protected LevelPlayerWeaponFiringHitbox basicFiringHitboxPrefab;

	// Token: 0x04004656 RID: 18006
	protected AbstractLevelWeapon.FiringSwitches firing;

	// Token: 0x04004657 RID: 18007
	protected LevelPlayerController player;

	// Token: 0x04004658 RID: 18008
	protected LevelPlayerWeaponManager weaponManager;

	// Token: 0x04004659 RID: 18009
	private string WeaponSound;

	// Token: 0x0400465A RID: 18010
	private bool isUsingLoop;

	// Token: 0x0400465B RID: 18011
	private bool coolingDown;

	// Token: 0x0400465C RID: 18012
	private float t = 1000f;

	// Token: 0x0400465D RID: 18013
	private bool charging;

	// Token: 0x0400465E RID: 18014
	private bool alreadyHeld;

	// Token: 0x0400465F RID: 18015
	private bool alreadyReleased;

	// Token: 0x02000A5E RID: 2654
	public enum Mode
	{
		// Token: 0x04004661 RID: 18017
		Basic,
		// Token: 0x04004662 RID: 18018
		Ex
	}

	// Token: 0x02000A5F RID: 2655
	// (Invoke) Token: 0x06003F67 RID: 16231
	private delegate AbstractProjectile FireProjectileDelegate();

	// Token: 0x02000A60 RID: 2656
	[Serializable]
	public class Prefabs
	{
		// Token: 0x06003F6B RID: 16235 RVA: 0x0022A846 File Offset: 0x00228C46
		public AbstractProjectile Get(AbstractLevelWeapon.Mode mode)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04004663 RID: 18019
		public AbstractProjectile basic;

		// Token: 0x04004664 RID: 18020
		public AbstractProjectile ex;
	}

	// Token: 0x02000A61 RID: 2657
	[Serializable]
	public class MuzzleEffects
	{
		// Token: 0x06003F6D RID: 16237 RVA: 0x0022A86F File Offset: 0x00228C6F
		public Effect Get(AbstractLevelWeapon.Mode mode)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x04004665 RID: 18021
		public Effect basic;

		// Token: 0x04004666 RID: 18022
		public Effect ex;
	}

	// Token: 0x02000A62 RID: 2658
	public class FiringSwitches
	{
		// Token: 0x06003F6F RID: 16239 RVA: 0x0022A898 File Offset: 0x00228C98
		public bool Get(AbstractLevelWeapon.Mode mode)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				return this.basic;
			}
			return this.ex;
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0022A8B9 File Offset: 0x00228CB9
		public void Set(AbstractLevelWeapon.Mode mode, bool val)
		{
			if (mode != AbstractLevelWeapon.Mode.Ex)
			{
				if (mode != AbstractLevelWeapon.Mode.Basic)
				{
				}
				this.basic = val;
			}
			else
			{
				this.ex = val;
			}
		}

		// Token: 0x04004667 RID: 18023
		public bool basic;

		// Token: 0x04004668 RID: 18024
		public bool ex;
	}
}
