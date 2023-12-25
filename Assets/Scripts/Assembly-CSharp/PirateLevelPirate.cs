using System;
using UnityEngine;

// Token: 0x0200072F RID: 1839
public class PirateLevelPirate : LevelProperties.Pirate.Entity
{
	// Token: 0x0600280A RID: 10250 RVA: 0x001763D0 File Offset: 0x001747D0
	protected override void Awake()
	{
		base.Awake();
		PirateLevel pirateLevel = Level.Current as PirateLevel;
		pirateLevel.OnWhistleEvent += this.onWhistle;
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x00176400 File Offset: 0x00174800
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnIntroEvent += this.OnIntroLaugh;
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x00176453 File Offset: 0x00174853
	private void OnIntroLaugh()
	{
		base.animator.SetTrigger("OnLaugh");
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x00176465 File Offset: 0x00174865
	private void onWhistle(PirateLevel.Creature creature)
	{
		this.whistles = 0;
		this.creature = creature;
		base.animator.SetTrigger("OnWhistle");
		this.loops = 1000;
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x00176490 File Offset: 0x00174890
	private void OnIdleEnd()
	{
		if (this.loops >= this.max)
		{
			int num = UnityEngine.Random.Range(0, 100);
			int value = 0;
			if (num <= this.bothChance)
			{
				value = 2;
			}
			else if (num <= this.patchChance + this.bothChance)
			{
				value = 1;
			}
			base.animator.SetInteger("Blink", value);
			base.animator.SetTrigger("OnBlink");
			return;
		}
		this.loops++;
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x00176511 File Offset: 0x00174911
	private void OnBlink()
	{
		this.max = UnityEngine.Random.Range(2, 5);
		this.loops = 0;
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x00176527 File Offset: 0x00174927
	private void OnBossDeath()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("level_pirate_fall_death");
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x00176549 File Offset: 0x00174949
	public void FireGun(LevelProperties.Pirate.Peashot properties)
	{
		base.animator.Play("Gun_Shoot");
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x0017655C File Offset: 0x0017495C
	private void Whistle()
	{
		int num = 1;
		PirateLevel.Creature creature = this.creature;
		if (creature != PirateLevel.Creature.DogFish)
		{
			if (creature == PirateLevel.Creature.Shark)
			{
				num = 3;
			}
		}
		else
		{
			num = 2;
		}
		if (this.whistles >= num)
		{
			return;
		}
		this.whistleEffect.Create(this.whistleRoot.position);
		this.whistles++;
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x001765C5 File Offset: 0x001749C5
	private void WhistleSFX()
	{
		AudioManager.Play("levels_pirate_whistle");
		this.emitAudioFromObject.Add("levels_pirate_whistle");
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x001765E1 File Offset: 0x001749E1
	public void EndGun()
	{
		base.animator.SetTrigger("OnGunEnd");
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x001765F3 File Offset: 0x001749F3
	private void PlayLaughSound()
	{
		AudioManager.Play("levels_pirate_laugh");
		this.emitAudioFromObject.Add("levels_pirate_laugh");
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x00176610 File Offset: 0x00174A10
	public void StartGun()
	{
		base.animator.SetTrigger("OnGunStart");
		this.gunProperties = base.properties.CurrentState.peashot;
		this.shotIndex = UnityEngine.Random.Range(0, this.gunProperties.shotType.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x0017666C File Offset: 0x00174A6C
	private void Shoot()
	{
		if (PlayerManager.Count <= 0)
		{
			this.gunRoot.LookAt2D(new Vector2(0f, 0f));
			return;
		}
		this.gunRoot.LookAt2D(PlayerManager.GetNext().center);
		AudioManager.Play("level_pirate_gun_shoot");
		this.emitAudioFromObject.Add("level_pirate_gun_shoot");
		this.muzzleFlash.Create(this.gunRoot.position);
		BasicProjectile basicProjectile = null;
		if (this.gunProperties.shotType.Split(new char[]
		{
			','
		})[this.shotIndex][0] == 'P')
		{
			basicProjectile = this.gunProjectile.Create(this.gunRoot.position, this.gunRoot.eulerAngles.z, new Vector3(-1f, -1f, 1f), this.gunProperties.speed);
			basicProjectile.SetParryable(true);
		}
		else if (this.gunProperties.shotType.Split(new char[]
		{
			','
		})[this.shotIndex][0] == 'R')
		{
			basicProjectile = this.gunProjectileRegular.Create(this.gunRoot.position, this.gunRoot.eulerAngles.z, new Vector3(-1f, -1f, 1f), this.gunProperties.speed);
		}
		basicProjectile.CollisionDeath.OnlyBounds();
		basicProjectile.CollisionDeath.Player = true;
		this.shotIndex = (this.shotIndex + 1) % this.gunProperties.shotType.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x0017683F File Offset: 0x00174C3F
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.properties.CurrentState.stateName == LevelProperties.Pirate.States.Boat)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x00176869 File Offset: 0x00174C69
	public void CleanUp()
	{
		base.properties.OnBossDeath -= this.OnBossDeath;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x0017688D File Offset: 0x00174C8D
	private void SoundGunStart()
	{
		AudioManager.Play("level_pirate_gun_start");
		this.emitAudioFromObject.Add("level_pirate_gun_start");
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x001768A9 File Offset: 0x00174CA9
	private void SoundGunEnd()
	{
		AudioManager.Play("level_pirate_gun_end");
		this.emitAudioFromObject.Add("level_pirate_gun_end");
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x001768C5 File Offset: 0x00174CC5
	private void SoundPirateFoot()
	{
		AudioManager.Play("level_pirate_pirate_foot");
		this.emitAudioFromObject.Add("level_pirate_pirate_foot");
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x001768E1 File Offset: 0x00174CE1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.gunProjectile = null;
		this.gunProjectileRegular = null;
		this.muzzleFlash = null;
		this.whistleEffect = null;
	}

	// Token: 0x040030CF RID: 12495
	private const int MIN_IDLE_LOOPS = 2;

	// Token: 0x040030D0 RID: 12496
	private const int MAX_IDLE_LOOPS = 4;

	// Token: 0x040030D1 RID: 12497
	[SerializeField]
	private Transform gunRoot;

	// Token: 0x040030D2 RID: 12498
	[SerializeField]
	private BasicProjectile gunProjectile;

	// Token: 0x040030D3 RID: 12499
	[SerializeField]
	private BasicProjectile gunProjectileRegular;

	// Token: 0x040030D4 RID: 12500
	[SerializeField]
	private Effect muzzleFlash;

	// Token: 0x040030D5 RID: 12501
	[SerializeField]
	private Transform whistleRoot;

	// Token: 0x040030D6 RID: 12502
	[SerializeField]
	private Effect whistleEffect;

	// Token: 0x040030D7 RID: 12503
	private LevelProperties.Pirate.Peashot gunProperties;

	// Token: 0x040030D8 RID: 12504
	private PirateLevel.Creature creature;

	// Token: 0x040030D9 RID: 12505
	private int whistles;

	// Token: 0x040030DA RID: 12506
	private int patchChance = 25;

	// Token: 0x040030DB RID: 12507
	private int bothChance = 15;

	// Token: 0x040030DC RID: 12508
	private int loops;

	// Token: 0x040030DD RID: 12509
	private int max = 2;

	// Token: 0x040030DE RID: 12510
	private int shotIndex;
}
