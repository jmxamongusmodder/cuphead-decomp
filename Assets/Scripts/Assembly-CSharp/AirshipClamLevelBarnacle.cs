using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class AirshipClamLevelBarnacle : AbstractProjectile
{
	// Token: 0x060014CB RID: 5323 RVA: 0x000BA49E File Offset: 0x000B889E
	protected override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x000BA4B1 File Offset: 0x000B88B1
	public void InitBarnacle(int dir, LevelProperties.AirshipClam properties)
	{
		this.properties = properties;
		this.direction = dir;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x000BA4D0 File Offset: 0x000B88D0
	private IEnumerator move_cr()
	{
		this.velocity = new Vector3(this.properties.CurrentState.barnacles.initialArcMovementX * (float)this.direction, this.properties.CurrentState.barnacles.initialArcMovementY, 0f);
		for (;;)
		{
			base.transform.position += this.velocity * CupheadTime.Delta;
			this.velocity.y = this.velocity.y + this.properties.CurrentState.barnacles.parryGravity;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x000BA4EB File Offset: 0x000B88EB
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter)
		{
			this.velocity.x = 0f;
		}
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000BA50C File Offset: 0x000B890C
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		this.velocity.y = 0f;
		this.velocity.x = this.properties.CurrentState.barnacles.rollingSpeed * (float)(-(float)this.direction);
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x000BA55A File Offset: 0x000B895A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x000BA57C File Offset: 0x000B897C
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04001E31 RID: 7729
	private int direction;

	// Token: 0x04001E32 RID: 7730
	private Vector3 velocity;

	// Token: 0x04001E33 RID: 7731
	private LevelProperties.AirshipClam properties;
}
