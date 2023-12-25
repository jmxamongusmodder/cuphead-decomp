using System;
using UnityEngine;

// Token: 0x02000B09 RID: 2825
public class ShopScenePig : AbstractMonoBehaviour
{
	// Token: 0x06004486 RID: 17542 RVA: 0x00244010 File Offset: 0x00242410
	private void OnIdleLoop()
	{
		this.idleLoops++;
		if (this.idleLoops >= this.idleLoopsMax)
		{
			base.animator.SetTrigger("OnClock");
			this.idleLoopsMax = UnityEngine.Random.Range(20, 35);
			this.idleLoops = 0;
		}
	}

	// Token: 0x06004487 RID: 17543 RVA: 0x00244062 File Offset: 0x00242462
	public void OnStart()
	{
		AudioManager.Play("shop_pig_welcome");
		base.animator.Play("Welcome");
	}

	// Token: 0x06004488 RID: 17544 RVA: 0x0024407E File Offset: 0x0024247E
	public void OnPurchase()
	{
		AudioManager.Play("shop_pig_nod");
		base.animator.Play("Nod");
	}

	// Token: 0x06004489 RID: 17545 RVA: 0x0024409A File Offset: 0x0024249A
	public void OnExit()
	{
		AudioManager.Play("shop_pig_bye");
		base.animator.Play("Bye");
	}

	// Token: 0x04004A2C RID: 18988
	private const int CLOCK_LOOPS_MIN = 20;

	// Token: 0x04004A2D RID: 18989
	private const int CLOCK_LOOPS_MAX = 35;

	// Token: 0x04004A2E RID: 18990
	private int idleLoopsMax = 35;

	// Token: 0x04004A2F RID: 18991
	private int idleLoops;
}
