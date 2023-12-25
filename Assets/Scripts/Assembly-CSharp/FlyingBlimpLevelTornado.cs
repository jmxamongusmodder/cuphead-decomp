using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000644 RID: 1604
public class FlyingBlimpLevelTornado : AbstractCollidableObject
{
	// Token: 0x17000389 RID: 905
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x00130329 File Offset: 0x0012E729
	// (set) Token: 0x060020ED RID: 8429 RVA: 0x00130331 File Offset: 0x0012E731
	public FlyingBlimpLevelTornado.State state { get; private set; }

	// Token: 0x060020EE RID: 8430 RVA: 0x0013033A File Offset: 0x0012E73A
	public void Init(Vector2 pos, AbstractPlayerController player, LevelProperties.FlyingBlimp.Tornado properties)
	{
		base.transform.position = pos;
		this.player = player;
		this.properties = properties;
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x0013035B File Offset: 0x0012E75B
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.state = FlyingBlimpLevelTornado.State.Alive;
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x0013036F File Offset: 0x0012E76F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x00130390 File Offset: 0x0012E790
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		while (base.transform.position.x >= -1280f)
		{
			t += CupheadTime.FixedDelta;
			Vector2 homingDirection = this.player.transform.position - base.transform.position;
			Vector2 homingVelocity = homingDirection * this.properties.homingSpeed;
			float velocity = homingVelocity.y;
			if (base.transform.position.x > this.player.transform.position.x)
			{
				velocity = Mathf.Lerp(this.properties.moveSpeed, homingVelocity.y, 1f);
			}
			else if (this.state != FlyingBlimpLevelTornado.State.Dead)
			{
				velocity = homingVelocity.y;
			}
			else
			{
				velocity = 0f;
			}
			base.transform.AddPosition(-this.properties.moveSpeed * CupheadTime.FixedDelta, velocity * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x001303AB File Offset: 0x0012E7AB
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002984 RID: 10628
	private LevelProperties.FlyingBlimp.Tornado properties;

	// Token: 0x04002985 RID: 10629
	private AbstractPlayerController player;

	// Token: 0x04002986 RID: 10630
	private float movementSpeed;

	// Token: 0x04002987 RID: 10631
	private DamageDealer damageDealer;

	// Token: 0x02000645 RID: 1605
	public enum State
	{
		// Token: 0x04002989 RID: 10633
		Alive,
		// Token: 0x0400298A RID: 10634
		Dead
	}
}
