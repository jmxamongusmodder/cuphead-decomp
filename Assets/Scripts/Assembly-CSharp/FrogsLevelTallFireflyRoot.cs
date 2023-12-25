using System;
using UnityEngine;

// Token: 0x020006C1 RID: 1729
public class FrogsLevelTallFireflyRoot : AbstractMonoBehaviour
{
	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060024C9 RID: 9417 RVA: 0x00158FA4 File Offset: 0x001573A4
	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x00158FAC File Offset: 0x001573AC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
		Gizmos.DrawSphere(base.baseTransform.position, this.radius);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.radius);
	}

	// Token: 0x04002D6D RID: 11629
	[SerializeField]
	private float _radius = 100f;
}
