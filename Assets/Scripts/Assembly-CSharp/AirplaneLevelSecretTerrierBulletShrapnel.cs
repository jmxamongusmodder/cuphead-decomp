using System;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public class AirplaneLevelSecretTerrierBulletShrapnel : BasicProjectile
{
	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001489 RID: 5257 RVA: 0x000B839F File Offset: 0x000B679F
	protected override Vector3 Direction
	{
		get
		{
			return -base.transform.up;
		}
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x000B83B4 File Offset: 0x000B67B4
	public override AbstractProjectile Create()
	{
		AirplaneLevelSecretTerrierBulletShrapnel airplaneLevelSecretTerrierBulletShrapnel = (AirplaneLevelSecretTerrierBulletShrapnel)base.Create();
		airplaneLevelSecretTerrierBulletShrapnel.anim.Play("Move", 0, (float)UnityEngine.Random.Range(0, 1));
		return airplaneLevelSecretTerrierBulletShrapnel;
	}

	// Token: 0x04001DDF RID: 7647
	[SerializeField]
	private Animator anim;
}
