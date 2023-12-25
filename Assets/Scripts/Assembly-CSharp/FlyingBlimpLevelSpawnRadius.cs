using System;
using UnityEngine;

// Token: 0x02000641 RID: 1601
public class FlyingBlimpLevelSpawnRadius : AbstractMonoBehaviour
{
	// Token: 0x17000387 RID: 903
	// (get) Token: 0x060020E0 RID: 8416 RVA: 0x0012FEE3 File Offset: 0x0012E2E3
	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x0012FEEC File Offset: 0x0012E2EC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
		Gizmos.DrawSphere(base.baseTransform.position, this.radius);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.radius);
	}

	// Token: 0x0400297A RID: 10618
	[SerializeField]
	private float _radius = 100f;
}
