using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class FlyingBlimpLevelArrowProjectile : HomingProjectile
{
	// Token: 0x06002034 RID: 8244 RVA: 0x00128158 File Offset: 0x00126558
	public FlyingBlimpLevelArrowProjectile Create(Vector2 pos, float startRotation, float startSpeed, float speed, float rotation, float timeBeforeDeath, float timeBeforeHoming, AbstractPlayerController player, float hp)
	{
		FlyingBlimpLevelArrowProjectile flyingBlimpLevelArrowProjectile = base.Create(pos, startRotation, startSpeed, speed, rotation, timeBeforeDeath, timeBeforeHoming, player) as FlyingBlimpLevelArrowProjectile;
		flyingBlimpLevelArrowProjectile.CollisionDeath.OnlyPlayer();
		flyingBlimpLevelArrowProjectile.DamagesType.OnlyPlayer();
		flyingBlimpLevelArrowProjectile.health = hp;
		flyingBlimpLevelArrowProjectile.timeToDeath = timeBeforeDeath;
		flyingBlimpLevelArrowProjectile.speed = speed;
		return flyingBlimpLevelArrowProjectile;
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x001281AE File Offset: 0x001265AE
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x001281E6 File Offset: 0x001265E6
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x001281FC File Offset: 0x001265FC
	private IEnumerator trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			Effect trail = UnityEngine.Object.Instantiate<Effect>(this.trailPrefab);
			trail.transform.position = base.transform.position;
			trail.GetComponent<Animator>().SetInteger("PickAni", UnityEngine.Random.Range(0, 3));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x00128217 File Offset: 0x00126617
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x00128244 File Offset: 0x00126644
	protected override void Die()
	{
		base.animator.SetTrigger("dead");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-90f));
		base.Die();
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x001282A2 File Offset: 0x001266A2
	private void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x001282B0 File Offset: 0x001266B0
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.timeToDeath);
		YieldInstruction wait = new WaitForFixedUpdate();
		base.HomingEnabled = false;
		for (;;)
		{
			base.transform.position += base.transform.right * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x040028B4 RID: 10420
	[SerializeField]
	private Effect trailPrefab;

	// Token: 0x040028B5 RID: 10421
	private float speed;

	// Token: 0x040028B6 RID: 10422
	private float health;

	// Token: 0x040028B7 RID: 10423
	private float timeToDeath;

	// Token: 0x040028B8 RID: 10424
	private DamageReceiver damageReceiver;
}
