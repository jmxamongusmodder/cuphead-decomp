using System;
using UnityEngine;

// Token: 0x0200058B RID: 1419
public class DevilLevelFireball : AbstractProjectile
{
	// Token: 0x06001B1C RID: 6940 RVA: 0x000F91D0 File Offset: 0x000F75D0
	public DevilLevelFireball Create(float xPos, float speed, float gravity, float xScale)
	{
		DevilLevelFireball devilLevelFireball = this.InstantiatePrefab<DevilLevelFireball>();
		devilLevelFireball.transform.position = new Vector2(xPos, 500f);
		devilLevelFireball.yVelocity = -speed;
		devilLevelFireball.gravity = gravity;
		devilLevelFireball.transform.SetScale(new float?(xScale), null, null);
		return devilLevelFireball;
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x000F9233 File Offset: 0x000F7633
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x000F9254 File Offset: 0x000F7654
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		this.yVelocity -= this.gravity * CupheadTime.FixedDelta;
		base.transform.AddPosition(0f, this.yVelocity * CupheadTime.FixedDelta, 0f);
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x000F92B0 File Offset: 0x000F76B0
	protected override void Die()
	{
		base.Die();
		this.poofEffect.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.parts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x000F9316 File Offset: 0x000F7716
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.poofEffect = null;
		this.parts = null;
	}

	// Token: 0x04002457 RID: 9303
	[SerializeField]
	private Effect poofEffect;

	// Token: 0x04002458 RID: 9304
	[SerializeField]
	private SpriteDeathParts[] parts;

	// Token: 0x04002459 RID: 9305
	private const float SPAWN_Y = 500f;

	// Token: 0x0400245A RID: 9306
	private float yVelocity;

	// Token: 0x0400245B RID: 9307
	private float gravity;
}
