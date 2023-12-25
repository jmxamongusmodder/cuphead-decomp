using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000856 RID: 2134
public class VeggiesLevelPotato : LevelProperties.Veggies.Entity
{
	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06003179 RID: 12665 RVA: 0x001CE805 File Offset: 0x001CCC05
	// (set) Token: 0x0600317A RID: 12666 RVA: 0x001CE80D File Offset: 0x001CCC0D
	public VeggiesLevelPotato.State state { get; private set; }

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x0600317B RID: 12667 RVA: 0x001CE818 File Offset: 0x001CCC18
	// (remove) Token: 0x0600317C RID: 12668 RVA: 0x001CE850 File Offset: 0x001CCC50
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VeggiesLevelPotato.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x0600317D RID: 12669 RVA: 0x001CE886 File Offset: 0x001CCC86
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x001CE8AB File Offset: 0x001CCCAB
	private void Start()
	{
		this.SfxGround();
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x001CE8B4 File Offset: 0x001CCCB4
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Potato);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.potato_cr());
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x001CE90A File Offset: 0x001CCD0A
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x001CE924 File Offset: 0x001CCD24
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x001CE976 File Offset: 0x001CCD76
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x001CE994 File Offset: 0x001CCD94
	private void Die()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Dead");
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x001CE9B8 File Offset: 0x001CCDB8
	private void StartExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x001CE9C5 File Offset: 0x001CCDC5
	private void EndExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x001CE9D2 File Offset: 0x001CCDD2
	private void SfxGround()
	{
		AudioManager.Play("level_veggies_potato_ground");
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x001CE9DE File Offset: 0x001CCDDE
	private void OnInAnimComplete()
	{
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x001CE9E0 File Offset: 0x001CCDE0
	private void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelPotato.State.Complete;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x001CE9F4 File Offset: 0x001CCDF4
	private void Shoot()
	{
		if (!this.projectileParryFlag)
		{
			AudioManager.Play("levels_veggies_potato_spit");
		}
		else
		{
			AudioManager.Play("level_veggies_potato_spit_worm");
		}
		this.didShoot = true;
		BasicProjectile basicProjectile = this.projectilePrefab.Create(this.gunRoot.position, this.gunRoot.eulerAngles.z, this.properties.bulletSpeed);
		basicProjectile.SetParryable(this.projectileParryFlag);
		this.spitEffect.Create(this.gunRoot.position);
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x001CEA8C File Offset: 0x001CCE8C
	private IEnumerator potato_cr()
	{
		for (;;)
		{
			int groups = 0;
			int shots = 0;
			while (groups < this.properties.seriesCount)
			{
				float delay = this.properties.bulletDelay.GetFloatAt(1f - (float)groups / ((float)this.properties.seriesCount - 1f));
				while (shots < this.properties.bulletCount)
				{
					shots++;
					base.animator.SetTrigger("Shoot");
					this.didShoot = false;
					this.projectileParryFlag = (shots == this.properties.bulletCount);
					while (!this.didShoot)
					{
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, delay);
				}
				groups++;
				shots = 0;
				if (groups != this.properties.seriesCount)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.seriesDelay);
					yield return CupheadTime.WaitForSeconds(this, 0.6f);
				}
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime);
		}
		yield break;
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x001CEAA7 File Offset: 0x001CCEA7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefab = null;
		this.spitEffect = null;
	}

	// Token: 0x040039F7 RID: 14839
	private const float START_SHOOTING_TIME = 0.6f;

	// Token: 0x040039F9 RID: 14841
	[SerializeField]
	private Transform gunRoot;

	// Token: 0x040039FA RID: 14842
	[SerializeField]
	private VeggiesLevelSpit projectilePrefab;

	// Token: 0x040039FB RID: 14843
	[SerializeField]
	private Effect spitEffect;

	// Token: 0x040039FC RID: 14844
	private new LevelProperties.Veggies.Potato properties;

	// Token: 0x040039FD RID: 14845
	private float hp;

	// Token: 0x040039FE RID: 14846
	private DamageDealer damageDealer;

	// Token: 0x040039FF RID: 14847
	private bool didShoot = true;

	// Token: 0x04003A00 RID: 14848
	private bool projectileParryFlag;

	// Token: 0x02000857 RID: 2135
	public enum State
	{
		// Token: 0x04003A03 RID: 14851
		Incomplete,
		// Token: 0x04003A04 RID: 14852
		Complete
	}

	// Token: 0x02000858 RID: 2136
	// (Invoke) Token: 0x0600318D RID: 12685
	public delegate void OnDamageTakenHandler(float damage);
}
