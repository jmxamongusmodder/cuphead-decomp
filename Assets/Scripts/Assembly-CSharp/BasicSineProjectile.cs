using System;
using UnityEngine;

// Token: 0x02000AE9 RID: 2793
public class BasicSineProjectile : BasicProjectile
{
	// Token: 0x060043AC RID: 17324 RVA: 0x000CD83C File Offset: 0x000CBC3C
	public BasicSineProjectile Create(Vector2 pos, float rotation, float velocity, float sinVelocity, float sinSize)
	{
		BasicSineProjectile basicSineProjectile = this.Create(pos) as BasicSineProjectile;
		basicSineProjectile.velocity = velocity;
		basicSineProjectile.rotation = rotation;
		basicSineProjectile.sinSize = sinSize;
		basicSineProjectile.sinVelocity = sinVelocity;
		return basicSineProjectile;
	}

	// Token: 0x060043AD RID: 17325 RVA: 0x000CD875 File Offset: 0x000CBC75
	protected override void Start()
	{
		base.Start();
		this.CalculateSin();
	}

	// Token: 0x060043AE RID: 17326 RVA: 0x000CD884 File Offset: 0x000CBC84
	private void CalculateSin()
	{
		Vector2 zero = Vector2.zero;
		zero.x = (this.direction.x + base.transform.position.x) / 2f;
		zero.y = (this.direction.y + base.transform.position.y) / 2f;
		float num = -((this.direction.x - base.transform.position.x) / (this.direction.y - base.transform.position.y));
		float num2 = zero.y - num * zero.x;
		Vector2 zero2 = Vector2.zero;
		zero2.x = zero.x + 1f;
		zero2.y = num * zero2.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero2 - zero;
		this.normalized.Normalize();
	}

	// Token: 0x060043AF RID: 17327 RVA: 0x000CD99C File Offset: 0x000CBD9C
	protected override void Move()
	{
		this.direction = MathUtils.AngleToDirection(this.rotation);
		Vector2 vector = base.transform.position;
		this.angle += this.sinVelocity * CupheadTime.Delta;
		vector += this.normalized * Mathf.Sin(this.angle) * this.sinSize;
		vector += this.direction * this.velocity * CupheadTime.Delta;
		base.transform.position = vector;
	}

	// Token: 0x04004960 RID: 18784
	protected Vector2 direction;

	// Token: 0x04004961 RID: 18785
	protected Vector2 normalized;

	// Token: 0x04004962 RID: 18786
	public float velocity;

	// Token: 0x04004963 RID: 18787
	public float sinVelocity;

	// Token: 0x04004964 RID: 18788
	protected float angle;

	// Token: 0x04004965 RID: 18789
	public float rotation;

	// Token: 0x04004966 RID: 18790
	public float sinSize;
}
