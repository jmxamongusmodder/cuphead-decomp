using System;
using UnityEngine;

// Token: 0x02000795 RID: 1941
public class RumRunnersLevelMobBossProjectile : BasicProjectile
{
	// Token: 0x06002B14 RID: 11028 RVA: 0x00192254 File Offset: 0x00190654
	public override BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		BasicProjectile basicProjectile = base.Create(position, rotation, speed);
		basicProjectile.CollisionDeath.None();
		basicProjectile.DamagesType.OnlyPlayer();
		return basicProjectile;
	}
}
