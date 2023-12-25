using System;
using UnityEngine;

// Token: 0x020006FF RID: 1791
public class OldManLevelBubbleController : MonoBehaviour
{
	// Token: 0x06002659 RID: 9817 RVA: 0x00166664 File Offset: 0x00164A64
	private void Start()
	{
		for (int i = 0; i < this.animators.Length; i++)
		{
			this.animators[i].Play("Bubble", 0, 1f);
		}
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x001666A4 File Offset: 0x00164AA4
	private void FixedUpdate()
	{
		this.timer -= CupheadTime.FixedDelta;
		if (this.timer <= 0f)
		{
			int num = UnityEngine.Random.Range(0, this.animators.Length);
			if (this.animators[num].GetCurrentAnimatorStateInfo(0).normalizedTime > this.minTimeToRepeat)
			{
				this.animators[num].Play("Bubble", 0, 0f);
			}
			this.timer = UnityEngine.Random.Range(this.minDelay, this.maxDelay);
		}
	}

	// Token: 0x04002EE6 RID: 12006
	[SerializeField]
	private float minDelay = 0.05f;

	// Token: 0x04002EE7 RID: 12007
	[SerializeField]
	private float maxDelay = 0.5f;

	// Token: 0x04002EE8 RID: 12008
	[SerializeField]
	private float minTimeToRepeat = 2f;

	// Token: 0x04002EE9 RID: 12009
	[SerializeField]
	private Animator[] animators;

	// Token: 0x04002EEA RID: 12010
	private float timer;
}
