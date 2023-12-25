using System;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public class SnowCultLevelShard : BasicProjectileContinuesOnLevelEnd
{
	// Token: 0x06002EBE RID: 11966 RVA: 0x001B965C File Offset: 0x001B7A5C
	public virtual SnowCultLevelShard Init(Vector3 pivotPos, float angle, float loopSizeX, float loopSizeY, LevelProperties.SnowCult.ShardAttack properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.pivotPos = pivotPos;
		this.speed = properties.shardSpeed;
		this.Health = properties.shardHealth;
		base.GetComponent<Collider2D>().enabled = false;
		angle *= 0.017453292f;
		base.transform.position = pivotPos + new Vector3(-Mathf.Sin(angle) * loopSizeX, Mathf.Cos(angle) * loopSizeY);
		base.transform.SetEulerAngles(null, null, new float?(90f + MathUtils.DirectionToAngle(pivotPos - base.transform.position)));
		this.basePos = base.transform.position;
		this.SFX_SNOWCULT_JackFrostIceCreamProjSplatLoop();
		return this;
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x001B9733 File Offset: 0x001B7B33
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f)
		{
			this.Recycle<SnowCultLevelShard>();
		}
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x001B975E File Offset: 0x001B7B5E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x001B977C File Offset: 0x001B7B7C
	public void Appear()
	{
		base.animator.SetTrigger("Appear");
		this.SFX_SNOWCULT_JackFrostIcecreamAppear();
		this.smoke.Create(base.transform.position);
		base.GetComponent<Collider2D>().enabled = true;
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x001B97B7 File Offset: 0x001B7BB7
	public void LaunchProjectile()
	{
		base.animator.SetTrigger("StartMove");
		this.moving = true;
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x001B97D0 File Offset: 0x001B7BD0
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.moving)
		{
			base.transform.position = this.basePos + Vector3.right * Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleX + Vector3.up * Mathf.Cos(this.wobbleTimer * 3f) * this.wobbleY;
			this.wobbleTimer += CupheadTime.FixedDelta * this.wobbleSpeed;
		}
		else
		{
			base.transform.position += base.transform.up * -this.speed * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x001B98AA File Offset: 0x001B7CAA
	private void SFX_SNOWCULT_JackFrostIceCreamProjSplatLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop");
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x001B98C6 File Offset: 0x001B7CC6
	private void SFX_SNOWCULT_JackFrostIcecreamAppear()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop");
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_icecreamcone_appear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamcone_appear");
	}

	// Token: 0x04003766 RID: 14182
	private Vector3 basePos;

	// Token: 0x04003767 RID: 14183
	[SerializeField]
	private float wobbleX = 10f;

	// Token: 0x04003768 RID: 14184
	[SerializeField]
	private float wobbleY = 10f;

	// Token: 0x04003769 RID: 14185
	[SerializeField]
	private float wobbleSpeed = 2f;

	// Token: 0x0400376A RID: 14186
	private float wobbleTimer;

	// Token: 0x0400376B RID: 14187
	private float speed;

	// Token: 0x0400376C RID: 14188
	private float Health;

	// Token: 0x0400376D RID: 14189
	private bool moving;

	// Token: 0x0400376E RID: 14190
	private Vector2 pivotPos;

	// Token: 0x0400376F RID: 14191
	private DamageReceiver damageReceiver;

	// Token: 0x04003770 RID: 14192
	[SerializeField]
	private Effect smoke;
}
