using System;
using UnityEngine;

// Token: 0x020007FB RID: 2043
public class SnowCultLevelWhaleCollision : AbstractCollidableObject
{
	// Token: 0x06002EF5 RID: 12021 RVA: 0x001BB3BF File Offset: 0x001B97BF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.wiz.PlayerHitByWhale(hit, phase);
		}
	}

	// Token: 0x040037AD RID: 14253
	[SerializeField]
	private SnowCultLevelWizard wiz;
}
