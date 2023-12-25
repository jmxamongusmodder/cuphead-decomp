using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000688 RID: 1672
public class FlyingMermaidLevelFishSpinner : AbstractProjectile
{
	// Token: 0x06002342 RID: 9026 RVA: 0x0014B21C File Offset: 0x0014961C
	public FlyingMermaidLevelFishSpinner Create(Vector2 pos, Vector2 direction, LevelProperties.FlyingMermaid.SpinnerFish properties)
	{
		FlyingMermaidLevelFishSpinner flyingMermaidLevelFishSpinner = base.Create() as FlyingMermaidLevelFishSpinner;
		flyingMermaidLevelFishSpinner.properties = properties;
		flyingMermaidLevelFishSpinner.direction = direction;
		flyingMermaidLevelFishSpinner.transform.position = pos;
		return flyingMermaidLevelFishSpinner;
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x0014B255 File Offset: 0x00149655
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.tails_cr());
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x0014B277 File Offset: 0x00149677
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x0014B298 File Offset: 0x00149698
	private IEnumerator tails_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeBeforeTails);
		base.animator.SetTrigger("StartTails");
		yield break;
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x0014B2B4 File Offset: 0x001496B4
	private IEnumerator move_cr()
	{
		base.transform.SetEulerAngles(null, null, new float?((float)UnityEngine.Random.Range(0, 360)));
		Vector2 velocity = this.direction * this.properties.bulletSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			base.transform.Rotate(0f, 0f, this.properties.rotationSpeed * CupheadTime.FixedDelta);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x0014B2CF File Offset: 0x001496CF
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002BE8 RID: 11240
	private LevelProperties.FlyingMermaid.SpinnerFish properties;

	// Token: 0x04002BE9 RID: 11241
	private Vector2 direction;
}
