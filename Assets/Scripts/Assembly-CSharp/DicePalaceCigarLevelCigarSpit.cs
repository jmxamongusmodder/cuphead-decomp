using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B0 RID: 1456
public class DicePalaceCigarLevelCigarSpit : AbstractProjectile
{
	// Token: 0x06001C2F RID: 7215 RVA: 0x001029C4 File Offset: 0x00100DC4
	public void InitProjectile(LevelProperties.DicePalaceCigar properties, bool clockwise, bool onRight)
	{
		this.time = 0f;
		this.centerPoint = base.transform.position;
		this.onRight = onRight;
		if (!clockwise)
		{
			this.circleSpeed = -properties.CurrentState.spiralSmoke.circleSpeed;
		}
		else
		{
			this.circleSpeed = properties.CurrentState.spiralSmoke.circleSpeed;
		}
		this.properties = properties;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.bullet_trail_cr());
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00102A50 File Offset: 0x00100E50
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			this.centerPoint += -base.transform.right * this.properties.CurrentState.spiralSmoke.horizontalSpeed * CupheadTime.FixedDelta;
			Vector3 newPos = this.centerPoint;
			newPos.y = this.centerPoint.y + Mathf.Sin(this.time * this.circleSpeed) * this.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
			if (this.onRight)
			{
				newPos.x = this.centerPoint.x + Mathf.Cos(this.time * this.circleSpeed) * this.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
			}
			else
			{
				newPos.x = this.centerPoint.x + -Mathf.Cos(this.time * this.circleSpeed) * this.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
			}
			base.transform.position = newPos;
			this.time += CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x00102A6C File Offset: 0x00100E6C
	private IEnumerator bullet_trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.16f, 0.2f));
			this.bulletFX.Create(base.transform.position);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x00102A87 File Offset: 0x00100E87
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0400253B RID: 9531
	[SerializeField]
	private Effect bulletFX;

	// Token: 0x0400253C RID: 9532
	private bool onRight;

	// Token: 0x0400253D RID: 9533
	private float time;

	// Token: 0x0400253E RID: 9534
	private float circleSpeed;

	// Token: 0x0400253F RID: 9535
	private Vector3 centerPoint;

	// Token: 0x04002540 RID: 9536
	private LevelProperties.DicePalaceCigar properties;
}
