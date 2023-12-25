using System;
using UnityEngine;

// Token: 0x02000597 RID: 1431
public class DevilLevelSplitDevilProjectile : BasicProjectile
{
	// Token: 0x06001B6C RID: 7020 RVA: 0x000FB020 File Offset: 0x000F9420
	public DevilLevelSplitDevilProjectile Create(Vector2 position, float rotation, float speed, DevilLevelSplitDevil devil)
	{
		DevilLevelSplitDevilProjectile devilLevelSplitDevilProjectile = base.Create(position, rotation, speed) as DevilLevelSplitDevilProjectile;
		devilLevelSplitDevilProjectile.devil = devil;
		return devilLevelSplitDevilProjectile;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x000FB045 File Offset: 0x000F9445
	protected override void Update()
	{
		base.Update();
		if (base.dead)
		{
			return;
		}
		if (this.devil == null)
		{
			this.Die();
			return;
		}
		this.UpdateColor();
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x000FB077 File Offset: 0x000F9477
	private void UpdateColor()
	{
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000FB079 File Offset: 0x000F9479
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040024A3 RID: 9379
	private DevilLevelSplitDevil devil;
}
