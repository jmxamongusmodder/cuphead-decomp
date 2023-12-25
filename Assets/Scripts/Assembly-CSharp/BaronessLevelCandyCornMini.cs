using System;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class BaronessLevelCandyCornMini : AbstractProjectile
{
	// Token: 0x1700031F RID: 799
	// (get) Token: 0x060015D3 RID: 5587 RVA: 0x000C42FE File Offset: 0x000C26FE
	// (set) Token: 0x060015D4 RID: 5588 RVA: 0x000C4306 File Offset: 0x000C2706
	public BaronessLevelCandyCornMini.State state { get; private set; }

	// Token: 0x060015D5 RID: 5589 RVA: 0x000C430F File Offset: 0x000C270F
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x000C433A File Offset: 0x000C273A
	protected override void Start()
	{
		base.Start();
		base.GetComponent<SpriteRenderer>().flipX = Rand.Bool();
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x000C4352 File Offset: 0x000C2752
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x000C4370 File Offset: 0x000C2770
	public void Init(Vector2 pos, float speed, float health)
	{
		this.speed = speed;
		base.transform.position = pos;
		this.health = health;
		this.state = BaronessLevelCandyCornMini.State.Spawned;
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x000C4398 File Offset: 0x000C2798
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x000C43B8 File Offset: 0x000C27B8
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		float num = 2f;
		Vector3 position = base.transform.position;
		position.y = Mathf.MoveTowards(base.transform.position.y, 720f + num, this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
		base.transform.position = position;
		if (base.transform.position.y == 720f + num)
		{
			this.Die();
		}
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x000C4447 File Offset: 0x000C2847
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x000C4468 File Offset: 0x000C2868
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.deathEffect.Create(base.transform.position);
			this.Die();
		}
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x000C44B5 File Offset: 0x000C28B5
	protected override void Die()
	{
		base.Die();
		this.state = BaronessLevelCandyCornMini.State.Unspawned;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x000C44D5 File Offset: 0x000C28D5
	private void SoundCandyCornMiniBite()
	{
		AudioManager.Play("level_baroness_candycorn_mini_bite");
		this.emitAudioFromObject.Add("level_baroness_candycorn_mini_bite");
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x000C44F1 File Offset: 0x000C28F1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.deathEffect = null;
	}

	// Token: 0x04001F27 RID: 7975
	[SerializeField]
	private Effect deathEffect;

	// Token: 0x04001F29 RID: 7977
	private float speed;

	// Token: 0x04001F2A RID: 7978
	private float health;

	// Token: 0x04001F2B RID: 7979
	private Vector3 lastPos;

	// Token: 0x04001F2C RID: 7980
	private Vector3 distFromLeaderX;

	// Token: 0x04001F2D RID: 7981
	private DamageReceiver damageReceiver;

	// Token: 0x020004EA RID: 1258
	public enum State
	{
		// Token: 0x04001F2F RID: 7983
		Unspawned,
		// Token: 0x04001F30 RID: 7984
		Spawned,
		// Token: 0x04001F31 RID: 7985
		Dying
	}
}
