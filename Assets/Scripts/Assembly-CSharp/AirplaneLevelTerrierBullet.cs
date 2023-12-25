using System;
using UnityEngine;

// Token: 0x02000AE5 RID: 2789
public class AirplaneLevelTerrierBullet : BasicProjectile
{
	// Token: 0x0600438D RID: 17293 RVA: 0x0023FDA8 File Offset: 0x0023E1A8
	public AirplaneLevelTerrierBullet Create(Vector2 position, float rotation, float speed, float acceleration)
	{
		AirplaneLevelTerrierBullet airplaneLevelTerrierBullet = this.Create(position, rotation) as AirplaneLevelTerrierBullet;
		airplaneLevelTerrierBullet.endVelocity = MathUtils.AngleToDirection(rotation) * speed;
		airplaneLevelTerrierBullet.startVelocity = airplaneLevelTerrierBullet.endVelocity.normalized * 0.1f;
		airplaneLevelTerrierBullet.accelT = 0f;
		airplaneLevelTerrierBullet.accel = acceleration;
		airplaneLevelTerrierBullet.transform.rotation = Quaternion.identity;
		float num = Vector3.SignedAngle(airplaneLevelTerrierBullet.velocity, Vector3.up, Vector3.forward);
		while (Mathf.Abs(num) > 45f)
		{
			num -= 90f * Mathf.Sign(num);
		}
		airplaneLevelTerrierBullet.transform.Rotate(new Vector3(0f, 0f, -num));
		return airplaneLevelTerrierBullet;
	}

	// Token: 0x0600438E RID: 17294 RVA: 0x0023FE6F File Offset: 0x0023E26F
	public void PlayWow()
	{
		base.animator.Play("WowIntro");
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x0023FE84 File Offset: 0x0023E284
	protected override void Move()
	{
		this.accelT += CupheadTime.FixedDelta * this.accel * 1.6f;
		this.velocity = Vector3.Lerp(this.startVelocity, this.endVelocity, this.accelT);
		base.transform.position += this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x04004953 RID: 18771
	private const float BASE_ACCELERATION = 1.6f;

	// Token: 0x04004954 RID: 18772
	protected Vector3 velocity;

	// Token: 0x04004955 RID: 18773
	protected Vector3 startVelocity;

	// Token: 0x04004956 RID: 18774
	protected Vector3 endVelocity;

	// Token: 0x04004957 RID: 18775
	protected float accelT;

	// Token: 0x04004958 RID: 18776
	protected float accel;
}
