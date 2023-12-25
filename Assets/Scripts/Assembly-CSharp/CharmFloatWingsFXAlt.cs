using System;
using UnityEngine;

// Token: 0x02000A46 RID: 2630
public class CharmFloatWingsFXAlt : Effect
{
	// Token: 0x06003EB4 RID: 16052 RVA: 0x00225FF4 File Offset: 0x002243F4
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		CharmFloatWingsFXAlt charmFloatWingsFXAlt = base.Create(position, scale) as CharmFloatWingsFXAlt;
		charmFloatWingsFXAlt.anim.speed = 1f;
		charmFloatWingsFXAlt.anim.Play("Feather", 0, UnityEngine.Random.Range(0f, 0.5f));
		charmFloatWingsFXAlt.vel = MathUtils.AngleToDirection((float)(UnityEngine.Random.Range(-45, -145) + ((!Rand.Bool()) ? -50 : 50))) * this.startSpeed;
		charmFloatWingsFXAlt.vel.y = 0f;
		charmFloatWingsFXAlt.startVel = charmFloatWingsFXAlt.vel.x;
		charmFloatWingsFXAlt.transform.rotation = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.vel) + -90f * Mathf.Sign(this.startVel));
		return charmFloatWingsFXAlt;
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x002260DC File Offset: 0x002244DC
	private void FixedUpdate()
	{
		if (CupheadTime.FixedDelta > 0f)
		{
			base.transform.position += this.vel;
			this.vel -= this.slowFactor * this.startVel * Vector3.right;
			this.vel.y = this.vel.y + this.riseFactor;
			if (Mathf.Sign(this.vel.x) != Mathf.Sign(this.startVel))
			{
				this.vel.x = this.vel.x * 0.95f;
			}
			base.transform.rotation = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.vel) + -90f * Mathf.Sign(this.startVel));
		}
	}

	// Token: 0x040045BE RID: 17854
	[SerializeField]
	private Animator anim;

	// Token: 0x040045BF RID: 17855
	[SerializeField]
	private Vector3 vel;

	// Token: 0x040045C0 RID: 17856
	[SerializeField]
	private float startVel;

	// Token: 0x040045C1 RID: 17857
	[SerializeField]
	private float startSpeed = 30f;

	// Token: 0x040045C2 RID: 17858
	[SerializeField]
	private float slowFactor = 0.95f;

	// Token: 0x040045C3 RID: 17859
	[SerializeField]
	private float riseFactor = 0.02f;
}
