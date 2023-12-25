using System;
using UnityEngine;

// Token: 0x02000647 RID: 1607
public class FlyingCowboyFloatingSausages : Effect
{
	// Token: 0x060020FE RID: 8446 RVA: 0x00130CCA File Offset: 0x0012F0CA
	public void SetAnimation(string name)
	{
		base.animator.Play(name);
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x00130CD8 File Offset: 0x0012F0D8
	private void FixedUpdate()
	{
		base.transform.position += Vector3.up * 200f * CupheadTime.FixedDelta;
		if (base.transform.position.y > 460f)
		{
			this.OnEffectComplete();
		}
	}

	// Token: 0x04002998 RID: 10648
	private const float OFFSET = 100f;

	// Token: 0x04002999 RID: 10649
	private const float SPEED = 200f;
}
