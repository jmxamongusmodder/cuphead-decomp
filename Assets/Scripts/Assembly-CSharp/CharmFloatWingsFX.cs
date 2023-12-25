using System;
using UnityEngine;

// Token: 0x02000A45 RID: 2629
public class CharmFloatWingsFX : Effect
{
	// Token: 0x06003EB1 RID: 16049 RVA: 0x00225ED8 File Offset: 0x002242D8
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		CharmFloatWingsFX charmFloatWingsFX = base.Create(position, scale) as CharmFloatWingsFX;
		charmFloatWingsFX.anim.Play("Feather", 0, UnityEngine.Random.Range(0f, 0.5f));
		charmFloatWingsFX.vel = MathUtils.AngleToDirection((float)UnityEngine.Random.Range(0, 360)) * (UnityEngine.Random.Range(this.startSpeedMin, this.startSpeedMax) + ((UnityEngine.Random.Range(0f, 6f) >= 1f) ? 0f : this.startSpeedMax));
		return charmFloatWingsFX;
	}

	// Token: 0x06003EB2 RID: 16050 RVA: 0x00225F70 File Offset: 0x00224370
	private void FixedUpdate()
	{
		base.transform.position += this.vel;
		this.vel *= this.slowFactor;
		this.vel.y = this.vel.y + this.riseFactor;
	}

	// Token: 0x040045B8 RID: 17848
	[SerializeField]
	private Animator anim;

	// Token: 0x040045B9 RID: 17849
	[SerializeField]
	private Vector3 vel;

	// Token: 0x040045BA RID: 17850
	[SerializeField]
	private float startSpeedMin = 10f;

	// Token: 0x040045BB RID: 17851
	[SerializeField]
	private float startSpeedMax = 20f;

	// Token: 0x040045BC RID: 17852
	[SerializeField]
	private float slowFactor = 0.95f;

	// Token: 0x040045BD RID: 17853
	[SerializeField]
	private float riseFactor = 0.02f;
}
