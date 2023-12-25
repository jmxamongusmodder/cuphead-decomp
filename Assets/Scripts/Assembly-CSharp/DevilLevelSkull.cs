using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000596 RID: 1430
public class DevilLevelSkull : AbstractProjectile
{
	// Token: 0x06001B67 RID: 7015 RVA: 0x000FACA8 File Offset: 0x000F90A8
	public DevilLevelSkull Create(Vector2 pos, LevelProperties.Devil.SkullEye properties)
	{
		DevilLevelSkull devilLevelSkull = this.InstantiatePrefab<DevilLevelSkull>();
		devilLevelSkull.properties = properties;
		devilLevelSkull.transform.position = pos;
		devilLevelSkull.StartCoroutine(devilLevelSkull.main_cr());
		return devilLevelSkull;
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x000FACE2 File Offset: 0x000F90E2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x000FAD00 File Offset: 0x000F9100
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 100f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x000FAD4C File Offset: 0x000F914C
	private IEnumerator main_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Start", false, true);
		AbstractPlayerController player = PlayerManager.GetNext();
		Vector2 moveDir = (player.transform.position - base.transform.position).normalized;
		Vector2 velocity = moveDir * this.properties.initialMoveSpeed;
		float rotation = MathUtils.DirectionToAngle(player.transform.position - base.transform.position);
		float t = 0f;
		while (t < this.properties.initialMoveDuration)
		{
			t += CupheadTime.FixedDelta;
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		float rotationSpeed = (float)Rand.PosOrNeg() * this.properties.swirlRotationSpeed;
		t = 0f;
		Vector2 spiralOrigin = base.transform.position;
		for (;;)
		{
			t += CupheadTime.FixedDelta;
			rotation += rotationSpeed * CupheadTime.FixedDelta;
			base.transform.position = spiralOrigin + MathUtils.AngleToDirection(rotation) * this.properties.swirlMoveOutwardSpeed * t;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x040024A2 RID: 9378
	private LevelProperties.Devil.SkullEye properties;
}
