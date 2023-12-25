using System;
using UnityEngine;

// Token: 0x02000623 RID: 1571
public class FlyingBirdLevelNursePillProjectile : BasicProjectile
{
	// Token: 0x06001FF7 RID: 8183 RVA: 0x00125A88 File Offset: 0x00123E88
	public void SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor color)
	{
		if (color == FlyingBirdLevelNursePillProjectile.PillColor.Yellow)
		{
			this.yellowPill.SetActive(true);
		}
		else if (color == FlyingBirdLevelNursePillProjectile.PillColor.Blue)
		{
			this.bluePill.SetActive(true);
		}
		else if (color == FlyingBirdLevelNursePillProjectile.PillColor.LightPink)
		{
			this.lightPinkPill.SetActive(true);
		}
		else
		{
			this.darkPinkPill.SetActive(true);
		}
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x00125AE8 File Offset: 0x00123EE8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002873 RID: 10355
	[SerializeField]
	private GameObject yellowPill;

	// Token: 0x04002874 RID: 10356
	[SerializeField]
	private GameObject bluePill;

	// Token: 0x04002875 RID: 10357
	[SerializeField]
	private GameObject lightPinkPill;

	// Token: 0x04002876 RID: 10358
	[SerializeField]
	private GameObject darkPinkPill;

	// Token: 0x02000624 RID: 1572
	public enum PillColor
	{
		// Token: 0x04002878 RID: 10360
		Yellow,
		// Token: 0x04002879 RID: 10361
		Blue,
		// Token: 0x0400287A RID: 10362
		LightPink,
		// Token: 0x0400287B RID: 10363
		DarkPink
	}
}
