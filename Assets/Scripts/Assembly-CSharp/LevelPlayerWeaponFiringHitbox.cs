using System;
using UnityEngine;

// Token: 0x02000A4A RID: 2634
public class LevelPlayerWeaponFiringHitbox : CollisionChild
{
	// Token: 0x06003EC2 RID: 16066 RVA: 0x00226640 File Offset: 0x00224A40
	public LevelPlayerWeaponFiringHitbox Create(Vector2 pos, float rotation)
	{
		LevelPlayerWeaponFiringHitbox levelPlayerWeaponFiringHitbox = this.InstantiatePrefab<LevelPlayerWeaponFiringHitbox>();
		levelPlayerWeaponFiringHitbox.transform.position = pos;
		levelPlayerWeaponFiringHitbox.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		return levelPlayerWeaponFiringHitbox;
	}
}
