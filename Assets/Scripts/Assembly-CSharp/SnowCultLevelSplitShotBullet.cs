using System;
using UnityEngine;

// Token: 0x020007F8 RID: 2040
public class SnowCultLevelSplitShotBullet : AbstractProjectile
{
	// Token: 0x06002EDF RID: 11999 RVA: 0x001BAA84 File Offset: 0x001B8E84
	public virtual SnowCultLevelSplitShotBullet Init(Vector3 pos, float rotation, float speed, int numOfBullets, float spreadAngle, LevelProperties.SnowCult.SplitShot properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.basePos = pos;
		this.rotation = rotation;
		this.speed = speed;
		this.moving = false;
		this.numOfBullets = numOfBullets;
		this.spreadAngle = spreadAngle;
		this.coll = base.GetComponent<Collider2D>();
		this.wobbleTimer = UnityEngine.Random.Range(0f, 6.2831855f);
		this.coll.enabled = false;
		this.spawnFX.Play("Spawn" + ((!base.CanParry) ? string.Empty : "Pink"));
		return this;
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x001BAB34 File Offset: 0x001B8F34
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.moving)
		{
			base.transform.position += MathUtils.AngleToDirection(this.rotation) * this.speed * CupheadTime.FixedDelta;
			if (this.coll.enabled && Mathf.Abs(base.transform.position.x) > (float)Level.Current.Right)
			{
				this.middleAngle = ((base.transform.position.x >= 0f) ? 180f : 0f);
				base.transform.localScale = new Vector3(-Mathf.Sign(base.transform.position.x), 1f);
				base.transform.position = new Vector3((float)(Level.Current.Left - 65) * -Mathf.Sign(base.transform.position.x), base.transform.position.y);
				base.animator.Play("BucketExplode");
				this.SFX_SNOWCULT_JackFrostSplitshotBucketImpact();
				this.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop();
				this.coll.enabled = false;
				this.SpawnProjectiles();
				this.speed = 0f;
			}
		}
		else
		{
			this.wobbleTimer += CupheadTime.FixedDelta * this.wobbleSpeed;
			base.transform.position = this.basePos + new Vector3(Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX, Mathf.Cos(this.wobbleTimer * 2f) * this.wobbleY);
		}
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x001BAD14 File Offset: 0x001B9114
	public void Grow()
	{
		this.coll.enabled = true;
		this.startedGrowing = true;
		base.animator.Play("BucketStart" + ((!base.CanParry) ? string.Empty : "Pink"));
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x001BAD64 File Offset: 0x001B9164
	public void Fire()
	{
		this.moving = true;
		base.animator.Play("BucketLoop" + ((!base.CanParry) ? string.Empty : "Pink"));
		this.shootFX.Create(base.transform.position, new Vector3(-Mathf.Sign(base.transform.position.x), 1f));
		this.spawnFX.Play("None");
		this.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoop();
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x001BADF7 File Offset: 0x001B91F7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.SFX_SNOWCULT_JackFrostSplitshotBucketImpact();
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x001BAE1B File Offset: 0x001B921B
	private void AniEvent_EndExplode()
	{
		this.Recycle<SnowCultLevelSplitShotBullet>();
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x001BAE24 File Offset: 0x001B9224
	private void SpawnProjectiles()
	{
		if (this.bulletsSpawned)
		{
			return;
		}
		this.bulletsSpawned = true;
		float num = this.spreadAngle / Mathf.Round((float)(this.numOfBullets / 2));
		float num2 = this.middleAngle - this.spreadAngle;
		for (int i = 0; i < this.numOfBullets; i++)
		{
			this.shatteredBullet.Create(base.transform.position, num2 + num * (float)i, this.speed);
		}
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x001BAEA8 File Offset: 0x001B92A8
	protected override void Update()
	{
		base.Update();
		if (this.main.dead != this.dead)
		{
			this.dead = true;
			this.SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop();
			if (!this.startedGrowing)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("BucketStart" + ((!base.CanParry) ? string.Empty : "Pink")))
			{
				this.spawnFX.Play("None");
				base.animator.Play("BucketStartReverse" + ((!base.CanParry) ? string.Empty : "Pink"), 0, 1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			}
		}
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x001BAF90 File Offset: 0x001B9390
	private void SFX_SNOWCULT_JackFrostSplitshotBucketImpact()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_impact");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_impact");
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x001BAFAC File Offset: 0x001B93AC
	private void SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop");
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x001BAFC8 File Offset: 0x001B93C8
	private void SFX_SNOWCULT_JackFrostSplitshotBucketTravelLoopStop()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_bucket_travel_loop");
	}

	// Token: 0x0400378D RID: 14221
	[SerializeField]
	private SnowCultLevelSplitShotBulletShattered shatteredBullet;

	// Token: 0x0400378E RID: 14222
	[SerializeField]
	private Effect shootFX;

	// Token: 0x0400378F RID: 14223
	[SerializeField]
	private Animator spawnFX;

	// Token: 0x04003790 RID: 14224
	private float middleAngle;

	// Token: 0x04003791 RID: 14225
	private float spreadAngle;

	// Token: 0x04003792 RID: 14226
	private float rotation;

	// Token: 0x04003793 RID: 14227
	private float speed;

	// Token: 0x04003794 RID: 14228
	private bool moving;

	// Token: 0x04003795 RID: 14229
	private int numOfBullets;

	// Token: 0x04003796 RID: 14230
	private bool bulletsSpawned;

	// Token: 0x04003797 RID: 14231
	private Collider2D coll;

	// Token: 0x04003798 RID: 14232
	private Vector3 basePos;

	// Token: 0x04003799 RID: 14233
	private float wobbleTimer;

	// Token: 0x0400379A RID: 14234
	[SerializeField]
	private float wobbleX = 10f;

	// Token: 0x0400379B RID: 14235
	[SerializeField]
	private float wobbleY = 10f;

	// Token: 0x0400379C RID: 14236
	[SerializeField]
	private float wobbleSpeed = 1f;

	// Token: 0x0400379D RID: 14237
	public SnowCultLevelJackFrost main;

	// Token: 0x0400379E RID: 14238
	private new bool dead;

	// Token: 0x0400379F RID: 14239
	private bool startedGrowing;
}
