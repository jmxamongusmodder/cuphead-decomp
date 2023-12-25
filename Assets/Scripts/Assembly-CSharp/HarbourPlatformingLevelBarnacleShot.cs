using System;
using UnityEngine;

// Token: 0x020008C3 RID: 2243
public class HarbourPlatformingLevelBarnacleShot : BasicProjectile
{
	// Token: 0x06003469 RID: 13417 RVA: 0x001E700C File Offset: 0x001E540C
	public override BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		HarbourPlatformingLevelBarnacleShot harbourPlatformingLevelBarnacleShot = base.Create(position, rotation, speed) as HarbourPlatformingLevelBarnacleShot;
		harbourPlatformingLevelBarnacleShot.animator.SetFloat("Speed", ((!Rand.Bool()) ? 1f : -1f) * 1f * UnityEngine.Random.Range(0.9f, 1.1f));
		return harbourPlatformingLevelBarnacleShot;
	}

	// Token: 0x04003C95 RID: 15509
	private const float ProjectileSpeed = 1f;
}
