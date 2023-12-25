using System;
using UnityEngine;

// Token: 0x02000609 RID: 1545
public class FlowerLevelFlowerAnimator : AbstractPausableComponent
{
	// Token: 0x06001F11 RID: 7953 RVA: 0x0011D9D4 File Offset: 0x0011BDD4
	private void OnIdleEnd()
	{
		if (this.loops >= this.max)
		{
			this.OnBlink();
			return;
		}
		this.loops++;
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x0011D9FC File Offset: 0x0011BDFC
	private void OnBlink()
	{
		base.animator.SetTrigger("OnBlink");
		this.max = UnityEngine.Random.Range(2, 5);
		this.loops = 0;
	}

	// Token: 0x040027B1 RID: 10161
	private const int MIN_IDLE_LOOPS = 2;

	// Token: 0x040027B2 RID: 10162
	private const int MAX_IDLE_LOOPS = 4;

	// Token: 0x040027B3 RID: 10163
	private int loops;

	// Token: 0x040027B4 RID: 10164
	private int max = 2;
}
