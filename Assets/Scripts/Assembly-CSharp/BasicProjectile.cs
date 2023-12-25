using System;
using UnityEngine;

// Token: 0x02000AE7 RID: 2791
public class BasicProjectile : AbstractProjectile
{
	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x06004398 RID: 17304 RVA: 0x000B8149 File Offset: 0x000B6549
	protected virtual Vector3 Direction
	{
		get
		{
			return base.transform.right;
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x06004399 RID: 17305 RVA: 0x000B8156 File Offset: 0x000B6556
	protected override float DestroyLifetime
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x0600439A RID: 17306 RVA: 0x000B815D File Offset: 0x000B655D
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x0600439B RID: 17307 RVA: 0x000B8160 File Offset: 0x000B6560
	protected virtual string projectileMissImpactSFX
	{
		get
		{
			return "player_weapon_peashot_miss";
		}
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x000B8168 File Offset: 0x000B6568
	public virtual BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		BasicProjectile basicProjectile = this.Create(position, rotation) as BasicProjectile;
		basicProjectile.Speed = speed;
		return basicProjectile;
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x000B818C File Offset: 0x000B658C
	public virtual BasicProjectile Create(Vector2 position, float rotation, Vector2 scale, float speed)
	{
		BasicProjectile basicProjectile = this.Create(position, rotation, scale) as BasicProjectile;
		basicProjectile.Speed = speed;
		return basicProjectile;
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x000B81B1 File Offset: 0x000B65B1
	protected override void Awake()
	{
		base.Awake();
		if (base.CompareTag("EnemyProjectile"))
		{
			this.DamagesType.Player = true;
		}
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x000B81D5 File Offset: 0x000B65D5
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x000B81DD File Offset: 0x000B65DD
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x000B81E7 File Offset: 0x000B65E7
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x000B8207 File Offset: 0x000B6607
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x000B821F File Offset: 0x000B661F
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060043A4 RID: 17316 RVA: 0x000B8238 File Offset: 0x000B6638
	protected override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if ((hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled) || (hit.GetComponent<DamageReceiverChild>() && hit.GetComponent<DamageReceiverChild>().enabled))
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play(this.projectileMissImpactSFX);
			}
		}
	}

	// Token: 0x060043A5 RID: 17317 RVA: 0x000B82C2 File Offset: 0x000B66C2
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060043A6 RID: 17318 RVA: 0x000B82D4 File Offset: 0x000B66D4
	protected override void Die()
	{
		this.move = false;
		EffectSpawner component = base.GetComponent<EffectSpawner>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		base.Die();
	}

	// Token: 0x060043A7 RID: 17319 RVA: 0x000B8307 File Offset: 0x000B6707
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.move)
		{
			this.Move();
		}
	}

	// Token: 0x060043A8 RID: 17320 RVA: 0x000B8320 File Offset: 0x000B6720
	protected virtual void Move()
	{
		base.transform.position += this.Direction * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		this._accumulativeGravity += this.Gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x0400495B RID: 18779
	[Space(10f)]
	public float Speed;

	// Token: 0x0400495C RID: 18780
	public float Gravity;

	// Token: 0x0400495D RID: 18781
	[Space(10f)]
	public Sfx SfxOnDeath;

	// Token: 0x0400495E RID: 18782
	protected bool move = true;

	// Token: 0x0400495F RID: 18783
	protected float _accumulativeGravity;
}
