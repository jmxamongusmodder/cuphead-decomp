using System;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
public class SpriteDeathPartsDLC : SpriteDeathParts
{
	// Token: 0x060044E8 RID: 17640 RVA: 0x002473B0 File Offset: 0x002457B0
	private void Start()
	{
		if (this.progressiveBlur)
		{
			this.rend.material.SetFloat("_BlurAmount", 0f);
			this.rend.material.SetFloat("_BlurLerp", 0f);
		}
		if (this.progressiveDim)
		{
			this.startColor = this.rend.color;
		}
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x00247418 File Offset: 0x00245818
	public void SetVelocity(Vector3 vel)
	{
		this.velocity = vel;
	}

	// Token: 0x060044EA RID: 17642 RVA: 0x00247426 File Offset: 0x00245826
	private void FixedUpdate()
	{
		if (CupheadTime.FixedDelta > 0f)
		{
			this.Step(CupheadTime.FixedDelta * 1.2f);
		}
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x00247448 File Offset: 0x00245848
	protected override void Step(float deltaTime)
	{
		base.Step(deltaTime);
		if (this.progressiveBlur)
		{
			this.rend.material.SetFloat("_BlurAmount", this.rend.material.GetFloat("_BlurAmount") + deltaTime * this.blurIncreaseSpeed);
			this.rend.material.SetFloat("_BlurLerp", this.rend.material.GetFloat("_BlurLerp") + deltaTime * this.blurIncreaseSpeed);
		}
		if (this.progressiveDim)
		{
			this.dimTimer += deltaTime * this.dimIncreaseSpeed;
			this.rend.color = Color.Lerp(this.startColor, Color.black, this.dimTimer);
		}
	}

	// Token: 0x060044EC RID: 17644 RVA: 0x0024750E File Offset: 0x0024590E
	protected override void Update()
	{
	}

	// Token: 0x04004ABB RID: 19131
	private const float UPDATE_TIMING_ADJUST = 1.2f;

	// Token: 0x04004ABC RID: 19132
	[SerializeField]
	private bool progressiveBlur;

	// Token: 0x04004ABD RID: 19133
	[SerializeField]
	private float blurIncreaseSpeed = 3f;

	// Token: 0x04004ABE RID: 19134
	[SerializeField]
	private bool progressiveDim;

	// Token: 0x04004ABF RID: 19135
	[SerializeField]
	private float dimIncreaseSpeed = 3f;

	// Token: 0x04004AC0 RID: 19136
	private Color startColor;

	// Token: 0x04004AC1 RID: 19137
	private float dimTimer;

	// Token: 0x04004AC2 RID: 19138
	[SerializeField]
	private SpriteRenderer rend;
}
