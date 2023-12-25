using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200069D RID: 1693
public class FlyingMermaidLevelSkullBubble : BasicSineProjectile
{
	// Token: 0x060023DF RID: 9183 RVA: 0x00151224 File Offset: 0x0014F624
	public FlyingMermaidLevelSkullBubble CreateBubble(Vector2 pos, float velocity, float sinVelocity, float sinSize, float rotation)
	{
		return base.Create(pos, rotation, velocity, sinVelocity, sinSize) as FlyingMermaidLevelSkullBubble;
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x00151245 File Offset: 0x0014F645
	protected override void Awake()
	{
		base.Awake();
		this.smallBubbles = new List<Effect>();
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x00151258 File Offset: 0x0014F658
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_small_bubbles_cr());
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x00151270 File Offset: 0x0014F670
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (!this.isDead)
		{
			if (phase != CollisionPhase.Exit)
			{
				this.damageDealer.DealDamage(hit);
			}
			this.isDead = true;
			this.StopAllCoroutines();
			base.StartCoroutine(this.dying_cr());
		}
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x001512C0 File Offset: 0x0014F6C0
	private IEnumerator spawn_small_bubbles_cr()
	{
		while (!this.isDead)
		{
			Vector3 offset = new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(-15f, 15f), 0f);
			this.smallBubbles.Add(this.smallBubblesPrefab.Create(base.transform.position + offset, new Vector3(this.smallBubblesSize, this.smallBubblesSize, this.smallBubblesSize)));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x001512DB File Offset: 0x0014F6DB
	protected override void Die()
	{
		base.Die();
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x001512E4 File Offset: 0x0014F6E4
	private IEnumerator dying_cr()
	{
		base.animator.Play("Pop");
		yield return base.animator.WaitForAnimationToEnd(this, "Pop", false, true);
		while (base.transform.position.y > -660f)
		{
			base.transform.AddPosition(0f, (-this.velocity + this.accumulatedGravity) * CupheadTime.Delta, 0f);
			this.accumulatedGravity += -25f;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x04002CAC RID: 11436
	[SerializeField]
	private Effect smallBubblesPrefab;

	// Token: 0x04002CAD RID: 11437
	[SerializeField]
	private float smallBubblesSize;

	// Token: 0x04002CAE RID: 11438
	private const float GRAVITY = -25f;

	// Token: 0x04002CAF RID: 11439
	private const float bubblesOffsetX = 15f;

	// Token: 0x04002CB0 RID: 11440
	private const float bubblesOffsetY = 15f;

	// Token: 0x04002CB1 RID: 11441
	private List<Effect> smallBubbles;

	// Token: 0x04002CB2 RID: 11442
	private float accumulatedGravity;

	// Token: 0x04002CB3 RID: 11443
	private bool isDead;
}
