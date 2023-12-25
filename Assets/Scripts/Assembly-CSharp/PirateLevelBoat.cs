using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200071B RID: 1819
public class PirateLevelBoat : LevelProperties.Pirate.Entity
{
	// Token: 0x0600278D RID: 10125 RVA: 0x001734E6 File Offset: 0x001718E6
	protected override void Awake()
	{
		base.Awake();
		this.idle = new PirateLevelBoat.IdleManager();
		this.ully.gameObject.SetActive(false);
		base.GetComponent<LevelBossDeathExploder>().enabled = false;
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x00173516 File Offset: 0x00171916
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		properties.OnStateChange += this.OnStateChange;
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x00173544 File Offset: 0x00171944
	private void OnStateChange()
	{
		base.animator.Play("Idle");
		this.StopAllCoroutines();
		if (base.properties.CurrentState.cannon.firing)
		{
			base.StartCoroutine(this.cannon_cr(base.properties.CurrentState.cannon.delayRange.RandomFloat()));
		}
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x001735A8 File Offset: 0x001719A8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.cannonProjectile = null;
		this.cannonSmokePrefab = null;
		this.projectilePrefab = null;
		this.beamPrefab = null;
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x001735CC File Offset: 0x001719CC
	private void OnIdleEnd()
	{
		if (this.idle.loops >= this.idle.max)
		{
			base.animator.SetTrigger("OnBlink");
			return;
		}
		this.idle.loops++;
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x00173618 File Offset: 0x00171A18
	private void OnBlink()
	{
		this.idle.OnBlink();
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x00173628 File Offset: 0x00171A28
	private void FireCannon()
	{
		AudioManager.Play("level_pirate_ship_cannon_fire");
		if (!base.properties.CurrentState.cannon.firing)
		{
			return;
		}
		this.cannonSmokePrefab.Create(new Vector2(this.cannonRoot.position.x + 50f, this.cannonRoot.position.y));
		BasicProjectile basicProjectile = this.cannonProjectile.Create(this.cannonRoot.position, 0f, -base.properties.CurrentState.cannon.speed);
		basicProjectile.CollisionDeath.None();
		basicProjectile.DamagesType.OnlyPlayer();
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x001736EC File Offset: 0x00171AEC
	private IEnumerator cannon_cr(float delay)
	{
		if (delay < 1f)
		{
			delay = 1f;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			base.animator.Play("Cannon");
		}
		yield break;
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x0017370E File Offset: 0x00171B0E
	private void ChewSound()
	{
		AudioManager.Play("level_pirate_ship_cannon_chew");
		this.emitAudioFromObject.Add("level_pirate_ship_cannon_chew");
	}

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06002796 RID: 10134 RVA: 0x0017372C File Offset: 0x00171B2C
	// (remove) Token: 0x06002797 RID: 10135 RVA: 0x00173764 File Offset: 0x00171B64
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnLaunchPirate;

	// Token: 0x06002798 RID: 10136 RVA: 0x0017379A File Offset: 0x00171B9A
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x001737AD File Offset: 0x00171BAD
	public void StartTransformation()
	{
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<LevelBossDeathExploder>().enabled = true;
		this.hasTransformed = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.transform_cr());
	}

	// Token: 0x0600279A RID: 10138 RVA: 0x001737EC File Offset: 0x00171BEC
	private void LaunchPirate()
	{
		CupheadLevelCamera.Current.Shake(15f, 1f, false);
		if (this.OnLaunchPirate != null)
		{
			this.OnLaunchPirate();
		}
		this.SetNewHeight();
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x00173820 File Offset: 0x00171C20
	private void Shoot()
	{
		AudioManager.Play("level_pirate_ship_uvula_shoot");
		this.emitAudioFromObject.Add("level_pirate_ship_uvula_shoot");
		this.projectilePrefab.Create(this.projectileRoot.position, this.boatProperties.bulletSpeed, this.boatProperties.bulletRotationSpeed);
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x0017387C File Offset: 0x00171C7C
	private void SetNewHeight()
	{
		UnityEngine.Object.FindObjectOfType<PirateLevelBoatContainer>().EndBobbing();
		base.transform.parent.SetLocalPosition(null, new float?(70f), null);
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x001738C0 File Offset: 0x00171CC0
	private void OnBossDeath()
	{
		this.StopAllCoroutines();
		CupheadLevelCamera.Current.ResetShake();
		if (this.hasTransformed)
		{
			if (this.beam != null)
			{
				this.beam.EndBeam();
			}
			base.animator.SetTrigger("OnDeath");
		}
		else
		{
			base.animator.SetTrigger("OnEasyDeath");
		}
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x0017392C File Offset: 0x00171D2C
	private IEnumerator transform_cr()
	{
		this.boatProperties = base.properties.CurrentState.boat;
		base.animator.Play("Idle");
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		base.animator.SetTrigger("OnTransform");
		AudioManager.Play("level_pirate_boat_transform");
		this.emitAudioFromObject.Add("level_pirate_boat_transform");
		yield return CupheadTime.WaitForSeconds(this, this.boatProperties.winceDuration);
		base.animator.SetTrigger("OnTransformContinue");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.ully.gameObject.SetActive(true);
		for (;;)
		{
			for (int count = 0; count < this.boatProperties.bulletCount; count++)
			{
				yield return CupheadTime.WaitForSeconds(this, this.boatProperties.attackDelay);
				base.animator.SetTrigger("OnShoot");
			}
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.bulletPostWait);
			base.animator.SetTrigger("OnBeamStart");
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.beamDelay + 1f);
			base.animator.SetTrigger("OnBeamContinue");
			CupheadLevelCamera.Current.StartShake(2f);
			this.beam = this.beamPrefab.Create(this.beamRoot);
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.beamDuration);
			base.animator.SetTrigger("OnBeamEnd");
			CupheadLevelCamera.Current.EndShake(0.4f);
			this.beam.EndBeam();
			this.beam = null;
			yield return CupheadTime.WaitForSeconds(this, this.boatProperties.beamPostWait);
			base.animator.Play("Transform_Idle");
		}
		yield break;
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x00173948 File Offset: 0x00171D48
	private IEnumerator delay_cr(int frameDelay)
	{
		for (int i = 0; i < frameDelay; i++)
		{
			yield return null;
		}
		base.StartCoroutine(this.transform_cr());
		yield break;
	}

	// Token: 0x04003057 RID: 12375
	[SerializeField]
	private DamageReceiver damageReceiver;

	// Token: 0x04003058 RID: 12376
	[Space(10f)]
	[SerializeField]
	private Transform cannonRoot;

	// Token: 0x04003059 RID: 12377
	[SerializeField]
	private BasicProjectile cannonProjectile;

	// Token: 0x0400305A RID: 12378
	[SerializeField]
	private Effect cannonSmokePrefab;

	// Token: 0x0400305B RID: 12379
	[Space(10f)]
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x0400305C RID: 12380
	[SerializeField]
	private Transform beamRoot;

	// Token: 0x0400305D RID: 12381
	[SerializeField]
	private PirateLevelBoatProjectile projectilePrefab;

	// Token: 0x0400305E RID: 12382
	[SerializeField]
	private PirateLevelBoatBeam beamPrefab;

	// Token: 0x0400305F RID: 12383
	[Space(10f)]
	[SerializeField]
	private SpriteRenderer ully;

	// Token: 0x04003060 RID: 12384
	private PirateLevelBoat.IdleManager idle;

	// Token: 0x04003061 RID: 12385
	private bool hasTransformed;

	// Token: 0x04003062 RID: 12386
	private PirateLevelBoatBeam beam;

	// Token: 0x04003063 RID: 12387
	private const float Y_TRANSFORMED = 70f;

	// Token: 0x04003065 RID: 12389
	private LevelProperties.Pirate.Boat boatProperties;

	// Token: 0x0200071C RID: 1820
	public class IdleManager
	{
		// Token: 0x060027A1 RID: 10145 RVA: 0x0017397A File Offset: 0x00171D7A
		public void OnBlink()
		{
			this.max = UnityEngine.Random.Range(20, 61);
			this.loops = 0;
		}

		// Token: 0x04003066 RID: 12390
		private const int MIN_LOOPS = 20;

		// Token: 0x04003067 RID: 12391
		private const int MAX_LOOPS = 60;

		// Token: 0x04003068 RID: 12392
		public int loops;

		// Token: 0x04003069 RID: 12393
		public int max = 20;
	}
}
