using System;
using UnityEngine;

// Token: 0x020006AE RID: 1710
public class FrogsLevelMorphedCoin : BasicProjectile
{
	// Token: 0x0600244F RID: 9295 RVA: 0x00155274 File Offset: 0x00153674
	public FrogsLevelMorphedCoin CreateCoin(Vector2 pos, float speed, float rotation)
	{
		FrogsLevelMorphedCoin frogsLevelMorphedCoin = base.Create(pos, rotation, speed) as FrogsLevelMorphedCoin;
		frogsLevelMorphedCoin.CollisionDeath.None();
		frogsLevelMorphedCoin.DamagesType.OnlyPlayer();
		return frogsLevelMorphedCoin;
	}
}
