using System;
using UnityEngine;

// Token: 0x02000879 RID: 2169
public class ForestPlatformingLevelAcornPropeller : AbstractPausableComponent
{
	// Token: 0x06003262 RID: 12898 RVA: 0x001D57F3 File Offset: 0x001D3BF3
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x001D57FC File Offset: 0x001D3BFC
	public ForestPlatformingLevelAcornPropeller Create(Vector2 position, float speed)
	{
		ForestPlatformingLevelAcornPropeller forestPlatformingLevelAcornPropeller = this.InstantiatePrefab<ForestPlatformingLevelAcornPropeller>();
		forestPlatformingLevelAcornPropeller.transform.position = position;
		forestPlatformingLevelAcornPropeller.speed = speed;
		return forestPlatformingLevelAcornPropeller;
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x001D582C File Offset: 0x001D3C2C
	private void Update()
	{
		base.transform.AddPosition(0f, this.speed * CupheadTime.Delta, 0f);
		this.t += CupheadTime.Delta;
		if (this.t > 5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04003AC2 RID: 15042
	private float speed;

	// Token: 0x04003AC3 RID: 15043
	private float t;

	// Token: 0x04003AC4 RID: 15044
	private const float LIFETIME = 5f;
}
