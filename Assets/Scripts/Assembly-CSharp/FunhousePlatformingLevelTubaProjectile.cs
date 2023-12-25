using System;

// Token: 0x020008C0 RID: 2240
public class FunhousePlatformingLevelTubaProjectile : BasicProjectile
{
	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06003451 RID: 13393 RVA: 0x001E5ECC File Offset: 0x001E42CC
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06003452 RID: 13394 RVA: 0x001E5ECF File Offset: 0x001E42CF
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x001E5ED6 File Offset: 0x001E42D6
	protected override void Awake()
	{
		base.Awake();
		this.DestroyDistance = 0f;
	}
}
