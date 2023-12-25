using System;
using UnityEngine;

// Token: 0x02000781 RID: 1921
public class RobotLevelSecondaryArms : AbstractCollidableObject
{
	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06002A37 RID: 10807 RVA: 0x0018B387 File Offset: 0x00189787
	// (set) Token: 0x06002A38 RID: 10808 RVA: 0x0018B38F File Offset: 0x0018978F
	public bool BossAlive { private get; set; }

	// Token: 0x06002A39 RID: 10809 RVA: 0x0018B398 File Offset: 0x00189798
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.BossAlive = true;
		base.Awake();
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x0018B3B2 File Offset: 0x001897B2
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x0018B3CA File Offset: 0x001897CA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x0018B3E8 File Offset: 0x001897E8
	public void InitHelper(LevelProperties.Robot properties)
	{
		this.spawnPoint = base.transform.GetChild(2).transform;
		this.ShootTwicePerCycle = properties.CurrentState.twistyArms.shootTwicePerCycle;
		this.bulletSpeed = properties.CurrentState.twistyArms.bulletSpeed;
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x0018B438 File Offset: 0x00189838
	private void OnTwistyArmsShoot()
	{
		if (this.twistyArmsProjectile != null)
		{
			AudioManager.Play("robot_arms_hand_shoot");
			this.emitAudioFromObject.Add("robot_arms_hand_shoot");
			this.twistyArmsProjectile.Create(this.spawnPoint.position + Vector3.up * 100f, 90f, this.bulletSpeed);
			this.twistyArmsProjectile.Create(this.spawnPoint.position + Vector3.down * 100f, -90f, this.bulletSpeed);
		}
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x0018B4E8 File Offset: 0x001898E8
	private void SwapAnimations()
	{
		if (this.BossAlive)
		{
			float num = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
			if (this.ShootTwicePerCycle && num < 0.5f)
			{
				base.animator.Play("Shoot B");
			}
			else if (num > 0.5f)
			{
				base.animator.Play("Shoot A");
			}
		}
	}

	// Token: 0x04003311 RID: 13073
	private bool ShootTwicePerCycle;

	// Token: 0x04003312 RID: 13074
	private float bulletSpeed;

	// Token: 0x04003313 RID: 13075
	private Transform spawnPoint;

	// Token: 0x04003314 RID: 13076
	private DamageDealer damageDealer;

	// Token: 0x04003316 RID: 13078
	[SerializeField]
	private BasicProjectile twistyArmsProjectile;
}
