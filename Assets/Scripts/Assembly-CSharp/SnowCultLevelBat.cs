using System;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
public class SnowCultLevelBat : AbstractProjectile
{
	// Token: 0x06002E47 RID: 11847 RVA: 0x001B48FC File Offset: 0x001B2CFC
	public virtual SnowCultLevelBat Init(Vector3 startPos, Vector3 launchVel, LevelProperties.SnowCult.Snowball properties, SnowCultLevelYeti parent, bool parryable, string suffix)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.parent = parent;
		this.parent.OnDeathEvent += this.Dead;
		base.transform.position = startPos;
		this.speed = properties.batAttackSpeed;
		this.readdOnEscape = properties.batsReaddedOnEscape;
		this.moving = false;
		this.launchVelocity = launchVel;
		base.transform.localScale = new Vector3(Mathf.Sign(-launchVel.x), 1f);
		this.Health = properties.batHP;
		this.shotSpeed = properties.batShotSpeed;
		this.animatorSuffix = suffix;
		this.SetParryable(parryable);
		base.animator.Play("Slowdown" + this.animatorSuffix, 0, UnityEngine.Random.Range(0f, 0.33f));
		return this;
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x001B49DC File Offset: 0x001B2DDC
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x001B4A07 File Offset: 0x001B2E07
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x001B4A09 File Offset: 0x001B2E09
	protected override void OnDieDistance()
	{
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x001B4A0B File Offset: 0x001B2E0B
	public override void OnParryDie()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.EasyModeDie();
		}
		else
		{
			base.OnParryDie();
		}
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x001B4A2D File Offset: 0x001B2E2D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x001B4A4B File Offset: 0x001B2E4B
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Dead();
		}
	}

	// Token: 0x06002E4E RID: 11854 RVA: 0x001B4A80 File Offset: 0x001B2E80
	public void AttackPlayer(Vector3 startPos, float height, float width, float arc)
	{
		this.moving = true;
		this.attackStart = startPos;
		this.attackHeight = startPos.y - (CupheadLevelCamera.Current.Bounds.y + 100f) - height;
		this.attackWidth = width;
		base.transform.localScale = new Vector3(Mathf.Sign(-this.attackWidth), 1f);
		this.attackTime = 0f;
		this.arcModifier = arc;
		base.animator.SetFloat("YSpeed", -10f);
		base.animator.Play("Enter" + this.animatorSuffix);
		this.spriteRenderer.sortingOrder = 30;
		this.collider.enabled = true;
		this.reachedCircle = true;
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x001B4B50 File Offset: 0x001B2F50
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.reachedCircle)
		{
			if (base.transform.position.y >= 460f)
			{
				this.reachedCircle = true;
				this.collider.enabled = false;
			}
			else
			{
				base.transform.position += this.launchVelocity * CupheadTime.FixedDelta;
				this.launchVelocity += Vector3.up * 500f * CupheadTime.FixedDelta;
			}
		}
		if (this.moving)
		{
			this.attackTime += CupheadTime.FixedDelta * this.speed;
			if (this.attackTime < 0.9f)
			{
				this.lastPos = base.transform.position;
				base.transform.position = new Vector3(Mathf.Lerp(this.attackStart.x, this.attackStart.x + this.attackWidth, this.attackTime), this.attackStart.y + Mathf.Pow(Mathf.Sin(this.attackTime * 3.1415927f), this.arcModifier) * -this.attackHeight);
				base.animator.SetFloat("YSpeed", base.transform.position.y - this.lastPos.y);
			}
			else
			{
				Vector3 b = new Vector3(this.attackStart.x + this.attackWidth, this.attackStart.y) - this.lastPos;
				if (b.magnitude > 15f)
				{
					b = b.normalized * 15f;
				}
				base.transform.position += b;
				base.animator.SetFloat("YSpeed", b.y);
			}
			if (base.transform.position.y - this.lastPos.y > -6f)
			{
				this.dripTimer -= CupheadTime.FixedDelta;
				if (this.dripTimer <= 0f)
				{
					SnowCultLevelBatDrip snowCultLevelBatDrip = this.dripPrefab.Create(base.transform.position + Vector3.down * 50f) as SnowCultLevelBatDrip;
					snowCultLevelBatDrip.SetColor(this.animatorSuffix);
					snowCultLevelBatDrip.vel.x = (base.transform.position.x - this.lastPos.x) / 2f;
					this.dripTimer = UnityEngine.Random.Range(0.3f, 0.7f);
				}
			}
			if (this.attackTime > 1.2f)
			{
				if (this.readdOnEscape)
				{
					this.moving = false;
					this.collider.enabled = false;
					this.parent.ReturnBatToList(this);
				}
				else
				{
					this.Dead();
				}
			}
		}
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x001B4E68 File Offset: 0x001B3268
	public void Dead()
	{
		if (base.transform.position.y < 360f)
		{
			((SnowCultLevelBatEffect)this.explosionPrefab.Create(base.transform.position)).SetColor(this.animatorSuffix);
			this.SFX_SNOWCULT_BatDie();
		}
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.EasyModeDie();
		}
		else
		{
			this.StopAllCoroutines();
			this.Recycle<SnowCultLevelBat>();
		}
	}

	// Token: 0x06002E51 RID: 11857 RVA: 0x001B4EE4 File Offset: 0x001B32E4
	private void EasyModeDie()
	{
		this.moving = false;
		this.collider.enabled = false;
		this.parent.ReturnBatToList(this);
		base.transform.position = new Vector3(base.transform.position.x, 460f);
	}

	// Token: 0x06002E52 RID: 11858 RVA: 0x001B4F38 File Offset: 0x001B3338
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.Dead;
	}

	// Token: 0x06002E53 RID: 11859 RVA: 0x001B4F57 File Offset: 0x001B3357
	private void SFX_SNOWCULT_BatDie()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_popsicle_bat_death");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_popsicle_bat_death");
	}

	// Token: 0x040036CE RID: 14030
	private const float PADDING_FLOOR = 100f;

	// Token: 0x040036CF RID: 14031
	private const float PADDING_CEILING = 100f;

	// Token: 0x040036D0 RID: 14032
	private const float DRIP_TIME_MIN = 0.3f;

	// Token: 0x040036D1 RID: 14033
	private const float DRIP_TIME_MAX = 0.7f;

	// Token: 0x040036D2 RID: 14034
	private DamageReceiver damageReceiver;

	// Token: 0x040036D3 RID: 14035
	private SnowCultLevelYeti parent;

	// Token: 0x040036D4 RID: 14036
	private float speed;

	// Token: 0x040036D5 RID: 14037
	private float Health;

	// Token: 0x040036D6 RID: 14038
	public bool reachedCircle;

	// Token: 0x040036D7 RID: 14039
	public bool moving;

	// Token: 0x040036D8 RID: 14040
	private Vector3 launchVelocity;

	// Token: 0x040036D9 RID: 14041
	private float attackHeight;

	// Token: 0x040036DA RID: 14042
	private float attackWidth;

	// Token: 0x040036DB RID: 14043
	private Vector3 attackStart;

	// Token: 0x040036DC RID: 14044
	private float attackTime;

	// Token: 0x040036DD RID: 14045
	private float arcModifier = 1f;

	// Token: 0x040036DE RID: 14046
	private float dripTimer;

	// Token: 0x040036DF RID: 14047
	private float shotSpeed;

	// Token: 0x040036E0 RID: 14048
	private bool readdOnEscape;

	// Token: 0x040036E1 RID: 14049
	private Vector3 lastPos;

	// Token: 0x040036E2 RID: 14050
	[SerializeField]
	private SnowCultLevelBatEffect explosionPrefab;

	// Token: 0x040036E3 RID: 14051
	[SerializeField]
	private SnowCultLevelBatEffect dripPrefab;

	// Token: 0x040036E4 RID: 14052
	[SerializeField]
	private Collider2D collider;

	// Token: 0x040036E5 RID: 14053
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x040036E6 RID: 14054
	private string animatorSuffix;
}
