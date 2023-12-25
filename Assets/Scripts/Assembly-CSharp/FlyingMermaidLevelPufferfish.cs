using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000697 RID: 1687
public class FlyingMermaidLevelPufferfish : AbstractProjectile
{
	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060023C2 RID: 9154 RVA: 0x00150282 File Offset: 0x0014E682
	// (set) Token: 0x060023C3 RID: 9155 RVA: 0x0015028A File Offset: 0x0014E68A
	public FlyingMermaidLevelPufferfish.State state { get; private set; }

	// Token: 0x060023C4 RID: 9156 RVA: 0x00150294 File Offset: 0x0014E694
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Vector2 v = base.transform.position;
		v.y = this.spawnY;
		base.transform.position = v;
		this.SetParryable(this.parryable);
		base.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x00150325 File Offset: 0x0014E725
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x00150344 File Offset: 0x0014E744
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		AudioManager.Play("level_mermaid_merdusa_puffer_fish_hit");
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != FlyingMermaidLevelPufferfish.State.Dying)
		{
			this.state = FlyingMermaidLevelPufferfish.State.Dying;
			this.StartDeath();
		}
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x00150397 File Offset: 0x0014E797
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.state != FlyingMermaidLevelPufferfish.State.Dying && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x001503C1 File Offset: 0x0014E7C1
	public void Init(LevelProperties.FlyingMermaid.Pufferfish properties)
	{
		this.properties = properties;
		this.hp = properties.hp;
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x001503E4 File Offset: 0x0014E7E4
	private IEnumerator loop_cr()
	{
		float speed = this.properties.floatSpeed * UnityEngine.Random.Range(0.9f, 1.1f);
		for (;;)
		{
			Vector2 position = base.transform.position;
			position.y += speed * CupheadTime.Delta;
			base.transform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x001503FF File Offset: 0x0014E7FF
	private void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x00150414 File Offset: 0x0014E814
	private IEnumerator dying_cr()
	{
		base.gameObject.tag = "EnemyProjectile";
		this.deathFX.Create(base.transform.position);
		base.animator.Play("Death");
		float velocity = 100f;
		while (base.transform.position.y > -660f)
		{
			velocity += CupheadTime.Delta * 300f;
			base.transform.AddPosition(0f, (-velocity + this.accumulatedGravity) * CupheadTime.Delta, 0f);
			this.accumulatedGravity += -100f;
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x0015042F File Offset: 0x0014E82F
	protected override void Die()
	{
		base.transform.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x00150448 File Offset: 0x0014E848
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.deathFX = null;
	}

	// Token: 0x04002C85 RID: 11397
	private const float GRAVITY = -100f;

	// Token: 0x04002C87 RID: 11399
	[SerializeField]
	private Effect deathFX;

	// Token: 0x04002C88 RID: 11400
	[SerializeField]
	private float spawnY;

	// Token: 0x04002C89 RID: 11401
	[SerializeField]
	private bool parryable;

	// Token: 0x04002C8A RID: 11402
	private DamageReceiver damageReceiver;

	// Token: 0x04002C8B RID: 11403
	private LevelProperties.FlyingMermaid.Pufferfish properties;

	// Token: 0x04002C8C RID: 11404
	private float hp;

	// Token: 0x04002C8D RID: 11405
	private float accumulatedGravity;

	// Token: 0x02000698 RID: 1688
	public enum State
	{
		// Token: 0x04002C8F RID: 11407
		Idle,
		// Token: 0x04002C90 RID: 11408
		Dying
	}
}
