using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
public class FlyingMermaidLevelTurtle : AbstractCollidableObject
{
	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060023F3 RID: 9203 RVA: 0x001518EA File Offset: 0x0014FCEA
	// (set) Token: 0x060023F4 RID: 9204 RVA: 0x001518F2 File Offset: 0x0014FCF2
	public FlyingMermaidLevelTurtle.State state { get; private set; }

	// Token: 0x060023F5 RID: 9205 RVA: 0x001518FC File Offset: 0x0014FCFC
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.intro_cr());
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Vector2 v = base.transform.position;
		v.y = this.spawnY;
		base.transform.position = v;
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x00151979 File Offset: 0x0014FD79
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x00151994 File Offset: 0x0014FD94
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != FlyingMermaidLevelTurtle.State.Dying)
		{
			this.state = FlyingMermaidLevelTurtle.State.Dying;
			this.StopAllCoroutines();
			base.StartCoroutine(this.die_cr());
		}
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x001519EA File Offset: 0x0014FDEA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x00151A08 File Offset: 0x0014FE08
	public void Init(LevelProperties.FlyingMermaid.Turtle properties)
	{
		this.properties = properties;
		this.hp = properties.hp;
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x00151A20 File Offset: 0x0014FE20
	private IEnumerator die_cr()
	{
		AudioManager.Play("level_mermaid_turtle_flag");
		this.state = FlyingMermaidLevelTurtle.State.Dying;
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.animator.SetTrigger("OnDeath");
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

	// Token: 0x060023FB RID: 9211 RVA: 0x00151A3C File Offset: 0x0014FE3C
	private IEnumerator intro_cr()
	{
		AudioManager.Play("level_mermaid_turtle_enter");
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		float t = 0f;
		while (t < this.riseTime)
		{
			t += CupheadTime.Delta;
			Vector2 position = base.transform.localPosition;
			position.y += this.riseDistance / this.riseTime * CupheadTime.Delta;
			base.transform.localPosition = position;
			yield return null;
		}
		Animator animator = base.GetComponent<Animator>();
		animator.SetTrigger("Continue");
		yield return animator.WaitForAnimationToEnd(this, "Intro", false, true);
		this.state = FlyingMermaidLevelTurtle.State.Idle;
		base.StartCoroutine(this.pattern_cr());
		base.StartCoroutine(this.move_cr());
		foreach (Collider2D collider2D2 in base.GetComponents<Collider2D>())
		{
			collider2D2.enabled = true;
		}
		yield break;
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x00151A58 File Offset: 0x0014FE58
	private IEnumerator move_cr()
	{
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		for (;;)
		{
			if (this.moving)
			{
				Vector2 v = base.transform.localPosition;
				v.x -= this.properties.speed * CupheadTime.Delta;
				base.transform.localPosition = v;
				if (v.x < (float)Level.Current.Left - sprite.bounds.size.x / 2f)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x00151A74 File Offset: 0x0014FE74
	private IEnumerator pattern_cr()
	{
		string[] pattern = this.properties.explodeSpreadshotString.GetRandom<string>().Split(new char[]
		{
			','
		});
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeUntilShoot.RandomFloat());
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				float waitTime = 0f;
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			else if (!((FlyingMermaidLevel)Level.Current).MerdusaTransformStarted)
			{
				this.currentExplodePattern = pattern[i];
				AudioManager.Play("level_mermaid_turtle_shell_pop");
				base.animator.SetTrigger("Shoot");
				yield return base.animator.WaitForAnimationToEnd(this, "Idle", false, true);
				this.moving = false;
				yield return base.animator.WaitForAnimationToEnd(this, "Shoot_B", false, true);
				this.moving = true;
				AudioManager.Play("level_mermaid_turtle_post_cannon");
			}
		}
		yield break;
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x00151A90 File Offset: 0x0014FE90
	private void OnShootFX()
	{
		this.shootEffectPrefab.Create(this.shootEffectRoot.position);
		this.cannonBallPrefab.Create(this.cannonBallRoot.transform.position, this.currentExplodePattern, this.properties);
	}

	// Token: 0x04002CBC RID: 11452
	[SerializeField]
	private float spawnY;

	// Token: 0x04002CBD RID: 11453
	[SerializeField]
	private float riseTime;

	// Token: 0x04002CBE RID: 11454
	[SerializeField]
	private float riseDistance;

	// Token: 0x04002CBF RID: 11455
	[SerializeField]
	private float deathStayTime;

	// Token: 0x04002CC0 RID: 11456
	[SerializeField]
	private float deathMoveTime;

	// Token: 0x04002CC1 RID: 11457
	[SerializeField]
	private float deathMoveDistance;

	// Token: 0x04002CC2 RID: 11458
	[SerializeField]
	private FlyingMermaidLevelTurtleCannonBall cannonBallPrefab;

	// Token: 0x04002CC3 RID: 11459
	[SerializeField]
	private Transform cannonBallRoot;

	// Token: 0x04002CC4 RID: 11460
	[SerializeField]
	private Transform shootEffectRoot;

	// Token: 0x04002CC5 RID: 11461
	[SerializeField]
	private Effect shootEffectPrefab;

	// Token: 0x04002CC6 RID: 11462
	private DamageDealer damageDealer;

	// Token: 0x04002CC7 RID: 11463
	private DamageReceiver damageReceiver;

	// Token: 0x04002CC8 RID: 11464
	private LevelProperties.FlyingMermaid.Turtle properties;

	// Token: 0x04002CC9 RID: 11465
	private float hp;

	// Token: 0x04002CCA RID: 11466
	private bool moving = true;

	// Token: 0x04002CCB RID: 11467
	private string currentExplodePattern;

	// Token: 0x020006A1 RID: 1697
	public enum State
	{
		// Token: 0x04002CCD RID: 11469
		Intro,
		// Token: 0x04002CCE RID: 11470
		Idle,
		// Token: 0x04002CCF RID: 11471
		Dying
	}
}
