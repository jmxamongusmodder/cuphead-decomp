using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class AirplaneLevelSecretTerrier : LevelProperties.Airplane.Entity
{
	// Token: 0x0600146F RID: 5231 RVA: 0x000B7550 File Offset: 0x000B5950
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.MoveToHolePosition();
		base.animator.Play("Intro_" + this.introNum[this.currentHole]);
		base.animator.Update(0f);
		this.firstAttack = true;
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x000B75CF File Offset: 0x000B59CF
	protected override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x000B75F4 File Offset: 0x000B59F4
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.hp = properties.CurrentState.secretTerriers.dogRetreatDamage;
		this.level.OccupyHole(this.currentHole);
		base.transform.localScale = new Vector3(-Mathf.Sign(this.level.GetHolePosition(this.currentHole, false).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetHolePosition(this.currentHole, false);
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x000B7699 File Offset: 0x000B5A99
	private void AniEvent_StartTerriers()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x000B76A8 File Offset: 0x000B5AA8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.hp > 0f)
		{
			this.hp -= info.damage;
			if (this.hp <= 0f)
			{
				Level.Current.RegisterMinionKilled();
				this.StopAllCoroutines();
				this.coll.enabled = false;
				base.StartCoroutine(this.timeout_cr());
			}
		}
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x000B7711 File Offset: 0x000B5B11
	public int CurrentHole()
	{
		return this.currentHole;
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x000B7719 File Offset: 0x000B5B19
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000B7730 File Offset: 0x000B5B30
	protected void MoveToHolePosition()
	{
		this.rend.sortingOrder = this.currentHole % 3 + 50;
		this.backerRend.sortingOrder = this.currentHole % 3 + 13;
		base.transform.localScale = new Vector3(-Mathf.Sign(this.level.GetHolePosition(this.currentHole, false).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetHolePosition(this.currentHole, false);
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x000B77D4 File Offset: 0x000B5BD4
	public void Die(int index)
	{
		this.StopAllCoroutines();
		while (this.currentHole == -1)
		{
			this.currentHole = this.level.GetNextHole();
			this.MoveToHolePosition();
		}
		string stateName = "Death_" + (index + 1).ToString();
		base.animator.Play(stateName);
		if (index + 1 == 1 || index + 1 == 4)
		{
			base.animator.Play("Stars", 1);
		}
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x000B785A File Offset: 0x000B5C5A
	private void HideAnimationComplete()
	{
		this.moved = true;
		this.coll.enabled = false;
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x000B7870 File Offset: 0x000B5C70
	private void AttackAnimationComplete()
	{
		LevelProperties.Airplane.SecretTerriers secretTerriers = base.properties.CurrentState.secretTerriers;
		if (this.nextAttackPink)
		{
			this.bulletPrefabPink.Create(this.bulletRoot.position, PlayerManager.GetNext().transform.position, secretTerriers, base.transform.localScale);
		}
		else
		{
			this.bulletPrefab.Create(this.bulletRoot.position, PlayerManager.GetNext().transform.position, secretTerriers, base.transform.localScale);
		}
		this.attacked = true;
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplethrow");
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000B7914 File Offset: 0x000B5D14
	public void TryStartAttack()
	{
		this.nextAttackPink = this.leader.TerrierProjectileParryable();
		if (this.canAttack)
		{
			base.animator.SetTrigger((!this.nextAttackPink) ? "Attack" : "AttackPink");
		}
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x000B7964 File Offset: 0x000B5D64
	private IEnumerator attack_cr()
	{
		this.level.OccupyHole(this.currentHole);
		for (;;)
		{
			this.MoveToHolePosition();
			if (!this.firstAttack)
			{
				base.animator.Play("Emerge");
			}
			this.firstAttack = false;
			this.canAttack = true;
			this.coll.enabled = true;
			this.attacked = false;
			this.moved = false;
			while (!this.attacked)
			{
				yield return null;
			}
			this.canAttack = false;
			this.attacked = false;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretTerriers.dogPostAttackDelay);
			base.animator.SetTrigger("OnMove");
			while (!this.moved)
			{
				yield return null;
			}
			this.moved = false;
			int previousHole = this.currentHole;
			this.currentHole = -1;
			while (this.currentHole == -1)
			{
				this.currentHole = this.level.GetNextHole();
			}
			this.level.LeaveHole(previousHole);
		}
		yield break;
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x000B7980 File Offset: 0x000B5D80
	private IEnumerator timeout_cr()
	{
		base.animator.ResetTrigger("Attack");
		base.animator.ResetTrigger("OnMove");
		base.animator.Play("Move");
		this.canAttack = false;
		this.level.LeaveHole(this.currentHole);
		this.currentHole = -1;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretTerriers.dogTimeOut);
		this.hp = base.properties.CurrentState.secretTerriers.dogRetreatDamage;
		while (this.currentHole == -1)
		{
			this.currentHole = this.level.GetNextHole();
			yield return null;
		}
		base.StartCoroutine(this.attack_cr());
		yield break;
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x000B799B File Offset: 0x000B5D9B
	private void AniEvent_PullGrenadePin()
	{
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplepinclink");
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x000B79A8 File Offset: 0x000B5DA8
	private void WORKAROUND_NullifyFields()
	{
		this.damageDealer = null;
		this.bulletRoot = null;
		this.bulletPrefab = null;
		this.bulletPrefabPink = null;
		this.level = null;
		this.introNum = null;
		this.coll = null;
		this.leader = null;
		this.rend = null;
		this.backerRend = null;
	}

	// Token: 0x04001DBE RID: 7614
	private bool isDead;

	// Token: 0x04001DBF RID: 7615
	private DamageDealer damageDealer;

	// Token: 0x04001DC0 RID: 7616
	private DamageReceiver damageReceiver;

	// Token: 0x04001DC1 RID: 7617
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x04001DC2 RID: 7618
	[SerializeField]
	private AirplaneLevelSecretTerrierBullet bulletPrefab;

	// Token: 0x04001DC3 RID: 7619
	[SerializeField]
	private AirplaneLevelSecretTerrierBullet bulletPrefabPink;

	// Token: 0x04001DC4 RID: 7620
	[SerializeField]
	private AirplaneLevel level;

	// Token: 0x04001DC5 RID: 7621
	private bool attacked;

	// Token: 0x04001DC6 RID: 7622
	private bool moved;

	// Token: 0x04001DC7 RID: 7623
	private bool canAttack;

	// Token: 0x04001DC8 RID: 7624
	private float hp;

	// Token: 0x04001DC9 RID: 7625
	[SerializeField]
	private int currentHole;

	// Token: 0x04001DCA RID: 7626
	private int[] introNum = new int[]
	{
		1,
		3,
		2,
		0,
		4
	};

	// Token: 0x04001DCB RID: 7627
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04001DCC RID: 7628
	[SerializeField]
	private AirplaneLevelSecretLeader leader;

	// Token: 0x04001DCD RID: 7629
	private bool firstAttack;

	// Token: 0x04001DCE RID: 7630
	private bool nextAttackPink;

	// Token: 0x04001DCF RID: 7631
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04001DD0 RID: 7632
	[SerializeField]
	private SpriteRenderer backerRend;
}
