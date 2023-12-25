using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public class RumRunnersLevelMobBoss : AbstractCollidableObject
{
	// Token: 0x06002B03 RID: 11011 RVA: 0x0019150A File Offset: 0x0018F90A
	protected override void Awake()
	{
		base.Awake();
		this.circleCollider = base.GetComponent<CircleCollider2D>();
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x00191520 File Offset: 0x0018F920
	public void Setup(LevelProperties.RumRunners properties, RumRunnersLevelAnteater anteater, Transform positioner)
	{
		this.properties = properties;
		this.anteater = anteater;
		this.positioner = positioner;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.parryString = new PatternString(properties.CurrentState.boss.bossProjectileParryString, true);
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x00191584 File Offset: 0x0018F984
	public void Begin()
	{
		this.begun = true;
		base.gameObject.SetActive(true);
		this.setActiveDirection(RumRunnersLevelMobBoss.Direction.Attack0);
		base.animator.Update(0f);
		base.StartCoroutine(this.timer_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x001915D5 File Offset: 0x0018F9D5
	private void LateUpdate()
	{
		if (!this.begun)
		{
			return;
		}
		this.updatePosition();
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x001915EC File Offset: 0x0018F9EC
	private IEnumerator shoot_cr()
	{
		LevelProperties.RumRunners.Boss p = this.properties.CurrentState.boss;
		yield return CupheadTime.WaitForSeconds(this, p.initialDelay);
		for (;;)
		{
			AbstractPlayerController player = PlayerManager.GetNext();
			this.targetedPlayer = player.id;
			this.targetedPosition = player.center;
			Vector3 bossCenter = this.circleCollider.bounds.center;
			float angle = MathUtils.DirectionToAngle(this.targetedPosition - bossCenter);
			if ((this.shootingRight && this.targetedPosition.x < bossCenter.x) || (!this.shootingRight && this.targetedPosition.x > bossCenter.x))
			{
				base.animator.SetTrigger("Turn");
			}
			this.targetedDirection = this.chooseDirection(angle, true);
			this.setActiveDirection(this.targetedDirection);
			base.animator.SetTrigger("Attack");
			int animatorHash = Animator.StringToHash("AttackMiddle");
			while (this.getAnimatorCurrentStateInfo().shortNameHash != animatorHash)
			{
				yield return null;
			}
			while (this.getAnimatorCurrentStateInfo().normalizedTime < 1f)
			{
				yield return null;
			}
			this.shoot();
			base.animator.SetTrigger("Continue");
			animatorHash = Animator.StringToHash("AttackEnd");
			while (this.getAnimatorCurrentStateInfo().shortNameHash != animatorHash)
			{
				yield return null;
			}
			while (this.getAnimatorCurrentStateInfo().shortNameHash == animatorHash)
			{
				yield return null;
			}
			float totalAttackDelay = p.coinDelay.GetFloatAt(this.minMaxParameter);
			if (totalAttackDelay > 0.6666667f)
			{
				if (totalAttackDelay <= 0.7083333f)
				{
					base.animator.SetFloat(RumRunnersLevelMobBoss.StartSpeedParameter, 1.2f);
				}
				float waitTime = totalAttackDelay - 0.6666667f;
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
			}
			else
			{
				float value;
				float value2;
				if (totalAttackDelay > 0.5833333f)
				{
					value = 1.2f;
					value2 = 1f;
				}
				else if (totalAttackDelay > 0.5416667f)
				{
					value = 1.2f;
					value2 = 1.3333334f;
				}
				else if (totalAttackDelay > 0.5f)
				{
					value = 1.5f;
					value2 = 1.3333334f;
				}
				else if (totalAttackDelay > 0.45833334f)
				{
					value = 2f;
					value2 = 1.3333334f;
				}
				else
				{
					value = 2f;
					value2 = 2f;
				}
				base.animator.SetFloat(RumRunnersLevelMobBoss.StartSpeedParameter, value);
				base.animator.SetFloat(RumRunnersLevelMobBoss.EndSpeedParameter, value2);
			}
		}
		yield break;
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x00191608 File Offset: 0x0018FA08
	private IEnumerator timer_cr()
	{
		LevelProperties.RumRunners.Boss p = this.properties.CurrentState.boss;
		float totalTime = p.coinMinMaxTime;
		float elapsedTime = 0f;
		while (elapsedTime < totalTime)
		{
			elapsedTime += CupheadTime.Delta;
			this.minMaxParameter = Mathf.Clamp01(elapsedTime / totalTime);
			yield return null;
		}
		this.minMaxParameter = 1f;
		yield break;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x00191623 File Offset: 0x0018FA23
	private void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.StopAllCoroutines();
		base.gameObject.SetActive(false);
		this.anteater.RealDeath();
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x00191655 File Offset: 0x0018FA55
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.properties.DealDamage(info.damage);
		if (this.properties.CurrentHealth <= 0f && !this.dead)
		{
			this.die();
		}
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x00191690 File Offset: 0x0018FA90
	private void animationEvent_Flip()
	{
		this.shootingRight = !this.shootingRight;
		Vector3 localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
		if (PlayerManager.DoesPlayerExist(this.targetedPlayer))
		{
			this.targetedPosition = PlayerManager.GetPlayer(this.targetedPlayer).center;
		}
		Vector3 center = this.circleCollider.bounds.center;
		float angle = MathUtils.DirectionToAngle(this.targetedPosition - center);
		this.targetedDirection = this.chooseDirection(angle, true);
		this.setActiveDirection(this.targetedDirection);
		this.updatePosition();
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x00191748 File Offset: 0x0018FB48
	private void shoot()
	{
		Vector3 center;
		if (PlayerManager.DoesPlayerExist(this.targetedPlayer))
		{
			center = PlayerManager.GetPlayer(this.targetedPlayer).center;
		}
		else
		{
			center = this.targetedPosition;
		}
		Vector3 vector = base.transform.TransformPoint(this.projectileRoots[(int)this.targetedDirection]);
		float num = MathUtils.DirectionToAngle(center - vector);
		float num2 = (!this.shootingRight) ? RumRunnersLevelMobBoss.ReferenceAnglesLeft[(int)this.targetedDirection] : RumRunnersLevelMobBoss.ReferenceAnglesRight[(int)this.targetedDirection];
		float num3 = (!this.shootingRight) ? ((num <= 0f) ? (-180f - num) : (180f - num)) : num;
		float num4 = (!this.shootingRight) ? ((num2 <= 0f) ? (-180f - num2) : (180f - num2)) : num2;
		if (Mathf.Abs(num3 - num4) > RumRunnersLevelMobBoss.AcceptableAngleVariance)
		{
			RumRunnersLevelMobBoss.Direction direction = this.chooseDirection(num, true);
			int num5 = direction - this.targetedDirection;
			if (Mathf.Abs(num5) > 1)
			{
				num5 = (int)Mathf.Sign((float)num5);
				this.targetedDirection = (RumRunnersLevelMobBoss.Direction)Mathf.Clamp((int)(this.targetedDirection + num5), 0, 8);
			}
			else
			{
				this.targetedDirection = direction;
			}
			this.setActiveDirection(this.targetedDirection);
			vector = base.transform.TransformPoint(this.projectileRoots[(int)this.targetedDirection]);
		}
		if (this.shootingRight)
		{
			num2 = RumRunnersLevelMobBoss.ReferenceAnglesRight[(int)this.targetedDirection];
			num = Mathf.Clamp(num, num2 - RumRunnersLevelMobBoss.AcceptableAngleVariance, num2 + RumRunnersLevelMobBoss.AcceptableAngleVariance);
		}
		else if (this.targetedDirection == RumRunnersLevelMobBoss.Direction.Attack0)
		{
			if (num < 0f)
			{
				num = Mathf.Clamp(num, -180f - RumRunnersLevelMobBoss.AcceptableAngleVariance, -180f + RumRunnersLevelMobBoss.AcceptableAngleVariance);
			}
			else
			{
				num = Mathf.Clamp(num, 180f - RumRunnersLevelMobBoss.AcceptableAngleVariance, 180f + RumRunnersLevelMobBoss.AcceptableAngleVariance);
			}
		}
		else
		{
			num2 = RumRunnersLevelMobBoss.ReferenceAnglesLeft[(int)this.targetedDirection];
			num = Mathf.Clamp(num, num2 - RumRunnersLevelMobBoss.AcceptableAngleVariance, num2 + RumRunnersLevelMobBoss.AcceptableAngleVariance);
		}
		float floatAt = this.properties.CurrentState.boss.coinSpeed.GetFloatAt(this.minMaxParameter);
		BasicProjectile basicProjectile = this.projectile.Create(vector, num, floatAt);
		this.projectileMuzzleFX.Create(vector).transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(num));
		basicProjectile.SetParryable(this.parryString.PopLetter() == 'P');
		this.SFX_RUMRUN_P4_Snail_ProjectileShoot();
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x00191A10 File Offset: 0x0018FE10
	private void updatePosition()
	{
		Vector3 a = this.positioner.position;
		if (!this.shootingRight)
		{
			a += this.flippedOffset;
		}
		base.transform.position = a + this.positionOffset;
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x00191A64 File Offset: 0x0018FE64
	private RumRunnersLevelMobBoss.Direction chooseDirection(float angle, bool canOvershoot)
	{
		RumRunnersLevelMobBoss.Direction result;
		if (this.shootingRight)
		{
			if (angle > 33.75f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack45;
			}
			else if (angle > 11.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack22;
			}
			else if (angle > -11.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack0;
			}
			else if (angle > -33.75f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack337;
			}
			else if (angle > -56.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack315;
			}
			else if (angle > -78.75f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack292;
			}
			else if (!canOvershoot)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack270;
			}
			else if (angle > -101.25f)
			{
				result = RumRunnersLevelMobBoss.Direction.Attack270;
			}
			else
			{
				result = RumRunnersLevelMobBoss.Direction.Attack247;
			}
		}
		else if (angle >= 168.75f || angle <= -168.75f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack0;
		}
		else if (angle < -146.25f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack337;
		}
		else if (angle < -123.75f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack315;
		}
		else if (angle < -101.25f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack292;
		}
		else if ((!canOvershoot && angle < 0f) || (canOvershoot && angle < -78.75f))
		{
			result = RumRunnersLevelMobBoss.Direction.Attack270;
		}
		else if (canOvershoot && angle < 0f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack247;
		}
		else if (angle < 168.75f)
		{
			result = RumRunnersLevelMobBoss.Direction.Attack22;
		}
		else
		{
			result = RumRunnersLevelMobBoss.Direction.Attack45;
		}
		return result;
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x00191BB7 File Offset: 0x0018FFB7
	private AnimatorStateInfo getAnimatorCurrentStateInfo()
	{
		return base.animator.GetCurrentAnimatorStateInfo((int)(this.targetedDirection + 1));
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x00191BCC File Offset: 0x0018FFCC
	private void setActiveDirection(RumRunnersLevelMobBoss.Direction direction)
	{
		for (int i = 1; i <= 8; i++)
		{
			float weight = (direction != (RumRunnersLevelMobBoss.Direction)(i - 1)) ? 0f : 1f;
			base.animator.SetLayerWeight(i, weight);
		}
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x00191C11 File Offset: 0x00190011
	private void SFX_RUMRUN_P4_Snail_ProjectileShoot()
	{
		AudioManager.Play("sfx_dlc_rumrun_p4_snail_projectile_shoot");
	}

	// Token: 0x040033B3 RID: 13235
	private static readonly float AcceptableAngleVariance = 15f;

	// Token: 0x040033B4 RID: 13236
	private static readonly float[] ReferenceAnglesRight = new float[]
	{
		45f,
		22.5f,
		0f,
		-22.5f,
		-45f,
		-67.5f,
		-90f,
		-112.5f
	};

	// Token: 0x040033B5 RID: 13237
	private static readonly float[] ReferenceAnglesLeft = new float[]
	{
		135f,
		157.5f,
		180f,
		-157.5f,
		-135f,
		-112.5f,
		-90f,
		-67.5f
	};

	// Token: 0x040033B6 RID: 13238
	private static readonly int StartSpeedParameter = Animator.StringToHash("StartSpeed");

	// Token: 0x040033B7 RID: 13239
	private static readonly int EndSpeedParameter = Animator.StringToHash("EndSpeed");

	// Token: 0x040033B8 RID: 13240
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x040033B9 RID: 13241
	[SerializeField]
	private Effect projectileMuzzleFX;

	// Token: 0x040033BA RID: 13242
	[SerializeField]
	private Vector2[] projectileRoots;

	// Token: 0x040033BB RID: 13243
	[SerializeField]
	private Vector2 positionOffset;

	// Token: 0x040033BC RID: 13244
	[SerializeField]
	private Vector2 flippedOffset;

	// Token: 0x040033BD RID: 13245
	private bool begun;

	// Token: 0x040033BE RID: 13246
	private bool dead;

	// Token: 0x040033BF RID: 13247
	private float minMaxParameter;

	// Token: 0x040033C0 RID: 13248
	private bool shootingRight = true;

	// Token: 0x040033C1 RID: 13249
	private PatternString parryString;

	// Token: 0x040033C2 RID: 13250
	private PlayerId targetedPlayer;

	// Token: 0x040033C3 RID: 13251
	private Vector3 targetedPosition;

	// Token: 0x040033C4 RID: 13252
	private RumRunnersLevelMobBoss.Direction targetedDirection;

	// Token: 0x040033C5 RID: 13253
	private LevelProperties.RumRunners properties;

	// Token: 0x040033C6 RID: 13254
	private RumRunnersLevelAnteater anteater;

	// Token: 0x040033C7 RID: 13255
	private Transform positioner;

	// Token: 0x040033C8 RID: 13256
	private DamageReceiver damageReceiver;

	// Token: 0x040033C9 RID: 13257
	private CircleCollider2D circleCollider;

	// Token: 0x02000794 RID: 1940
	private enum Direction
	{
		// Token: 0x040033CB RID: 13259
		Attack45,
		// Token: 0x040033CC RID: 13260
		Attack22,
		// Token: 0x040033CD RID: 13261
		Attack0,
		// Token: 0x040033CE RID: 13262
		Attack337,
		// Token: 0x040033CF RID: 13263
		Attack315,
		// Token: 0x040033D0 RID: 13264
		Attack292,
		// Token: 0x040033D1 RID: 13265
		Attack270,
		// Token: 0x040033D2 RID: 13266
		Attack247,
		// Token: 0x040033D3 RID: 13267
		AttackCount
	}
}
