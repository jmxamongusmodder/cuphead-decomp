using System;
using UnityEngine;

// Token: 0x02000AEA RID: 2794
public class BasicUprightProjectile : BasicProjectile
{
	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x060043B1 RID: 17329 RVA: 0x001314D3 File Offset: 0x0012F8D3
	protected override Vector3 Direction
	{
		get
		{
			return this._direction;
		}
	}

	// Token: 0x060043B2 RID: 17330 RVA: 0x001314DC File Offset: 0x0012F8DC
	public override AbstractProjectile Create(Vector2 position, float rotation)
	{
		BasicUprightProjectile basicUprightProjectile = base.Create(position, 0f) as BasicUprightProjectile;
		basicUprightProjectile._direction = Quaternion.Euler(0f, 0f, rotation) * Vector3.right;
		return basicUprightProjectile;
	}

	// Token: 0x04004967 RID: 18791
	private Vector3 _direction;
}
