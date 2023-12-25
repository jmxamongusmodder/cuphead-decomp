using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000899 RID: 2201
public class CircusPlatformingLevelArcade : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600332F RID: 13103 RVA: 0x001DCB07 File Offset: 0x001DAF07
	protected override void OnStart()
	{
		base.StartCoroutine(this.shoot_cr());
		this.goingRight = Rand.Bool();
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x001DCB21 File Offset: 0x001DAF21
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x001DCB38 File Offset: 0x001DAF38
	private IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x001DCB54 File Offset: 0x001DAF54
	private IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeAttackDelayInit.RandomFloat());
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			base.animator.SetBool("IsAttacking", true);
			this.isAttacking = true;
			while (this.isAttacking)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeAttackDelay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x001DCB70 File Offset: 0x001DAF70
	private void Shoot()
	{
		this.goingRight = !this.goingRight;
		this.introBulletInstance = UnityEngine.Object.Instantiate<Transform>(this.introBullet);
		base.StartCoroutine(this.shoot_intro_cr());
		base.animator.SetBool("IsAttacking", false);
		base.StartCoroutine(this.drop_cr());
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x001DCBC8 File Offset: 0x001DAFC8
	private IEnumerator shoot_intro_cr()
	{
		while (this.introBulletInstance.position.y < (float)Level.Current.Ceiling + 100f)
		{
			this.introBulletInstance.position += Vector3.up * base.Properties.arcadeBulletSpeed * CupheadTime.Delta;
			yield return null;
		}
		UnityEngine.Object.Destroy(this.introBulletInstance.gameObject);
		yield break;
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x001DCBE4 File Offset: 0x001DAFE4
	private IEnumerator drop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeBulletReturnDelay);
		AbstractPlayerController player = PlayerManager.GetNext();
		float sizeX = 100f;
		float posX = (!this.goingRight) ? this.bulletSpawnB.transform.position.x : this.bulletSpawnA.transform.position.x;
		for (int i = 0; i < base.Properties.arcadeBulletCount; i++)
		{
			if (player == null)
			{
				player = PlayerManager.GetNext();
			}
			yield return null;
			this.bullet.Create(new Vector2((!this.goingRight) ? (posX - sizeX * (float)i) : (posX + sizeX * (float)i), CupheadLevelCamera.Current.Bounds.yMax + 50f), -90f, base.Properties.arcadeBulletSpeed);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeBulletIndividualDelay);
		}
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x001DCC00 File Offset: 0x001DB000
	protected override void Die()
	{
		AudioManager.Play("circus_arcade_death");
		this.emitAudioFromObject.Add("circus_arcade_death");
		base.animator.Play("Death");
		this.effect.Create(base.transform.position);
		this.StopAllCoroutines();
		if (this.introBulletInstance != null)
		{
			UnityEngine.Object.Destroy(this.introBulletInstance.gameObject);
		}
		base.StartCoroutine(this.Explosion_cr());
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x001DCC90 File Offset: 0x001DB090
	private IEnumerator Explosion_cr()
	{
		this.exploder.StartExplosion();
		yield return new WaitForSeconds(2.5f);
		this.exploder.StopExplosions();
		yield break;
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x001DCCAB File Offset: 0x001DB0AB
	private void AttackSFX()
	{
		AudioManager.Play("circus_arcade_attack");
		this.emitAudioFromObject.Add("circus_arcade_attack");
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x001DCCC7 File Offset: 0x001DB0C7
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(this.bulletSpawnA.position, 50f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.bulletSpawnB.position, 50f);
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x001DCD03 File Offset: 0x001DB103
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effect = null;
		this.bullet = null;
		this.introBullet = null;
		this.introBulletInstance = null;
	}

	// Token: 0x04003B71 RID: 15217
	[SerializeField]
	private Transform bulletSpawnA;

	// Token: 0x04003B72 RID: 15218
	[SerializeField]
	private Transform bulletSpawnB;

	// Token: 0x04003B73 RID: 15219
	[SerializeField]
	private Effect effect;

	// Token: 0x04003B74 RID: 15220
	[SerializeField]
	private Transform arcadeRoot;

	// Token: 0x04003B75 RID: 15221
	[SerializeField]
	private Transform introBullet;

	// Token: 0x04003B76 RID: 15222
	[SerializeField]
	private BasicProjectile bullet;

	// Token: 0x04003B77 RID: 15223
	[SerializeField]
	private LevelBossDeathExploder exploder;

	// Token: 0x04003B78 RID: 15224
	private float offset = 50f;

	// Token: 0x04003B79 RID: 15225
	private bool isAttacking;

	// Token: 0x04003B7A RID: 15226
	private bool goingRight;

	// Token: 0x04003B7B RID: 15227
	private Transform introBulletInstance;
}
