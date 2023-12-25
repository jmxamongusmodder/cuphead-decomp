using System;
using UnityEngine;

// Token: 0x02000A0C RID: 2572
public class HealerCharmParticleEffect : AbstractPausableComponent
{
	// Token: 0x06003CD0 RID: 15568 RVA: 0x0021A43C File Offset: 0x0021883C
	protected override void Awake()
	{
		base.Awake();
		base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), 1f);
		this.acceleration = this.accelerationRange.RandomFloat();
		this.timeBeforeSeek = this.timeBeforeSeekRange.RandomFloat();
		this.maxSpeed = this.maxSpeedRange.RandomFloat();
		base.animator.Play("Loop", 0, UnityEngine.Random.Range(0f, 1f));
		base.animator.Update(0f);
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x0021A4D0 File Offset: 0x002188D0
	public void SetVars(Vector2 newVel, AbstractPlayerController newTarget, HealerCharmSparkEffect newMain)
	{
		this.target = newTarget;
		this.vel = newVel * this.initialEmissionSpeed;
		this.main = newMain;
		base.transform.position += this.vel * 0.04f;
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x0021A528 File Offset: 0x00218928
	private void FixedUpdate()
	{
		if (CupheadTime.FixedDelta == 0f)
		{
			return;
		}
		if (this.target == null)
		{
			return;
		}
		this.frameTimer += CupheadTime.FixedDelta;
		while (this.frameTimer > this.frameTime)
		{
			this.frameTimer -= this.frameTime;
			this.FrameUpdate();
		}
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x0021A598 File Offset: 0x00218998
	private void FrameUpdate()
	{
		base.transform.position += this.vel * this.frameTime;
		base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-25f, 25f, Mathf.InverseLerp(-this.maxSpeed, this.maxSpeed, this.vel.x)) * -Mathf.Sign(base.transform.localScale.x));
		this.timer += this.frameTime;
		if (this.timer > this.timeBeforeSeek)
		{
			Vector3 a = this.target.center - base.transform.position;
			float magnitude = a.magnitude;
			if (magnitude < this.contactDistance && this.timer > this.timeBeforeCanCollect)
			{
				if (this.main)
				{
					this.main.StartPlayerFlash();
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.vel += a * (this.timer - this.timeBeforeSeek) * (this.timer - this.timeBeforeSeek) * this.acceleration * this.frameTime;
				if (this.vel.magnitude > this.maxSpeed)
				{
					this.vel = this.vel.normalized * this.maxSpeed;
				}
				if (this.timer > this.timeBeforeLerp)
				{
					float num = Mathf.InverseLerp(this.timeBeforeLerp, this.maxTime, this.timer);
					num *= num;
					base.transform.position = Vector3.Lerp(base.transform.position, this.target.center, num);
				}
			}
		}
	}

	// Token: 0x04004417 RID: 17431
	[SerializeField]
	private float initialEmissionSpeed = 400f;

	// Token: 0x04004418 RID: 17432
	[SerializeField]
	private MinMax timeBeforeSeekRange = new MinMax(0.025f, 0.075f);

	// Token: 0x04004419 RID: 17433
	private float timeBeforeSeek;

	// Token: 0x0400441A RID: 17434
	[SerializeField]
	private float timeBeforeCanCollect = 0.5f;

	// Token: 0x0400441B RID: 17435
	[SerializeField]
	private float timeBeforeLerp = 1f;

	// Token: 0x0400441C RID: 17436
	[SerializeField]
	private float maxTime = 0.75f;

	// Token: 0x0400441D RID: 17437
	[SerializeField]
	private MinMax accelerationRange = new MinMax(150f, 250f);

	// Token: 0x0400441E RID: 17438
	private float acceleration;

	// Token: 0x0400441F RID: 17439
	[SerializeField]
	private MinMax maxSpeedRange = new MinMax(1500f, 2500f);

	// Token: 0x04004420 RID: 17440
	private float maxSpeed;

	// Token: 0x04004421 RID: 17441
	[SerializeField]
	private float contactDistance = 25f;

	// Token: 0x04004422 RID: 17442
	private AbstractPlayerController target;

	// Token: 0x04004423 RID: 17443
	private float timer;

	// Token: 0x04004424 RID: 17444
	private Vector3 vel;

	// Token: 0x04004425 RID: 17445
	[SerializeField]
	private float frameTime = 0.041666668f;

	// Token: 0x04004426 RID: 17446
	private float frameTimer;

	// Token: 0x04004427 RID: 17447
	private HealerCharmSparkEffect main;
}
