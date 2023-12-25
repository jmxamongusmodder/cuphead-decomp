using System;
using UnityEngine;

// Token: 0x02000807 RID: 2055
public class AbstractTrainLevelSkeletonPart : AbstractCollidableObject
{
	// Token: 0x06002F90 RID: 12176 RVA: 0x001C3A03 File Offset: 0x001C1E03
	protected override void Awake()
	{
		base.Awake();
		this.exploder = base.GetComponent<LevelBossDeathExploder>();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x001C3A24 File Offset: 0x001C1E24
	public void SetPosition(TrainLevelSkeleton.Position position)
	{
		switch (position)
		{
		case TrainLevelSkeleton.Position.Right:
			base.transform.SetPosition(new float?(470f), null, null);
			break;
		default:
			base.transform.SetPosition(new float?(0f), null, null);
			break;
		case TrainLevelSkeleton.Position.Left:
			base.transform.SetPosition(new float?(-470f), null, null);
			break;
		}
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x001C3ACC File Offset: 0x001C1ECC
	public void In()
	{
		base.animator.Play("In");
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x001C3ADE File Offset: 0x001C1EDE
	public void Out()
	{
		base.animator.SetTrigger("Out");
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x001C3AF0 File Offset: 0x001C1EF0
	public void Die()
	{
		this.exploder.StartExplosion();
		base.animator.Play("Death");
	}

	// Token: 0x06002F95 RID: 12181 RVA: 0x001C3B0D File Offset: 0x001C1F0D
	public void EndDeath()
	{
		this.exploder.StopExplosions();
	}

	// Token: 0x06002F96 RID: 12182 RVA: 0x001C3B1A File Offset: 0x001C1F1A
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002F97 RID: 12183 RVA: 0x001C3B32 File Offset: 0x001C1F32
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0400387E RID: 14462
	public const float X = 470f;

	// Token: 0x0400387F RID: 14463
	private LevelBossDeathExploder exploder;

	// Token: 0x04003880 RID: 14464
	private DamageDealer damageDealer;
}
