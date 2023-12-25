using System;
using UnityEngine;

// Token: 0x02000A06 RID: 2566
public class ArcadeWeaponBullet : BasicProjectile
{
	// Token: 0x06003CA0 RID: 15520 RVA: 0x00219F08 File Offset: 0x00218308
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<RetroArcadeEnemy>())
		{
			ArcadeWeaponBullet.IN_COMBO = true;
			ArcadeWeaponBullet.POINTS_BONUS_ACCURACY += RetroArcadeLevel.ACCURACY_BONUS;
		}
		else if (ArcadeWeaponBullet.IN_COMBO)
		{
			RetroArcadeLevel.TOTAL_POINTS += ArcadeWeaponBullet.POINTS_BONUS_ACCURACY;
			ArcadeWeaponBullet.POINTS_BONUS_ACCURACY = 0f;
			ArcadeWeaponBullet.IN_COMBO = false;
		}
	}

	// Token: 0x040043F2 RID: 17394
	private static float POINTS_BONUS_ACCURACY;

	// Token: 0x040043F3 RID: 17395
	private static bool IN_COMBO;
}
