using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005BF RID: 1471
public class DicePalaceEightBallLevelEightBall : LevelProperties.DicePalaceEightBall.Entity
{
	// Token: 0x06001C97 RID: 7319 RVA: 0x001051FC File Offset: 0x001035FC
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		List<int> list = new List<int>(this.balls.Count);
		this.newList = new List<int>();
		for (int i = 0; i < this.balls.Count; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < this.balls.Count; j++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			this.newList.Add(list[index]);
			list.RemoveAt(index);
		}
		this.ballIndex = 0;
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x001052BA File Offset: 0x001036BA
	public override void LevelInit(LevelProperties.DicePalaceEightBall properties)
	{
		base.LevelInit(properties);
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x001052E6 File Offset: 0x001036E6
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x001052FC File Offset: 0x001036FC
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		AudioManager.Play("dice_palace_eight_ball_intro");
		this.emitAudioFromObject.Add("dice_palace_eight_ball_intro");
		yield return base.animator.WaitForAnimationToStart(this, "Right_Idle", false);
		base.StartCoroutine(this.shoot_bullet_cr());
		base.StartCoroutine(this.spawn_balls_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x00105318 File Offset: 0x00103718
	private void LoopCounter()
	{
		if (this.currentLoops < base.properties.CurrentState.general.idleLoopAmount)
		{
			this.currentLoops++;
		}
		else
		{
			base.animator.SetTrigger("Continue");
			this.currentLoops = 0;
		}
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x0010536F File Offset: 0x0010376F
	private void HitLeftIdle()
	{
		base.animator.SetBool("MovingLeft", false);
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x00105382 File Offset: 0x00103782
	private void HitRightIdle()
	{
		base.animator.SetBool("MovingLeft", true);
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x00105395 File Offset: 0x00103795
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.attackEffect = null;
		this.projectileEffect = null;
		this.projectile = null;
		this.pinkProjectile = null;
		this.balls = null;
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x001053C0 File Offset: 0x001037C0
	private IEnumerator shoot_bullet_cr()
	{
		LevelProperties.DicePalaceEightBall.General p = base.properties.CurrentState.general;
		string[] projectileType = p.shootString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int projectileIndex = UnityEngine.Random.Range(0, projectileType.Length);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shootDelay);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack_Start", false);
			AudioManager.Play("dice_palace_eight_ball_attack_start");
			this.emitAudioFromObject.Add("dice_palace_eight_ball_attack_start");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			Effect effect = UnityEngine.Object.Instantiate<Effect>(this.projectileEffect);
			effect.transform.position = this.root.transform.position;
			yield return effect.GetComponent<Animator>().WaitForAnimationToEnd(this, "Projectile", false, true);
			AbstractPlayerController player = PlayerManager.GetNext();
			Vector3 dir = player.transform.position - base.transform.position;
			AudioManager.Play("dice_palace_eight_ball_eight_attack_fire");
			this.emitAudioFromObject.Add("dice_palace_eight_ball_eight_attack_fire");
			if (projectileType[projectileIndex][0] == 'R')
			{
				this.attackEffect.Create(this.root.transform.position);
				this.projectile.Create(this.root.transform.position, MathUtils.DirectionToAngle(dir), base.properties.CurrentState.general.shootSpeed);
			}
			else if (projectileType[projectileIndex][0] == 'P')
			{
				this.attackEffect.Create(this.root.transform.position);
				this.pinkProjectile.Create(this.root.transform.position, MathUtils.DirectionToAngle(dir), base.properties.CurrentState.general.shootSpeed);
			}
			projectileIndex = (projectileIndex + 1) % projectileType.Length;
			yield return CupheadTime.WaitForSeconds(this, p.attackDuration);
			base.animator.SetTrigger("OnEnd");
			AudioManager.Play("dice_palace_eight_ball_attack_end");
			this.emitAudioFromObject.Add("dice_palace_eight_ball_attack_end");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x001053DB File Offset: 0x001037DB
	private void IntroSFX()
	{
		AudioManager.Play("dice_palace_eight_ball_eight_intro");
		this.emitAudioFromObject.Add("dice_palace_eight_ball_eight_intro");
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x001053F8 File Offset: 0x001037F8
	private IEnumerator spawn_balls_cr()
	{
		LevelProperties.DicePalaceEightBall.PoolBalls p = base.properties.CurrentState.poolBalls;
		string[] side = p.sideString.GetRandom<string>().Split(new char[]
		{
			','
		});
		float offset = base.GetComponent<Renderer>().bounds.size.x / 2f;
		int sideIndex = UnityEngine.Random.Range(0, side.Length);
		bool onLeft = false;
		Vector3 leftPos = new Vector3(-640f, 360f + offset, 0f);
		Vector3 rightPos = new Vector3(640f, 360f + offset, 0f);
		Vector3 pos = Vector3.zero;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			DicePalaceEightBallLevelPoolBall ballInstance = null;
			if (side[sideIndex][0] == 'L')
			{
				onLeft = true;
				pos = leftPos;
			}
			else if (side[sideIndex][0] == 'R')
			{
				onLeft = false;
				pos = rightPos;
			}
			else
			{
				global::Debug.LogError("sideString pattern is wrong", null);
			}
			int index = this.newList[this.ballIndex];
			while (index < 0 || index > this.balls.Count)
			{
				this.ballIndex = (this.ballIndex + 1) % this.balls.Count;
				index = this.newList[this.ballIndex];
				yield return null;
			}
			if (index == 0)
			{
				ballInstance = this.balls[index].Create(pos, p.oneJumpHorizontalSpeed, p.oneJumpVerticalSpeed, p.oneJumpGravity, p.oneGroundDelay, onLeft, this);
			}
			else if (index == 1)
			{
				ballInstance = this.balls[index].Create(pos, p.twoJumpHorizontalSpeed, p.twoJumpVerticalSpeed, p.twoJumpGravity, p.twoGroundDelay, onLeft, this);
			}
			else if (index == 2)
			{
				ballInstance = this.balls[index].Create(pos, p.threeJumpHorizontalSpeed, p.threeJumpVerticalSpeed, p.threeJumpGravity, p.threeGroundDelay, onLeft, this);
			}
			else if (index == 3)
			{
				ballInstance = this.balls[index].Create(pos, p.fourJumpHorizontalSpeed, p.fourJumpVerticalSpeed, p.fourJumpGravity, p.fourGroundDelay, onLeft, this);
			}
			else if (index == 4)
			{
				ballInstance = this.balls[index].Create(pos, p.fiveJumpHorizontalSpeed, p.fiveJumpVerticalSpeed, p.fiveJumpGravity, p.fiveGroundDelay, onLeft, this);
			}
			else
			{
				global::Debug.LogError("Invalid index", null);
			}
			if (ballInstance != null)
			{
				ballInstance.SetVariation(this.newList[this.ballIndex]);
			}
			this.ballIndex = (this.ballIndex + 1) % this.balls.Count;
			sideIndex = (sideIndex + 1) % side.Length;
		}
		yield break;
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x00105414 File Offset: 0x00103814
	private void OnDeath()
	{
		if (this.OnEightBallDeath != null)
		{
			this.OnEightBallDeath();
		}
		this.StopAllCoroutines();
		AudioManager.PlayLoop("dice_palace_eight_ball_attack_death_loop");
		this.emitAudioFromObject.Add("dice_palace_eight_ball_attack_death_loop");
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04002582 RID: 9602
	[SerializeField]
	private Effect attackEffect;

	// Token: 0x04002583 RID: 9603
	[SerializeField]
	private Effect projectileEffect;

	// Token: 0x04002584 RID: 9604
	[SerializeField]
	private Transform root;

	// Token: 0x04002585 RID: 9605
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04002586 RID: 9606
	[SerializeField]
	private BasicProjectile pinkProjectile;

	// Token: 0x04002587 RID: 9607
	[SerializeField]
	private List<DicePalaceEightBallLevelPoolBall> balls;

	// Token: 0x04002588 RID: 9608
	private List<int> newList;

	// Token: 0x04002589 RID: 9609
	private DamageReceiver damageReceiver;

	// Token: 0x0400258A RID: 9610
	private int currentLoops;

	// Token: 0x0400258B RID: 9611
	private int ballIndex;

	// Token: 0x0400258C RID: 9612
	public Action OnEightBallDeath;
}
