using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200074D RID: 1869
public abstract class RetroArcadeEnemy : AbstractProjectile
{
	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060028BB RID: 10427 RVA: 0x00176BD5 File Offset: 0x00174FD5
	// (set) Token: 0x060028BC RID: 10428 RVA: 0x00176BDD File Offset: 0x00174FDD
	public bool IsDead { get; protected set; }

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x060028BD RID: 10429 RVA: 0x00176BE6 File Offset: 0x00174FE6
	// (set) Token: 0x060028BE RID: 10430 RVA: 0x00176BEE File Offset: 0x00174FEE
	public float PointsWorth { get; protected set; }

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060028BF RID: 10431 RVA: 0x00176BF7 File Offset: 0x00174FF7
	// (set) Token: 0x060028C0 RID: 10432 RVA: 0x00176BFF File Offset: 0x00174FFF
	public float PointsBonus { get; protected set; }

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x060028C1 RID: 10433 RVA: 0x00176C08 File Offset: 0x00175008
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x00176C0F File Offset: 0x0017500F
	protected override void Awake()
	{
		base.Awake();
		if (base.GetComponent<DamageReceiver>() != null)
		{
			base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		}
		this.IsDead = false;
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x00176C47 File Offset: 0x00175047
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x00176C65 File Offset: 0x00175065
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x00176C84 File Offset: 0x00175084
	protected virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		RetroArcadeLevel.TOTAL_POINTS += this.PointsWorth;
		this.hp -= info.damage;
		if (this.hp < 0f && !this.IsDead)
		{
			this.Dead();
		}
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x00176CD6 File Offset: 0x001750D6
	public void MoveY(float moveAmount, float moveSpeed)
	{
		if (this.moveCoroutine != null)
		{
			base.StopCoroutine(this.moveCoroutine);
		}
		this.movingY = true;
		this.moveCoroutine = base.StartCoroutine(this.moveY_cr(moveAmount, Mathf.Abs(moveAmount) / moveSpeed));
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x00176D14 File Offset: 0x00175114
	private IEnumerator moveY_cr(float moveAmount, float time)
	{
		float t = 0f;
		float startY = base.transform.position.y;
		float endY = startY + moveAmount;
		while (t < time)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startY, endY, t / time)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(endY), null);
		this.movingY = false;
		yield break;
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x00176D40 File Offset: 0x00175140
	public virtual void Dead()
	{
		if (this.type != RetroArcadeEnemy.Type.IsBoss)
		{
			this.CheckForColorBonus();
		}
		Collider2D component = base.GetComponent<Collider2D>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.IsDead = true;
		base.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.25f);
	}

	// Token: 0x060028C9 RID: 10441 RVA: 0x00176DA4 File Offset: 0x001751A4
	private void CheckForColorBonus()
	{
		if (this.type == RetroArcadeEnemy.LAST_TYPE)
		{
			RetroArcadeEnemy.COUNTER++;
			if (RetroArcadeEnemy.COUNTER >= 3)
			{
				this.GiveBonus();
				RetroArcadeEnemy.COUNTER = 0;
				RetroArcadeEnemy.LAST_TYPE = RetroArcadeEnemy.Type.None;
			}
			else
			{
				RetroArcadeEnemy.LAST_TYPE = this.type;
			}
		}
		else
		{
			RetroArcadeEnemy.COUNTER = 1;
			RetroArcadeEnemy.LAST_TYPE = this.type;
		}
	}

	// Token: 0x060028CA RID: 10442 RVA: 0x00176E10 File Offset: 0x00175210
	protected virtual void GiveBonus()
	{
		RetroArcadeLevel.TOTAL_POINTS += this.PointsBonus;
	}

	// Token: 0x04003193 RID: 12691
	[SerializeField]
	public RetroArcadeEnemy.Type type;

	// Token: 0x04003194 RID: 12692
	private static int COUNTER;

	// Token: 0x04003195 RID: 12693
	private static RetroArcadeEnemy.Type LAST_TYPE;

	// Token: 0x04003196 RID: 12694
	private Coroutine moveCoroutine;

	// Token: 0x0400319A RID: 12698
	private float pointsBonusAccuracy;

	// Token: 0x0400319B RID: 12699
	private bool inComboChain;

	// Token: 0x0400319C RID: 12700
	protected float hp;

	// Token: 0x0400319D RID: 12701
	protected bool movingY;

	// Token: 0x0200074E RID: 1870
	public enum Type
	{
		// Token: 0x0400319F RID: 12703
		A,
		// Token: 0x040031A0 RID: 12704
		B,
		// Token: 0x040031A1 RID: 12705
		C,
		// Token: 0x040031A2 RID: 12706
		None,
		// Token: 0x040031A3 RID: 12707
		IsBoss
	}
}
