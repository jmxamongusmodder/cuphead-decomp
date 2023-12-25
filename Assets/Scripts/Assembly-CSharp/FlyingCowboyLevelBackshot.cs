using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public class FlyingCowboyLevelBackshot : BasicUprightProjectile
{
	// Token: 0x06002107 RID: 8455 RVA: 0x00131524 File Offset: 0x0012F924
	public virtual BasicProjectile Create(Vector3 position, float rotation, float speed, float bulletSpeed, float health, float anticipationStartDistance, bool childParryable)
	{
		FlyingCowboyLevelBackshot flyingCowboyLevelBackshot = this.Create(position, rotation, speed) as FlyingCowboyLevelBackshot;
		flyingCowboyLevelBackshot.bulletSpeed = bulletSpeed;
		flyingCowboyLevelBackshot.StartCoroutine(flyingCowboyLevelBackshot.waitToShoot_cr(speed, anticipationStartDistance));
		flyingCowboyLevelBackshot.health = health;
		flyingCowboyLevelBackshot.childParryable = childParryable;
		return flyingCowboyLevelBackshot;
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x0013156E File Offset: 0x0012F96E
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x0013159C File Offset: 0x0012F99C
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.transform.position.x < FlyingCowboyLevelBackshot.AttackPosition)
		{
			base.transform.SetPosition(new float?(FlyingCowboyLevelBackshot.AttackPosition), null, null);
		}
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x001315F3 File Offset: 0x0012F9F3
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && !base.dead)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x00131634 File Offset: 0x0012FA34
	protected override void Die()
	{
		float speed = this.Speed;
		base.Die();
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr(speed));
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x00131664 File Offset: 0x0012FA64
	private IEnumerator death_cr(float speed)
	{
		Transform leftWing = this.leftWings.GetRandom<Transform>();
		leftWing.GetComponent<SpriteRenderer>().enabled = true;
		Transform rightWing = this.rightWings.GetRandom<Transform>();
		rightWing.GetComponent<SpriteRenderer>().enabled = true;
		base.animator.Play("Death");
		base.StartCoroutine(this.moveWings_cr(speed, leftWing, rightWing));
		this.SFX_COWGIRL_P1_HorseflySpit();
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Death", 0, true, false, true);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x00131688 File Offset: 0x0012FA88
	private IEnumerator moveWings_cr(float speed, Transform leftWing, Transform rightWing)
	{
		Vector3 wingSpeedLeft = new Vector2(-speed * UnityEngine.Random.Range(0.25f, 0.5f), -UnityEngine.Random.Range(75f, 125f));
		Vector3 windSpeedRight = new Vector2(-speed * UnityEngine.Random.Range(0.25f, 0.5f), -UnityEngine.Random.Range(75f, 125f));
		for (;;)
		{
			yield return null;
			Vector3 position = leftWing.position;
			position += wingSpeedLeft * CupheadTime.Delta;
			leftWing.position = position;
			position = rightWing.position;
			position += windSpeedRight * CupheadTime.Delta;
			rightWing.position = position;
		}
		yield break;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x001316B4 File Offset: 0x0012FAB4
	private IEnumerator waitToShoot_cr(float speed, float anticipationStartDistance)
	{
		float timeToAnticipation = anticipationStartDistance / speed;
		float remainder = MathUtilities.DecimalPart(timeToAnticipation / 1f);
		float offset = 1f - remainder;
		float totalNormalizedTime = timeToAnticipation / 1f + offset + 0.625f;
		base.animator.Update(0f);
		base.animator.Play(0, 0, 0.625f + offset);
		yield return base.animator.WaitForNormalizedTime(this, totalNormalizedTime, "Idle", 0, false, false, true);
		base.animator.Play("AnticipationStart");
		while (base.transform.position.x > -550f)
		{
			yield return null;
		}
		base.animator.SetTrigger("Attack");
		float initialSpeed = this.Speed;
		float decelerationTime = KinematicUtilities.CalculateTimeToChangeVelocity(initialSpeed, 0f, -550f - FlyingCowboyLevelBackshot.AttackPosition);
		float elapsedTime = 0f;
		while (elapsedTime < decelerationTime)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			this.Speed = Mathf.Lerp(initialSpeed, 0f, elapsedTime / decelerationTime);
		}
		this.move = false;
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Attack", 0, true, false, true);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x001316E0 File Offset: 0x0012FAE0
	private void animationEvent_ShootBullet()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float rotation = MathUtils.DirectionToAngle(next.center - this.projectileSpawnPosition.position);
		BasicProjectile basicProjectile = this.projectile.Create(this.projectileSpawnPosition.position, rotation, this.bulletSpeed);
		basicProjectile.SetParryable(this.childParryable);
		basicProjectile.StartCoroutine(this.growBullet(basicProjectile.transform));
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x00131758 File Offset: 0x0012FB58
	private IEnumerator growBullet(Transform transform)
	{
		transform.SetScale(new float?(0.6f), new float?(0.6f), null);
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.041666668f, false);
		float elapsedTime = 0f;
		while (elapsedTime < 0.3f)
		{
			yield return wait;
			elapsedTime += wait.totalDelta;
			float scale = Mathf.Lerp(0.6f, 1f, elapsedTime / 0.3f);
			transform.SetScale(new float?(scale), new float?(scale), null);
		}
		yield break;
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x00131773 File Offset: 0x0012FB73
	private void AnimationEvent_SFX_COWGIRL_P1_HorseflySpit()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P1_Horsefly_Spit");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_Horsefly_Spit");
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x0013178F File Offset: 0x0012FB8F
	private void SFX_COWGIRL_P1_HorseflySpit()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P1_Horsefly_Death");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_Horsefly_Death");
	}

	// Token: 0x040029AA RID: 10666
	private static readonly float AttackPosition = -600f;

	// Token: 0x040029AB RID: 10667
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x040029AC RID: 10668
	[SerializeField]
	private Transform projectileSpawnPosition;

	// Token: 0x040029AD RID: 10669
	[SerializeField]
	private Transform[] leftWings;

	// Token: 0x040029AE RID: 10670
	[SerializeField]
	private Transform[] rightWings;

	// Token: 0x040029AF RID: 10671
	private DamageReceiver damageReceiver;

	// Token: 0x040029B0 RID: 10672
	private float bulletSpeed;

	// Token: 0x040029B1 RID: 10673
	private float health;

	// Token: 0x040029B2 RID: 10674
	private bool childParryable;
}
