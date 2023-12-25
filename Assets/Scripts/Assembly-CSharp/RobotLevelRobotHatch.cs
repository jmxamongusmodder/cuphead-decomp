using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000778 RID: 1912
public class RobotLevelRobotHatch : RobotLevelRobotBodyPart
{
	// Token: 0x060029D1 RID: 10705 RVA: 0x00186E40 File Offset: 0x00185240
	public override void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		this.primaryAttackDelay = properties.CurrentState.shotBot.initialSpawnDelay.RandomFloat();
		this.secondaryAttackDelay = properties.CurrentState.bombBot.bombDelay;
		primaryHP = properties.CurrentState.shotBot.hatchGateHealth;
		attackDelayMinus = properties.CurrentState.shotBot.shotbotSpawnDelayMinus;
		this.shotbotSpawnDelay = properties.CurrentState.shotBot.shotbotDelay;
		base.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus);
		base.animator.Play("Closed", 0, 0.75f);
		base.animator.Play("Loop", 1, 0.75f);
		base.animator.Play("Loop", 2, 0.75f);
		base.animator.Play("Loop", 3, 0.75f);
		this.StartPrimary();
		this.damageEffectRenderer = this.damageEffect.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x00186F34 File Offset: 0x00185334
	protected override void OnPrimaryAttack()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.StartCoroutine(this.openHatch_cr());
			this.primaryAttackDelay = this.properties.CurrentState.shotBot.shotbotWaveDelay.RandomFloat();
			base.OnPrimaryAttack();
		}
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x00186F74 File Offset: 0x00185374
	private IEnumerator openHatch_cr()
	{
		float elapsedTime = base.animator.GetCurrentAnimatorStateInfo(2).length;
		float normalizedTime = this.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime;
		normalizedTime %= 1f;
		float delay = normalizedTime * 24f;
		int currentFrame = (int)(delay / 24f);
		if (currentFrame < 2)
		{
			delay = (float)((2 - currentFrame) / 24) * elapsedTime;
			this.nearestEventFrame = 2;
		}
		else if (currentFrame < 14)
		{
			delay = (float)((14 - currentFrame) / 24) * elapsedTime;
			this.nearestEventFrame = 14;
		}
		else
		{
			delay = (float)(24 - currentFrame) * elapsedTime;
			delay += (float)(2 - currentFrame) * elapsedTime;
			this.nearestEventFrame = 2;
		}
		yield return CupheadTime.WaitForSeconds(this, delay);
		yield return null;
		normalizedTime = this.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime;
		if (this.nearestEventFrame == 2)
		{
			base.animator.SetTrigger("IsOpenFrame2");
		}
		else if (this.nearestEventFrame == 14)
		{
			base.animator.SetTrigger("IsOpenFrame14");
		}
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
		}
		yield break;
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x00186F90 File Offset: 0x00185390
	private void Open()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary)
		{
			base.GetComponent<SpriteRenderer>().enabled = true;
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x00186FE0 File Offset: 0x001853E0
	private void Close()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary)
		{
			base.GetComponent<SpriteRenderer>().enabled = false;
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x00187030 File Offset: 0x00185430
	private IEnumerator closeHatch_cr()
	{
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.animator.SetTrigger("IsClosing");
		yield return base.animator.WaitForAnimationToEnd(this, true);
		yield return null;
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			this.isAttacking = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x0018704B File Offset: 0x0018544B
	private void SpawnShotbotWave()
	{
		base.StartCoroutine(this.spawnShotbotWave_cr());
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x0018705C File Offset: 0x0018545C
	private IEnumerator spawnShotbotWave_cr()
	{
		for (int i = 0; i < this.properties.CurrentState.shotBot.shotbotCount; i++)
		{
			GameObject shotbot = UnityEngine.Object.Instantiate<GameObject>(this.primary, base.transform.position + Vector3.right * 80f + Vector3.down * 20f, Quaternion.identity);
			shotbot.GetComponent<RobotLevelHatchShotbot>().InitShotbot(this.properties.CurrentState.shotBot.shotbotHealth, this.properties.CurrentState.shotBot.bulletSpeed, this.properties.CurrentState.shotBot.pinkBulletCount, this.properties.CurrentState.shotBot.shotbotShootDelay, this.properties.CurrentState.shotBot.shotbotFlightSpeed);
			yield return CupheadTime.WaitForSeconds(this, this.shotbotSpawnDelay);
			if (this.current != RobotLevelRobotBodyPart.state.primary)
			{
				break;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.4f);
		base.StartCoroutine(this.closeHatch_cr());
		yield break;
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x00187078 File Offset: 0x00185478
	protected override void OnSecondaryAttack()
	{
		HomingProjectile homingProjectile = this.secondary.GetComponent<RobotLevelHatchBombBot>().Create(base.transform.position, 180f, (float)this.properties.CurrentState.bombBot.initialBombMovementSpeed, (float)this.properties.CurrentState.bombBot.bombHomingSpeed, this.properties.CurrentState.bombBot.bombRotationSpeed, (float)this.properties.CurrentState.bombBot.bombLifeTime, this.properties.CurrentState.bombBot.bombInitialMovementDuration.RandomFloat(), 4f, PlayerManager.GetNext());
		homingProjectile.GetComponent<RobotLevelHatchBombBot>().InitBombBot(this.properties.CurrentState.bombBot);
		homingProjectile.transform.right = Vector3.down;
		if (this.currentHealth[1] <= 0f)
		{
			base.gameObject.SetActive(false);
			this.StopAllCoroutines();
		}
		base.OnSecondaryAttack();
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x0018717C File Offset: 0x0018557C
	protected override void OnPrimaryDeath()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary && this.currentHealth[0] <= 0f)
		{
			AudioManager.Play("robot_lower_chest_port_destroyed");
			this.emitAudioFromObject.Add("robot_lower_chest_port_destroyed");
			base.animator.Play("Off");
			base.GetComponent<BoxCollider2D>().enabled = false;
			this.StartSecondary();
			this.DeathEffect();
			base.StopCoroutine(this.openHatch_cr());
			base.StopCoroutine(this.closeHatch_cr());
			base.enabled = false;
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
			foreach (GameObject gameObject in this.damagedHatches)
			{
				gameObject.SetActive(true);
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		base.OnPrimaryDeath();
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x00187274 File Offset: 0x00185674
	protected override void ExitCurrentAttacks()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.StopCoroutine(this.openHatch_cr());
			base.StartCoroutine(this.closeHatch_cr());
		}
		if (this.current == RobotLevelRobotBodyPart.state.secondary)
		{
			base.StopCoroutine(this.secondaryAttack_cr());
		}
		base.ExitCurrentAttacks();
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x001872C3 File Offset: 0x001856C3
	public void InitAnims()
	{
		base.animator.SetTrigger("OnRobotIntro");
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x001872D8 File Offset: 0x001856D8
	protected override void Die()
	{
		if (this.damageEffectRoutine != null)
		{
			base.StopCoroutine(this.damageEffectRoutine);
		}
		this.damageEffect.SetActive(false);
		foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
		base.Die();
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x00187339 File Offset: 0x00185739
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.OnDamageTaken(info);
		if (this.damageEffectRoutine != null)
		{
			base.StopCoroutine(this.damageEffectRoutine);
		}
		this.damageEffectRoutine = this.damageEffect_cr();
		base.StartCoroutine(this.damageEffectRoutine);
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x00187374 File Offset: 0x00185774
	private IEnumerator damageEffect_cr()
	{
		for (int i = 0; i < 3; i++)
		{
			this.damageEffectRenderer.enabled = true;
			this.damageEffect.SetActive(true);
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
			this.damageEffect.SetActive(false);
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		}
		yield break;
	}

	// Token: 0x040032C3 RID: 12995
	private float shotbotSpawnDelay;

	// Token: 0x040032C4 RID: 12996
	private int nearestEventFrame;

	// Token: 0x040032C5 RID: 12997
	[SerializeField]
	private GameObject[] damagedHatches;

	// Token: 0x040032C6 RID: 12998
	[SerializeField]
	private GameObject damageEffect;

	// Token: 0x040032C7 RID: 12999
	private IEnumerator damageEffectRoutine;

	// Token: 0x040032C8 RID: 13000
	private SpriteRenderer damageEffectRenderer;
}
