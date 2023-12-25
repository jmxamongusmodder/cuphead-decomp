using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000699 RID: 1689
public class FlyingMermaidLevelSeahorse : AbstractCollidableObject
{
	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060023CF RID: 9167 RVA: 0x001506DE File Offset: 0x0014EADE
	// (set) Token: 0x060023D0 RID: 9168 RVA: 0x001506E6 File Offset: 0x0014EAE6
	public FlyingMermaidLevelSeahorse.State state { get; private set; }

	// Token: 0x060023D1 RID: 9169 RVA: 0x001506F0 File Offset: 0x0014EAF0
	protected override void Awake()
	{
		base.Awake();
		this.spray.enabled = false;
		base.StartCoroutine(this.intro_cr());
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Vector2 v = base.transform.position;
		v.y = this.spawnY;
		base.transform.position = v;
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x00150779 File Offset: 0x0014EB79
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x00150794 File Offset: 0x0014EB94
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != FlyingMermaidLevelSeahorse.State.Dying)
		{
			AudioManager.Play("level_mermaid_seahorse_death");
			this.state = FlyingMermaidLevelSeahorse.State.Dying;
			this.StopAllCoroutines();
			base.StartCoroutine(this.die_cr());
		}
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x001507F4 File Offset: 0x0014EBF4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x00150814 File Offset: 0x0014EC14
	public void Init(LevelProperties.FlyingMermaid.Seahorse properties)
	{
		this.properties = properties;
		GroundHomingMovement component = base.GetComponent<GroundHomingMovement>();
		component.acceleration = properties.acceleration;
		component.maxSpeed = properties.maxSpeed;
		component.bounceRatio = properties.bounceRatio;
		this.hp = properties.hp;
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x00150860 File Offset: 0x0014EC60
	private IEnumerator die_cr()
	{
		this.state = FlyingMermaidLevelSeahorse.State.Dying;
		GroundHomingMovement homer = base.GetComponent<GroundHomingMovement>();
		Collider2D collider = base.GetComponent<Collider2D>();
		homer.enabled = false;
		collider.enabled = false;
		base.animator.SetTrigger("SprayDeath");
		this.spray.End();
		AudioManager.Play("level_mermaid_seahorse_death");
		base.animator.SetTrigger("OnDeath");
		Transform deathFx = UnityEngine.Object.Instantiate<Transform>(this.deathFxPrefab);
		deathFx.SetParent(this.deathFxRoot);
		deathFx.ResetLocalTransforms();
		yield return CupheadTime.WaitForSeconds(this, this.deathStayTime);
		float t = 0f;
		while (t < this.deathMoveTime)
		{
			t += CupheadTime.Delta;
			Vector2 position = base.transform.localPosition;
			position.y -= this.deathMoveDistance * CupheadTime.Delta / this.deathMoveTime;
			base.transform.localPosition = position;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x0015087C File Offset: 0x0014EC7C
	private IEnumerator intro_cr()
	{
		GroundHomingMovement homer = base.GetComponent<GroundHomingMovement>();
		Collider2D collider = base.GetComponent<Collider2D>();
		homer.enabled = false;
		collider.enabled = false;
		float t = 0f;
		while (t < this.riseTime)
		{
			t += CupheadTime.Delta;
			Vector2 position = base.transform.localPosition;
			position.y += this.riseDistance / this.riseTime * CupheadTime.Delta;
			base.transform.localPosition = position;
			yield return null;
		}
		AudioManager.Play("level_mermaid_seahorse_intro");
		Animator animator = base.GetComponent<Animator>();
		animator.SetTrigger("Continue");
		yield return animator.WaitForAnimationToStart(this, "Spit_Start", false);
		this.spray.enabled = true;
		this.spray.Init(this.properties);
		yield return animator.WaitForAnimationToEnd(this, "Spit_Start", false, true);
		this.state = FlyingMermaidLevelSeahorse.State.Spit;
		base.StartCoroutine(this.spit_cr());
		yield break;
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x00150898 File Offset: 0x0014EC98
	private IEnumerator spit_cr()
	{
		AudioManager.Play("level_mermaid_seahorse_spit");
		GroundHomingMovement homer = base.GetComponent<GroundHomingMovement>();
		Collider2D collider = base.GetComponent<Collider2D>();
		homer.enabled = true;
		collider.enabled = true;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			if (t > this.properties.homingDuration || ((FlyingMermaidLevel)Level.Current).MerdusaTransformStarted)
			{
				break;
			}
			yield return null;
		}
		homer.EnableHoming = false;
		homer.bounceEnabled = false;
		base.animator.SetTrigger("SprayDeath");
		this.spray.End();
		yield break;
	}

	// Token: 0x04002C92 RID: 11410
	[SerializeField]
	private float spawnY;

	// Token: 0x04002C93 RID: 11411
	[SerializeField]
	private float riseTime;

	// Token: 0x04002C94 RID: 11412
	[SerializeField]
	private float riseDistance;

	// Token: 0x04002C95 RID: 11413
	[SerializeField]
	private float deathStayTime;

	// Token: 0x04002C96 RID: 11414
	[SerializeField]
	private float deathMoveTime;

	// Token: 0x04002C97 RID: 11415
	[SerializeField]
	private float deathMoveDistance;

	// Token: 0x04002C98 RID: 11416
	[SerializeField]
	private FlyingMermaidLevelSeahorseSpray spray;

	// Token: 0x04002C99 RID: 11417
	[SerializeField]
	private Transform deathFxRoot;

	// Token: 0x04002C9A RID: 11418
	[SerializeField]
	private Transform deathFxPrefab;

	// Token: 0x04002C9B RID: 11419
	private DamageDealer damageDealer;

	// Token: 0x04002C9C RID: 11420
	private DamageReceiver damageReceiver;

	// Token: 0x04002C9D RID: 11421
	private LevelProperties.FlyingMermaid.Seahorse properties;

	// Token: 0x04002C9E RID: 11422
	private float hp;

	// Token: 0x0200069A RID: 1690
	public enum State
	{
		// Token: 0x04002CA0 RID: 11424
		Intro,
		// Token: 0x04002CA1 RID: 11425
		Spit,
		// Token: 0x04002CA2 RID: 11426
		Dying
	}
}
