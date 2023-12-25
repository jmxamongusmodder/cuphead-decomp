using System;

// Token: 0x02000A55 RID: 2645
public class PlayerSuperChaliceShmupBullet : BasicProjectile
{
	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06003F0E RID: 16142 RVA: 0x002289C9 File Offset: 0x00226DC9
	protected override float DestroyLifetime
	{
		get
		{
			return this.lifetimeMax;
		}
	}

	// Token: 0x04004625 RID: 17957
	public float lifetimeMax = 20f;
}
