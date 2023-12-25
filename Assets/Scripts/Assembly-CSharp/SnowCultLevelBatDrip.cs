using System;
using UnityEngine;

// Token: 0x020007E7 RID: 2023
public class SnowCultLevelBatDrip : SnowCultLevelBatEffect
{
	// Token: 0x06002E55 RID: 11861 RVA: 0x001B4FD0 File Offset: 0x001B33D0
	private void FixedUpdate()
	{
		base.transform.position += this.vel * CupheadTime.FixedDelta;
		this.vel.y = this.vel.y - this.gravity * CupheadTime.FixedDelta;
		if (base.transform.position.y <= (float)Level.Current.Ground + -20f)
		{
			this.vel = Vector3.zero;
			this.gravity = 0f;
			base.animator.Play("Splat" + this.colorString);
		}
	}

	// Token: 0x040036E7 RID: 14055
	private const float GROUND_OFFSET = -20f;

	// Token: 0x040036E8 RID: 14056
	[SerializeField]
	private float gravity = 10f;

	// Token: 0x040036E9 RID: 14057
	public Vector3 vel;
}
