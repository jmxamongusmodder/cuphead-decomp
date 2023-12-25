using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CE RID: 2254
public class HarbourPlatformingLevelOctoProjectile : AbstractProjectile
{
	// Token: 0x060034B6 RID: 13494 RVA: 0x001EA34E File Offset: 0x001E874E
	protected override void Start()
	{
		base.Start();
		this.velocity.y = this.speedY;
		this.velocity.x = this.speedX;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x001EA388 File Offset: 0x001E8788
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003CE1 RID: 15585
	[SerializeField]
	private float speedX;

	// Token: 0x04003CE2 RID: 15586
	[SerializeField]
	private float speedY;

	// Token: 0x04003CE3 RID: 15587
	[SerializeField]
	private float gravity;

	// Token: 0x04003CE4 RID: 15588
	private Vector2 velocity;
}
