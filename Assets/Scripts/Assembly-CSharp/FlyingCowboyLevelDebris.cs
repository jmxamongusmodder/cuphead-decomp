using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class FlyingCowboyLevelDebris : BasicProjectile
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x0600219A RID: 8602 RVA: 0x00137C79 File Offset: 0x00136079
	private bool isCurved
	{
		get
		{
			return this.gravity != 0f;
		}
	}

	// Token: 0x0600219B RID: 8603 RVA: 0x00137C8C File Offset: 0x0013608C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector3(FlyingCowboyLevelDebris.OffsetAimXCutoff, 1000f), new Vector3(FlyingCowboyLevelDebris.OffsetAimXCutoff, -1000f));
		Vector3 vector = base.transform.position;
		Vector3 a = this.curveSpeed;
		for (int i = 0; i < 50; i++)
		{
			if (this.isCurved)
			{
				a += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
				vector += a * CupheadTime.FixedDelta;
			}
			else
			{
				vector += base.transform.right * this.Speed * CupheadTime.FixedDelta;
			}
			Gizmos.DrawWireSphere(vector, 10f);
		}
	}

	// Token: 0x0600219C RID: 8604 RVA: 0x00137D59 File Offset: 0x00136159
	public void SetupLinearSpeed(MinMax speedRange, float speedUpDistance, Transform aimTransform)
	{
		base.StartCoroutine(this.speedUp_cr(speedRange, speedUpDistance, aimTransform));
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x00137D6C File Offset: 0x0013616C
	private IEnumerator speedUp_cr(MinMax speedRange, float distance, Transform aimTransform)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		float sqrDistance = distance * distance;
		while (Vector3.SqrMagnitude(base.transform.position - aimTransform.position) > sqrDistance)
		{
			yield return wait;
		}
		float duration = KinematicUtilities.CalculateTimeToChangeVelocity(speedRange.min, speedRange.max, distance);
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return wait;
			elapsedTime += CupheadTime.FixedDelta;
			this.Speed = Mathf.Lerp(speedRange.min, speedRange.max, elapsedTime / duration);
		}
		this.Speed = speedRange.max;
		yield break;
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x00137D9C File Offset: 0x0013619C
	public void SetupVacuum(Transform aimTransform, Transform destroyTransform)
	{
		base.StartCoroutine(this.vacuumAim_cr(aimTransform, destroyTransform));
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x00137DB0 File Offset: 0x001361B0
	private IEnumerator vacuumAim_cr(Transform aimTransform, Transform destroyTransform)
	{
		if (this.isCurved)
		{
			while (this.curveSpeed.x < 0f)
			{
				yield return null;
			}
		}
		else if (base.transform.position.x >= FlyingCowboyLevelDebris.OffsetAimXCutoff)
		{
			float distanceThreshold = (base.transform.position.y <= 0f) ? 105f : 80f;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(aimTransform.position + FlyingCowboyLevelDebris.OffsetAimAmount - base.transform.position)));
			if (base.transform.position.x > aimTransform.position.x + FlyingCowboyLevelDebris.OffsetAimAmount.x)
			{
				while (Mathf.Abs(base.transform.position.y - aimTransform.position.y) > distanceThreshold)
				{
					yield return null;
				}
			}
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtilities.DirectionToAngle(aimTransform.position - base.transform.position)));
		}
		while (base.transform.position.x < aimTransform.position.x)
		{
			yield return null;
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.vacuumSuckIn_cr(destroyTransform));
		yield break;
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x00137DDC File Offset: 0x001361DC
	private IEnumerator vacuumSuckIn_cr(Transform destroyTransform)
	{
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtilities.DirectionToAngle(destroyTransform.position - base.transform.position)));
		this.move = true;
		if (this.isCurved)
		{
			this.Speed = this.curveSpeed.magnitude;
		}
		this.Speed *= 1.25f;
		base.StartCoroutine(this.squash_cr());
		while (base.transform.position.x < destroyTransform.position.x)
		{
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.Die();
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x00137E00 File Offset: 0x00136200
	private IEnumerator squash_cr()
	{
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.041666668f, false);
		float elapsedTime = 0f;
		while (elapsedTime < FlyingCowboyLevelDebris.SquashDuration)
		{
			yield return wait;
			elapsedTime += wait.frameTime + wait.accumulator;
			Vector3 scale = Vector3.Lerp(Vector2.one, FlyingCowboyLevelDebris.SquashAmount, elapsedTime / FlyingCowboyLevelDebris.SquashDuration);
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x00137E1B File Offset: 0x0013621B
	public void ToCurve(Vector3 speed, float gravity)
	{
		this.curveSpeed = speed;
		this.gravity = gravity;
		base.StartCoroutine(this.gravity_cr());
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x00137E38 File Offset: 0x00136238
	private IEnumerator gravity_cr()
	{
		this.move = false;
		for (;;)
		{
			this.curveSpeed += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
			base.transform.Translate(this.curveSpeed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04002A38 RID: 10808
	private static readonly float OffsetAimXCutoff = 0f;

	// Token: 0x04002A39 RID: 10809
	private static readonly Vector3 OffsetAimAmount = new Vector3(-50f, 0f);

	// Token: 0x04002A3A RID: 10810
	private static readonly Vector3 SquashAmount = new Vector3(1.2f, 0.5f, 1f);

	// Token: 0x04002A3B RID: 10811
	private static readonly float SquashDuration = 0.4f;

	// Token: 0x04002A3C RID: 10812
	private Vector3 curveSpeed;

	// Token: 0x04002A3D RID: 10813
	private float gravity;
}
