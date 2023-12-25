using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000774 RID: 1908
public class RobotLevelRobotBodyPart : AbstractCollidableObject
{
	// Token: 0x060029A4 RID: 10660 RVA: 0x0018485C File Offset: 0x00182C5C
	public virtual void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		this.parent = parent;
		this.parent.OnDeathEvent += this.Die;
		this.parent.OnPrimaryDeathEvent += this.OnPrimaryDeath;
		this.parent.OnSecondaryDeathEvent += this.OnSecondaryDeath;
		this.properties = properties;
		this.current = RobotLevelRobotBodyPart.state.primary;
		this.currentHealth[0] = (float)primaryHP;
		this.currentHealth[1] = (float)secondaryHP;
		this.attackDelayMinus = attackDelayMinus;
		base.StartCoroutine(this.checkCurrentState_cr());
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x001848F0 File Offset: 0x00182CF0
	protected virtual void StartPrimary()
	{
		base.StartCoroutine(this.primaryAttack_cr());
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x00184900 File Offset: 0x00182D00
	protected virtual IEnumerator primaryAttack_cr()
	{
		while (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			yield return CupheadTime.WaitForSeconds(this, this.primaryAttackDelay);
			this.isAttacking = true;
			this.OnPrimaryAttack();
			while (this.isAttacking)
			{
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x0018491B File Offset: 0x00182D1B
	protected virtual void StartSecondary()
	{
		base.StartCoroutine(this.secondaryAttack_cr());
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x0018492C File Offset: 0x00182D2C
	protected virtual IEnumerator secondaryAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		while (this.current == RobotLevelRobotBodyPart.state.secondary)
		{
			this.OnSecondaryAttack();
			yield return null;
			if (this.secondaryAttackDelay > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, this.secondaryAttackDelay);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x00184948 File Offset: 0x00182D48
	protected virtual void AttackDestroyed(bool isPrimary)
	{
		if (isPrimary)
		{
			AudioManager.Play("robot_vocals_angry");
			this.emitAudioFromObject.Add("robot_vocals_angry");
			this.parent.PrimaryDied();
			this.current = RobotLevelRobotBodyPart.state.secondary;
		}
		else
		{
			this.current = RobotLevelRobotBodyPart.state.none;
			base.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x0018499F File Offset: 0x00182D9F
	protected override void Awake()
	{
		this.currentHealth = new float[2];
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x001849D8 File Offset: 0x00182DD8
	protected virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float num = this.currentHealth[(int)this.current];
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			this.currentHealth[(int)this.current] -= info.damage;
		}
		if (num > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(num - this.currentHealth[(int)this.current], 0f, num));
		}
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x00184A50 File Offset: 0x00182E50
	protected IEnumerator checkCurrentState_cr()
	{
		while (this.current != RobotLevelRobotBodyPart.state.none)
		{
			RobotLevelRobotBodyPart.state state = this.current;
			if (state != RobotLevelRobotBodyPart.state.primary)
			{
				if (state == RobotLevelRobotBodyPart.state.secondary)
				{
					if (this.currentHealth[1] <= 0f)
					{
						this.AttackDestroyed(false);
					}
				}
			}
			else if (this.currentHealth[0] <= 0f)
			{
				this.AttackDestroyed(true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x00184A6B File Offset: 0x00182E6B
	protected virtual void OnPrimaryAttack()
	{
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x00184A6D File Offset: 0x00182E6D
	protected virtual void OnPrimaryDeath()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			this.primaryAttackDelay -= this.attackDelayMinus;
		}
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x00184A8D File Offset: 0x00182E8D
	protected virtual void OnSecondaryAttack()
	{
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x00184A8F File Offset: 0x00182E8F
	protected virtual void OnSecondaryDeath()
	{
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x00184A91 File Offset: 0x00182E91
	protected virtual void ExitCurrentAttacks()
	{
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x00184A93 File Offset: 0x00182E93
	protected virtual void DeathEffect()
	{
		if (this.deathEffect != null)
		{
			base.StartCoroutine(this.death_effects_cr());
		}
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x00184AB4 File Offset: 0x00182EB4
	private IEnumerator death_effects_cr()
	{
		for (;;)
		{
			yield return null;
			this.deathEffect.Create(base.transform.position).GetComponent<Animator>().SetBool("IsA", Rand.Bool());
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(2f, 5f));
		}
		yield break;
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x00184AD0 File Offset: 0x00182ED0
	protected virtual void Die()
	{
		this.StopAllCoroutines();
		this.ExitCurrentAttacks();
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
		UnityEngine.Object.Destroy(base.gameObject, 15f);
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x00184B14 File Offset: 0x00182F14
	protected override void OnDestroy()
	{
		if (this.parent != null)
		{
			this.parent.OnDeathEvent -= this.Die;
			this.parent.OnPrimaryDeathEvent -= this.OnPrimaryDeath;
			this.parent.OnSecondaryDeathEvent -= this.OnSecondaryDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x04003292 RID: 12946
	protected RobotLevelRobotBodyPart.state current;

	// Token: 0x04003293 RID: 12947
	protected LevelProperties.Robot properties;

	// Token: 0x04003294 RID: 12948
	protected RobotLevelRobot parent;

	// Token: 0x04003295 RID: 12949
	protected float decreaseAttackDelayAmount;

	// Token: 0x04003296 RID: 12950
	protected float[] currentHealth;

	// Token: 0x04003297 RID: 12951
	protected float primaryAttackDelay;

	// Token: 0x04003298 RID: 12952
	protected float secondaryAttackDelay;

	// Token: 0x04003299 RID: 12953
	protected float attackDelayMinus;

	// Token: 0x0400329A RID: 12954
	protected bool isAttacking;

	// Token: 0x0400329B RID: 12955
	protected DamageReceiver damageReceiver;

	// Token: 0x0400329C RID: 12956
	[SerializeField]
	protected Effect deathEffect;

	// Token: 0x0400329D RID: 12957
	[SerializeField]
	protected GameObject primary;

	// Token: 0x0400329E RID: 12958
	[SerializeField]
	protected GameObject secondary;

	// Token: 0x02000775 RID: 1909
	protected enum state
	{
		// Token: 0x040032A0 RID: 12960
		primary,
		// Token: 0x040032A1 RID: 12961
		secondary,
		// Token: 0x040032A2 RID: 12962
		none
	}
}
