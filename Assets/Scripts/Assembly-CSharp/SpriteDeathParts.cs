using System;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
public class SpriteDeathParts : AbstractCollidableObject
{
	// Token: 0x060044E1 RID: 17633 RVA: 0x0013DD3C File Offset: 0x0013C13C
	public SpriteDeathParts CreatePart(Vector3 position)
	{
		SpriteDeathParts spriteDeathParts = this.InstantiatePrefab<SpriteDeathParts>();
		spriteDeathParts.transform.position = position;
		return spriteDeathParts;
	}

	// Token: 0x060044E2 RID: 17634 RVA: 0x0013DD60 File Offset: 0x0013C160
	protected override void Awake()
	{
		base.Awake();
		this.velocity = new Vector2(UnityEngine.Random.Range(this.VelocityXMin, this.VelocityXMax), UnityEngine.Random.Range(this.VelocityYMin, this.VelocityYMax));
		if (this.rotate)
		{
			this.rotationSpeed = UnityEngine.Random.Range(this.rotationSpeedRange.minimum, this.rotationSpeedRange.maximum) * (float)Rand.PosOrNeg();
		}
	}

	// Token: 0x060044E3 RID: 17635 RVA: 0x0013DDD3 File Offset: 0x0013C1D3
	protected virtual void Update()
	{
		this.Step(Time.fixedDeltaTime);
	}

	// Token: 0x060044E4 RID: 17636 RVA: 0x0013DDE0 File Offset: 0x0013C1E0
	protected virtual void Step(float deltaTime)
	{
		if (this.clampFallVelocity)
		{
			this.velocity.y = Mathf.Clamp(this.velocity.y, -5000f, float.MaxValue);
		}
		base.transform.position += (this.velocity + new Vector2(0f, this.accumulatedGravity)) * deltaTime;
		this.accumulatedGravity += this.GRAVITY;
		if (this.rotate)
		{
			this.currentAngle += this.rotationSpeed * deltaTime;
			base.transform.rotation = Quaternion.Euler(0f, 0f, this.currentAngle);
		}
		if (base.transform.position.y < -360f - this.bottomOffset)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x0013DEDB File Offset: 0x0013C2DB
	public void SetVelocityX(float min, float max)
	{
		this.velocity.x = UnityEngine.Random.Range(min, max);
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x0013DEEF File Offset: 0x0013C2EF
	public void SetVelocityY(float min, float max)
	{
		this.velocity.y = UnityEngine.Random.Range(min, max);
	}

	// Token: 0x04004AAE RID: 19118
	public float bottomOffset = 100f;

	// Token: 0x04004AAF RID: 19119
	public float VelocityXMin = -500f;

	// Token: 0x04004AB0 RID: 19120
	public float VelocityXMax = 500f;

	// Token: 0x04004AB1 RID: 19121
	public float VelocityYMin = 500f;

	// Token: 0x04004AB2 RID: 19122
	public float VelocityYMax = 1000f;

	// Token: 0x04004AB3 RID: 19123
	public float GRAVITY = -100f;

	// Token: 0x04004AB4 RID: 19124
	[SerializeField]
	protected bool clampFallVelocity;

	// Token: 0x04004AB5 RID: 19125
	[SerializeField]
	private bool rotate;

	// Token: 0x04004AB6 RID: 19126
	[SerializeField]
	private Rangef rotationSpeedRange;

	// Token: 0x04004AB7 RID: 19127
	protected Vector2 velocity;

	// Token: 0x04004AB8 RID: 19128
	private float accumulatedGravity;

	// Token: 0x04004AB9 RID: 19129
	private float rotationSpeed;

	// Token: 0x04004ABA RID: 19130
	private float currentAngle;
}
