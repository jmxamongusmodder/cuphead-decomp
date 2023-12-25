using System;
using UnityEngine;

// Token: 0x0200095B RID: 2395
public class MapNPCRandomTrigger : MonoBehaviour
{
	// Token: 0x060037EE RID: 14318 RVA: 0x00200CBA File Offset: 0x001FF0BA
	private void Start()
	{
		this.loopToWait = UnityEngine.Random.Range(this.triggerMinFrequency, this.triggerMaxFrequency + 1);
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x00200CD5 File Offset: 0x001FF0D5
	private void Looped()
	{
		this.loopToWait--;
		if (this.loopToWait <= 0)
		{
			this.loopToWait = UnityEngine.Random.Range(this.triggerMinFrequency, this.triggerMaxFrequency + 1);
			this.Trigger();
		}
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x00200D10 File Offset: 0x001FF110
	private void Trigger()
	{
		this.animator.SetTrigger(this.trigger);
	}

	// Token: 0x04003FDB RID: 16347
	[SerializeField]
	private Animator animator;

	// Token: 0x04003FDC RID: 16348
	[SerializeField]
	private int triggerMinFrequency = 3;

	// Token: 0x04003FDD RID: 16349
	[SerializeField]
	private int triggerMaxFrequency = 5;

	// Token: 0x04003FDE RID: 16350
	public string trigger = "blink";

	// Token: 0x04003FDF RID: 16351
	private int loopToWait;
}
