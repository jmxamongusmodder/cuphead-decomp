using System;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
public class SnowCultLevelSplitShotBulletShattered : BasicProjectile
{
	// Token: 0x06002EEB RID: 12011 RVA: 0x001BAFE8 File Offset: 0x001B93E8
	public override BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		SnowCultLevelSplitShotBulletShattered snowCultLevelSplitShotBulletShattered = base.Create(position, rotation, speed) as SnowCultLevelSplitShotBulletShattered;
		snowCultLevelSplitShotBulletShattered.animator.Play((!Rand.Bool()) ? "MoonB" : "MoonA");
		snowCultLevelSplitShotBulletShattered.fxTimer = 0f;
		return snowCultLevelSplitShotBulletShattered;
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x001BB034 File Offset: 0x001B9434
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.fxTimer += CupheadTime.FixedDelta;
		if (this.fxTimer > this.fxDelay)
		{
			this.fxTimer -= this.fxDelay;
			this.trailFX.Create(base.transform.position);
		}
	}

	// Token: 0x040037A0 RID: 14240
	[SerializeField]
	private Effect trailFX;

	// Token: 0x040037A1 RID: 14241
	[SerializeField]
	private float fxDelay = 0.3f;

	// Token: 0x040037A2 RID: 14242
	private float fxTimer;
}
