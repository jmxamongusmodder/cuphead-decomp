using System;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
public class AirplaneLevelTerrierSmokeFX : Effect
{
	// Token: 0x060014B4 RID: 5300 RVA: 0x000BA0B6 File Offset: 0x000B84B6
	public void Step(float t)
	{
		this.myTransform.position += this.vel * t;
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x000BA0DA File Offset: 0x000B84DA
	protected override void OnEffectComplete()
	{
		if (this.dead)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.inUse = false;
		}
	}

	// Token: 0x04001E22 RID: 7714
	public SpriteRenderer rend;

	// Token: 0x04001E23 RID: 7715
	public Vector3 vel;

	// Token: 0x04001E24 RID: 7716
	public bool dead;

	// Token: 0x04001E25 RID: 7717
	public new bool inUse = true;

	// Token: 0x04001E26 RID: 7718
	public Transform myTransform;
}
