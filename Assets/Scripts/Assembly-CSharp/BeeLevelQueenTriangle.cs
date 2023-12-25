using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class BeeLevelQueenTriangle : AbstractProjectile
{
	// Token: 0x06001785 RID: 6021 RVA: 0x000D3DF8 File Offset: 0x000D21F8
	public BeeLevelQueenTriangle Create(BeeLevelQueenTriangle.Properties properties)
	{
		BeeLevelQueenTriangle beeLevelQueenTriangle = base.Create() as BeeLevelQueenTriangle;
		beeLevelQueenTriangle.transform.position = properties.player.center;
		beeLevelQueenTriangle.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		beeLevelQueenTriangle.properties = properties;
		return beeLevelQueenTriangle;
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06001786 RID: 6022 RVA: 0x000D3E5F File Offset: 0x000D225F
	protected override float DestroyLifetime
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x000D3E68 File Offset: 0x000D2268
	protected override void Awake()
	{
		base.Awake();
		if (!this.isInvincible)
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
			this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
		AudioManager.Play("bee_queen_triangle_spawn");
		this.emitAudioFromObject.Add("bee_queen_triangle_spawn");
		AudioManager.PlayLoop("bee_queen_triangle_loop");
		this.emitAudioFromObject.Add("bee_queen_triangle_loop");
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000D3EDD File Offset: 0x000D22DD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000D3EFB File Offset: 0x000D22FB
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000D3F10 File Offset: 0x000D2310
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!this.isInvincible)
		{
			this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		}
		this.childPrefab = null;
		this.childPrefabInvincible = null;
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x000D3F48 File Offset: 0x000D2348
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.properties.health -= info.damage;
		if (this.properties.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x000D3F7D File Offset: 0x000D237D
	protected override void Die()
	{
		base.Die();
		AudioManager.Stop("bee_queen_triangle_loop");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000D3FA4 File Offset: 0x000D23A4
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.properties != null)
		{
			base.transform.AddPosition(this.forward.x * CupheadTime.Delta * this.properties.speed, this.forward.y * CupheadTime.Delta * this.properties.speed, 0f);
			base.transform.AddEulerAngles(0f, 0f, this.properties.rotationSpeed * (float)this.properties.direction * CupheadTime.Delta);
		}
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x000D4064 File Offset: 0x000D2464
	private IEnumerator go_cr()
	{
		base.transform.GetComponent<Collider2D>().enabled = false;
		Transform aim = new GameObject("Aim").transform;
		aim.SetParent(base.transform);
		aim.ResetLocalTransforms();
		yield return base.StartCoroutine(this.tweenColor_cr(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), this.properties.introTime / 2f));
		yield return base.StartCoroutine(this.tweenColor_cr(new Color(0f, 0f, 0f, 1f), new Color(1f, 1f, 1f, 1f), this.properties.introTime / 2f));
		aim.LookAt2D(this.properties.player.center);
		this.forward = aim.transform.right;
		base.transform.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.shoot_cr());
		float t = 0f;
		while (t < 1f)
		{
			float val = t / 1f;
			this.properties.speed = Mathf.Lerp(0f, this.properties.speedMax, val);
			this.properties.rotationSpeed = Mathf.Lerp(0f, this.properties.rotationSpeedMax, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.properties.speed = this.properties.speedMax;
		this.properties.rotationSpeed = this.properties.rotationSpeedMax;
		yield break;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x000D4080 File Offset: 0x000D2480
	private IEnumerator shoot_cr()
	{
		int count = 0;
		while (count < this.properties.childCount)
		{
			AudioManager.Play("bee_queen_triangle_shoot");
			this.emitAudioFromObject.Add("bee_queen_triangle_shoot");
			foreach (Transform transform in this.roots)
			{
				if (this.properties.damageable)
				{
					this.childPrefab.Create(transform.position, transform.eulerAngles.z, this.properties.childSpeed, this.properties.childHealth).SetParryable(true);
				}
				else
				{
					this.childPrefabInvincible.Create(transform.position, transform.eulerAngles.z, this.properties.childSpeed).SetParryable(true);
				}
			}
			base.animator.Play("Attack");
			count++;
			yield return CupheadTime.WaitForSeconds(this, this.properties.childDelay);
		}
		base.animator.Play("Idle");
		yield break;
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x000D409C File Offset: 0x000D249C
	private IEnumerator tweenColor_cr(Color start, Color end, float time)
	{
		SpriteRenderer r = base.GetComponent<SpriteRenderer>();
		r.color = start;
		yield return null;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			r.color = Color.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		r.color = end;
		yield return null;
		yield break;
	}

	// Token: 0x040020B6 RID: 8374
	[SerializeField]
	private bool isInvincible;

	// Token: 0x040020B7 RID: 8375
	[SerializeField]
	private Transform[] roots;

	// Token: 0x040020B8 RID: 8376
	[SerializeField]
	private BasicDamagableProjectile childPrefab;

	// Token: 0x040020B9 RID: 8377
	[SerializeField]
	private BasicProjectile childPrefabInvincible;

	// Token: 0x040020BA RID: 8378
	private BeeLevelQueenTriangle.Properties properties;

	// Token: 0x040020BB RID: 8379
	private Vector2 forward;

	// Token: 0x040020BC RID: 8380
	private DamageReceiver damageReceiver;

	// Token: 0x02000521 RID: 1313
	public class Properties
	{
		// Token: 0x06001791 RID: 6033 RVA: 0x000D40CC File Offset: 0x000D24CC
		public Properties(AbstractPlayerController player, float introTime, float speed, float rotationSpeed, float health, float childSpeed, float childDelay, float childHealth, int childCount, bool damageable)
		{
			this.player = player;
			this.damageable = damageable;
			this.introTime = introTime;
			this.speedMax = speed;
			this.rotationSpeedMax = rotationSpeed;
			this.healthMax = health;
			this.childSpeed = childSpeed;
			this.childDelay = childDelay;
			this.childHealth = childHealth;
			this.childCount = childCount;
			this.direction = MathUtils.PlusOrMinus();
			this.speed = 0f;
			this.rotationSpeed = 0f;
			this.health = health;
		}

		// Token: 0x040020BD RID: 8381
		public readonly AbstractPlayerController player;

		// Token: 0x040020BE RID: 8382
		public readonly bool damageable;

		// Token: 0x040020BF RID: 8383
		public readonly float introTime;

		// Token: 0x040020C0 RID: 8384
		public readonly float speedMax;

		// Token: 0x040020C1 RID: 8385
		public readonly float rotationSpeedMax;

		// Token: 0x040020C2 RID: 8386
		public readonly float healthMax;

		// Token: 0x040020C3 RID: 8387
		public readonly float childSpeed;

		// Token: 0x040020C4 RID: 8388
		public readonly float childDelay;

		// Token: 0x040020C5 RID: 8389
		public readonly float childHealth;

		// Token: 0x040020C6 RID: 8390
		public readonly int childCount;

		// Token: 0x040020C7 RID: 8391
		public readonly int direction;

		// Token: 0x040020C8 RID: 8392
		public float speed;

		// Token: 0x040020C9 RID: 8393
		public float rotationSpeed;

		// Token: 0x040020CA RID: 8394
		public float health;
	}
}
