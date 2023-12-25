using System;
using UnityEngine;

// Token: 0x020008B5 RID: 2229
public class FunhousePlatformingLevelExplosionFX : Effect
{
	// Token: 0x060033F8 RID: 13304 RVA: 0x001E2B7E File Offset: 0x001E0F7E
	private void SpawnSmoke()
	{
		this.smoke.Create(base.transform.position);
	}

	// Token: 0x060033F9 RID: 13305 RVA: 0x001E2B97 File Offset: 0x001E0F97
	private void FirecrackerLines()
	{
		this.firecracker.Create(base.transform.position);
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x001E2BB0 File Offset: 0x001E0FB0
	private void MiniExplosion()
	{
		base.GetComponent<EffectRadius>().CreateInRadius();
	}

	// Token: 0x04003C45 RID: 15429
	[SerializeField]
	private Effect smoke;

	// Token: 0x04003C46 RID: 15430
	[SerializeField]
	private Effect firecracker;
}
