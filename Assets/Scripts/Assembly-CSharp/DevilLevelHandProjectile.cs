using System;
using System.Collections;

// Token: 0x02000579 RID: 1401
public class DevilLevelHandProjectile : BasicProjectile
{
	// Token: 0x06001AAD RID: 6829 RVA: 0x000F4ADF File Offset: 0x000F2EDF
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.animation_cr());
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000F4AF4 File Offset: 0x000F2EF4
	private IEnumerator animation_cr()
	{
		this.move = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Projectile", false, true);
		this.move = true;
		yield break;
	}
}
