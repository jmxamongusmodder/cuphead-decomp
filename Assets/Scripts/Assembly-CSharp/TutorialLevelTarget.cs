using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000835 RID: 2101
public class TutorialLevelTarget : AbstractCollidableObject
{
	// Token: 0x14000059 RID: 89
	// (add) Token: 0x060030B7 RID: 12471 RVA: 0x001CA654 File Offset: 0x001C8A54
	// (remove) Token: 0x060030B8 RID: 12472 RVA: 0x001CA68C File Offset: 0x001C8A8C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnShotEvent;

	// Token: 0x060030B9 RID: 12473 RVA: 0x001CA6C2 File Offset: 0x001C8AC2
	protected override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayerProjectile(hit, phase);
		base.GetComponent<Collider2D>().enabled = false;
		if (this.OnShotEvent != null)
		{
			this.OnShotEvent();
		}
	}
}
