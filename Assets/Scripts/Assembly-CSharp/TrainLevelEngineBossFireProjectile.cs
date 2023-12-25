using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000814 RID: 2068
public class TrainLevelEngineBossFireProjectile : AbstractProjectile
{
	// Token: 0x06002FF3 RID: 12275 RVA: 0x001C5E53 File Offset: 0x001C4253
	public void Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		this.InstantiatePrefab<TrainLevelEngineBossFireProjectile>().Init(pos, velocity, gravity);
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x001C5E64 File Offset: 0x001C4264
	protected override void Start()
	{
		base.Start();
		base.animator.SetFloat("Direction", (float)((this.velocity.x <= 0f) ? 1 : -1));
		base.animator.Play("Idle", 0, UnityEngine.Random.value);
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.collider2d = base.GetComponent<CircleCollider2D>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.spawnTrail_cr());
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x001C5F08 File Offset: 0x001C4308
	protected override void Update()
	{
		base.Update();
		if (this.state == TrainLevelEngineBossFireProjectile.State.Moving)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			if (base.transform.position.y < -300f)
			{
				this.Die();
			}
		}
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x001C5FAC File Offset: 0x001C43AC
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		int num = this.collider2d.OverlapCollider(TrainLevelEngineBossFireProjectile.filter, TrainLevelEngineBossFireProjectile.buffer);
		for (int i = 0; i < num; i++)
		{
			if (TrainLevelEngineBossFireProjectile.buffer[i].GetComponent<TrainLevelPlatform>())
			{
				this.Die();
				break;
			}
		}
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x001C6008 File Offset: 0x001C4408
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x001C602C File Offset: 0x001C442C
	protected override void Die()
	{
		base.Die();
		base.transform.rotation = Quaternion.identity;
		this.spriteRenderer.flipX = Rand.Bool();
		this.state = TrainLevelEngineBossFireProjectile.State.Dead;
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x001C605B File Offset: 0x001C445B
	private void Init(Vector2 pos, Vector2 velocity, float gravity)
	{
		base.transform.position = pos;
		this.velocity = velocity;
		this.gravity = gravity;
		this.state = TrainLevelEngineBossFireProjectile.State.Moving;
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x001C6084 File Offset: 0x001C4484
	public IEnumerator spawnTrail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.trailPrefab.Create(base.transform.position + TrainLevelEngineBossFireProjectile.TrailOffset).Play();
		}
		yield break;
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x001C609F File Offset: 0x001C449F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.trailPrefab = null;
	}

	// Token: 0x040038CB RID: 14539
	private const string IdleStateName = "Idle";

	// Token: 0x040038CC RID: 14540
	private const string DirectionParameterName = "Direction";

	// Token: 0x040038CD RID: 14541
	private static readonly Vector3 TrailOffset = new Vector3(0f, 10f, 0f);

	// Token: 0x040038CE RID: 14542
	private static ContactFilter2D filter = default(ContactFilter2D).NoFilter();

	// Token: 0x040038CF RID: 14543
	private static Collider2D[] buffer = new Collider2D[10];

	// Token: 0x040038D0 RID: 14544
	[SerializeField]
	private Effect trailPrefab;

	// Token: 0x040038D1 RID: 14545
	public const float GROUND_Y = -300f;

	// Token: 0x040038D2 RID: 14546
	private TrainLevelEngineBossFireProjectile.State state;

	// Token: 0x040038D3 RID: 14547
	private Vector2 velocity;

	// Token: 0x040038D4 RID: 14548
	private float gravity;

	// Token: 0x040038D5 RID: 14549
	private CircleCollider2D collider2d;

	// Token: 0x040038D6 RID: 14550
	private SpriteRenderer spriteRenderer;

	// Token: 0x02000815 RID: 2069
	public enum State
	{
		// Token: 0x040038D8 RID: 14552
		Init,
		// Token: 0x040038D9 RID: 14553
		Moving,
		// Token: 0x040038DA RID: 14554
		Dead
	}
}
