using System;
using UnityEngine;

// Token: 0x0200077D RID: 1917
public class RobotLevelHatchBombBot : HomingProjectile
{
	// Token: 0x06002A0C RID: 10764 RVA: 0x001894F3 File Offset: 0x001878F3
	public void InitBombBot(LevelProperties.Robot.BombBot properties)
	{
		this.properties = properties;
		this.health = (float)properties.bombHP;
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x00189509 File Offset: 0x00187909
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.Die();
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x00189519 File Offset: 0x00187919
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (hit.GetComponent<RobotLevelRobotBodyPart>() != null)
		{
			this.Die();
		}
		else if (hit.GetComponent<RobotLevelHatchBombBot>() != null)
		{
			this.Die();
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x00189556 File Offset: 0x00187956
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f && !this.isDead)
		{
			this.Die();
		}
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x0018958C File Offset: 0x0018798C
	protected override void Awake()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x001895B7 File Offset: 0x001879B7
	protected override void Start()
	{
		base.Start();
		this.damageDealer.SetDamage((float)this.properties.bombBossDamage);
		this.damageDealer.SetRate(0f);
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x001895E6 File Offset: 0x001879E6
	protected override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x001895F9 File Offset: 0x001879F9
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x00189604 File Offset: 0x00187A04
	protected override void Die()
	{
		base.Die();
		this.isDead = true;
		this.StopAllCoroutines();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.animator.Play("Explode");
		AudioManager.Play("robot_bombbot_death");
		this.emitAudioFromObject.Add("robot_bombbot_death");
	}

	// Token: 0x040032EF RID: 13039
	[SerializeField]
	private Sprite explosion;

	// Token: 0x040032F0 RID: 13040
	private bool isDead;

	// Token: 0x040032F1 RID: 13041
	private float health;

	// Token: 0x040032F2 RID: 13042
	private DamageReceiver damageReceiver;

	// Token: 0x040032F3 RID: 13043
	private LevelProperties.Robot.BombBot properties;
}
