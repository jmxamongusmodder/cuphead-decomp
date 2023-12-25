using System;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
public class MouseLevelRomanCandleProjectile : HomingProjectile
{
	// Token: 0x0600261F RID: 9759 RVA: 0x0016456D File Offset: 0x0016296D
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
