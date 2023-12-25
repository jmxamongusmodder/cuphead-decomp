using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000522 RID: 1314
public class BeeLevelSecurityGuard : LevelProperties.Bee.Entity
{
	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06001793 RID: 6035 RVA: 0x000D47EC File Offset: 0x000D2BEC
	// (set) Token: 0x06001794 RID: 6036 RVA: 0x000D47F4 File Offset: 0x000D2BF4
	public BeeLevelSecurityGuard.State state { get; private set; }

	// Token: 0x06001795 RID: 6037 RVA: 0x000D4800 File Offset: 0x000D2C00
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnTakeDamage;
		this.damageDealer = DamageDealer.NewEnemy();
		this.circleCollider = base.GetComponent<CircleCollider2D>();
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x000D484D File Offset: 0x000D2C4D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x000D4865 File Offset: 0x000D2C65
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x000D488E File Offset: 0x000D2C8E
	private void OnTakeDamage(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x000D48A4 File Offset: 0x000D2CA4
	public void StartSecurityGuard()
	{
		this.ResetGuard();
		this.p = base.properties.CurrentState.securityGuard;
		base.properties.OnStateChange += this.OnStateChange;
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x000D48F1 File Offset: 0x000D2CF1
	private void OnStateChange()
	{
		base.properties.OnStateChange -= this.OnStateChange;
		this.Die();
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000D4910 File Offset: 0x000D2D10
	private void Die()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x000D4925 File Offset: 0x000D2D25
	private void ResetGuard()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x000D492D File Offset: 0x000D2D2D
	private void SfxThrow()
	{
		AudioManager.Play("bee_guard_attack");
		this.emitAudioFromObject.Add("bee_guard_attack");
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000D494C File Offset: 0x000D2D4C
	private void Attack()
	{
		this.bombPrefab.Create(this.bombRoot.position, -(int)base.transform.localScale.x, this.p.idleTime, this.p.warningTime, this.p.childSpeed, this.p.childCount);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x000D49B6 File Offset: 0x000D2DB6
	private void AttackComplete()
	{
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x000D49B8 File Offset: 0x000D2DB8
	private void FlipX()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x000D49FD File Offset: 0x000D2DFD
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x000D4A20 File Offset: 0x000D2E20
	private IEnumerator go_cr()
	{
		AudioManager.Play("bee_guard_spawn");
		this.emitAudioFromObject.Add("bee_guard_spawn");
		AudioManager.PlayLoop("bee_guard_flying_loop");
		this.emitAudioFromObject.Add("bee_guard_flying_loop");
		for (;;)
		{
			yield return base.StartCoroutine(this.move_cr());
			yield return base.StartCoroutine(this.attack_cr());
		}
		yield break;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000D4A3C File Offset: 0x000D2E3C
	private IEnumerator move_cr()
	{
		this.state = BeeLevelSecurityGuard.State.Move;
		float t = 0f;
		float time = this.p.attackDelay.RandomFloat();
		while (t < time)
		{
			base.transform.AddPositionForward2D(-this.p.speed * CupheadTime.Delta * base.transform.localScale.x * this.hitPauseCoefficient());
			if ((base.transform.localScale.x > 0f && base.transform.position.x <= -490f) || (base.transform.localScale.x < 0f && base.transform.position.x >= 490f))
			{
				yield return base.StartCoroutine(this.turn_cr());
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000D4A58 File Offset: 0x000D2E58
	private IEnumerator attack_cr()
	{
		this.state = BeeLevelSecurityGuard.State.Attack;
		base.animator.SetTrigger("OnAttack");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
		yield break;
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x000D4A74 File Offset: 0x000D2E74
	private IEnumerator leave_cr()
	{
		LevelBossDeathExploder exploder = base.GetComponent<LevelBossDeathExploder>();
		this.state = BeeLevelSecurityGuard.State.Leaving;
		exploder.StartExplosion();
		if (base.transform.localScale.x < 0f && base.transform.position.x < 0f)
		{
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
		}
		if (base.transform.localScale.x > 0f && base.transform.position.x > 0f)
		{
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		}
		base.animator.Play("Leave");
		AudioManager.Stop("bee_guard_flying_loop");
		AudioManager.Play("bee_guard_leave");
		this.emitAudioFromObject.Add("bee_guard_leave");
		this.circleCollider.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		exploder.StopExplosions();
		AudioManager.Play("bee_guard_death");
		this.emitAudioFromObject.Add("bee_guard_death");
		bool leave = true;
		while (leave)
		{
			base.transform.AddPositionForward2D(-this.p.speed * CupheadTime.Delta * base.transform.localScale.x * this.hitPauseCoefficient());
			yield return null;
			if (base.transform.position.x > 1280f || base.transform.position.x < -1280f)
			{
				leave = false;
			}
		}
		this.state = BeeLevelSecurityGuard.State.Ready;
		yield break;
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x000D4A90 File Offset: 0x000D2E90
	private IEnumerator turn_cr()
	{
		base.animator.Play("Turn");
		yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		yield break;
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x000D4AAB File Offset: 0x000D2EAB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bombPrefab = null;
	}

	// Token: 0x040020CC RID: 8396
	[SerializeField]
	private Transform bombRoot;

	// Token: 0x040020CD RID: 8397
	[SerializeField]
	private BeeLevelSecurityGuardBomb bombPrefab;

	// Token: 0x040020CE RID: 8398
	private LevelProperties.Bee.SecurityGuard p;

	// Token: 0x040020CF RID: 8399
	private DamageReceiver damageReceiver;

	// Token: 0x040020D0 RID: 8400
	private DamageDealer damageDealer;

	// Token: 0x040020D1 RID: 8401
	private CircleCollider2D circleCollider;

	// Token: 0x02000523 RID: 1315
	public enum State
	{
		// Token: 0x040020D3 RID: 8403
		Ready,
		// Token: 0x040020D4 RID: 8404
		Move,
		// Token: 0x040020D5 RID: 8405
		Attack,
		// Token: 0x040020D6 RID: 8406
		Leaving
	}
}
