using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public class AirplaneLevelSecretLeader : LevelProperties.Airplane.Entity
{
	// Token: 0x0600145F RID: 5215 RVA: 0x000B69B2 File Offset: 0x000B4DB2
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.effectSide = Rand.Bool();
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000B69ED File Offset: 0x000B4DED
	protected override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x000B6A14 File Offset: 0x000B4E14
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.rocketPositionString = new PatternString(properties.CurrentState.secretLeader.rocketHomingSpawnLocation, true, true);
		this.terrierProjectileParryableString = new PatternString(properties.CurrentState.secretTerriers.dogBulletParryString, true);
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x000B6A6E File Offset: 0x000B4E6E
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && !this.isDead)
		{
			this.Die();
		}
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x000B6AA7 File Offset: 0x000B4EA7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x000B6ABD File Offset: 0x000B4EBD
	public bool TerrierProjectileParryable()
	{
		return this.terrierProjectileParryableString.PopLetter() == 'P';
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x000B6ACE File Offset: 0x000B4ECE
	public void DieMain()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_main_cr());
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x000B6AE4 File Offset: 0x000B4EE4
	private IEnumerator die_main_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.hiding = true;
		this.currentHole = 3;
		base.animator.Play("Death");
		this.isDead = true;
		base.transform.localScale = new Vector3(Mathf.Sign(this.level.GetHolePosition(this.currentHole, true).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetLeaderDeathPosition(this.currentHole);
		yield return base.animator.WaitForAnimationToStart(this, "DeathLoop", false);
		base.animator.Play("Tears", 1);
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_death");
		yield break;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x000B6B00 File Offset: 0x000B4F00
	private void Die()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		for (int i = 0; i < this.terriers.Length; i++)
		{
			this.terriers[i].Die(i);
		}
		this.level.leader.animator.Play("Off");
		this.level.leader.animator.Play("Copter_Death", this.level.leader.animator.GetLayerIndex("Death"));
		this.level.leader.animator.Play("Blades", base.animator.GetLayerIndex("DeathBlades"));
		base.animator.Play("DeathLoop");
		base.animator.Play("Tears", 1);
		base.transform.localScale = new Vector3(Mathf.Sign(this.level.GetHolePosition(this.currentHole, true).x - Camera.main.transform.position.x), 1f);
		base.transform.position = this.level.GetLeaderDeathPosition(this.currentHole);
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x000B6C42 File Offset: 0x000B5042
	private void HideAnimationComplete()
	{
		this.moved = true;
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x000B6C4C File Offset: 0x000B504C
	private void AttackAnimationStart()
	{
		LevelProperties.Airplane.SecretLeader secretLeader = base.properties.CurrentState.secretLeader;
		Vector3 b = new Vector3((float)((!this.effectSide) ? 120 : -120), 120f);
		this.rocketBGPrefab.Create(Camera.main.transform.position + b, MathUtils.DirectionToAngle(Vector3.up) + UnityEngine.Random.Range(5f, 12f) * (float)((!this.effectSide) ? -1 : 1), new Vector3(2f, 2f), 600f);
		this.rocketBGEffect.Create(Camera.main.transform.position + b);
		this.effectSide = !this.effectSide;
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x000B6D30 File Offset: 0x000B5130
	private void AttackAnimationComplete()
	{
		LevelProperties.Airplane.SecretLeader secretLeader = base.properties.CurrentState.secretLeader;
		this.rocketPrefab.Create(PlayerManager.GetNext(), Camera.main.transform.position + Vector3.up * 800f + this.rocketPositionString.PopFloat() * Vector3.right, secretLeader.rocketHomingSpeed, secretLeader.rocketHomingRotation, secretLeader.rocketHomingHP, secretLeader.rocketHomingTime);
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x000B6DBC File Offset: 0x000B51BC
	private IEnumerator attack_cr()
	{
		this.level.OccupyHole(this.currentHole);
		for (;;)
		{
			base.transform.localScale = new Vector3(Mathf.Sign(this.level.GetHolePosition(this.currentHole, true).x - Camera.main.transform.position.x), 1f);
			base.transform.position = this.level.GetHolePosition(this.currentHole, true);
			this.rend.sortingOrder = this.currentHole % 3 + 50;
			this.backerRend.sortingOrder = this.currentHole % 3 + 13;
			bool lookingStraight = this.currentHole == 2 || this.currentHole == 5;
			base.animator.SetBool("EyesDown", !lookingStraight);
			this.hiding = false;
			if (!this.first)
			{
				base.animator.Play("Emerge");
			}
			this.first = false;
			this.boxCollider.enabled = true;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.leaderPreAttackDelay);
			base.animator.Play("AttackStart");
			yield return base.animator.WaitForAnimationToStart(this, "AttackPreHold", false);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.attackAnticipationHold);
			base.animator.SetTrigger("ContinueAttack");
			yield return base.animator.WaitForAnimationToStart(this, "AttackPostHold", false);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.attackRecoveryHold);
			base.animator.SetTrigger("ContinueAttack");
			yield return base.animator.WaitForAnimationToEnd(this, (!base.animator.GetBool("EyesDown")) ? "AttackEnd" : "AttackEndEyesDown", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.leaderPostAttackDelay);
			base.animator.Play((this.currentHole % 3 != 3) ? "Exit" : "ExitLow");
			while (!this.moved)
			{
				yield return null;
			}
			this.boxCollider.enabled = false;
			this.hiding = true;
			this.moved = false;
			int previousHole = this.currentHole;
			this.currentHole = -1;
			while (this.currentHole == -1)
			{
				this.currentHole = this.level.GetNextHole();
			}
			this.level.LeaveHole(previousHole);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.secretLeader.hideTime);
		}
		yield break;
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x000B6DD7 File Offset: 0x000B51D7
	private void AnimationEvent_SFX_DOGFIGHT_PS_LeaderAttack()
	{
		AudioManager.Play("sfx_dlc_dogfight_ps_leader_batonattack");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_ps_leader_batonattack");
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_command");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_leadervocal_command");
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000B6E10 File Offset: 0x000B5210
	private void WORKAROUND_NullifyFields()
	{
		this.damageDealer = null;
		this.rocketBGPrefab = null;
		this.rocketPrefab = null;
		this.rocketBGEffect = null;
		this.level = null;
		this.terriers = null;
		this.rocketPositionString = null;
		this.terrierProjectileParryableString = null;
		this.boxCollider = null;
		this.rend = null;
		this.backerRend = null;
	}

	// Token: 0x04001DAB RID: 7595
	private bool isDead;

	// Token: 0x04001DAC RID: 7596
	private DamageDealer damageDealer;

	// Token: 0x04001DAD RID: 7597
	private DamageReceiver damageReceiver;

	// Token: 0x04001DAE RID: 7598
	[SerializeField]
	private BasicProjectile rocketBGPrefab;

	// Token: 0x04001DAF RID: 7599
	[SerializeField]
	private AirplaneLevelRocket rocketPrefab;

	// Token: 0x04001DB0 RID: 7600
	[SerializeField]
	private Effect rocketBGEffect;

	// Token: 0x04001DB1 RID: 7601
	[SerializeField]
	private AirplaneLevel level;

	// Token: 0x04001DB2 RID: 7602
	[SerializeField]
	private AirplaneLevelSecretTerrier[] terriers;

	// Token: 0x04001DB3 RID: 7603
	private PatternString rocketPositionString;

	// Token: 0x04001DB4 RID: 7604
	private PatternString terrierProjectileParryableString;

	// Token: 0x04001DB5 RID: 7605
	private bool attacked;

	// Token: 0x04001DB6 RID: 7606
	private bool moved;

	// Token: 0x04001DB7 RID: 7607
	private bool hiding;

	// Token: 0x04001DB8 RID: 7608
	private bool first = true;

	// Token: 0x04001DB9 RID: 7609
	[SerializeField]
	private int currentHole;

	// Token: 0x04001DBA RID: 7610
	[SerializeField]
	private BoxCollider2D boxCollider;

	// Token: 0x04001DBB RID: 7611
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04001DBC RID: 7612
	[SerializeField]
	private SpriteRenderer backerRend;

	// Token: 0x04001DBD RID: 7613
	private bool effectSide;
}
